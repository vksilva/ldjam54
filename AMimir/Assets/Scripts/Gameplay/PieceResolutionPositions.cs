using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Busta.Gameplay
{
    public class PieceResolutionPositions : MonoBehaviour
    {
        private List<Vector2Int> positions;

        public Vector2Int GetResolutionPosition(int position = 0)
        {
            return positions[position];
        }

        public void AddResolutionPositions(List<Vector2Int> resolutions)
        {
            foreach (var resolution in resolutions.Where(resolution => !positions.Contains(resolution)))
            {
                positions.Add(resolution);
            }
        }
        
        public void AddResolutionPosition(Vector2Int resolution)
        {
            if(!positions.Contains(resolution))
            {
                positions.Add(resolution);
            }
        }
    }
}