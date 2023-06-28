//Class written by Tjalle Borgers on 11/05/2023

using System;
using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// this struct functions as a go between to store a layout for the cameras.
    /// I would normally do this with a 2d array but unity can't serialize that
    /// so I would be stuck making any adjustments by hand, this way I can
    /// move that task to the designers.
    /// </summary>
    [Serializable]
    public struct CameraLayout
    {
        public Rect[] rects;
    }

    /// <summary>
    /// This class handles the layout of the split screen cameras.
    /// </summary>
    public class SplitScreenManager : MonoBehaviour
    {
        [SerializeField] private CameraLayout[] cameraLayouts;
        [SerializeField] private Camera[] cameras;
        [SerializeField] private UIManager uiManager;

        private int activeCams = 0;
        
        public Rect GetRectForPlayerIndex(int index)
        {
            return cameraLayouts[activeCams].rects[index];
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCamera">
        /// The new camera that will be added.
        /// </param>
        /// <param name="player">
        /// This parameter is used to keep track of what camera needs to be removed when a player disconnects
        /// </param>
        /// <returns>
        /// In the event that the camera limit has been reached
        /// the function will return false.
        /// </returns>
        public void AddCamera(Camera iCamera, Player player)
        {
            cameras[GameManager.GetInstance().GetPlayerIndex(player)] = iCamera;
            
            UpdateCameraLayout();
            uiManager.UpdateSplitScreen();

            activeCams++;
        }

        /// <summary>
        /// Removes a camera and adjusts the layout accordingly.
        /// </summary>
        /// <param name="player">
        /// The player whose camera needs to be removed from the layout.
        /// </param>
        /// <returns></returns>
        public void RemoveCamera(Player player)
        {
            cameras[GameManager.GetInstance().GetPlayerIndex(player)] = null;
            
            UpdateCameraLayout();

            activeCams--;
        }

        public int NumberOfActiveCameras()
        {
            return activeCams;
        }

        /// <summary>
        /// Updates the layout of the cameras;
        /// </summary>
        private void UpdateCameraLayout()
        {
            for (int i = 0; i < 4; i++)
            {
                if(cameras[i] == null)
                    continue;
                cameras[i].rect = cameraLayouts[activeCams].rects[i];         
            }
        }
    }
}