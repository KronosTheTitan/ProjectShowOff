using UnityEngine;
using Gameplay.Player;

namespace Gameplay
{
    public interface IDamageable
    {
        /// <summary>
        /// applies knock-back to the player
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="direction"></param>
        /// <param name="source"></param>
        public void TakeDamage(Vector3 direction, Player.Player source, float strength, float duration);
    }
}