using UnityEngine;

namespace Busta.Gameplay
{
    public class HighlightGridTile : MonoBehaviour
    {
        [SerializeField] private Color freeSpaceColor;
        [SerializeField] private Color occupiedSpaceColor;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetColor(bool free)
        {
            spriteRenderer.color = free ? freeSpaceColor : occupiedSpaceColor;
        }
    }
}