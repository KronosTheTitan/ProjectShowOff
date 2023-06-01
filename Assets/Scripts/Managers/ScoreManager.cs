﻿using System.Collections.Generic;
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

       
        [SerializeField] private TextMeshProUGUI[] playerScores;
        [SerializeField] private GameObject FourWaySplitScreen;

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
        {/*
            if (GameManager.GetInstance().GetPlayers().Length > 2)
            {
                FourWaySplitScreen.SetActive(true);

            }
            else if (GameManager.GetInstance().GetPlayers().Length < 3)
            {
                FourWaySplitScreen.SetActive(false);
            }*/
                

            _scoreTable.Add(player, 0);
            _lastScoreReceived.Add(player, Time.time - scoreIntervalInSeconds);
            //updateScore();
           
        }

        public void RemoveNewPlayer(Player player)
        {/*
            if (GameManager.GetInstance().GetPlayers().Length > 2)
            {
                FourWaySplitScreen.SetActive(true);

            }
            else FourWaySplitScreen.SetActive(false);*/

            _scoreTable.Remove(player);
            _lastScoreReceived.Remove(player);
            //updateScore();

        }

        /*private void updateScore()
        {
            for(int i = 0; i <= _scoreTable.Count-1; i++)
            {
                playerScores[i].text = "Score: " + GetScore(GameManager.GetInstance().GetPlayer(i));
            }
        }*/
    }
}