using Gameplay;
using Packages.Hinput.Scripts.Utils;
using UnityEngine;
using UserInterface;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
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

        [SerializeField] private Player[] players;
        void Start()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if(Hinput.gamepad[i] == null) continue;
                players[i].SetGamepad(Hinput.gamepad[i]);
            }
        }

        public void HandleVictory(Player winner)
        {
            GameOverScreen.instance.gameObject.SetActive(true);
        }
    }
}
