using Packages.Hinput.Scripts.Gamepad;
using Packages.Hinput.Scripts.Utils;

namespace Packages.Hinput.Scripts.Pressable {
    /// <summary>
    /// Hinput class representing a stick or D-pad as a button. It is considered pressed if the stick is pushed in any
    /// direction.
    /// </summary>
    public class StickPressedZone : StickPressable {
        // --------------------
        // CONSTRUCTOR
        // --------------------

        public StickPressedZone(string pressableName, Stick stick) : 
            base(pressableName, stick) { }

	    
        // --------------------
        // UPDATE
        // --------------------

        protected override bool GetPressed() { return stick.distance > Settings.stickPressedZone; }
    }
}