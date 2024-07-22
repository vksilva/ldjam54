using System;
using UnityEditor;
using UnityEngine;

namespace Busta.Gameplay
{
    [ExecuteInEditMode]
    public class SpriteOverlay : MonoBehaviour
    {
        [SerializeField] private Texture2D sprite;

        private SpriteRenderer spriteRenderer;
        
        private static readonly int OverlayTex = Shader.PropertyToID("_OverlayTex");

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateOverlaySprite();
        }

        private void OnValidate()
        {
            UpdateOverlaySprite();
        }

        private void UpdateOverlaySprite()
        {
            if (!spriteRenderer || !sprite)
            {
                return;
            }
            var mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetTexture(OverlayTex, sprite);
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}