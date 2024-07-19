using System;
using UnityEngine;

namespace Busta.Gameplay
{
    [ExecuteInEditMode]
    public class SpriteOverlay : MonoBehaviour
    {
        [SerializeField] private Texture2D sprite;

        private SpriteRenderer renderer;
        
        private static readonly int OverlayTex = Shader.PropertyToID("_OverlayTex");

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            UpdateOverlaySprite();
        }

        private void OnValidate()
        {
            UpdateOverlaySprite();
        }

        private void UpdateOverlaySprite()
        {
            if (!renderer || !sprite)
            {
                return;
            }
            var mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);
            mpb.SetTexture(OverlayTex, sprite);
            renderer.SetPropertyBlock(mpb);
        }
    }
}