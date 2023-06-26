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
        [SerializeField] private RectTransform scoreTickBorder;
        [SerializeField] private RectTransform scoreContestedBorder;
        [SerializeField] private float tickTime = 0.3f ;

        private void Start()
        {
            player.OnScoreIncrease += UpdateScore;
            player.OnScoreContested += ScoreContested;
        }

        private void UpdateScore()
        {
            score.text = GameManager.GetInstance().GetScoreManager().GetScore(player).ToString();
            StartCoroutine(ScoreTick(tickTime));
        }

        private void ScoreContested()
        {
            StartCoroutine(ScoreContestedTick(tickTime));
        }

        IEnumerator ScoreTick(float tickTime)
        {
            scoreTickBorder.anchorMin = GameManager.GetInstance().GetSplitScreenManager()
                .GetRectForPlayerIndex(GameManager.GetInstance().GetPlayerIndex(player)).min;
            scoreTickBorder.anchorMax = GameManager.GetInstance().GetSplitScreenManager()
                .GetRectForPlayerIndex(GameManager.GetInstance().GetPlayerIndex(player)).max;
            scoreTickBorder.gameObject.SetActive(true);
            yield return new WaitForSeconds(tickTime);
            scoreTickBorder.gameObject.SetActive(false);
        }

        IEnumerator ScoreContestedTick(float tickTime)
        {
            scoreContestedBorder.anchorMin = GameManager.GetInstance().GetSplitScreenManager()
                .GetRectForPlayerIndex(GameManager.GetInstance().GetPlayerIndex(player)).min;
            scoreContestedBorder.anchorMax = GameManager.GetInstance().GetSplitScreenManager()
                .GetRectForPlayerIndex(GameManager.GetInstance().GetPlayerIndex(player)).max;
            scoreContestedBorder.gameObject.SetActive(true);
            yield return new WaitForSeconds(tickTime);
            scoreContestedBorder.gameObject.SetActive(false);
        }
    }
}
