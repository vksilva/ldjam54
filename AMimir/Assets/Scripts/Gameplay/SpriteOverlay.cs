using System;
using UnityEditor;
using UnityEngine;

namespace Busta.Gameplay
{
    [ExecuteInEditMode]
    public class SpriteOverlay : MonoBehaviour
    {
        [SerializeField] private Sprite sprite;

        private SpriteRenderer spriteRenderer;
        
        private static readonly int OverlayTex = Shader.PropertyToID("_OverlayTex");
        private static readonly int OverlayOffsetUv = Shader.PropertyToID("_OverlayOffsetUv");

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

            var baseUv = spriteRenderer.sprite.uv[0];
            var overlayUv = sprite.uv[0];
            var offsetUv = overlayUv - baseUv;
            
            var mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetTexture(OverlayTex, sprite.texture);
            mpb.SetVector(OverlayOffsetUv, offsetUv);
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}