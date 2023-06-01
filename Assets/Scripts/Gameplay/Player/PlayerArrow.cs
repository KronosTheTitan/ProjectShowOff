using System;
using Managers;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerArrow : MonoBehaviour
    {
        private void Update()
        {
            transform.LookAt(GameManager.GetInstance().GetHill().transform.position);
        }
    }
}