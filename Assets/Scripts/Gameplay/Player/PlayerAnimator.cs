using System.Collections;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float jumpDuration;
        [SerializeField] private float groundedCheckOffset;
        [SerializeField] private PlayerCombat combat;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private Player player;

        private bool ableToGroundCheck;

        private void Start()
        {
            movement.OnJump += Jump;
            combat.OnKick += Kick;
            combat.OnVocalSack += VocalSack;
            combat.OnTonguePull += Tongue;  
        }
        
        private void Update()
        {
            if (rb.velocity.magnitude > 0 && player.IsGrounded() == true)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving",false);
            }

             

            // Later "isJumping" should be changed to a SetTrigger and the bool should be moved onto the Falling animation
            if(player.IsGrounded()==true && ableToGroundCheck == true)
            {
                animator.SetBool("isJumping", false);
                ableToGroundCheck = false;
            }
        }
       
        /// <summary>
        /// We should be able to attack while jumping/falling i think
        /// Also need falling animation
        /// 
        /// </summary>
        private void Jump()
        {
            animator.SetBool("isJumping", true);
            StartCoroutine(jumpTimer());
            
        }

        private void Kick()
        {

            animator.SetTrigger("Kick");
        }

        private void VocalSack()
        {
            animator.SetTrigger("vocalSack");
        }

        private void Tongue()
        {
           
            animator.SetTrigger("Tongue");
        }

        private void Falling()
        {
            animator.SetBool("isFalling", true);
        }

        IEnumerator jumpTimer()
        {

            yield return new WaitForSeconds(groundedCheckOffset);
            ableToGroundCheck = true;
            
        }
      
    }
}