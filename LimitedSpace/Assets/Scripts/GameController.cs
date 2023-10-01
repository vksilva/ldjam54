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
    [SerializeField] private Button _backToHomeButton;
    [SerializeField] private TMP_Text _endGameText;

    private Camera _camera;
    private Vector2Int _bedSize;
    private static readonly Vector3 _offset = new Vector3(0.5f, 0.5f, 0f);

    private void Start()
    {
        _camera = Camera.main;
        var bedCollider = bed.GetComponent<Collider2D>();
        _bedSize = new Vector2Int(
            Mathf.RoundToInt(bedCollider.bounds.size.x),
            Mathf.RoundToInt(bedCollider.bounds.size.y)
        );

        _backToHomeButton.gameObject.SetActive(false);
        _backToHomeButton.onClick.AddListener(BackToHomeClicked);
        _endGameText.gameObject.SetActive(false);
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
        if (emptySpaceCount ==0)
        {
            Debug.Log("Todos os gatinhos a mimir");
            _backToHomeButton.gameObject.SetActive(true);
            _endGameText.gameObject.SetActive(true);
        }
    }
}
