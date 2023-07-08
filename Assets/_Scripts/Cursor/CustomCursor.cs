using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D mouseCursor;
    public Texture2D clickCursor;
    public InputManager _inputManager;

    Vector2 hotSpot = new Vector2(8, 8);
    CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        _inputManager = InputManager.instance;

        Cursor.SetCursor(mouseCursor, hotSpot, cursorMode);
    }

    private void Update()
    {
        var clickHeld = _inputManager.selectHeld;
        if(clickHeld)
        {
            Cursor.SetCursor(clickCursor, hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(mouseCursor, hotSpot, cursorMode);
        }
    }
}
