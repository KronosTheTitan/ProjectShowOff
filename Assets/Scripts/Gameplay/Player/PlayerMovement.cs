using Managers;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Rigidbody rb;

        public delegate void MovementDelegate();

        public event MovementDelegate OnJump;
        
        private void Update()
        {
            if(player.GetInKnockback())
                return;
            
            Move();
            
            if(!player.IsGrounded())
                return;

            Jump();

            
            
        }

        private void Move()
        {
            transform.position+=(transform.forward * (player.GetInput().actions["Move"].ReadValue<Vector2>().y * speed * Time.deltaTime));
            transform.Rotate(0, player.GetInput().actions["Move"].ReadValue<Vector2>().x * Time.deltaTime * rotationSpeed, 0);
        }

        private void Jump()
        {
            if (player.GetInput().actions["Jump"].WasPerformedThisFrame())
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
                Debug.Log("jump");

                OnJump?.Invoke();
            }
        }
    }
}