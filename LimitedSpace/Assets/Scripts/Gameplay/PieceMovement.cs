using System;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    public class PieceMovement : MonoBehaviour
    {
        [SerializeField] private bool _obstacle = false;
        
        private Vector3 _shadowOffset = new (-0.1f, -0.3f);
        private static Material _shadowMaterial;
        private LayerMask pieceLayer;
        private Vector3 _anchor;
        private Camera _camera;
        private Vector3 _positionBeforeMove;
        private Vector2Int _size;
        private static readonly Vector3 _offset = new (0.5f, 0.5f, 0f);
        private GameObject _shadow;

        private const string FloatingPieceSortingLayer = "FloatingPiece";
        private const string DefaultPieceSortingLayer = "Default";

        private SpriteRenderer _pieceSpriteRenderer;
        
        private void Awake()
        {
           pieceLayer = LayerMask.GetMask("Default");
           _pieceSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _camera = Camera.main;
            var pieceCollider = GetComponent<Collider2D>();
            _size = new Vector2Int(
                Mathf.RoundToInt(pieceCollider.bounds.size.x),
                Mathf.RoundToInt(pieceCollider.bounds.size.y)
            );

            SetCatShadow();
        }

        private void SetCatShadow()
        {
            if (_shadowMaterial == null)
            {
                _shadowMaterial = Resources.Load<Material>("Materials/CatShadowMaterial");
            }
            
            _shadow = new GameObject("Shadow");
            
            _shadow.transform.SetParent(transform, true);
            _shadow.transform.localPosition = _shadowOffset;
            _shadow.transform.rotation = quaternion.identity;
            
            var catRenderer = GetComponent<SpriteRenderer>();
            var shadowSpriteRenderer = _shadow.AddComponent<SpriteRenderer>();
            shadowSpriteRenderer.sprite = catRenderer.sprite;
            shadowSpriteRenderer.material = _shadowMaterial;

            shadowSpriteRenderer.sortingLayerName = FloatingPieceSortingLayer;
            shadowSpriteRenderer.sortingOrder = catRenderer.sortingOrder - 1;
            
            _shadow.SetActive(false);
        }

        private void DisplayShadow(bool show)
        {
            _shadow.SetActive(show);
        }

        private void LateUpdate()
        {
            _shadow.transform.localPosition = _shadowOffset;
        }

        private void OnMouseDrag()
        {
            if (_obstacle)
            {
                return;
            }
            var worldPosition = GetMousePosition();
            var position = worldPosition - _anchor;

            transform.position = position;
        }

        private Vector3 GetMousePosition()
        {
            var position = Input.mousePosition;
            var worldPosition = _camera.ScreenToWorldPoint(position);
            worldPosition.z = 0;
            return worldPosition;
        }

        private void OnMouseDown()
        {
            if (_obstacle)
            {
                return;
            }
            
            DisplayShadow(true);
            _pieceSpriteRenderer.sortingLayerName = FloatingPieceSortingLayer;
            
            var piecePosition = transform.position;
            _positionBeforeMove = piecePosition;

            var position = GetMousePosition();
            _anchor = position - piecePosition;
        }

        private void OnMouseUp()
        {
            if (_obstacle)
            {
                return;
            }
            
            DisplayShadow(false);
            _pieceSpriteRenderer.sortingLayerName = DefaultPieceSortingLayer;
            
            //check if final position is valid
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    var tile = new Vector3(x, y, 0);
                    var hits = Physics2D.RaycastAll(transform.position + tile + _offset, Vector2.zero, Mathf.Infinity, pieceLayer);
                    if (hits.Length > 1)
                    {
                        //Move piece back to original position
                        transform.position = _positionBeforeMove;
                        return;
                    }
                }
            }

            //move piece to near int position
            var position = transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            position.z = 0.0f;
            transform.position = position;
        
            GameController.Instance.OnPiecePlaced();
        }

        // private void Update()
        // {
        //     for (int x = 0; x < _size.x; x++)
        //     {
        //         for (int y = 0; y < _size.y; y++)
        //         {
        //             var tile = new Vector3(x, y, 0);
        //             Debug.DrawLine(Vector3.zero, transform.position + tile + _offset);
        //         }
        //     }
        // }
    }
}