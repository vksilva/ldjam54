using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Busta.AppCore;
using Busta.AppCore.Audio;
using Busta.AppCore.State;
using Busta.AppCore.Tracking;
using Busta.Extensions;
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
        private int failedMovesCounter = 0;
        private readonly RaycastHit2D[] _rayCastResult = new RaycastHit2D[5];
        private const string matchResultWin = "Win";
        private const string matchResultQuit = "Quit";
        private const string matchResultAbandoned = "Abandoned";
        private const string matchResultRestarted = "Restart";
        private Func<Task> OnBeforeEndGame;
        private PieceSolutionPositions[] cats;
        private Dictionary<PieceSolutionPositions, bool> isHintDisplayed;

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
            // SetUpHints();

            SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);

            _stateService.gameState.levelsState.lastPlayedLevel = SceneManager.GetActiveScene().name;
            _stateService.Save();

            _trackingService.TrackLevelStarted(SceneManager.GetActiveScene().name);
        }

        public void IncrementFailedMovements()
        {
            failedMovesCounter++;
        }

        public void Hint()
        {
            foreach (var cat in cats)
                if (!isHintDisplayed[cat])
                {
                    // Show hint for this cat
                    isHintDisplayed[cat] = false;
                    break;
                }
        }

        private void SetUpHints()
        {
            cats = FindObjectsOfType<PieceSolutionPositions>();
            foreach (var cat in cats) isHintDisplayed[cat] = false;
        }

        private void OnApplicationQuit()
        {
            _trackingService.TrackLevelEnded(SceneManager.GetActiveScene().name, movesCounter, failedMovesCounter,
                timeCounter, matchResultQuit);
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
            for (var x = 0; x < _bedSize.x; x++)
            {
                var posX = bedPosition.x + x;
                for (var y = 0; y < _bedSize.y; y++)
                {
                    var posY = bedPosition.y + y;
                    var tilePos = new Vector3(posX, posY, 0);

                    //f is double bed gap, continue
                    if (_bed.IsDoubleBed())
                        if (FindObjectOfType<DoubleBedGap>().GetComponent<BoxCollider2D>()
                            .OverlapPoint(tilePos + _offset))
                            continue;

                    var tile = Instantiate(gridTilePrefab, tilePos, Quaternion.identity);
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
            _trackingService.TrackLevelEnded(SceneManager.GetActiveScene().name, movesCounter, failedMovesCounter,
                timeCounter, matchResultWin);

            await DoBeforeEndGame();

            await Tasks.WaitForSeconds(0.5f);
            _audioService.PlaySfx(AudioSFXEnum.EndGameCelebration);
            LevelUIController.Instance.ShowEndGameCanvas();
        }

        public void SetBeforeEndgameAction(Func<Task> task)
        {
            OnBeforeEndGame = task;
        }

        private async Task DoBeforeEndGame()
        {
            if (OnBeforeEndGame == null) return;
            await OnBeforeEndGame.Invoke();
        }

        public void RestartLevel()
        {
            _trackingService.TrackLevelEnded(SceneManager.GetActiveScene().name, movesCounter, failedMovesCounter,
                timeCounter, matchResultRestarted);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void TrackAbandonLevel()
        {
            _trackingService.TrackLevelEnded(SceneManager.GetActiveScene().name, movesCounter, failedMovesCounter,
                timeCounter, matchResultAbandoned);
        }

        public void OnPiecePlaced()
        {
            movesCounter++;
            if (IsBedCompleted())
            {
                if (!_stateService.gameState.levelsState.winLevels.Contains(SceneManager.GetActiveScene().name))
                {
                    _stateService.gameState.levelsState.winLevels.Add(SceneManager.GetActiveScene().name);
                    _stateService.Save();
                }

                OpenEndGamePopUp();
            }
        }

        private bool IsBedCompleted()
        {
            var emptySpaceCount = 0;
            for (var x = 0; x < _bedSize.x; x++)
            for (var y = 0; y < _bedSize.y; y++)
            {
                var tile = new Vector3(x, y, 0);
                var size = Physics2D.RaycastNonAlloc(_bed.transform.position + tile + _offset, Vector2.zero,
                    _rayCastResult, Mathf.Infinity, _checkLayer);
                if (size < 2) emptySpaceCount++;
            }

            return emptySpaceCount == 0;
        }

        public void PlayGameSfx(AudioSFXEnum sfx)
        {
            _audioService.PlaySfx(sfx);
        }
    }
}