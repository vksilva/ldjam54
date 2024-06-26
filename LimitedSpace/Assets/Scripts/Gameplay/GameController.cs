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
        public static GameController Instance { get; private set; }
        private Bed _bed;
        private Camera _camera;
        private Vector2Int _bedSize;
        private static readonly Vector3 _offset = new (0.5f, 0.5f, 0f);
        private readonly LayerMask _checkLayer = 9;

        private AudioService _audioService;
        private StateService _stateService;

        private readonly RaycastHit2D[] _rayCastResult = new RaycastHit2D[5];

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (!Application.Initialized)
            {
                Initialize.SceneToStart = SceneManager.GetActiveScene().name;
                Debug.LogWarning($"Application not initialized, starting from {Initialize.SceneToStart}");
                SceneManager.LoadScene(0);
                return;
            }
            GetServices();
            
            SetUpCamera();
            SetUpBed();

            SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
        }

        private void SetUpBed()
        {
            _bed = FindObjectOfType<Bed>();
            var bedCollider = _bed.GetComponent<Collider2D>();
            _bedSize = new Vector2Int(
                Mathf.RoundToInt(bedCollider.bounds.size.x),
                Mathf.RoundToInt(bedCollider.bounds.size.y)
            );
            
            //Force Bed z = 1 to avoid issue with pieces
            var bedTransform = _bed.transform;
            var position = bedTransform.position;
            bedTransform.position = new Vector3(position.x, position.y, 1);
        }

        private void SetUpCamera()
        {
            _camera = Camera.main;
            if (!_camera)
            {
                throw new Exception("Game camera missing");
            }

        }

        private void GetServices()
        {
            _audioService = Application.Get<AudioService>();
            _stateService = Application.Get<StateService>();
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

        public void PlayGameSfx(AudioSFXEnum sfx)
        {
            _audioService.PlaySfx(sfx);
        }
    }
}
