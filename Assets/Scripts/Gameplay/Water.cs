using UnityEngine;

namespace Gameplay
{
    public class Water : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Player.Player player = other.GetComponent<Player.Player>();
            
            if(player == null)
                return;
            
            player.Respawn();
        }
    }
}