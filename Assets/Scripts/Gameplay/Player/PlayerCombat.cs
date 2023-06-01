using UnityEngine;

namespace Gameplay.Player
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
            if(player.GetInKnockback())
                return;
            
            if(!player.IsGrounded())
                return;

            if (player.GetInput().actions["Kick"].WasPerformedThisFrame())
            {
                OnKick?.Invoke();
                Invoke("FrogKick",kickWindup);
            }
            
            if (player.GetInput().actions["VocalSack"].WasPerformedThisFrame())
            {
                OnVocalSack?.Invoke();
                Invoke("VocalSack",vocalSackWindup);
            }
            
            if (player.GetInput().actions["TonguePull"].WasPerformedThisFrame())
            {
                OnTonguePull?.Invoke();
                Invoke("TongueHook",tongueWindup);
            }
        }

        [Header("Tongue Hook")]
        [SerializeField] private float tongueRange = 15;
        [SerializeField] private float tongueStrength;
        [SerializeField] private float tonguePullDuration;
        [SerializeField] private float tongueWindup;
        /// <summary>
        /// the tongue hook ability, it a targeted enemy in.
        /// </summary>
        public void TongueHook()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + transform.forward, transform.forward, out hit, tongueRange);
                    
            if (hit.collider == null)
                return;

            IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if(damageable == null)
                return;

            damageable.TakeDamage(-(transform.position - hit.collider.transform.position), player, tongueStrength,
                tonguePullDuration);
        }

        [Header("Vocal Sack")]
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
        }

        [Header("Frog Kick")]
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
        }
    }
}