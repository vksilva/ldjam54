using AppCore;
using AppCore.Audio;
using UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Application = AppCore.Application;

namespace Gameplay
{
    public class PieceMovement : MonoBehaviour
    {
        [SerializeField] private bool _obstacle = false;

        private LayerMask pieceLayer;
        private LayerMask bedLayer;
        private Vector3 _anchor;
        private Camera _camera;
        private Vector3 _positionBeforeMove;
        private Vector2Int _size;
        private static readonly Vector3 _offset = new(0.5f, 0.5f, 0f);
        private bool isDragging;

        private const string FloatingPieceSortingLayer = "FloatingPiece";
        private const string DefaultPieceSortingLayer = "Default";
        private const string BedSortingLayer = "Bed";

        private SortingGroup _pieceSortingGroup;

        private GameController _gameController;

        private readonly RaycastHit2D[] _hitResults = new RaycastHit2D[5];
        private SpriteRenderer _catRenderer;
        private static readonly int ShadowOffset = Shader.PropertyToID("_ShadowOffset");

        private void Awake()
        {
            pieceLayer = LayerMask.GetMask(DefaultPieceSortingLayer);
            bedLayer = LayerMask.GetMask(BedSortingLayer);
            _pieceSortingGroup = GetComponent<SortingGroup>();
        }

        private void Start()
        {
            _gameController = GameController.Instance;

            var position = transform.position;
            position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
            transform.position = position;

            _camera = Camera.main;
            var pieceCollider = GetComponent<Collider2D>();
            _size = new Vector2Int(
                Mathf.RoundToInt(pieceCollider.bounds.size.x),
                Mathf.RoundToInt(pieceCollider.bounds.size.y)
            );

            isDragging = false;
            _catRenderer = GetComponentInChildren<SpriteRenderer>();
            DisplayShadow(false);
        }

        private void DisplayShadow(bool show)
        {
            var mpb = new MaterialPropertyBlock();
            _catRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(ShadowOffset, show ? 2f : 0.5f);
            _catRenderer.SetPropertyBlock(mpb);
        }

        private void OnMouseDrag()
        {
            if (!isDragging)
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
            if (_obstacle || IsPointerOverUI())
            {
                return;
            }

            isDragging = true;

            _gameController.PlayGameSfx(AudioSFXEnum.MoveUpPiece);

            DisplayShadow(true);
            _pieceSortingGroup.sortingLayerName = FloatingPieceSortingLayer;

            var piecePosition = transform.position;
            _positionBeforeMove = piecePosition;

            var position = GetMousePosition();
            _anchor = position - piecePosition;
        }

        private void OnMouseUp()
        {
            if (_obstacle || IsPointerOverUI())
            {
                return;
            }

            DisplayShadow(false);
            isDragging = false;
            _pieceSortingGroup.sortingLayerName = DefaultPieceSortingLayer;

            _gameController.PlayGameSfx(AudioSFXEnum.MoveDownPiece);

            var position = transform.position;

            //check if final position is valid
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    var tile = new Vector3(x, y, 0);

                    var hitGrabArea = CheckHitArea(_gameController.PiecesGrabArea, position + tile + _offset);

                    var pieceHits = Physics2D.RaycastNonAlloc(position + tile + _offset, Vector2.zero, _hitResults,
                        Mathf.Infinity, pieceLayer);
                    var bedHits = Physics2D.RaycastNonAlloc(position + tile + _offset, Vector2.zero, _hitResults,
                        Mathf.Infinity, bedLayer);
                    if (pieceHits > 1 || (bedHits == 0 && !hitGrabArea))
                    {
                        //Move piece back to original position
                        transform.position = _positionBeforeMove;
                        return;
                    }
                }
            }

            //move piece to near int position
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            position.z = 0.0f;
            transform.position = position;

            GameController.Instance.OnPiecePlaced();
        }

        private bool IsPointerOverUI()
        {
            return MouseOverUILayerObject.IsPointerOverUIObject();
        }

        private bool CheckHitArea(RectInt area, Vector3 position)
        {
            return position.x > area.xMin && position.x < area.xMax && position.y > area.yMin && position.y < area.yMax;
        }
    }
}