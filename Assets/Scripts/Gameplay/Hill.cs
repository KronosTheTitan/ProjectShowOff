using System;
using UnityEngine;

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
        [SerializeField] private Transform[] hillSites;
        [SerializeField] private int currentSite;

        private void Update()
        {
            if(lastMoveInSeconds + moveTimeInSeconds > Time.time)
                return;

            currentSite++;

            if (currentSite >= hillSites.Length)
                currentSite = 0;

            transform.position = hillSites[currentSite].position;
            lastMoveInSeconds = Time.time;
        }

        private void OnTriggerStay(Collider other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(player == null) return;
        
            player.ReceiveScore(scorePerTick);
        }
    }
}
