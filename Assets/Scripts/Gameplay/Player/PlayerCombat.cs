using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Player))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Player player;
        
        public delegate void CombatDelegate();

        public event CombatDelegate OnKick;
        public event CombatDelegate OnVocalSack;
        public event CombatDelegate OnTonguePull;

        private void Update()
        {
            if (player.GetInput().actions["Kick"].WasPerformedThisFrame())
            {
                Invoke("FrogKick",kickWindup);
                Debug.Log("Kick");
            }
            
            if (player.GetInput().actions["VocalSack"].WasPerformedThisFrame())
            {
                Invoke("VocalSack",vocalSackWindup);
                Debug.Log("Vocal sack");
            }
            
            if (player.GetInput().actions["TonguePull"].WasPerformedThisFrame())
            {
                Invoke("TongueHook",tongueWindup);
                Debug.Log("Tongue pull");
            }
        }

        [SerializeField] private float tongueRange = 15;
        [SerializeField] private float tongueStrength;
        [SerializeField] private float tonguePullDuration;
        [SerializeField] private float tongueWindup;
        public void TongueHook()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + transform.forward, transform.forward, out hit, tongueRange);
                    
            if (hit.collider == null)
            {
                return;
            }

            IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if(damageable == null)
                return;

            damageable.TakeDamage(-(transform.position - hit.collider.transform.position), player, tongueStrength,
                tonguePullDuration);
            Debug.Log("hit someone");
            
            OnTonguePull?.Invoke();
        }


        [SerializeField] private int horizontalInterval = 15; 
        [SerializeField] private int verticalInterval = 15;
        [SerializeField] private float vocalSackRange = 1;
        [SerializeField] private float vocalSackStrength = 1;
        [SerializeField] private float vocalSackKnockbackDuration = 1;
        [SerializeField] private float vocalSackWindup;
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
                        continue;

                    IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                    if(damageable == null)
                        continue;

                    damageable.TakeDamage(transform.position - hit.collider.transform.position, player,
                        vocalSackStrength, vocalSackKnockbackDuration);
                }
            }
            
            OnVocalSack?.Invoke();
        }

        [SerializeField] private float kickRange = 5;
        [SerializeField] private float kickStrength = 1;
        [SerializeField] private float kickDamageDuration = 1;
        [SerializeField] private float kickWindup;
        public void FrogKick()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + transform.forward, transform.forward, out hit, kickRange);
                    
            if (hit.collider == null)
                return;

            IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if(damageable == null)
                return;

            damageable.TakeDamage(transform.position - hit.collider.transform.position, player, kickStrength,
                kickDamageDuration);
            Debug.Log("hit someone");
            
            OnKick?.Invoke();
        }
    }
}