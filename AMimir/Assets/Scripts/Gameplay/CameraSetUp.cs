using Extensions;
using UnityEngine;

namespace Gameplay
{
    [ExecuteInEditMode]
    public class CameraSetUp : MonoBehaviour
    {
        private const int GameAreaDivisionAxis = -1;

        private Camera _camera;

        [SerializeField] private bool oddSize;

        [SerializeField] private Vector2Int _gameArea = new(15, 20);

        public RectInt GrabArea => _grabArea;
        private RectInt _grabArea;
        private RectInt _catPlacementArea;

        private float XOffset => oddSize ? 0.5f : 0.0f;
        private int intXOffset => oddSize ? 1 : 0;

        private Vector2Int _previousSize;
        private float _previousAspect;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            UpdateCameraSize();
        }

        private void UpdateCameraSize()
        {
            if (_previousSize != _gameArea || !Mathf.Approximately(_previousAspect, _camera.aspect))
            {
                _camera.orthographicSize = CalculateCameraSize(_gameArea, _camera.aspect);
                transform.position = new Vector3(XOffset, 0f, -10f);
                _previousSize = _gameArea;
                _previousAspect = _camera.aspect;

                var cameraHeight = _camera.orthographicSize;
                var cameraWidth = cameraHeight * _camera.aspect;
                _grabArea.SetMinMax(
                    new Vector2Int(
                        intXOffset - Mathf.FloorToInt(cameraWidth),
                        -Mathf.FloorToInt(cameraHeight)
                    ),
                    new Vector2Int(
                        Mathf.FloorToInt(cameraWidth)
                        , GameAreaDivisionAxis
                    )
                );


                _catPlacementArea.SetMinMax(
                    new Vector2Int(
                        intXOffset -_gameArea.x/2,
                        -_gameArea.y/2
                    ),
                    new Vector2Int(
                        _gameArea.x/2,
                        GameAreaDivisionAxis
                    )
                );
            }
        }

        public static float CalculateCameraSize(Vector2Int gameArea, float cameraAspect)
        {
            var gameAspect = gameArea.x / (float)gameArea.y;

            return gameAspect < cameraAspect
                ? gameArea.y / 2.0f
                : gameArea.x / (2.0f * cameraAspect);
        }

#if UNITY_EDITOR
        private void Update()
        {
            UpdateCameraSize();
        }
#endif

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(new Vector3(XOffset, 0f), _gameArea.ToVector3());
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_grabArea.center, _grabArea.size.ToVector3());
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_catPlacementArea.center, _catPlacementArea.size.ToVector3());
        }
    }
}