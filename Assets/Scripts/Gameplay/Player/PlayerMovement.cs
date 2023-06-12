using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private CameraController camera;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Rigidbody rb;

        public delegate void MovementDelegate();

        public event MovementDelegate OnJump;
        
        private void Update()
        {
            if(player.GetController() == null)
                return;
            if (player.GetController().GetRemovePlayerButton())
                player.RemovePlayer(player);

            //if the player is currently under the effect of knockback any further movement commands
            //are ignored.
            if (player.GetInKnockback())
                return;
            
            Move();
            
            if(!player.IsGrounded())
                return;

            Jump();
        }

        /// <summary>
        /// Moves the player based on the movement input.
        /// </summary>
        private void Move()
        {
            Vector3 joystick = player.GetController().GetJoystickLeft();

            Vector3 forward = camera.transform.forward;
            Vector3 right = camera.transform.right;
            
            rb.AddForce((joystick.x * right + joystick.y * forward) * (speed * Time.deltaTime), ForceMode.Acceleration);

            transform.LookAt(transform.position + (joystick.x * right + joystick.y * forward));
        }

        /// <summary>
        /// Shoots the player into the air when the Jump button is pressed.
        /// </summary>
        private void Jump()
        {
            if (player.GetController().GetJumpButton())
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);

                OnJump?.Invoke();
            }
        }
    }
}