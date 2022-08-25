using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject StartGamePanel;
    public GameObject GameOverPanel;
    public GameObject WinPanel;
    public GameObject PauseBtn;
    public GameObject ResumeBtn;
    public LevelManager levelManager;
    public ScoreManager scoreManager;
    public PlayerData playerData;

    public string file = "player.txt";

    void Start()
    {
        StartGamePanel.gameObject.transform.DOScale(1, 0.1f);
    }

    public void StartGameButton()
    {
        playerData.TimeOfAttempt++;
        StartGamePanel.gameObject.transform.DOScale(0, 0.25f);
        levelManager.StartGame();
    }

    public void Win()
    {
        Save();
        levelManager.GameEnding();
        StopAllCoroutines();
        WinPanel.gameObject.transform.DOScale(1, 0.25f);
    }
     public void GameOver()
    {
        Save();
        levelManager.GameEnding();
        StopAllCoroutines();
        GameOverPanel.gameObject.transform.DOScale(1, 0.25f);
    }

    public void StartAgainGameButton()
    {
        WinPanel.gameObject.transform.DOScale(0, 0.1f);
        GameOverPanel.gameObject.transform.DOScale(0, 0.1f);
        StartGamePanel.gameObject.transform.DOScale(1, 0.25f);
    }
    public void PauseButton()
    {
        ResumeBtn.SetActive(true);
        PauseBtn.SetActive(false);
        Time.timeScale = 0;
    }
    public void ResumeButton()
    {
        ResumeBtn.SetActive(false);
        PauseBtn.SetActive(true);
        Time.timeScale = 1;
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(playerData);
        WriteToFile(file, json);
    }
    
    public void Load()
    {
        playerData = new PlayerData();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, playerData);
    }
    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
            return "";
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}
