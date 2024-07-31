using System.Collections.Generic;
using System.Linq;
using Busta.Gameplay;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Busta.Editor
{
    public static class GenerateLevelSolution
    {
        [MenuItem("Vanessa/Generate levels resolutions")]
        public static void GenerateResolutions()
        {
            var bed = Object.FindObjectOfType<Bed>();
            var bedCollider = bed.GetComponent<BoxCollider2D>();
            var cats = Object.FindObjectsOfType<PieceSolutionPositions>();
            
            Dictionary<PieceSolutionPositions, Vector2> catsSize = new();
            Dictionary<PieceSolutionPositions, List<Vector2>> catsPossiblePositions = new();
            
            SetCatsPossiblePositions(cats, catsSize, bedCollider, catsPossiblePositions);

            foreach (var cat in cats)
            {
                Debug.Log($"Bed: {bedCollider.size}");
                Debug.Log($"Cat size: {catsSize[cat]}");
                Debug.Log($"Possible position for cat {cat}");
                foreach (var position in catsPossiblePositions[cat])
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

        private static void SetCatsPossiblePositions(PieceSolutionPositions[] cats, Dictionary<PieceSolutionPositions, 
                Vector2> catsSize, BoxCollider2D bedCollider,
                Dictionary<PieceSolutionPositions, List<Vector2>> catsPossiblePositions)
        {
            foreach (var cat in cats)
            {
                catsSize[cat] = GetCatSize(cat);

                catsPossiblePositions[cat] = GetCatPositions(catsSize[cat], bedCollider);
            }
        }

        private static List<Vector2> GetCatPositions(Vector2 catSize, BoxCollider2D bedCollider)
        {
            List<Vector2> catPositions = new();
            for (var x = 0; x < (bedCollider.size.x - catSize.x + 1); x++)
            {
                for (var y = 0; y < (bedCollider.size.y - catSize.y + 1); y++)
                {
                    catPositions.Add(new Vector2(x, y));
                }
            }
            return catPositions;
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

        private static bool TryToPositionCatonBed(PieceSolutionPositions catToPosition, Bed bed,
            PieceSolutionPositions[] catsPositioned)
        {
            
            return false;
        }
    }
}