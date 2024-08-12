using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Busta.Gameplay
{
    public class PieceSolutionPositions : MonoBehaviour
    {
        public List<Vector2Int> positions;

#if UNITY_EDITOR
        [SerializeField] private int positionIndex;
        [Button("Set Positions")]
        private void SetPositions()
        {
            var piecesSolutionsList = FindObjectsOfType<PieceSolutionPositions>();
            foreach (var pieceSolutions in piecesSolutionsList)
            {
                var pieceMovement = pieceSolutions.GetComponent<PieceMovement>();
                pieceMovement.solutionPos = pieceSolutions.positions[positionIndex];

                EditorUtility.SetDirty(pieceMovement); // Indicate changes so level can be saved
            }
        }
#endif
    }
}