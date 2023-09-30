using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject bed;
    [SerializeField] private LayerMask checkLayer;

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
        }
    }
}
