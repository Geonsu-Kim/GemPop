
using UnityEngine;

public class MouseHandler : IInputHandlerBase
{
    public bool isInputDown => Input.GetMouseButtonDown(0);

    public bool isInputUp => Input.GetMouseButtonUp(0);

    public Vector2 InputPos => Input.mousePosition;
}

