using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private Dictionary<Player, int> _scoreTable = new Dictionary<Player, int>();
        private Dictionary<Player, float> _lastScoreReceived = new Dictionary<Player, float>();

        [SerializeField] private float scoreIntervalInSeconds;
        [SerializeField] private int scoreNeededForVictory;
        
        public int GetScore(Player player)
        {
            return _scoreTable[player];
        }

        public void AddScore(Player player, int amount)
        {
            if(Time.time < _lastScoreReceived[player] + scoreIntervalInSeconds)
                return;
            
            _lastScoreReceived[player] = Time.time;
            _scoreTable[player] += amount;
            
            player.InvokeOnScoreIncrease();

            if (_scoreTable[player] >= scoreNeededForVictory)
            {
                GameManager.GetInstance().HandleVictory(player);
            }
        }

        public void AddNewPlayer(Player player)
        {
            _scoreTable.Add(player, 0);
            _lastScoreReceived.Add(player, Time.time - scoreIntervalInSeconds);
        }
    }
}