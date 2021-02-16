using UnityEngine;
public enum Swipe
{
    NA=-1,RIGHT,UP,LEFT,DOWN
}
public static class TouchEvaluator
{
    public static Swipe EvalSwipeDir(Vector2 start,Vector2 end)
    {
        float angle = EvalDragAnle(start, end);
        if (angle < 0) return Swipe.NA;
        int swipe = (((int)angle + 45) % 360) / 90;
        return (Swipe)swipe;
    }
    static float EvalDragAnle(Vector2 start,Vector2 end)
    {
        Vector2 dragDir = end = start;
        if (dragDir.magnitude <= 0.2f) return -1f;
        float aimAngle = Mathf.Atan2(dragDir.y, dragDir.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }
        return aimAngle * Mathf.Rad2Deg;
    }
}
