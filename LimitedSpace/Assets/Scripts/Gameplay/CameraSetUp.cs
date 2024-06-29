using System;
using Gameplay;
using UnityEngine;

namespace AppCore
{
    [ExecuteInEditMode]
    public class CameraSetUp : MonoBehaviour
    {
        private Camera _camera;
        
        [SerializeField] private Vector2Int _gameArea = new (15, 20);

        private Vector2Int _previousSize;
        
        private void Start()
        {
            _camera = GetComponent<Camera>();

            UpdateCameraSize();
        }

        private void UpdateCameraSize()
        {
            if (_previousSize != _gameArea)
            {
                _camera.orthographicSize = CalculateCameraSize(_gameArea, _camera.aspect);
                _previousSize = _gameArea;
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
            Gizmos.DrawWireCube(Vector2.zero, new Vector3(_gameArea.x, _gameArea.y));
        }
    }
}