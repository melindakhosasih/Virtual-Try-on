using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitData : MonoBehaviour
{
    void Start(){
        PlayerPrefs.SetInt("watchNum", 1);
    }
}
