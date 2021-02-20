using System;
using UnityEngine;
using System.Collections;
public static class Action2D
{
    public static IEnumerator MoveTo(Transform mono, Vector3 to, float duration, bool selfRemove = false)
    {
        Vector2 start = mono.transform.position;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime * Time.timeScale;
            mono.position = Vector2.Lerp(start, to, t / duration);
            yield return null;
        }
        mono.transform.position = to;
        if (selfRemove)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            mono.gameObject.SetActive(false);
        }
    }
}