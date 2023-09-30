using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private Vector3 _anchor;
    private Camera _camera;
    private Vector3 _positionBeforeMove;
    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnMouseDrag()
    {
        var worldPosition = GetMousePosition();
        var position = worldPosition - _anchor;

        transform.position = position;
    }

    private Vector3 GetMousePosition()
    {
        var position = Input.mousePosition;
        var worldPosition = _camera.ScreenToWorldPoint(position);
        worldPosition.z = 0;
        return worldPosition;
    }

    private void OnMouseDown()
    {
        _positionBeforeMove = transform.position;
        var position = GetMousePosition();
        _anchor = position - transform.position;
    }

    private void OnMouseUp()
    {
        //check if final position is valid
        var hits = Physics2D.RaycastAll(transform.position + new Vector3(0.5f, 0.5f, 0f), Vector2.zero);
        if (hits.Length > 1) 
        {
            foreach (var hit in hits)
            {
                Debug.Log("Hit " + hit.collider.name);
            }

            transform.position = _positionBeforeMove;
        }
        
        //move piece to near int position
        var position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        transform.position = position;
    }

    private void Update()
    {
        Debug.DrawLine(Vector3.zero, transform.position);
    }
}
