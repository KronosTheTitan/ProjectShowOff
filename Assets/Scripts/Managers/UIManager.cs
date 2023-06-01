using UnityEngine;
using Managers;

namespace Manager {
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject fourWaySplitScreen;
        
        public void UpdateSplitScreen()
        {

            if (GameManager.GetInstance().GetPlayers().Length >= 1)
            {
                fourWaySplitScreen.SetActive(false);
                
            }
            else if (GameManager.GetInstance().GetPlayers().Length >= 2)
            {
                
                fourWaySplitScreen.SetActive(true);
            }
        }
    }
}