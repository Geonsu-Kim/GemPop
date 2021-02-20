
using UnityEngine;

public class TouchHandler : IInputHandlerBase
{
    public bool isInputDown => Input.GetTouch(0).phase == TouchPhase.Began;

    public bool isInputUp => Input.GetTouch(0).phase == TouchPhase.Ended;

    public Vector2 InputPos => Input.GetTouch(0).position;
}
