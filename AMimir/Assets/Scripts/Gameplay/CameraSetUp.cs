using Busta.AppCore.SafeArea;
using Busta.Extensions;
using UnityEngine;
using Application = Busta.AppCore.Application;

namespace Busta.Gameplay
{
    [ExecuteInEditMode]
    public class CameraSetUp : MonoBehaviour
    {
        private const int GameAreaDivisionAxis = -1;

        private Camera _camera;

        [SerializeField] private bool oddSize;

        [SerializeField] private Vector2Int _gameArea = new(15, 20);

        public RectInt GrabArea => grabArea;
        private RectInt grabArea;
        private RectInt catPlacementArea;

        private float XOffset => oddSize ? 0.5f : 0.0f;
        private int intXOffset => oddSize ? 1 : 0;

        private Rect cameraRect;
        private Rect safeArea;
        private Rect safeAreaAnchors = Rect.MinMaxRect(0, 0, 1, 1);

        private Vector2Int _previousSize;
        private float _previousAspect;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            UpdateCameraSize();

            if (!Application.Initialized)
            {
                return;
            }

            Application.Get<SafeAreaService>().RegisterSafeArea(UpdateSafeArea);
        }

        private void OnDestroy()
        {
            if (!Application.Initialized)
            {
                return;
            }

            Application.Get<SafeAreaService>().UnregisterSafeArea(UpdateSafeArea);
        }

        private void UpdateSafeArea(Rect anchors)
        {
            safeAreaAnchors = anchors;
            var min = new Vector2(
                Mathf.Lerp(cameraRect.xMin, cameraRect.xMax, safeAreaAnchors.xMin),
                Mathf.Lerp(cameraRect.yMin, cameraRect.yMax, safeAreaAnchors.yMin)
            );
            var max = new Vector2(
                Mathf.Lerp(cameraRect.xMin, cameraRect.xMax, safeAreaAnchors.xMax),
                Mathf.Lerp(cameraRect.yMin, cameraRect.yMax, safeAreaAnchors.yMax)
            );
            safeArea = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private void UpdateCameraSize()
        {
            if (_previousSize != _gameArea || !Mathf.Approximately(_previousAspect, _camera.aspect))
            {
                _camera.orthographicSize = CalculateCameraSize(_gameArea, _camera.aspect);
                transform.position = new Vector3(XOffset, 0f, -10f);
                _previousSize = _gameArea;
                _previousAspect = _camera.aspect;
                cameraRect.min = -_gameArea/2;
                cameraRect.size = _gameArea;

                var cameraHeight = _camera.orthographicSize;
                var cameraWidth = cameraHeight * _camera.aspect;
                grabArea.SetMinMax(
                    new Vector2Int(
                        intXOffset - Mathf.FloorToInt(cameraWidth),
                        -Mathf.FloorToInt(cameraHeight)
                    ),
                    new Vector2Int(
                        Mathf.FloorToInt(cameraWidth)
                        , GameAreaDivisionAxis
                    )
                );


                catPlacementArea.SetMinMax(
                    new Vector2Int(
                        intXOffset - _gameArea.x / 2,
                        -_gameArea.y / 2
                    ),
                    new Vector2Int(
                        _gameArea.x / 2,
                        GameAreaDivisionAxis
                    )
                );

                UpdateSafeArea(safeAreaAnchors);
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
            Gizmos.DrawWireCube(cameraRect.center, cameraRect.size);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(grabArea.center, grabArea.size.ToVector3());
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(catPlacementArea.center, catPlacementArea.size.ToVector3());
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(safeArea.center, safeArea.size);
        }
    }
}