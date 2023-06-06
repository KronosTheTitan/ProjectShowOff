using System.Collections.Generic;
using Gameplay;
using Gameplay.Player;
using UnityEngine;

namespace Managers
{
    public class ControllerManager : MonoBehaviour
    {
        private readonly Dictionary<Player, Controller> _playerControllerTable = new();
        [SerializeField] private List<Controller> controllers;

        public void GrantControlOfPlayer(Controller controller)
        {
            if(controllers.Contains(controller))
                return;
            if(controllers.Count >= GameManager.GetInstance().GetPlayers().Length)
                return;
            
            foreach (Player player in GameManager.GetInstance().GetPlayers())
            {
                if(_playerControllerTable[player] != null)
                    continue;

                _playerControllerTable[player] = controller;            
                player.Activate(controller);
                controllers.Add(controller);
                
                return;
            }
        }

        public void RemoveControllerFromPlayer(Player player)
        {
            _playerControllerTable[player] = null;
        }

        public void AddPlayerToTable(Player player)
        {
            _playerControllerTable.Add(player, null);
        }
    }
}