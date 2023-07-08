using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FruitLifter : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    public Rigidbody2D selectedObject;
    Vector3 _offset;
    Vector3 _mousePosition;

    void Update()
    {
        _mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(_mousePosition);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
                _offset = selectedObject.transform.position - _mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
        }
    }

    void FixedUpdate()
    {
        if (selectedObject)
        {
            selectedObject.MovePosition(_mousePosition + _offset);
        }
    }
}