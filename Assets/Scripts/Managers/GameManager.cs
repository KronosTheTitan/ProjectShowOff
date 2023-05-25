using System;
using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;

        private void Awake()
        {
            if (_instance != null)
                Destroy(this);
            else
                _instance = this;
        }

        public delegate void GameOverDelegate(Player winner);

        public event GameOverDelegate OnGameOver;

        public static GameManager GetInstance()
        {
            return _instance;
        }
        #endregion

        [SerializeField] private List<Player> players;
        [SerializeField] private ScoreManager scoreManager;

        public Player[] GetPlayers()
        {
            return players.ToArray();
        }

        public ScoreManager GetScoreManager()
        {
            return scoreManager;
        }

        public void AddPlayer(Player newPlayer)
        {
            players.Add(newPlayer);
            scoreManager.AddNewPlayer(newPlayer);
        }
        
        public void HandleVictory(Player winner)
        {
            OnGameOver?.Invoke(winner);
        }
    }
}