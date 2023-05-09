using System.Collections.Generic;
using System.Linq;

namespace Packages.Hinput.Scripts.Pressable {
    public class AnyGamepadButton : Button {
        // --------------------
        // CONSTRUCTOR
        // --------------------
        
        public AnyGamepadButton(string pressableName, Gamepad.Gamepad gamepad, int index, bool isEnabled) :
            base(pressableName, gamepad, index, isEnabled) {
            buttons = Utils.Hinput.gamepad.Select(g => g.buttons[index]).ToList();
        }


        // --------------------
        // BUTTONS
        // --------------------

        // Every stick of this type
        private readonly List<Pressable> buttons;


        // --------------------
        // UPDATE
        // --------------------

        protected override bool GetPressed() { return buttons.Any(button => button.simplePress.pressed); }
    }
}
