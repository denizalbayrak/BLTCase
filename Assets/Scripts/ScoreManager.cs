using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ScoreManager : MonoBehaviour
{
    public GameManager gameManager;
    public LevelManager levelManager;
    public PlayerData playerData;
    public int Score;
    public int SphereCoefficient = 1;
    public int CapsuleCoefficient = 2;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI LevelUpText;
    public bool LevelUpCheck;
    public int ScoreCheck = 100;


    void LevelUp()
    {
        StartCoroutine(LevelUpAnimation());
        SphereCoefficient = levelManager.Level + 1 * 10;
        CapsuleCoefficient += levelManager.Level + 1 * 10;
        levelManager.LevelUp();
    }
    IEnumerator LevelUpAnimation()
    {
        LevelUpText.gameObject.transform.DOScale(2, 1f);
        yield return new WaitForSeconds(1f);
        LevelUpText.gameObject.transform.DOScale(0, 1f);
    }

    public void StartGame()
    {
        playerData.AmountofPushedObjecst = 0;
        SphereCoefficient = 1;
        CapsuleCoefficient = 2;
        ScoreCheck = 100;
        scoreText.text = "0";
        Score = 0;
        scoreText.text = Score.ToString();
    }

    public void SphereScore()
    {
        playerData.AmountofPushedObjecst++;
        Score += SphereCoefficient;
        scoreText.text = Score.ToString();
        playerData.Score = Score;
        if (Score >= 400)
        {
            levelManager.isLevelUp = true;
      
            gameManager.Win();
        }
        else if (Score >= ScoreCheck)
        {
            ScoreCheck += 100;
            LevelUp();
        }
    }

    public void CapsuleScore()
    {
        playerData.AmountofPushedObjecst++;
        Score += CapsuleCoefficient;
        playerData.Score = Score;
        scoreText.text = Score.ToString();
        if (Score >= 400)
        {
            levelManager.isLevelUp = true;
         
            gameManager.Win();
        }
        else if (Score >= ScoreCheck)
        {
            ScoreCheck += 100;
            LevelUp();
        }
    }

    public void LastEatedScore(string LastEated)
    {
        playerData.AmountofPushedObjecst++;
        if (LastEated == "Sphere")
        {
            Score -= SphereCoefficient * 2;
        }
        else
        {
            Score -= CapsuleCoefficient * 2;
        }
        scoreText.text = Score.ToString();
    }
}
