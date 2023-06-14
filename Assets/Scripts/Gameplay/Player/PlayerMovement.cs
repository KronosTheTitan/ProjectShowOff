using Managers;
using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float speed;
        [SerializeField] private float rotationSmoothing = 5;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private MapManager mapManager;

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

            if (player.GetController().GetResetToMainMenuButton())
            {
                mapManager.ResetToMainMenu();
            }
        }

        /// <summary>
        /// Moves the player based on the movement input.
        /// </summary>
        private void Move()
        {
            Vector3 joystick = player.GetController().GetJoystickLeft();

            Vector3 forward = cameraController.transform.forward;
            Vector3 right = cameraController.transform.right;
            
            rb.AddForce((joystick.x * right + joystick.y * forward) * (speed * Time.deltaTime), ForceMode.Acceleration);

            if(joystick.magnitude < Controller.CONTROLLER_DEADZONE)
                return;
            
            Quaternion desiredRotation = Quaternion.LookRotation(joystick.x * right + joystick.y * forward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothing * Time.deltaTime);
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