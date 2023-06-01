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
        [SerializeField] private Hill hill;

        [SerializeField] private float gameStartTimeInSeconds;
        [SerializeField] private float gameDurationInSeconds;

        #endregion

        #region UnityEventMethods
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

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
            players.Clear();
            scoreManager.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            gameStartTimeInSeconds = Time.time;
        }

        public void AddPlayer(Player newPlayer)
        {
            players.Add(newPlayer);
            scoreManager.AddNewPlayer(newPlayer);
            uiManager.UpdateSplitScreen();
        }

        public void RemovePlayer(Player thisPlayer)
        {
            players.Remove(thisPlayer);
            scoreManager.AddNewPlayer(thisPlayer);
            playerUIManager.RemovePlayer(thisPlayer);
            uiManager.UpdateSplitScreen();
        }

        public void HandleVictory(Player winner)
        {
            OnGameOver?.Invoke(winner);
        }
    }
}