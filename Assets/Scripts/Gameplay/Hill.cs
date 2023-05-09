using UnityEngine;

namespace Gameplay
{
    public class Hill : MonoBehaviour
    {
        /// <summary>
        /// For the speed of each tick check the score interval variable on the player class.
        /// </summary>
        [SerializeField] private int scorePerTick;
        private void OnTriggerStay(Collider other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(player == null) return;
        
            player.ReceiveScore(scorePerTick);
        }
    }
}
