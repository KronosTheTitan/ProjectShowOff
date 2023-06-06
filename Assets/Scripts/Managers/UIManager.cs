using UnityEngine;
using Managers;

namespace Managers {
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject fourWaySplitScreen;
        
        public void UpdateSplitScreen()
        {
            if (GameManager.GetInstance().GetSplitScreenManager().NumberOfActiveCameras() >= 1)
            {
                fourWaySplitScreen.SetActive(false);
            }
            else if (GameManager.GetInstance().GetSplitScreenManager().NumberOfActiveCameras() >= 2)
            {
                fourWaySplitScreen.SetActive(true);
            }
        }
    }
}