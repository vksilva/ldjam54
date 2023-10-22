using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject bed;
    [SerializeField] private LayerMask checkLayer;
    [SerializeField] private Vector2 _gameArea = new (15, 20);

    public static GameController Instance { get; private set; }

    private Camera _camera;
    private Vector2Int _bedSize;
    private static readonly Vector3 _offset = new (0.5f, 0.5f, 0f);

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
                var hits = Physics2D.RaycastAll(bed.transform.position + tile + _offset, Vector2.zero, Mathf.Infinity,checkLayer);
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
        Gizmos.DrawWireCube(Vector2.zero, _gameArea);
    }

    private void SetCameraSize()
    {
        var gameAspect = _gameArea.x / _gameArea.y;
        if (gameAspect < _camera.aspect)
        {
            _camera.orthographicSize = _gameArea.y / 2;
        }
        else
        {
            _camera.orthographicSize = _gameArea.x / (2 * _camera.aspect);
        }
    }
}
