using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Managers
{
    public class SpawnManager : MonoBehaviour
    {
        private readonly Dictionary<Player, Transform> _spawnOwnershipTable = new();
        private readonly Dictionary<Transform, bool> _spawnAvailabilityTable = new();

        [SerializeField] private List<Transform> spawnSites;

        private void Start()
        {
            foreach (Transform site in spawnSites)
            {
                _spawnAvailabilityTable.Add(site, true);
            }
        }

        public void AssignAvailableSpawn(Player player)
        {
            foreach (Transform site in spawnSites)
            {
                if(!_spawnAvailabilityTable[site])
                    continue;

                player.transform.parent.position = site.position;
                player.transform.parent.rotation = site.rotation;

                _spawnAvailabilityTable[site] = false;
                _spawnOwnershipTable.Add(player, site);
            }
        }

        public void RemovePlayer(Player player)
        {
            _spawnAvailabilityTable[_spawnOwnershipTable[player]] = true;
            _spawnOwnershipTable.Remove(player);
        }
    }
}