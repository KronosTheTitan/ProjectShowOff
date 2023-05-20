using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Player))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Player player;

        [SerializeField] private float tongueAttack;

        public void TongueHook()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + transform.forward, transform.forward, out hit);
        }
    }
}
