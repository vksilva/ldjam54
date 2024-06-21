using System;
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

            if (!_camera)
            {
                throw new Exception("Game camera missing");
            }
            
            _camera.orthographicSize = CalculateCameraSize(_gameArea, _camera.aspect);
            var bedCollider = bed.GetComponent<Collider2D>();
            _bedSize = new Vector2Int(
                Mathf.RoundToInt(bedCollider.bounds.size.x),
                Mathf.RoundToInt(bedCollider.bounds.size.y)
            );
            
            //Force Bed z = 1 to avoid issue with pieces
            var position = bed.transform.position;
            bed.transform.position = new Vector3(position.x, position.y, 1);

            SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void BackToHomeClicked()
        {
            SceneManager.LoadScene("LevelSelector");
        }

        public void OnPiecePlaced()
        {
            if (IsBedCompleted())
            {
                LevelUIController.Instance.ShowEndGameCanvas();
            }
        }

        private bool IsBedCompleted()
        {
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

            return emptySpaceCount == 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(Vector2.zero, new Vector3(_gameArea.x, _gameArea.y));
        }

        public static float CalculateCameraSize(Vector2Int gameArea, float cameraAspect)
        {
            var gameAspect = gameArea.x / (float)gameArea.y;

            if (gameAspect < cameraAspect)
            {
                return gameArea.y / 2.0f;
            }
            else
            {
                return gameArea.x / (2.0f * cameraAspect);
            }
        }
    }
}
