using UnityEngine;
public class InputManager
{
    private Transform mParent;
    private IInputHandlerBase mHandler;
    public InputManager(Transform parent)
    {
        mParent = parent;
        if (Application.platform == RuntimePlatform.WindowsEditor)
            mHandler = new MouseHandler();
        else
            mHandler = new TouchHandler();
    }
    public bool isDown => mHandler.isInputDown;
    public bool isUp => mHandler.isInputUp;
    public Vector2 Pos => mHandler.InputPos;
    public Vector2 PosToBoard => TouchToBoard(mHandler.InputPos);
    Vector2 TouchToBoard(Vector3 pos)
    {
        Vector3 wolrdPos = Camera.main.ScreenToWorldPoint(pos);
        Vector3 parentLocal = mParent.transform.InverseTransformPoint(wolrdPos);
        return parentLocal;
    }
    public Swipe EvalSwipeDir(Vector2 start,Vector2 end)
    {
        return TouchEvaluator.EvalSwipeDir(start, end);
    }
}
