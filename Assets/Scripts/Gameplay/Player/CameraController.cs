using UnityEngine;

namespace Gameplay.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float smoothing;
        
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