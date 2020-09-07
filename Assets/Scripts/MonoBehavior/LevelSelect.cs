using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public Dropdown levels;
    public Text HighScore;

    private void Awake()
    {
        levels.ClearOptions();
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels");
        if(unlockedLevels == 0)
        {
            PlayerPrefs.SetInt("UnlockedLevels",1);
            unlockedLevels = 1;
        }
        List<string> optionsString = new List<string>();
        for (int i = 1; i <= unlockedLevels; i++)
        {
            optionsString.Add("Level "+ i);
        }
        levels.AddOptions(optionsString);
        levels.value = unlockedLevels-1;
        SelectLevel();
    }

    public void Play()
    {
        SceneManager.LoadScene(levels.value+1);
    }

    public void SelectLevel()
    {
        float time = PlayerPrefs.GetFloat((levels.value+1).ToString());
        if(time > 0)
        {
            HighScore.text = "Highscore : " + LevelManager.FloatToTime(time);
        }
        else
        {
            HighScore.text = "Highscore : not set";
        }
    }
}
