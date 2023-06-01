using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Player;
using Managers;

namespace Manager {
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject FourWaySplitScreen;





        public void updateSplitScreen()
        {

            if (GameManager.GetInstance().GetPlayers().Length >= 1)
            {
                FourWaySplitScreen.SetActive(false);
                
            }
            else if (GameManager.GetInstance().GetPlayers().Length >= 2)
            {
                
                FourWaySplitScreen.SetActive(true);
            }

        }

    }
}
