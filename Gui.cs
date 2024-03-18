using Godot;

public partial class Gui : CanvasLayer
{

	[Signal]
	public delegate void PlayAgainButtonPressedEventHandler();

	public void OnPlayAgainButtonPressed()
	{
		EmitSignal(SignalName.PlayAgainButtonPressed);
	}

}
