using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Player player;

        [SerializeField] private float lastTonguePull;
        [SerializeField] private float lastVocalSack;
        [SerializeField] private float tonguePullCooldownSeconds;
        [SerializeField] private float vocalSackCooldownSeconds;

        [SerializeField] private GameObject tonguePosition;
        [SerializeField] private GameObject tongueBasePosition;
        [SerializeField] private float tongueSpeed = 5;
        [SerializeField] private MultiPositionConstraint tongueSettings;


        
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

            if (player.GetController().GetKickButton())
            {
                OnKick?.Invoke();
                Invoke("FrogKick",kickWindup);
            }
            
            if (player.GetController().GetVocalSackButton())
            {
                OnVocalSack?.Invoke();
                Invoke("VocalSack",vocalSackWindup);
            }
            
            if (player.GetController().GetTonguePullButton())
            {
                OnTonguePull?.Invoke();
                Invoke("TongueHook",tongueWindup);
            }
        }

        [Header("Tongue Hook")]
        [SerializeField] private int horizontalRaysTongue = 6; 
        [SerializeField] private int horizontalArcTongue = 90; 
        [SerializeField] private int verticalRaysTongue = 6;
        [SerializeField] private int verticalArcTongue = 45;
        [SerializeField] private float tongueRange = 15;
        [SerializeField] private float tongueStrength;
        [SerializeField] private float tonguePullDuration;
        [SerializeField] private float tongueWindup;
        /// <summary>
        /// the tongue hook ability, it a targeted enemy in.
        /// </summary>
        public void TongueHook()
        {
            if(RemainingTonguePullCooldown() > 0)
                return;

            lastTonguePull = Time.time;

            for (float x = -(horizontalArc / 2); x < horizontalArcTongue/2; x += horizontalArcTongue/horizontalRaysTongue)
            {
                Quaternion horizontal = Quaternion.AngleAxis(x , transform.up);
                
                for (float y = -(verticalArc / 2); y < verticalArcTongue / 2; y += verticalArc/verticalRaysTongue)
                {
                    Quaternion vertical = Quaternion.AngleAxis(y,transform.right);
                    Quaternion direction = horizontal * vertical;
                    
                    RaycastHit hit;
                    Physics.Raycast(transform.position + transform.forward / 2, direction * transform.forward, out hit, tongueRange);
                    
                    if (hit.collider == null)
                        continue;

                    IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                    if(damageable == null)
                        continue;

                    damageable.TakeDamage(-(transform.position - hit.collider.transform.position), player, tongueStrength,
                        tonguePullDuration);

                    Debug.Log("Player position: " + transform.position + " hit position: " + hit.collider.transform.position + " combined: " + (gameObject.transform.position - hit.collider.transform.position));
                    doTongueAnimation(hit.collider.transform.position);

                    return;
                }
            }

            doTongueAnimation(tongueBasePosition.transform.position);

        }

        [SerializeField] private float currentTongueTime = 1;

        private void doTongueAnimation(Vector3 position)
        {
            tonguePosition.transform.position = position;
            currentTongueTime = 1;
            StartCoroutine(TongueAnimation());
        }

        private IEnumerator TongueAnimation()
        {
            //1 to -1 absolute do -1 absolute
            while (currentTongueTime > -1)
            {
                currentTongueTime -= Time.deltaTime * tongueSpeed;
                if (currentTongueTime <= -1)
                {
                    currentTongueTime = -1;
                }
                tongueSettings.weight = Mathf.Abs(Mathf.Abs(currentTongueTime) - 1);
                yield return new WaitForSeconds(0.01f);
                
            }
        }


        public float RemainingTonguePullCooldown()
        {
            return lastTonguePull + tonguePullCooldownSeconds - Time.time;
        }

        [Header("Vocal Sack")]
        [SerializeField] private int horizontalRays = 6; 
        [SerializeField] private int horizontalArc = 90; 
        [SerializeField] private int verticalRays = 6;
        [SerializeField] private int verticalArc = 45;
        [SerializeField] private float vocalSackRange = 1;
        [SerializeField] private float vocalSackStrength = 1;
        [SerializeField] private float vocalSackKnockbackDuration = 1;
        [SerializeField] private float vocalSackWindup;

        public float RemainingVocalSackCooldown()
        {
            return lastVocalSack + vocalSackCooldownSeconds - Time.time;
        }
        public void VocalSack()
        {
            if(RemainingVocalSackCooldown() > 0)
                return;
            lastVocalSack = Time.time;

            List<IDamageable> previousHits = new List<IDamageable>();

            for (float x = -(horizontalArc / 2); x < horizontalArc/2; x += horizontalArc/horizontalRays)
            {
                Quaternion horizontal = Quaternion.AngleAxis(x , transform.up);
                
                for (float y = -(verticalArc / 2); y < verticalArc / 2; y += verticalArc/verticalRays)
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
                    
                    if(previousHits.Contains(damageable))
                        continue;
                    
                    previousHits.Add(damageable);

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