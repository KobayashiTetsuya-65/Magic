using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("�����K�v�_"),SerializeField] public float MaxScore = 100;
    [Header("������"), SerializeField] private float _increase = 10;
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
    /// �X�R�A�����Z
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
    /// �X�R�A�̒l�����Z�b�g
    /// </summary>
    public void ScoreReset()
    {
        PlayerScore = 0;
        EnemyScore = 0;
    }
}
