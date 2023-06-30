using Managers;
using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float speed;
        [SerializeField] private float speedMax = 7f;
        [SerializeField] private float rotationSmoothing = 5;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float slopeCheckDistance = 5f;
        [SerializeField] private float maxSlopeAngle = 40f;
        [SerializeField] private bool slopeJump;
        private RaycastHit slopeHit;

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

            if (GameManager.GetInstance().gameState == GameManager.GameStates.MainMenu)
            {
                MoveMainMenu();
            }
            else
            {
                Move();
            }
            
            if(!player.IsGrounded())
                return;

            Jump();

            if (player.GetController().GetResetToMainMenuButton())
            {
                GameManager.GetInstance().GetMapManager().ResetToMainMenu();
            }

            if (player.GetController().GetRespawnPlayerButton()){
                player.Respawn();
            }

            SpeedControl();
        }

        /// <summary>
        /// Moves the player based on the movement input.
        /// </summary>
        private void Move()
        {
            Vector3 joystick = player.GetController().GetJoystickLeft();

            Vector3 forward = cameraController.transform.forward;
            Vector3 right = cameraController.transform.right;

            Vector3 moveDirection = (joystick.x * right + joystick.y * forward).normalized;

            if (OnSlope() && !slopeJump)
            {
                moveDirection = GetSlopeMoveDirection(moveDirection);
                rb.AddForce(moveDirection * (speed * Time.deltaTime * 6f), ForceMode.Force);

                if (rb.velocity.y > 0)
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection * (speed * Time.deltaTime), ForceMode.Force);
            }

            rb.useGravity = !OnSlope();
            
            if(joystick.magnitude < Controller.CONTROLLER_DEADZONE)
                return;
            
            Quaternion desiredRotation = Quaternion.LookRotation(joystick.x * right + joystick.y * forward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothing * Time.deltaTime);
        }

        /// <summary>
        /// Moves the player based on the movement input.
        /// </summary>
        private void MoveMainMenu()
        {
            Vector3 joystick = player.GetController().GetJoystickLeft();
            joystick = new Vector3(joystick.x, 0, joystick.y) * -1;
            rb.AddForce(joystick * (speed * Time.deltaTime), ForceMode.Force);
            
            if(joystick.magnitude < Controller.CONTROLLER_DEADZONE)
                return;
            
            Quaternion desiredRotation = Quaternion.LookRotation(joystick, Vector3.up);
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

        private void SpeedControl()
        {
            if (OnSlope() && !slopeJump)
            {
                if (rb.velocity.magnitude > speedMax)
                    rb.velocity = rb.velocity.normalized * speedMax;
            }

            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                if (flatVel.magnitude > speedMax)
                {
                    Vector3 limitedVel = flatVel.normalized * speedMax;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }

        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, slopeCheckDistance))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection(Vector3 moveDirection)
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }
    }
}