using System;
using System.Threading.Tasks;
using AppCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application = AppCore.Application;

namespace Gameplay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gameArea = new (15, 20);

        public static GameController Instance { get; private set; }
        private Bed _bed;
        private Camera _camera;
        private Vector2Int _bedSize;
        private static readonly Vector3 _offset = new (0.5f, 0.5f, 0f);
        private readonly LayerMask _checkLayer = 9;

        private static AudioService _audioService;
        private static StateService _stateService;

        private RaycastHit2D[] _rayCastResult = new RaycastHit2D[5];

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _camera = Camera.main;

            _bed = FindObjectOfType<Bed>();

            if (!_camera)
            {
                throw new Exception("Game camera missing");
            }
            
            _camera.orthographicSize = CalculateCameraSize(_gameArea, _camera.aspect);
            var bedCollider = _bed.GetComponent<Collider2D>();
            _bedSize = new Vector2Int(
                Mathf.RoundToInt(bedCollider.bounds.size.x),
                Mathf.RoundToInt(bedCollider.bounds.size.y)
            );
            
            //Force Bed z = 1 to avoid issue with pieces
            var position = _bed.transform.position;
            _bed.transform.position = new Vector3(position.x, position.y, 1);

            GetServices();

            SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
        }

        private static void GetServices()
        {
            _audioService = Application.Instance.Get<AudioService>();
            _stateService = Application.Instance.Get<StateService>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private async void OpenEndGamePopUp()
        {
            await Task.Delay(500);
            _audioService.PlaySfx(AudioSFXEnum.EndGameCelebration);
            LevelUIController.Instance.ShowEndGameCanvas();
        }

        public void OnPiecePlaced()
        {
            if (IsBedCompleted())
            {
                if (!_stateService.gameState.LevelsState.winLevels.Contains(SceneManager.GetActiveScene().name))
                {
                    _stateService.gameState.LevelsState.winLevels.Add(SceneManager.GetActiveScene().name);
                    _stateService.Save();
                }
                
                OpenEndGamePopUp();
            }
        }

        private bool IsBedCompleted()
        {
            var emptySpaceCount = 0;
            for (var x = 0; x < _bedSize.x; x++)
            {
                for (var y = 0; y < _bedSize.y; y++)
                {
                    var tile = new Vector3(x, y, 0);
                    var size = Physics2D.RaycastNonAlloc(_bed.transform.position + tile + _offset, Vector2.zero, _rayCastResult, Mathf.Infinity, _checkLayer);
                    if (size < 2)
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
