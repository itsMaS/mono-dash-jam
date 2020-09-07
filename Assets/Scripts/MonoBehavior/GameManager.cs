using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static bool exists = false;
    static int currentLevel = 0;

    private void Awake()
    {
        if (exists)
        {
            Destroy(this);
        }
        else
        {
            exists = true;
            DontDestroyOnLoad(this);
            AudioManager.PlayMusic("Music",0.05f);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F9))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
