using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("勝利必要点"),SerializeField] public float MaxScore = 100;
    [Header("増加量"), SerializeField] private float _increase = 10;
    public float PlayerScore;
    public float EnemyScore;
    public static ScoreManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ScoreReset();
    }
    /// <summary>
    /// スコアを加算
    /// </summary>
    /// <param name="character"></param>
    public void ScoreAddition(PointErements character)
    {

        if(PlayerScore >= MaxScore || EnemyScore >= MaxScore)
        {
            GamaManager.Instance.Finish = true;
        }
        else
        {
            switch (character)
            {
                case PointErements.Player:
                    PlayerScore += _increase;
                    break;
                case PointErements.Enemy:
                    EnemyScore += _increase;
                    break;
            }
        }
        UIManager uIManager = FindObjectOfType<UIManager>();
        if (uIManager != null)
        {
            uIManager.SliderFluctuation(character);
        }
    }
    /// <summary>
    /// スコアの値をリセット
    /// </summary>
    public void ScoreReset()
    {
        PlayerScore = 0;
        EnemyScore = 0;
    }
}
