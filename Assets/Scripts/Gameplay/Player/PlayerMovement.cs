using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Rigidbody rb;

        public delegate void MovementDelegate();

        public event MovementDelegate OnJump;
        
        private void Update()
        {
            Move();
            
            if(!IsGrounded())
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
                
                OnJump?.Invoke();
            }
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
    }
}