﻿//Class written by Tjalle Borgers on 11/05/2023
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
         private Dictionary<Player, Camera> _playerCameraTable = new();

         [SerializeField] private CameraLayout[] cameraLayouts;
         [SerializeField] private List<Camera> cameras;
         [SerializeField] private int maximumNumberOfCameras = 4;
         [SerializeField] private UIManager uiManager;

         public Rect GetRectForPlayerIndex(int index)
         {
             return cameraLayouts[cameras.Count - 1].rects[cameras.IndexOf(_playerCameraTable[GameManager.GetInstance().GetPlayer(index)])];
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
         public bool AddCamera(Camera iCamera, Player player)
         {
             if (cameras.Count == maximumNumberOfCameras)
                 return false;

             cameras.Add(iCamera);
             _playerCameraTable.Add(player, iCamera);

             UpdateCameraLayout();
             uiManager.UpdateSplitScreen();

             return true;
         }

         /// <summary>
         /// Removes a camera and adjusts the layout accordingly.
         /// </summary>
         /// <param name="player">
         /// The player whose camera needs to be removed from the layout.
         /// </param>
         /// <returns></returns>
         public bool RemoveCamera(Player player)
         {
             if (cameras.Count == 1)
                 return false;

             cameras.Remove(_playerCameraTable[player]);
             _playerCameraTable.Remove(player);

             UpdateCameraLayout();
            uiManager.UpdateSplitScreen();

             return false;
         }

         public int NumberOfActiveCameras()
         {
             return cameras.Count;
         }

         /// <summary>
         /// Updates the layout of the cameras;
         /// </summary>
         private void UpdateCameraLayout()
         {
             for (int i = 0; i < cameras.Count; i++)
             {
                 cameras[i].rect = cameraLayouts[cameras.Count - 1].rects[i];
             }
         }
     }
 }