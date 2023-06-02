using System.Collections.Generic;
using Gameplay;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        #region Variables

        [SerializeField] private List<Player> players;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private PlayerUIManager playerUIManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private Hill hill;

        [SerializeField] private float gameStartTimeInSeconds;
        [SerializeField] private float gameDurationInSeconds;

        #endregion

        #region UnityEventMethods

        private void Update()
        {
            UpdateGameTime();
        }

        #endregion
        
        #region GetMethods
        public Player[] GetPlayers()
        {
            return players.ToArray();
        }

        public Player GetPlayer(int index)
        {
            return players[index];
        }
        
        public ScoreManager GetScoreManager()
        {
            return scoreManager;
        }
        
        public PlayerUIManager GetPlayerUIManager()
        {
            return playerUIManager;
        }
        
        public SpawnManager GetSpawnManager()
        {
            return spawnManager;
        }

        public Hill GetHill()
        {
            return hill;
        }

        #endregion

        private void UpdateGameTime()
        {
            if(gameStartTimeInSeconds + gameDurationInSeconds > Time.time)
                return;

            Player highestScoringPlayer = null;
            int highestScore = int.MinValue;

            foreach (Player player in players)
            {
                if(highestScore > scoreManager.GetScore(player))
                    continue;

                highestScore = scoreManager.GetScore(player);
                highestScoringPlayer = player;
            }
            
            HandleVictory(highestScoringPlayer);
        }
        
        public void ReplayGame()
        {
            //reloads the scene
            _instance = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            gameStartTimeInSeconds = Time.time;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
            scoreManager.AddNewPlayer(player);
            spawnManager.AssignAvailableSpawn(player);
            uiManager.UpdateSplitScreen();
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            scoreManager.AddNewPlayer(player);
            playerUIManager.RemovePlayer(player);
            spawnManager.RemovePlayer(player);
            uiManager.UpdateSplitScreen();
        }

        public void HandleVictory(Player winner)
        {
            OnGameOver?.Invoke(winner);
        }
    }
}