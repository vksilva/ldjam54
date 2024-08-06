using System.Linq;
using Busta.AppCore.Audio;
using Busta.Extensions;
using Busta.UI;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Busta.Gameplay
{
    public class PieceMovement : MonoBehaviour
    {
        [SerializeField] private bool _obstacle = false;
        [SerializeField] private bool isInvisibleObstacle = false;

        [SerializeField] private Vector2Int solutionPos;

        private LayerMask pieceLayer;
        private LayerMask bedLayer;
        private Vector3 anchor;
        private Camera cam;
        private Vector3 positionBeforeMove;
        private Vector2Int size;
        private static readonly Vector3 gridOffset = new(0.5f, 0.5f, 0f);
        public bool IsDragging { get; private set; }
        private const float catReturnSpeed = 15f;

        private SortingGroup pieceSortingGroup;
        private GameController gameController;
        private Collider2D catCollider;

        private readonly RaycastHit2D[] hitResults = new RaycastHit2D[5];
        private SpriteRenderer catRenderer;

        private const string FloatingPieceSortingLayer = "FloatingPiece";
        private const string DefaultPieceSortingLayer = "Default";
        private const string BedSortingLayer = "Bed";

        private static readonly int ShadowOffset = Shader.PropertyToID("_ShadowOffset");
        private static readonly int ShadowNoise = Shader.PropertyToID("_BreathNoise");

        private void Awake()
        {
            pieceLayer = LayerMask.GetMask(DefaultPieceSortingLayer);
            bedLayer = LayerMask.GetMask(BedSortingLayer);
            pieceSortingGroup = GetComponent<SortingGroup>();
            catCollider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            gameController = GameController.Instance;

            var position = transform.position;
            position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
            transform.position = position;

            cam = Camera.main;
            var pieceCollider = GetComponent<Collider2D>();
            size = new Vector2Int(
                Mathf.RoundToInt(pieceCollider.bounds.size.x),
                Mathf.RoundToInt(pieceCollider.bounds.size.y)
            );

            IsDragging = false;
            catRenderer = GetComponentInChildren<SpriteRenderer>();

            if (!isInvisibleObstacle)
            {
                DisplayShadow(false);
                SetNoise(Random.Range(0, Mathf.PI * 2f));
            }
        }

        public bool IsObstacle()
        {
            return _obstacle;
        }

        private void SetNoise(float noise)
        {
            var mpb = new MaterialPropertyBlock();
            catRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(ShadowNoise, noise);
            foreach (var r in GetComponentsInChildren<Renderer>())
            {
                r.SetPropertyBlock(mpb);
            }

            catRenderer.SetPropertyBlock(mpb);
        }

        private void DisplayShadow(bool show)
        {
            var mpb = new MaterialPropertyBlock();
            catRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(ShadowOffset, show ? 2f : 0.5f);
            catRenderer.SetPropertyBlock(mpb);
        }

        private void OnMouseDrag()
        {
            if (!IsDragging)
            {
                return;
            }

            var worldPosition = GetMousePosition();
            var position = worldPosition - anchor;

            transform.position = position;

            ShowBackgroundHighLight();
        }

        private void ShowBackgroundHighLight()
        {
            CleanHighlightGrid();

            var nearIntPosition = NearIntPosition();

            for (var x = 0; x < size.x; x++)
            for (var y = 0; y < size.y; y++)
            {
                var tile = new Vector3(x, y, 0);
                var tileCoordinate = nearIntPosition + tile;
                var tileCenterPosition = tileCoordinate + gridOffset;

                var hitBed = Physics2D.RaycastNonAlloc(tileCenterPosition, Vector2.zero, hitResults,
                    Mathf.Infinity, bedLayer);
                var hitPiece = Physics2D.RaycastNonAlloc(tileCenterPosition, Vector2.zero, hitResults,
                    Mathf.Infinity, pieceLayer);
                if (hitPiece > 0 && hitBed > 0)
                {
                    if (gameController.highlightGridTiles.TryGetValue(
                            new Vector2Int((int)tileCoordinate.x, (int)tileCoordinate.y), out var gridTile))
                    {
                        if (hitResults[0].collider.gameObject != gameObject)
                        {
                            continue;
                        }

                        gridTile.gameObject.SetActive(true);
                        gridTile.SetColor(hitPiece == 1);
                    }
                }
            }
        }

        private void CleanHighlightGrid()
        {
            foreach (var tile in gameController.highlightGridTiles.Values)
            {
                tile.gameObject.SetActive(false);
            }
        }

        private Vector3 GetMousePosition()
        {
            var position = Input.mousePosition;
            var worldPosition = cam.ScreenToWorldPoint(position);
            worldPosition.z = 0;
            return worldPosition;
        }

        private void OnMouseDown()
        {
            if (_obstacle || IsPointerOverUI())
            {
                return;
            }

            IsDragging = true;

            gameController.PlayGameSfx(AudioSFXEnum.MoveUpPiece);

            DisplayShadow(true);
            pieceSortingGroup.sortingLayerName = FloatingPieceSortingLayer;

            var piecePosition = transform.position;
            positionBeforeMove = piecePosition;

            var position = GetMousePosition();
            anchor = position - piecePosition;
        }

        private void OnMouseUp()
        {
            if (_obstacle || IsPointerOverUI())
            {
                return;
            }

            DisplayShadow(false);
            CleanHighlightGrid();
            IsDragging = false;

            gameController.PlayGameSfx(AudioSFXEnum.MoveDownPiece);

            var position = transform.position;

            //check if final position is valid
            for (var x = 0; x < size.x; x++)
            for (var y = 0; y < size.y; y++)
            {
                var tile = new Vector3(x, y, 0);
                var hitGrabArea = CheckHitArea(gameController.PiecesGrabArea, position + tile + gridOffset);

                var origin = position + tile + gridOffset;
                var pieceHits = Physics2D.RaycastNonAlloc(origin, Vector2.zero, hitResults,
                    Mathf.Infinity, pieceLayer);
                var bedHits = Physics2D.RaycastNonAlloc(origin, Vector2.zero, hitResults,
                    Mathf.Infinity, bedLayer);
                if (pieceHits > 1 || (bedHits == 0 && !hitGrabArea))
                {
                    //Move piece back to original position
                    // TODO check surrounding area for a valid position instead of returning piece
                    catCollider.enabled = false;
                    transform.DOMove(positionBeforeMove, catReturnSpeed).SetSpeedBased().OnComplete(() =>
                    {
                        catCollider.enabled = true;
                        pieceSortingGroup.sortingLayerName = DefaultPieceSortingLayer;
                    });
                    gameController.IncrementFailedMovements();
                    return;
                }
            }

            //move piece to near int position
            transform.position = NearIntPosition();
            pieceSortingGroup.sortingLayerName = DefaultPieceSortingLayer;

            GameController.Instance.OnPiecePlaced();
        }

        private Vector3 NearIntPosition()
        {
            var position = transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            position.z = 0;

            return position;
        }

        private bool IsPointerOverUI()
        {
            return MouseOverUILayerObject.IsPointerOverUIObject();
        }

        private bool CheckHitArea(RectInt area, Vector3 position)
        {
            return position.x > area.xMin && position.x < area.xMax && position.y > area.yMin && position.y < area.yMax;
        }

        public void SetObstacle(bool isObstacle)
        {
            _obstacle = isObstacle;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_obstacle)
            {
                return;
            }

            var bed = FindObjectOfType<Bed>();
            var bedPosition = new Vector2(bed.transform.position.x, bed.transform.position.y);

            Handles.color = Color.green;
            Handles.Label(bed.transform.position + new Vector3(solutionPos.x + 0.1f, solutionPos.y + 0.3f), name);

            Gizmos.color = Color.magenta;

            var boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider)
            {
                Gizmos.matrix = bed.transform.localToWorldMatrix * Matrix4x4.Translate(solutionPos.ToVector3());
                Gizmos.DrawWireCube(boxCollider.offset - bedPosition, boxCollider.size);
                return;
            }

            var polygonCollider = GetComponent<PolygonCollider2D>();
            if (polygonCollider)
            {
                Gizmos.matrix = bed.transform.localToWorldMatrix * Matrix4x4.Translate(solutionPos.ToVector3());
                Gizmos.DrawLineStrip(
                    polygonCollider.points.Select(v => new Vector3(v.x, v.y) - bed.transform.position).ToArray(), true);
            }
        }
#endif
    }
}