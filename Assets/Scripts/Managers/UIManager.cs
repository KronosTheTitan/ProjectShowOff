using System;
using Gameplay.Player;
using UnityEngine;
using Managers;

namespace Managers {
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject fourWaySplitScreen;
        [SerializeField] private GameObject twoWaySplitScreen;
        [SerializeField] private GameObject threeWaySplitScreen;

        private void Start()
        {
            GameManager.GetInstance().OnGameOver += OnGameOver;
        }

        public void OnGameOver(Player player)
        {
            UpdateSplitScreen();
        }

        public void UpdateSplitScreen()
        {
            twoWaySplitScreen.SetActive(false);
            threeWaySplitScreen.SetActive(false);
            fourWaySplitScreen.SetActive(false);
            
            if(GameManager.GetInstance().gameState == GameManager.GameStates.MainMenu)
                return;
            if(GameManager.GetInstance().gameState == GameManager.GameStates.Victory)
                return;
            
            if (GameManager.GetInstance().GetSplitScreenManager().NumberOfActiveCameras() == 2)
            {
                twoWaySplitScreen.SetActive(true);
                threeWaySplitScreen.SetActive(false);
                fourWaySplitScreen.SetActive(false);
                return;
            }
            if (GameManager.GetInstance().GetSplitScreenManager().NumberOfActiveCameras() == 3)
            {
                twoWaySplitScreen.SetActive(false);
                threeWaySplitScreen.SetActive(true);
                fourWaySplitScreen.SetActive(false);
                return;
            }
            if (GameManager.GetInstance().GetSplitScreenManager().NumberOfActiveCameras() == 4)
            {
                twoWaySplitScreen.SetActive(false);
                threeWaySplitScreen.SetActive(false);
                fourWaySplitScreen.SetActive(true);
            }
        }
    }
}