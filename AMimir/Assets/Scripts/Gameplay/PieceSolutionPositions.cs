using System.Collections.Generic;
using UnityEngine;

namespace Busta.Gameplay
{
    public class PieceSolutionPositions : MonoBehaviour
    {
        [SerializeField]
        private List<Vector2Int> positions;

        public Vector2Int GetSolutionPosition(int index = 0)
        {
            return positions[index];
        }

        public void AddSolutionPositions(List<Vector2Int> solutions)
        {
            positions.AddRange(solutions);
        }
        
        public void AddSolutionPosition(Vector2Int solution)
        {
            positions.Add(solution);
        }
    }
}