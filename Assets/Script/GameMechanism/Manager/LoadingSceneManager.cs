using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadingSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static string nextScene;
    public Image loadingBar;
    public TextMeshProUGUI percentage;
    void Start()
    {
        StartCoroutine(SceneLoading());
    }

    public static void LoadScene(string name)
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
        nextScene = name;
        SceneManager.LoadScene("scLoading");
    }
    IEnumerator SceneLoading()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            loadingBar.fillAmount = op.progress;
            percentage.text = ((int)(op.progress * 100)).ToString()+"%";
            if (op.progress >= 0.9f)
            {
                loadingBar.fillAmount = 1f;
                percentage.text = "100%";
                op.allowSceneActivation = true;
            }

        }
    }
}
