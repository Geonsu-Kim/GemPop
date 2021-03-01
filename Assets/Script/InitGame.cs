using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
public class InitGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool _bSuccess) =>
        {
            if (true == _bSuccess)
            {
            }
            else
            {
            }
        });
        StageInfoList.Initialization();

        SceneManager.LoadScene("scLobby");
    }
}
