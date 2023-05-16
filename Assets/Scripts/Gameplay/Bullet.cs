using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// This class is used to control projectiles.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private int bounceCount = 1;
        [SerializeField] private float speed = 7.5f;
        [SerializeField] private Player source;

        private int bounces = 0;

        private void Start()
        {
            rigidbody.velocity = transform.forward * speed;
        }

        private void Update()
        {
            rigidbody.velocity = rigidbody.velocity.normalized * speed;
        }

        public void SetSource(Player iSource)
        {
            source = iSource;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            //cast the collision to check if it is a player.
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(rigidbody.velocity, source);
                Destroy(gameObject);
                return;
            }
            if (bounceCount <= bounces)
            {
                Destroy(gameObject);
                return;
            }
        
            bounces++;
        }
    }
}