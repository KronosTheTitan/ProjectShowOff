using System;
using Managers;
using UnityEngine;

namespace Gameplay.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float smoothing;
        [SerializeField] private Camera targetCamera;

        private void Start()
        {
            player.OnConnect += Connect;
            player.OnDisconnect += Disconnect;
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

        /// <summary>
        /// this is important to be in late update to avoid jitter for the camera.
        /// </summary>
        private void LateUpdate()
        {
            transform.position = Vector3.Slerp(transform.position, player.transform.position, Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime * smoothing);
        }
    }
}