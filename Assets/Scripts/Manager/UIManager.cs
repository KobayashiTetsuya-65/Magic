using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("スコアゲージ")]
    [SerializeField] private Slider _playerSlider;
    [SerializeField] private Slider _enemySlider;
    
    private void Start()
    {

    }
    public void SliderFluctuation(PointErements character)
    {
        switch (character)
        {
            case PointErements.Player:
                _playerSlider.value = ScoreManager.Instance.PlayerScore / ScoreManager.Instance.MaxScore;
                break;
            case PointErements.Enemy:
                _enemySlider.value = ScoreManager.Instance.EnemyScore / ScoreManager.Instance.MaxScore;
                break;
        }
    }
}
