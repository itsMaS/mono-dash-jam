using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;

    public TextMeshProUGUI timer;
    [SerializeField] private int minutesAllowed;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            minutesAllowed = Config();
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        timeLeft = new TimeSpan(0, minutesAllowed, 0);
        timer.SetText(timeLeft.ToString(@"mm\:ss"));
    }
    public void StartTimer()
    {
        StopAllCoroutines();
        StartCoroutine(Timer());
    }
    TimeSpan timeLeft;
    IEnumerator Timer()
    {
        DateTime target = DateTime.Now.AddMinutes(minutesAllowed);
        TimeSpan danger = new TimeSpan(0, 1, 0);
        while((timeLeft=target- DateTime.Now) > TimeSpan.Zero)
        {
            timer.SetText(timeLeft.ToString(@"mm\:ss"));
            timer.color = timeLeft < danger ? Color.red : Color.white;
            yield return null;
        }
        StopTimer();
        Register();
    }

    private int lastLevel;
    private TimeSpan time;
    private string email;

    public void StopTimer()
    {
        lastLevel = GameManager.currentLevel;
        time = timeLeft;
    }
    public void Register()
    {
        SceneManager.LoadScene("Registration");
        StopAllCoroutines();
    }
    public void ResetGame(string email = "")
    {
        this.email = email;
        WriteToFile();
        timeLeft = new TimeSpan(0, minutesAllowed, 0);
        timer.SetText(timeLeft.ToString(@"mm\:ss"));
        SceneManager.LoadScene(0);
    }
    public void WriteToFile()
    {
        FileStream fs = new FileStream($"{Application.dataPath}/Results.txt", FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);
        sw.BaseStream.Seek(0, SeekOrigin.End);
        sw.WriteLine($"{email} | {lastLevel} | {time.ToString(@"mm\:ss")}");

        sw.Flush();
        sw.Close();
    }
    private static int Config()
    {
        if (File.Exists($"{Application.dataPath}/Config.txt"))
        {
            return int.Parse(File.ReadAllText($"{Application.dataPath}/Config.txt"));
        }
        else
        {
            return 20;
        }
    }
}
