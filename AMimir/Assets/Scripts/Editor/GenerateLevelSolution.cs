using System.Collections.Generic;
using System.Linq;
using System.Text;
using Busta.Extensions;
using Busta.Gameplay;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Busta.Editor
{
    public static class GenerateLevelSolution
    {
        private static readonly RaycastHit2D[] HitResults = new RaycastHit2D[5];
        private const string DefaultPieceSortingLayer = "Default";
        private static LayerMask pieceLayer;
        private static readonly Vector3 PieceCenterOffset = new(0.5f, 0.5f);
        private static readonly Vector2Int invalidPosition = new(-1, -1);

        public struct PiecePlacementData
        {
            public Vector2Int Size;
            public List<Vector2Int> PossiblePositions;
            public int[,] Matrix;
        }

        public struct BedData
        {
            public Vector2Int Size;
            public int[,] Matrix;
        }

        public struct CatState
        {
            public Vector2Int[] Positions;

            public CatState(int size)
            {
                Positions = new Vector2Int[size];
                for (var i = 0; i < size; i++)
                {
                    Positions[i] = invalidPosition;
                }
            }

            public CatState MakeCopy()
            {
                var newPositions = new Vector2Int[Positions.Length];
                Positions.CopyTo(newPositions, 0);
                return new CatState
                {
                    Positions = newPositions
                };
            }

            public override string ToString()
            {
                return string.Join("\n", Positions);
            }
        }

        [MenuItem("Vanessa/Levels/Generate levels resolutions")]
        public static void GenerateResolutions()
        {
            var bed = Object.FindObjectOfType<Bed>();
            var cats = Object.FindObjectsOfType<PieceSolutionPositions>();
            var catsPositions = cats.Select(cat => cat.transform.position).ToArray();
            var bedCollider = bed.GetComponent<BoxCollider2D>();
            var bedSize = bedCollider.size.ToVector2Int();
            var bedPosition = bedCollider.transform.position;
            var catsData = new PiecePlacementData[cats.Length];
            pieceLayer = LayerMask.GetMask(DefaultPieceSortingLayer);

            SetCatsPossiblePositions(cats, catsData, bedSize);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Bed size: {bedSize}");
            stringBuilder.AppendLine($"Bed position: {bedPosition}");

            var bedMatrix = GetBedMatrix(bedSize, bedPosition);

            var bedData = new BedData
            {
                Size = bedSize,
                Matrix = bedMatrix
            };

            for (var index = 0; index < cats.Length; index++)
            {
                var cat = cats[index];
                cat.positions.Clear();
                var data = catsData[index];

                stringBuilder.AppendLine("---------------");
                stringBuilder.AppendLine($"Cat: {cat.name}");
                stringBuilder.AppendLine($"Size: {data.Size}");
            }

            Debug.Log(stringBuilder.ToString());

            var solutions = GetSolutionsRecursive(cats, catsData, new CatState(cats.Length), bedData, 0);

            for (var i = 0; i < cats.Length; i++)
            {
                cats[i].gameObject.SetActive(true);
                foreach (var solution in solutions)
                {
                    cats[i].positions.Add(solution.Positions[i]);
                }

                cats[i].transform.position = catsPositions[i];
            }
        }

        private static int[,] GetBedMatrix(Vector2Int bedSize, Vector3 bedPosition)
        {
            var bedMatrix = new int[bedSize.x, bedSize.y];
            for (var y = bedSize.y - 1; y >= 0; y--)
            {
                for (var x = 0; x < bedSize.x; x++)
                {
                    var origin = new Vector2(x, y) + new Vector2(bedPosition.x, bedPosition.y) +
                                 new Vector2(PieceCenterOffset.x, PieceCenterOffset.y);
                    var pieceHits = Physics2D.RaycastNonAlloc(origin, Vector2.zero, HitResults,
                        Mathf.Infinity, pieceLayer);
                    bedMatrix[x, y] = pieceHits > 0 ? 1 : 0;
                }
            }

            return bedMatrix;
        }

        private static void SetCatsPossiblePositions(PieceSolutionPositions[] cats, PiecePlacementData[] catData,
            Vector2 bedSize)
        {
            for (var index = 0; index < cats.Length; index++)
            {
                var cat = cats[index];
                var catSize = GetCatSize(cat);
                var positions = GetCatPositions(catSize, bedSize);
                var catMatrix = GetCatMatrix(catSize, cat);

                catData[index] = new PiecePlacementData
                {
                    Size = catSize,
                    PossiblePositions = positions,
                    Matrix = catMatrix
                };
            }
        }

        private static int[,] GetCatMatrix(Vector2Int catSize, PieceSolutionPositions cat)
        {
            var catMatrix = new int[catSize.x, catSize.y];

            for (var y = catSize.y - 1; y >= 0; y--)
            {
                for (var x = 0; x < catSize.x; x++)
                {
                    var overlap = cat.GetComponent<Collider2D>()
                        .OverlapPoint(cat.transform.position + new Vector3(x, y) + PieceCenterOffset);
                    catMatrix[x, y] = overlap ? 1 : 0;
                }
            }

            return catMatrix;
        }

        private static Vector2Int GetCatSize(PieceSolutionPositions cat)
        {
            var boxCollider = cat.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                return boxCollider.size.ToVector2Int();
            }

            var polygonCollider = cat.GetComponent<PolygonCollider2D>();
            if (polygonCollider != null)
            {
                return polygonCollider.bounds.size.ToVector2Int();
            }

            return Vector2Int.zero;
        }

        private static List<Vector2Int> GetCatPositions(Vector2 catSize, Vector2 bedSize)
        {
            List<Vector2Int> catPositions = new();
            for (var x = 0; x < (bedSize.x - catSize.x + 1); x++)
            {
                for (var y = 0; y < (bedSize.y - catSize.y + 1); y++)
                {
                    catPositions.Add(new Vector2Int(x, y));
                }
            }

            return catPositions;
        }

        public static bool CheckIfValid(int index, CatState newState, PiecePlacementData[] catsData, BedData bedData)
        {
            var currentCat = catsData[index];
            var currentCatPosition = newState.Positions[index]; // relative to bed

            for (var x = 0; x < currentCat.Size.x; x++)
            {
                for (var y = 0; y < currentCat.Size.y; y++)
                {
                    var bedPosition = currentCatPosition + new Vector2Int(x, y);

                    var value = GetBedValueAtPosition(bedPosition, bedData.Matrix);
                    for (var i = 0; i < catsData.Length; i++)
                    {
                        value += GetCatValueAtPosition(bedPosition, newState.Positions[i], catsData[i].Matrix);
                        if (value > 1)
                        {
                            // Pieces are either overlapping bed obstacles or other cats 
                            return false;
                        }
                    }
                }
            }

            // No pieces are overlapping after placing the new cat, this might be part of a solution
            return true;
        }

        private static int GetCatValueAtPosition(Vector2Int checkPosition, Vector2Int catStatePosition,
            int[,] catMatrix)
        {
            if (catStatePosition == invalidPosition)
            {
                return 0; // cat not placed, it won't contribute
            }

            var relativePosition = checkPosition - catStatePosition;
            if (relativePosition.x >= catMatrix.GetLength(0) ||
                relativePosition.x < 0 ||
                relativePosition.y >= catMatrix.GetLength(1) || 
                relativePosition.y < 0)
            {
                return 0; // point is outside of cat
            }

            return catMatrix[relativePosition.x, relativePosition.y];
        }

        private static int GetBedValueAtPosition(Vector2Int checkPosition, int[,] bedMatrix)
        {
            if (checkPosition.x >= bedMatrix.GetLength(0) || checkPosition.y >= bedMatrix.GetLength(1))
            {
                return 0;
            }

            return bedMatrix[checkPosition.x, checkPosition.y];
        }

        private static List<CatState> GetSolutionsRecursive(PieceSolutionPositions[] cats,
            PiecePlacementData[] catsData, CatState catState, BedData bedData, int catIndex)
        {
            var solutions = new List<CatState>();
            var currentPlacementData = catsData[catIndex];

            for (var index = 0; index < currentPlacementData.PossiblePositions.Count; index++)
            {
                if (catIndex == 0)
                {
                    EditorUtility.DisplayProgressBar("Calculating", $"Testing position {index}/{currentPlacementData.PossiblePositions.Count-1}", 
                        index / (currentPlacementData.PossiblePositions.Count - 1f));
                }
                var position = currentPlacementData.PossiblePositions[index];
                var newState = catState.MakeCopy();

                newState.Positions[catIndex] = position;

                var valid = CheckIfValid(catIndex, newState, catsData, bedData);
                if (!valid)
                {
                    // Invalid state, cut the tree branch by not adding to solutions
                    continue;
                }

                if (catIndex >= cats.Length - 1)
                {
                    // Tree leaf, add current valid state
                    solutions.Add(newState);
                }
                else
                {
                    // Recursion, add solution from child nodes.
                    solutions.AddRange(GetSolutionsRecursive(cats, catsData, newState, bedData, catIndex + 1));
                }
            }

            if (catIndex == 0)
            {
                EditorUtility.ClearProgressBar();
            }

            return solutions;
        }
    }
}