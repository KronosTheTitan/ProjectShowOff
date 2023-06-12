using System;
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

        [SerializeField] private Player[] players;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private SplitScreenManager splitScreenManager;
        [SerializeField] private ControllerManager controllerManager;
        [SerializeField] private Hill hill;

        [SerializeField] private float gameStartTimeInSeconds;
        [SerializeField] private float gameDurationInSeconds;

        #endregion

        #region UnityEventMethods

        private void Start()
        {
            foreach (Player player in players)
            {
                AddPlayer(player);
            }
        }

        private void Update()
        {
            UpdateGameTime();
        }

        #endregion
        
        #region GetMethods
        public Player[] GetPlayers()
        {
            //Yes I am fully aware this code is a little odd.
            //I wrote it this way to avoid the list of players being edited from
            //outside this class. And just returning the array directly makes this possible.
            Player[] output = new Player[players.Length];
            
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = players[i];
            }

            return output;
        }

        public Player GetPlayer(int index)
        {
            return players[index];
        }
        
        public ScoreManager GetScoreManager()
        {
            return scoreManager;
        }
        
        public SplitScreenManager GetSplitScreenManager()
        {
            return splitScreenManager;
        }
        
        public ControllerManager GetControllerManager()
        {
            return controllerManager;
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
            scoreManager.AddNewPlayer(player);
            uiManager.UpdateSplitScreen();
            controllerManager.AddPlayerToTable(player);
        }

        public void RemovePlayer(Player player)
        {
            uiManager.UpdateSplitScreen();
        }

        public void HandleVictory(Player winner)
        {
            OnGameOver?.Invoke(winner);
        }
    }
}