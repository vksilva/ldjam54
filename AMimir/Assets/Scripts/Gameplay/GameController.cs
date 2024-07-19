using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Busta.AppCore;
using Busta.AppCore.Audio;
using Busta.AppCore.State;
using Busta.AppCore.Tracking;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application = Busta.AppCore.Application;

namespace Busta.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public RectInt PiecesGrabArea => _cameraSetUp.GrabArea;
        
        public static GameController Instance { get; private set; }
        public readonly Dictionary<Vector2Int, HighlightGridTile> highlightGridTiles = new();
        public int failedMovesCounter = 0;
        
        private AudioService _audioService;
        private StateService _stateService;
        private TrackingService _trackingService;
        private Bed _bed;
        private CameraSetUp _cameraSetUp;
        private Vector2Int _bedSize;
        private static readonly Vector3 _offset = new(0.5f, 0.5f, 0f);
        private readonly LayerMask _checkLayer = 9;
        private HighlightGridTile gridTilePrefab;
        private int movesCounter = 0;
        private float timeCounter = 0;
        private readonly RaycastHit2D[] _rayCastResult = new RaycastHit2D[5];
        private const string matchResultWin = "Win";
        private const string matchResultQuit = "Quit";
        private const string matchResultAbandoned = "Abandoned";
        private const string matchResultRestarted = "Restart";

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

            _cameraSetUp = FindObjectOfType<CameraSetUp>();
            if (!_cameraSetUp)
            {
                Debug.LogError("Missing camera");
                return;
            }
            gridTilePrefab = Resources.Load<HighlightGridTile>("GameElements/grid_tile");

            GetServices();
            SetUpBed();

            SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
            
            _stateService.gameState.LevelsState.lastPlayedLevel = SceneManager.GetActiveScene().name;
            _stateService.Save();
            
            _trackingService.TrackLevelStarted(SceneManager.GetActiveScene().name);
        }

        private void OnApplicationQuit()
        {
            _trackingService.TrackLevelEnded(SceneManager.GetActiveScene().name, movesCounter, failedMovesCounter, timeCounter, matchResultQuit);
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
            var pos = bedTransform.position;
            pos.z = 1;
            bedTransform.position = pos;

            var bedPosition = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

            // Set Highlight grid in the bed
            for (int x = 0; x < _bedSize.x; x++)
            {
                var posX = bedPosition.x + x;
                for (int y = 0; y < _bedSize.y; y++)
                {
                    var posY = bedPosition.y + y;
                    var tile = Instantiate(gridTilePrefab, new Vector3(posX, posY, 0), Quaternion.identity);
                    tile.gameObject.SetActive(false);
                    highlightGridTiles[new Vector2Int(posX, posY)] = tile;
                }
            }
        }

        private void Update()
        {
            timeCounter += Time.deltaTime;
        }

        private void GetServices()
        {
            _audioService = Application.Get<AudioService>();
            _stateService = Application.Get<StateService>();
            _trackingService = Application.Get<TrackingService>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private async void OpenEndGamePopUp()
        {
            _trackingService.TrackLevelEnded(SceneManager.GetActiveScene().name, movesCounter, failedMovesCounter, timeCounter, matchResultWin);
            
            await Task.Delay(500);
            _audioService.PlaySfx(AudioSFXEnum.EndGameCelebration);
            LevelUIController.Instance.ShowEndGameCanvas();
        }

        public void RestartLevel()
        {
            _trackingService.TrackLevelEnded(SceneManager.GetActiveScene().name, movesCounter, failedMovesCounter, timeCounter, matchResultRestarted);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void TrackAbandonLevel()
        {
            _trackingService.TrackLevelEnded(SceneManager.GetActiveScene().name, movesCounter, failedMovesCounter, timeCounter,matchResultAbandoned);
        }

        public void OnPiecePlaced()
        {
            movesCounter++;
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
                    var size = Physics2D.RaycastNonAlloc(_bed.transform.position + tile + _offset, Vector2.zero,
                        _rayCastResult, Mathf.Infinity, _checkLayer);
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