using System;
using Gameplay;
using Managers;
using UnityEngine;

namespace UserInterface
{
    public class LevelSelect : MonoBehaviour
    {
        [SerializeField] private LevelSelectOption targetedOption;
        [SerializeField] private Controller controller;

        private void Start()
        {
            controller = GameManager.GetInstance().GetControllerManager().GetPlayerOne();
        }

        private void Update()
        {
            if (controller.GetJumpButton())
            {
                targetedOption.OnInteract.Invoke();
                gameObject.SetActive(false);
                return;
            }

            if (controller.GetJoystickLeft().x > Controller.CONTROLLER_DEADZONE)
                ChangeTarget(targetedOption.right);

            if (controller.GetJoystickLeft().x < -Controller.CONTROLLER_DEADZONE)
                ChangeTarget(targetedOption.left);

            if (controller.GetJoystickLeft().y > Controller.CONTROLLER_DEADZONE)
                ChangeTarget(targetedOption.up);

            if (controller.GetJoystickLeft().y < -Controller.CONTROLLER_DEADZONE)
                ChangeTarget(targetedOption.down);
        }

        private void ChangeTarget(LevelSelectOption selectOption)
        {
            if(selectOption == null)
                return;
            
            targetedOption.selectionIndicator.SetActive(false);
            targetedOption = selectOption;
            targetedOption.selectionIndicator.SetActive(true);
        }
    }
}