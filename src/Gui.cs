using Godot;

namespace Platformer;

public partial class Gui : CanvasLayer {
	[Signal]
	public delegate void PlayAgainButtonPressedEventHandler();

	public void OnPlayAgainButtonPressed() {
		_ = EmitSignal(SignalName.PlayAgainButtonPressed);
	}
}
