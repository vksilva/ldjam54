using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Busta.Extensions;
using Busta.Gameplay;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Busta.Editor
{
    public static class GenerateLevelSolution
    {
        private static readonly RaycastHit2D[] hitResults = new RaycastHit2D[5];
        private const string DefaultPieceSortingLayer = "Default";
        private static LayerMask pieceLayer;

        private struct PiecePlacementData
        {
            public Vector2 size;
            public List<Vector2Int> possiblePositions;
        }

        private struct CatState
        {
            public Vector2Int[] positions;

            public CatState(int size)
            {
                positions = new Vector2Int[size];
                for (var i = 0; i < size; i++)
                {
                    positions[i] = new Vector2Int(-1, -1);
                }
            }

            public CatState MakeCopy()
            {
                var newPositions = new Vector2Int[positions.Length];
                positions.CopyTo(newPositions, 0);
                return new CatState
                {
                    positions = newPositions
                };
            }

            public override string ToString()
            {
                return string.Join("\n", positions);
            }
        }

        [MenuItem("Vanessa/Levels/Generate levels resolutions")]
        public static async void GenerateResolutions()
        {
            var bed = Object.FindObjectOfType<Bed>();
            var cats = Object.FindObjectsOfType<PieceSolutionPositions>();
            var bedCollider = bed.GetComponent<BoxCollider2D>();
            var bedSize = bedCollider.size;
            var bedPosition = bedCollider.transform.position;
            var catsData = new Dictionary<PieceSolutionPositions, PiecePlacementData>();
            pieceLayer = LayerMask.GetMask(DefaultPieceSortingLayer);

            SetCatsPossiblePositions(cats, catsData, bedSize);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Bed size: {bedSize}");
            stringBuilder.AppendLine($"Bed position: {bedPosition}");
            foreach (var cat in cats)
            {
                cat.positions.Clear();
                var data = catsData[cat];
                stringBuilder.AppendLine("---------------");
                stringBuilder.AppendLine($"Cat: {cat.name}");
                stringBuilder.AppendLine($"Size: {data.size}");
                stringBuilder.AppendLine("Possible position:");
                foreach (var position in data.possiblePositions)
                {
                    stringBuilder.AppendLine($"- {position}");
                }
            }

            Debug.Log(stringBuilder.ToString());

            var solutions = await GetSolutionsRecursive(cats, catsData, new CatState(cats.Length), bedPosition, 0);

            for (var i = 0; i < cats.Length; i++)
            {
                cats[i].gameObject.SetActive(true);
                foreach (var solution in solutions)
                {
                    cats[i].positions.Add(solution.positions[i]);
                }
            }
        }

        private static void SetCatsPossiblePositions(PieceSolutionPositions[] cats,
            Dictionary<PieceSolutionPositions, PiecePlacementData> catData, Vector2 bedSize)
        {
            foreach (var cat in cats)
            {
                var catSize = GetCatSize(cat);
                var positions = GetCatPositions(catSize, bedSize);

                catData[cat] = new PiecePlacementData
                {
                    size = catSize,
                    possiblePositions = positions
                };
            }
        }

        private static Vector2 GetCatSize(PieceSolutionPositions cat)
        {
            var boxCollider = cat.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                return boxCollider.size;
            }

            var polygonCollider = cat.GetComponent<PolygonCollider2D>();
            if (polygonCollider != null)
            {
                return polygonCollider.bounds.size;
            }

            return Vector2.zero;
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

        private static bool CheckIfValid(Vector2 catPosition, Vector2 catSize, Vector2 bedPosition)
        {
            var pieces = Object.FindObjectsOfType<PieceMovement>();
            
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Raycasting");
            for (var x = catPosition.x; x < catPosition.x + catSize.x; x++)
            {
                for (var y = catPosition.y; y < catPosition.y + catSize.y; y++)
                {
                    var origin = new Vector2(x, y) + bedPosition + new Vector2(0.5f, 0.5f);
                    //var pieceHits = colliders.Where(c => c.OverlapPoint(origin)).Count();
                    var pieceHits = Physics2D.RaycastNonAlloc(origin, Vector2.zero, hitResults,
                        Mathf.Infinity, pieceLayer);
                    stringBuilder.AppendLine($"Ray {origin} - Piece Hits {pieceHits}");
                    if (pieceHits > 1)
                    {
                        stringBuilder.AppendLine("false");
                        Debug.Log(stringBuilder.ToString());
                        // More than one piece (cat/obstacle) overlapping
                        return false;
                    }
                }
            }
            
            stringBuilder.AppendLine("true");
            Debug.Log(stringBuilder.ToString());
            // No pieces are overlapping after placing the new cat
            return true;
        }

        private static void SetCatsPositionsFromState(PieceSolutionPositions[] cats, CatState catState,
            Vector2 bedPosition)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Cat setup from state:\n{catState.ToString()}");
            for (var i = 0; i < cats.Length; i++)
            {
                var pos = catState.positions[i];
                if (pos == new Vector2Int(-1, -1))
                {
                    cats[i].gameObject.SetActive(false);
                    stringBuilder.AppendLine($"Cat {cats[i].name} deactivated");
                }
                else
                {
                    cats[i].gameObject.SetActive(true);
                    cats[i].transform.position = pos + bedPosition;
                    stringBuilder.AppendLine($"Cat {cats[i].name} set at pos {pos+bedPosition}");
                }
            }
            Debug.Log(stringBuilder.ToString());
        }

        private static async Task<List<CatState>> GetSolutionsRecursive(PieceSolutionPositions[] cats,
            Dictionary<PieceSolutionPositions, PiecePlacementData> catsData, CatState catState, Vector2 bedPosition,
            int catIndex)
        {
            var solutions = new List<CatState>();
            var currentCat = cats[catIndex];
            var currentPlacementData = catsData[currentCat];

            foreach (var position in currentPlacementData.possiblePositions)
            {
                var newState = catState.MakeCopy();

                newState.positions[catIndex] = position;
                SetCatsPositionsFromState(cats, newState, bedPosition);
                await Tasks.WaitUntilNextFrame();

                var valid = CheckIfValid(position, catsData[currentCat].size, bedPosition);
                if (!valid)
                {
                    Debug.Log($"Add cat {currentCat.name}[{catIndex}] to position {position}\nState not valid:\n{newState.ToString()}");
                    continue; // Invalid state, cut the tree branch by not adding to solutions
                }

                if (catIndex >= cats.Length - 1) // Tree leaf, add current valid state
                {
                    Debug.Log($"Add cat {currentCat.name}[{catIndex}] to position {position}\nValid state\n{newState.ToString()}");
                    solutions.Add(newState);
                }
                else // Recursion, add solution from child nodes.
                {
                    Debug.Log($"Add cat {currentCat.name}[{catIndex}] to position {position}\nTree recursion\n{newState.ToString()}");
                    solutions.AddRange(await GetSolutionsRecursive(cats, catsData, newState, bedPosition, catIndex + 1));
                }
            }

            return solutions;
        }
    }
}