using System;
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

        private void Start()
        {
            GameManager.GetInstance().GetPlayerUIManager().AddPlayerUI(this, player);
            player.OnScoreIncrease += UpdateScore;
        }

        private void UpdateScore()
        {
            score.text = GameManager.GetInstance().GetScoreManager().GetScore(player).ToString();
        }
    }
}
