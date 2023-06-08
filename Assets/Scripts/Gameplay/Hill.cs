using System.Collections.Generic;
using Managers;
using UnityEngine;
using Gameplay.Player;

namespace Gameplay
{
    public class Hill : MonoBehaviour
    {
        /// <summary>
        /// For the speed of each tick check the score interval variable on the player class.
        /// </summary>
        [SerializeField] private int scorePerTick;

        [SerializeField] private float moveTimeInSeconds;
        [SerializeField] private float lastMoveInSeconds;
        [SerializeField] private GameObject[] hillSites;
        [SerializeField] private int currentSite;

        [SerializeField] private List<Player.Player> playersOnHill;

        private const int MaximumPlayersOnHill = 1;
        private const int FirstPlayerInList = 0;

        private void Update()
        {
            HandleScore();
            
            if (lastMoveInSeconds + moveTimeInSeconds > Time.time)
                return;

            hillSites[currentSite].SetActive(false);
            currentSite++;

            if (currentSite >= hillSites.Length)
                currentSite = 0;
            
            transform.position = hillSites[currentSite].transform.position;
            hillSites[currentSite].SetActive(true);

            lastMoveInSeconds = Time.time;
        }

        void HandleScore()
        {
            if(playersOnHill.Count == 0)
                return;
            
            if(playersOnHill.Count > MaximumPlayersOnHill)
                return;

            GameManager.GetInstance().GetScoreManager().AddScore(playersOnHill[FirstPlayerInList], scorePerTick);
            
            playersOnHill.Clear();
        }

        private void OnTriggerStay(Collider other)
        {
            Player.Player player = other.gameObject.GetComponent<Player.Player>();
            if(player == null) return;
            
            playersOnHill.Add(player);
        }
    }
}