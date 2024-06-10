using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject bed;
        [SerializeField] private Vector2Int _gameArea = new (15, 20);

        public static GameController Instance { get; private set; }

        private Camera _camera;
        private Vector2Int _bedSize;
        private static readonly Vector3 _offset = new (0.5f, 0.5f, 0f);
        private readonly LayerMask _checkLayer = 9;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _camera = Camera.main;
            SetCameraSize();
            var bedCollider = bed.GetComponent<Collider2D>();
            _bedSize = new Vector2Int(
                Mathf.RoundToInt(bedCollider.bounds.size.x),
                Mathf.RoundToInt(bedCollider.bounds.size.y)
            );

            SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void BackToHomeClicked()
        {
            SceneManager.LoadScene("Home");
        }

        public void OnPiecePlaced()
        {
            //Check if all bed is populated
            var emptySpaceCount = 0;
            for (int x = 0; x < _bedSize.x; x++)
            {
                for (int y = 0; y < _bedSize.y; y++)
                {
                    var tile = new Vector3(x, y, 0);
                    var hits = Physics2D.RaycastAll(bed.transform.position + tile + _offset, Vector2.zero, Mathf.Infinity,_checkLayer);
                    if (hits.Length < 2)
                    {
                        emptySpaceCount++;
                    }
                }
            }
            if (emptySpaceCount == 0)
            {
                LevelUIController.Instance.ShowEndGameCanvas();
            }
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(Vector2.zero, new Vector3(_gameArea.x, _gameArea.y));
        }

        private void SetCameraSize()
        {
            var gameAspect = _gameArea.x / _gameArea.y;
            if (gameAspect < _camera.aspect)
            {
                _camera.orthographicSize = _gameArea.y / 2.0f;
            }
            else
            {
                _camera.orthographicSize = _gameArea.x / (2.0f * _camera.aspect);
            }
        }
    }
}
