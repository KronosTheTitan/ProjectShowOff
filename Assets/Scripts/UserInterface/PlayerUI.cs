using System;
using System.Collections;
using Gameplay.Player;
using Managers;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    [RequireComponent(typeof(RectTransform))]
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private TMP_Text score;
        [SerializeField] private GameObject ScoreTickBorder;
        [SerializeField] private float tickTime = 0.3f ;

        private void Start()
        {
            player.OnScoreIncrease += UpdateScore;
            
        }

        private void UpdateScore()
        {
            score.text = GameManager.GetInstance().GetScoreManager().GetScore(player).ToString();
            StartCoroutine(ScoreTick(tickTime));
        }

        IEnumerator ScoreTick(float tickTime)
        {
            ScoreTickBorder.gameObject.SetActive(true);
            yield return new WaitForSeconds(tickTime);
            ScoreTickBorder.gameObject.SetActive(false);
        }
    }
}
