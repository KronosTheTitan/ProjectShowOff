namespace Packages.Hinput.Scripts.Pressable {
	/// <summary>
	/// Hinput class representing a physical button of a controller, such as the A button, a bumper or a stick click.
	/// </summary>
	public class Button : GamepadPressable {
		// --------------------
		// CONSTRUCTOR
		// --------------------

		public Button(string pressableName, Gamepad.Gamepad gamepad, int index, bool isEnabled) : 
			base(pressableName, gamepad, index, isEnabled) { }

	
		// --------------------
		// UPDATE
		// --------------------

		protected override bool GetPressed() {
			try { return Utils.Utils.GetButton(Utils.Utils.os + "_" + name); } 
			catch { /* Ignore exceptions here */ }
			return false; }
	}
}