using Gameplay;
using Packages.Hinput.Scripts.Utils;
using UnityEngine;
using UserInterface;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Singleton
        private static PlayerManager _instance;

        private void Awake()
        {
            if (_instance != null)
                Destroy(this);
            else
                _instance = this;
        }

        public static PlayerManager GetInstance()
        {
            return _instance;
        }
        #endregion

        [SerializeField] private Player[] players;
        [SerializeField] private SplitScreenManager splitScreenManager;
        void Start()
        {
            
        }

        public void HandleVictory(Player winner)
        {
            GameOverScreen.instance.gameObject.SetActive(true);
        }
    }
}
