using System.Collections.Generic;
using Busta.Gameplay;
using UnityEditor;
using UnityEngine;

namespace Busta.Editor
{
    public static class GenerateLevelSolution
    {
        private struct PiecePlacementData
        {
            public Vector2 size;
            public List<Vector2> possiblePositions;
        }
        
        [MenuItem("Vanessa/Generate levels resolutions")]
        public static void GenerateResolutions()
        {
            var bed = Object.FindObjectOfType<Bed>();
            var cats = Object.FindObjectsOfType<PieceSolutionPositions>();
            var bedSize = bed.GetComponent<BoxCollider2D>().size;
            var catsData = new Dictionary<PieceSolutionPositions, PiecePlacementData>();
            
            SetCatsPossiblePositions(cats, catsData, bedSize);

            foreach (var cat in cats)
            {
                var data = catsData[cat];
                Debug.Log($"Bed: {bedSize}");
                Debug.Log($"Cat size: {data.size}");
                Debug.Log($"Possible position for cat {cat}");
                foreach (var position in data.possiblePositions)
                {
                    Debug.Log($"{position}");
                }
            }

            Stack<PieceSolutionPositions> catsStack = new();
            foreach (var cat in cats)
            {
                catsStack.Push(cat);
            }

            
            // foreach cat
            // try to place the cat on possible position
            // if valid, check if cat list is empty
            // if empty, return solution
            // if not empty, go to 2
            // if invalid, return null
        }

        private static void SetCatsPossiblePositions(PieceSolutionPositions[] cats, Dictionary<PieceSolutionPositions, PiecePlacementData> catData, Vector2 bedSize)
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

        private static List<Vector2> GetCatPositions(Vector2 catSize, Vector2 bedSize)
        {
            List<Vector2> catPositions = new();
            for (var x = 0; x < (bedSize.x - catSize.x + 1); x++)
            {
                for (var y = 0; y < (bedSize.y - catSize.y + 1); y++)
                {
                    catPositions.Add(new Vector2(x, y));
                }
            }
            return catPositions;
        }

        private static bool CheckIfValid(Vector2 catPosition, Vector2 catSize)
        {
            for (var x = catPosition.x; x < catPosition.x + catSize.x; x++)
            {
                for (var y = catPosition.y; y < catPosition.y + catSize.y; y++)
                {
                    var origin = new Vector2(x, y);
                    var pieceHits = Physics2D.RaycastNonAlloc(origin, Vector2.zero, hitResults,
                        Mathf.Infinity, pieceLayer);
                }
            }

            return true;
        }
    }
}