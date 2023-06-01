using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour, IDamageable
    {
        #region Variables
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Transform respawnPoint;
    
        [Header("Movement")]
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundedDistance;

        [SerializeField] private bool inKnockback = false;
        #endregion

        #region Events
        public delegate void PlayerDelegate();
        public event PlayerDelegate OnScoreIncrease;
        public event PlayerDelegate OnTakeDamage;
        #endregion

        #region UnityEventMethods

        private void Start()
        {
            GameManager.GetInstance().AddPlayer(this);
            
        }

        #endregion

        public PlayerInput GetInput()
        {
            return playerInput;
        }

        public bool IsGrounded() {
            Vector3 position = transform.position;
            Vector3 direction = Vector2.down;
            float distance = groundedDistance;
             
            Debug.DrawRay(position, direction, Color.green);
            Physics.Raycast(position, direction, out RaycastHit hit, distance, groundLayer);
                 
            if (hit.collider == null)
                return false;

            return true;
        }
        
        /// <summary>
        /// Checks if the player is in knockback.
        /// </summary>
        /// <returns></returns>
        public bool GetInKnockback()
        {
            return inKnockback;
        }

        /// <summary>
        /// used to notify elements like the UI that the player has received score.
        /// </summary>
        public void InvokeOnScoreIncrease()
        {
            OnScoreIncrease?.Invoke();
        }

        /// <summary>
        /// starts the process of removing a player from the game.
        /// </summary>
        /// <param name="player"></param>
        public void RemovePlayer(Player player)
        {
            StartCoroutine(DelayedRemovePlayer(player));
        }

        /// <summary>
        /// Respawns the player at its spawn point.
        /// </summary>
        public void Respawn()
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;

            OnTakeDamage?.Invoke();

            inKnockback = false;
        
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// The implementation that allows the player to take knockback.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="source"></param>
        /// <param name="strength"></param>
        /// <param name="duration"></param>
        public void TakeDamage(Vector3 direction, Player source, float strength, float duration)
        {
            OnTakeDamage?.Invoke();
            StartCoroutine(Knockback(direction, strength, duration));
        }

        private IEnumerator DelayedRemovePlayer(Player player)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            Destroy(player.gameObject);
            GameManager.GetInstance().RemovePlayer(player);
            yield return null;
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
            inKnockback = true;
            rigidbody.AddForce(direction * (-1 * knockbackStrength), ForceMode.Impulse);
            yield return new WaitForSeconds(knockbackDuration);
            inKnockback = false;
        }
    }
}