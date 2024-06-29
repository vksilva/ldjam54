using System;
using Gameplay;
using UnityEngine;

namespace AppCore
{
    [ExecuteInEditMode]
    public class CameraSetUp : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField] private bool oddSize;
        
        [SerializeField] private Vector2Int _gameArea = new (15, 20);

        private float XOffset => oddSize ? 0.5f : 0.0f;
        
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
            Gizmos.DrawWireCube(new Vector2(XOffset,0f), new Vector3(_gameArea.x, _gameArea.y));
        }
    }
}