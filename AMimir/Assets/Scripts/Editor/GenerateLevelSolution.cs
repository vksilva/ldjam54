using System.Collections.Generic;
using Busta.Gameplay;
using UnityEditor;
using UnityEngine;

namespace Busta.Editor
{
    public class GenerateLevelSolution
    {
        [MenuItem("Vanessa/Generate levels resolutions")]
        public static void GenerateResolutions()
        {

            var bed = GameObject.FindObjectOfType<Bed>();
            var bedCollider = bed.GetComponent<BoxCollider2D>();
            var catsAndObstacles = GameObject.FindObjectsOfType<PieceMovement>();
            var cats = new List<PieceMovement>();
            var obstacles = new List<PieceMovement>();
            
            foreach (var catOrObstacle in catsAndObstacles)
            {
                if (catOrObstacle.IsObstacle())
                {
                    if (catOrObstacle.GetComponent<PieceMovement>().enabled)
                    {
                        obstacles.Add(catOrObstacle);
                    }
                }
                else
                {
                    cats.Add(catOrObstacle);
                }
            }
            
            Debug.Log($"Bed: {bed} {bedCollider.size} {bed.transform.position}");
            foreach (var pieceMovement in cats)
            {
                Debug.Log($"Cat {pieceMovement}");
            }
            foreach (var pieceMovement in obstacles)
            {
                Debug.Log($"Obstacles: {pieceMovement}, {pieceMovement.transform.position}");
            }
            
            
        }
    }
}