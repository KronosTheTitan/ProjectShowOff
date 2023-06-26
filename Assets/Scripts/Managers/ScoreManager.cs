using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private readonly Dictionary<Player, int> _scoreTable = new();
        private readonly Dictionary<Player, float> _lastScoreReceived = new();

        [SerializeField] private float scoreIntervalInSeconds;
        [SerializeField] private int scoreNeededForVictory;

        public int GetScore(Player player)
        {
            return _scoreTable[player];
        }

        public void Clear()
        {
            _scoreTable.Clear();
            _lastScoreReceived.Clear();
        }

        public void AddScore(Player player, int amount)
        {
            if(!CanReceiveScore(player))
                return;

            _lastScoreReceived[player] = Time.time;
            _scoreTable[player] += amount;
            
            player.InvokeOnScoreIncrease();

            if (_scoreTable[player] >= scoreNeededForVictory)
            {
                GameManager.GetInstance().HandleVictory(player);
            }
        }

        public bool CanReceiveScore(Player player)
        {
            return Time.time > _lastScoreReceived[player] + scoreIntervalInSeconds;
        }

        public void TakeEmptyTick(Player player)
        {
            if(!CanReceiveScore(player))
                return;
            _lastScoreReceived[player] = Time.time;
                    
            player.InvokeOnScoreContested();
        }

        public void AddNewPlayer(Player player)
        {
            _scoreTable.Add(player, 0);
            _lastScoreReceived.Add(player, Time.time - scoreIntervalInSeconds);
        }

        public void RemoveNewPlayer(Player player)
        {
            _scoreTable.Remove(player);
            _lastScoreReceived.Remove(player);
        }
    }
}