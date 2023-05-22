using System;
using System.Collections;
using Managers;
using Packages.Hinput.Scripts.Gamepad;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] public int health = 100;
        [SerializeField] private float respawnDelay = 1;
        [SerializeField] private Transform respawnPoint;

        [Header("Score")]
        public int score;
        [SerializeField] private float lastScore;
        [SerializeField] private float scoreInterval;
        [SerializeField] private int amountNeededForVictory = 100;
    
        [Header("Movement")]
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float speed = 5;
        [SerializeField] private float maxSpeed = 5;
        [SerializeField] private float knockbackStrength = 1.5f;
        [SerializeField] private float knockbackDuration = .5f;
        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float jumpForce;

        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float cameraSmoothing = 1.5f;

        private bool _inKnockback = false;

        [SerializeField] private bool useDpad = true;
    
        [Header("Shooting")]
        [SerializeField] private float rateOfFire = .1f;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletSpawn;
        [SerializeField] private AudioSource shotEffect;
    
        private float _lastShot = -1;

        [Header("Gamepad")] 
        [SerializeField] private float vibrationStrength = 10f;
        [SerializeField] private float vibrationDuration = .5f;
        private Gamepad _gamepad;

        [Header("Events")]
        public UnityEvent onScoreIncrease;
        public UnityEvent onTakeDamage;
        public UnityEvent onDeath;

    
        public void Update()
        {
            if (_inKnockback)
                return;
            if(_gamepad == null) 
                return;

            if (useDpad)
            {
                Vector2 direction = _gamepad.dPad.position; 
                MovePlayer(direction.normalized);
            }
            else
            {
                Vector2 direction = _gamepad.leftStick.position; 
                MovePlayer(direction.normalized);
            }
            
            if(!IsGrounded())
                return;
            
            if (_gamepad.A.justPressed) 
                rigidbody.AddForce(Vector3.up * jumpForce);
            
            if (_gamepad.rightTrigger.pressed || Input.GetKeyDown(KeyCode.Space))
                Shoot();
        }
        bool IsGrounded() {
                 Vector3 position = transform.position;
                 Vector3 direction = Vector2.down;
                 float distance = 1f;
             
                 Debug.DrawRay(position, direction, Color.green);
                 RaycastHit hit;
                 Physics.Raycast(position, direction, out hit, distance, groundLayer);
                 
                 if (hit.collider != null) {
                     return true;
                 }
             
                 return false;
             }

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
        /// <returns></returns>
        private IEnumerator Knockback(Vector2 direction)
        {
            _inKnockback = true;
            rigidbody.AddForce(new Vector3(direction.x,0,direction.y) * (-1 * knockbackStrength), ForceMode.Impulse);
            yield return new WaitForSeconds(knockbackDuration);
            _inKnockback = false;
            print("Done!");
        }

        /// <summary>
        /// Use this function in events whenever you want the controller to vibrate.
        /// strength and duration variables can be found in the gamepad section.
        /// </summary>
        public void VibrateController()
        {
            _gamepad?.Vibrate(vibrationStrength, vibrationStrength, vibrationDuration);
        }

        /// <summary>
        /// MovePlayer is used to move the player and change their
        /// rotation.
        /// </summary>
        /// <param name="direction"></param>
        private void MovePlayer(Vector2 direction)
        {
            transform.rotation = transform.rotation * Quaternion.AngleAxis((direction.x * 90) * Time.deltaTime, transform.up);
            direction *= speed;
            rigidbody.velocity = Vector3.ClampMagnitude((transform.forward * direction.y) + rigidbody.velocity, maxSpeed);
        }

        /// <summary>
        /// this is important to be in late update to avoid jitter for the camera.
        /// </summary>
        private void LateUpdate()
        {
            cameraTarget.position = Vector3.Slerp(cameraTarget.position, transform.position, Time.deltaTime * cameraSmoothing);
            cameraTarget.rotation = Quaternion.Slerp(cameraTarget.rotation, transform.rotation, Time.deltaTime * cameraSmoothing);
        }

        /// <summary>
        /// This function shoots a projectile towards the direction the player is currently
        /// rotated.
        /// </summary>
        protected virtual void Shoot()
        {
            if( _lastShot + rateOfFire > Time.time) return;
        
            _lastShot = Time.time;
            shotEffect.Play();
            Bullet bullet = Instantiate(bulletPrefab,bulletSpawn.position,bulletSpawn.rotation);
            bullet.SetSource(this);
        }

        /// <summary>
        /// Assigns the gamepad from the player manager.
        /// </summary>
        /// <param name="gamepad"></param>
        public void SetGamepad(Gamepad gamepad)
        {
            _gamepad = gamepad;
        }

        public Gamepad GetGamepad()
        {
            return _gamepad;
        }
        
        public void TakeDamage(Vector3 direction, Player source)
        {
            onTakeDamage.Invoke();
            StartCoroutine(Knockback(direction));
        }

        /// <summary>
        /// Respawns the player at its spawn point.
        /// </summary>
        public void Respawn()
        {
            health = maxHealth;
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
            onTakeDamage.Invoke();

            _inKnockback = false;
        
            gameObject.SetActive(true);
        }
    }
}
