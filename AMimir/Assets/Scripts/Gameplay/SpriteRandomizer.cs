using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Busta.Gameplay
{
    public class SpriteRandomizer : MonoBehaviour
    {
        [SerializeField] private List<Sprite> catSprites;

        private void Start()
        {
            var catSpriteRenderer = GetComponent<SpriteRenderer>();
            var catSpriteIndex = Random.Range(0, catSprites.Count);
            catSpriteRenderer.sprite = catSprites[catSpriteIndex];
            catSpriteRenderer.color = Color.white;
        }
    }
}