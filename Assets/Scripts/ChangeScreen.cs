using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScreen : MonoBehaviour
{
    public int watchNum_ = 0;

    public void MoveToScene(string sceneName)
    {
        if(watchNum_ != 0){
            PlayerPrefs.SetInt("watchNum", watchNum_);
        }
        SceneManager.LoadScene(sceneName);
    }
}
