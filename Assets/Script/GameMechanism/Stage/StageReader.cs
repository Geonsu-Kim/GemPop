
using UnityEngine;

public static class StageReader
{
    public static StageInfo LoadStage(int stageNumber)
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Stage/{GetFileName(stageNumber)}");
        if (textAsset == null) return null;
        StageInfo info = JsonUtility.FromJson<StageInfo>(textAsset.text);
        return info;
    }
    static string GetFileName(int stageNumber)
    {
        return string.Format("{0:D4}", stageNumber);
    }
}
