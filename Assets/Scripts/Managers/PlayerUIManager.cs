using System;
using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;
using UserInterface;

namespace Managers
{
    [RequireComponent(typeof(GameManager))]
    public class PlayerUIManager : MonoBehaviour
    {
        [SerializeField] private Rect[] uiRects;
        [SerializeField] private List<PlayerUI> playerUis;

        private Dictionary<Player, PlayerUI> _playerUIPairs = new();

        public void AddPlayerUI(PlayerUI playerUI, Player player)
        {
            playerUis.Add(playerUI);
            _playerUIPairs.Add(player, playerUI);
            RecalculatePlayerUIScales();
        }

        public void RemovePlayer(Player owner)
        {
            _playerUIPairs.Remove(owner);
        }
        
        private void RecalculatePlayerUIScales()
        {
            for (int i = 0; i < playerUis.Count-1; i++)
            {
                playerUis[i].gameObject.GetComponent<RectTransform>().rect.Set(
                    uiRects[i].x,
                    uiRects[i].y,
                    uiRects[i].width,
                    uiRects[i].height);
            }
        }
    }
}