using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class Controller : MonoBehaviour
    {
        public const float CONTROLLER_DEADZONE = 0.1f;
        [SerializeField] private PlayerInput input;

        private void Start()
        {
            GameManager.GetInstance().GetControllerManager().GrantControlOfPlayer(this);
        }

        public Vector2 GetJoystickLeft()
        {
            return input.actions["Move"].ReadValue<Vector2>();
        }public Vector2 GetJoystickRight()
        {
            return input.actions["Camera"].ReadValue<Vector2>();
        }
        public bool GetJumpButton()
        {
            return input.actions["Jump"].WasPerformedThisFrame();
        }
        public bool GetRemovePlayerButton()
        {
            return input.actions["RemovePlayer"].WasPerformedThisFrame();
        }
        public bool GetKickButton()
        {
            return input.actions["Kick"].WasPerformedThisFrame();
        }
        public bool GetVocalSackButton()
        {
            return input.actions["VocalSack"].WasPerformedThisFrame();
        }
        public bool GetTonguePullButton()
        {
            return input.actions["TonguePull"].WasPerformedThisFrame();
        }
    }
}