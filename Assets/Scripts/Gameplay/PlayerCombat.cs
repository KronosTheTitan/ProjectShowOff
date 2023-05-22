using System;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Player))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Player player;

        private void Update()
        {
            if (player.GetGamepad().X.justReleased)
            {
                
            }
            
            if (player.GetGamepad().Y.justReleased)
            {
                TongueHook();
            }

            if (player.GetGamepad().B.justReleased)
            {
                VocalSack();
            }
        }

        public void TongueHook()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + transform.forward, transform.forward, out hit);
        }


        [SerializeField] private int horizontalInterval = 15; 
        [SerializeField] private int verticalInterval = 15;
        [SerializeField] private float vocalSackRange = 1;
        public void VocalSack()
        {
            for (int x = -(horizontalInterval * 2); x < horizontalInterval * 2; x += horizontalInterval)
            {
                Quaternion horizontal = Quaternion.AngleAxis(x , transform.up);
                
                for (int y = -(verticalInterval * 2); y < verticalInterval * 2; y += verticalInterval)
                {
                    Quaternion vertical = Quaternion.AngleAxis(y,transform.right);
                    Quaternion direction = horizontal * vertical;
                    
                    RaycastHit hit;
                    Physics.Raycast(transform.position + transform.forward / 2, direction * transform.forward, out hit, vocalSackRange);
                    
                    if (hit.collider == null)
                    {
                        Debug.Log("No collider found");
                        continue;
                    }

                    IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                    if(damageable == null)
                        continue;

                    damageable.TakeDamage(transform.position - hit.collider.transform.position, player);
                    Debug.Log("hit someone");
                }
            }
        }
    }
}
