using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Transform respawnPoint;

        [Header("Score")]
        public int score;
        [SerializeField] private float lastScore;
        [SerializeField] private float scoreInterval;
        [SerializeField] private int amountNeededForVictory = 100;
    
        [Header("Movement")]
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private LayerMask groundLayer;

        private bool _inKnockback = false;
    
        private float _lastShot = -1;

        [Header("Events")]
        public UnityEvent onScoreIncrease;
        public UnityEvent onTakeDamage;
        public UnityEvent onDeath;

        public PlayerInput GetInput()
        {
            return playerInput;
        }

        public bool IsGrounded() {
            Vector3 position = transform.position;
            Vector3 direction = Vector2.down;
            float distance = 1f;
             
            Debug.DrawRay(position, direction, Color.green);
            RaycastHit hit;
            Physics.Raycast(position, direction, out hit, distance, groundLayer);
                 
            if (hit.collider != null)
                return true;

            return false;
        }
        
        /// <summary>
        /// Checks if the player is in knockback.
        /// </summary>
        /// <returns></returns>
        public bool GetInKnockback()
        {
            return _inKnockback;
        }
        
        /// <summary>
        /// this function is used to add score to the player
        /// </summary>
        /// <param name="amount"></param>
        public void ReceiveScore(int amount)
        {
            if(Time.time < lastScore + scoreInterval) return;
            lastScore = Time.time;
            score += amount;
            onScoreIncrease.Invoke();

            if (score >= amountNeededForVictory)
            {
                PlayerManager.GetInstance().HandleVictory(this);
            }
        }

        /// <summary>
        /// This function is used to knock back the player.
        /// It is used as a coroutine so you it can lock
        /// the controls until the knockback effect is over.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="knockbackStrength">This is how much force will be applied to the player</param>
        /// <param name="knockbackDuration">This controls how long a player is locked out of the controls</param>
        /// <returns></returns>
        private IEnumerator Knockback(Vector3 direction, float knockbackStrength, float knockbackDuration)
        {
            _inKnockback = true;
            rigidbody.AddForce(direction * (-1 * knockbackStrength), ForceMode.Impulse);
            yield return new WaitForSeconds(knockbackDuration);
            _inKnockback = false;
            print("Done!");
        }
        
        public void TakeDamage(Vector3 direction, Player source, float strength, float duration)
        {
            onTakeDamage.Invoke();
            StartCoroutine(Knockback(direction, strength, duration));
        }

        /// <summary>
        /// Respawns the player at its spawn point.
        /// </summary>
        public void Respawn()
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
            onTakeDamage.Invoke();

            _inKnockback = false;
        
            gameObject.SetActive(true);
        }
    }
}