using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Transform respawnPoint;
    
        [Header("Movement")]
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private LayerMask groundLayer;

        private bool _inKnockback = false;

        public delegate void PlayerDelegate();
        public event PlayerDelegate OnScoreIncrease;
        public event PlayerDelegate OnTakeDamage;
        public event PlayerDelegate onDeath;

        private void Start()
        {
            GameManager.GetInstance().AddPlayer(this);
        }

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
        }
        
        public void TakeDamage(Vector3 direction, Player source, float strength, float duration)
        {
            OnTakeDamage?.Invoke();
            StartCoroutine(Knockback(direction, strength, duration));
        }

        public void InvokeOnScoreIncrease()
        {
            OnScoreIncrease?.Invoke();
        }

        /// <summary>
        /// Respawns the player at its spawn point.
        /// </summary>
        public void Respawn()
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
            OnTakeDamage?.Invoke();

            _inKnockback = false;
        
            gameObject.SetActive(true);
        }
    }
}