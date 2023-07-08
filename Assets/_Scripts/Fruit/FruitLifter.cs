using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FruitLifter : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private InputManager inputManager;
    
    public Rigidbody2D selectedObject;
    Vector3 _offset;
    Vector3 _mousePosition;

    private void Start()
    {
        inputManager = InputManager.instance;
    }

    void Update()
    {
        _mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(inputManager.horizontalLookAxis, inputManager.verticalLookAxis));
        
        Debug.Log(_mousePosition);
        
        if (inputManager.selectHeld)
        {
            Collider2D targetObject = Physics2D.OverlapPoint(_mousePosition);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
                _offset = selectedObject.transform.position - _mousePosition;
            }
        }

        if (!inputManager.selectHeld && selectedObject)
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