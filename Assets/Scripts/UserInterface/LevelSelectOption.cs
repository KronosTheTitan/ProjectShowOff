using UnityEngine;
using UnityEngine.Events;

namespace UserInterface
{
    public class LevelSelectOption : MonoBehaviour
    {
        public UnityEvent OnInteract;
        public GameObject selectionIndicator;

        public LevelSelectOption left;
        public LevelSelectOption right;
        public LevelSelectOption up;
        public LevelSelectOption down;
    }
}