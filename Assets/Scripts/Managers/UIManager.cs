using UnityEngine;
using Managers;

namespace Managers {
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject fourWaySplitScreen;
        [SerializeField] private GameObject twoWaySplitScreen;
        [SerializeField] private GameObject threeWaySplitScreen;

        public void UpdateSplitScreen()
        {
            if (GameManager.GetInstance().GetSplitScreenManager().NumberOfActiveCameras() == 2)
            {
                twoWaySplitScreen.SetActive(true);
                threeWaySplitScreen.SetActive(false);
                fourWaySplitScreen.SetActive(false);             
            }
            else if (GameManager.GetInstance().GetSplitScreenManager().NumberOfActiveCameras() == 3)
            {
                twoWaySplitScreen.SetActive(false);
                threeWaySplitScreen.SetActive(true);
                fourWaySplitScreen.SetActive(false);
            }
            else if (GameManager.GetInstance().GetSplitScreenManager().NumberOfActiveCameras() == 4)
            {
                twoWaySplitScreen.SetActive(false);
                threeWaySplitScreen.SetActive(false);
                fourWaySplitScreen.SetActive(true);
            }
        }
    }
}