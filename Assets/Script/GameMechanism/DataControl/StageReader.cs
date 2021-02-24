using System.IO;
using System;
using UnityEngine;

public static class StageReader
{
    public static StageInfo LoadStage(int stageNumber)
    {
            StageInfo info=null;
        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Path.Combine(Application.streamingAssetsPath, GetFileName(stageNumber) + ".json");
            if (path == null) return null;
            WWW reader = new WWW(path);
            while (!reader.isDone) { }
            string str = reader.text;
            info=JsonUtility.FromJson<StageInfo>(str);
        }
        else
        {
            TextAsset textAsset = Resources.Load<TextAsset>($"Stage/{GetFileName(stageNumber)}");
            if (textAsset == null) return null;
            info = JsonUtility.FromJson<StageInfo>(textAsset.text);
        }
        return info;
    }
    static string GetFileName(int stageNumber)
    {
        return string.Format("{0:D2}", stageNumber);
    }
}
