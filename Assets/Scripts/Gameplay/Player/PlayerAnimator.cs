using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float jumpDuration;
        [SerializeField] private PlayerCombat combat;
        [SerializeField] private PlayerMovement movement;

        private void Start()
        {
            movement.OnJump += Jump;
        }

        private void Update()
        {
            if (rb.velocity.magnitude > 0)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving",false);
            }
        }

        private void Jump()
        {
            animator.SetBool("isJumping", true);
        }

        private void Kick()
        {
            animator.SetBool("Kick", true);
        }

        private void VocalSack()
        {
            animator.SetBool("vocalSack", true);
        }

        private void Tongue()
        {
            animator.SetBool("tongue", true);
        }
    }
}