using System;
using Managers;
using UnityEngine;

namespace Gameplay.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private PlayerCombat combat;
        [SerializeField] private float smoothing;
        [SerializeField] private Camera targetCamera;

        [SerializeField] private float rotationSpeed = 1;

        private void Start()
        {
            player.OnConnect += Connect;
            player.OnDisconnect += Disconnect;

            combat.OnKick += CenterCamera;
            combat.OnVocalSack += CenterCamera;
            combat.OnTonguePull += CenterCamera;
        }

        private void Connect()
        {
            GameManager.GetInstance().GetSplitScreenManager().AddCamera(targetCamera, player);
        }

        private void Disconnect()
        {
            targetCamera.rect = new Rect(0, 0, 0, 0);
            GameManager.GetInstance().GetSplitScreenManager().RemoveCamera(player);
          
        }

        private void CenterCamera()
        {
            transform.rotation = player.transform.rotation;
        }

        /// <summary>
        /// this is important to be in late update to avoid jitter for the camera.
        /// </summary>
        private void LateUpdate()
        {
            transform.position = Vector3.Slerp(transform.position, player.transform.position, Time.deltaTime * smoothing);

            if(player.GetController() == null)
                return;
            
            transform.rotation *= Quaternion.AngleAxis(player.GetController().GetJoystickRight().x * Time.deltaTime * rotationSpeed, Vector3.up);
        }
    }
}