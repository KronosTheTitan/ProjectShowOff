using UnityEngine;

namespace Gameplay
{
    public abstract class GameMode : MonoBehaviour
    {
        public abstract float[] GetScores();
        public abstract void Update();
    }
}