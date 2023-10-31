using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager> 
{
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text enemyText;

    private void Start()
    {
        UpdateScore(0);
        UpdateTime(0);
        UpdateEnemyCount(0);
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = "Scoretum: " + _score.ToString(); 
    }

    public void UpdateTime(float _time)
    {
        timerText.text = _time.ToString("F2");  
    }

    public void UpdateEnemyCount(int _count)
    {
        enemyText.text = "Enemies: " + _count.ToString();
    }
}
