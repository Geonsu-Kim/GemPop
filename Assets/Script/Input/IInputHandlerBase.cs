using UnityEngine;

public interface IInputHandlerBase
{
    bool isInputDown { get; }
    bool isInputUp { get; }
    Vector2 InputPos { get; }
}

