using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private TMP_Text score;

        public void UpdateScore()
        {
            score.text = player.score.ToString();
        }
    }
}
