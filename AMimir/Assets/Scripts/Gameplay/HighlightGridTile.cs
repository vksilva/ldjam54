using UnityEngine;

namespace Gameplay
{
    public class HighlightGridTile : MonoBehaviour
    {
        [SerializeField] private Color freeSpaceColor;
        [SerializeField] private Color occupiedSpaceColor;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void SetColor(bool free)
        {
            if (free)
            {
                spriteRenderer.color = freeSpaceColor;
            }
            else
            {
                spriteRenderer.color = occupiedSpaceColor;
            }
        }
    }
}
