using UnityEngine;

namespace Gameplay
{
    public interface IDamageable
    {
        public void TakeDamage(int amount, Vector3 direction, Player source);
    }
}