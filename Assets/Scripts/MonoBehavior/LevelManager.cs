using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject RestartText;
    public GameObject NewRecordText;
    public Text TimeText;
    public Text TimeWhenPlaying;
    public Slider GlobalVolume;
    public Toggle Music;
    public Toggle ScreenShake;
    public GameObject OptionsMenu;

    public bool levelComplete = false;
    bool death = false;

    float previousTimeScale;
    float currentTime;

    private void Awake()
    {
        currentTime = Time.time;
        Music.isOn = !AudioManager.musicMuted;
        ScreenShake.isOn = CameraShake.ScreenShake;
    }
    private void Update()
    {
        if(TimeWhenPlaying && !levelComplete && !death)
        {
            TimeWhenPlaying.text = FloatToTime(CompletionTime());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (levelComplete && Input.GetKeyDown(PlayerController.actionKey))
        {
            NextLevel();
        }
        if(death == true)
        {
            RestartText.SetActive(true);
            if(Input.GetKeyDown(PlayerController.actionKey))
            {
                RestartLevel();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            ToggleOptionsMenu();
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.sceneCountInBuildSettings > index + 1)
        {
            SceneManager.LoadScene(index + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    public void LevelCompleted()
    {
        if(!levelComplete)
        {
            levelComplete = true;
            AudioManager.PlaySound("Finish", 0.5f);
            int index = SceneManager.GetActiveScene().buildIndex;

            if (PlayerPrefs.GetInt("UnlockedLevels") <= index && index != 20)
            {
                PlayerPrefs.SetInt("UnlockedLevels", index + 1);
            }


            float oldTime = PlayerPrefs.GetFloat(index.ToString());
            TimeText.text = FloatToTime(CompletionTime()) + " / " + FloatToTime(oldTime);


            if (oldTime == 0 || oldTime > CompletionTime())
            {
                NewRecordText.SetActive(true);
                PlayerPrefs.SetFloat(index.ToString(), CompletionTime());
            }
        }
    }

    public static string FloatToTime(float time)
    {
        return string.Format("{0:00}:{1:00}:{2:00}", (int)time / 60, (int)time % 60, time/0.01%100);
    }

    public void Death()
    {
        if(!levelComplete)
        {
            AudioManager.PlaySound("Death", 0.8f);
            death = true;
        }
    }

    float CompletionTime()
    {
        return Time.time - currentTime;
    }

    public void SetGlobalVolume()
    {
        AudioListener.volume = GlobalVolume.value;
    }

    public void MuteMusic()
    {
        AudioManager.SetMusic(!Music.isOn);
    }

    public void ToggleScreenShake()
    {
        CameraShake.ScreenShake = ScreenShake.isOn;
    }

    public void ToggleOptionsMenu()
    {
        OptionsMenu.SetActive(!OptionsMenu.activeSelf);
        if (OptionsMenu.activeSelf)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = previousTimeScale;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonPressedSound()
    {
        AudioManager.PlaySound("Pressed",0.6f);
    }
}
