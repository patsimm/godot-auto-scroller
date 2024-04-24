using Godot;

namespace Platformer;

public partial class Gui : CanvasLayer {
	[Signal]
	public delegate void PlayAgainButtonPressedEventHandler();

	[Export]
	public required BoxContainer GameOverContainer { get; set; }

	[Export]
	public required BoxContainer LevelCompletedContainer { get; set; }

	public override void _Ready() {
		GameOverContainer.Hide();
		LevelCompletedContainer.Hide();
	}

	public void ShowGameOver() {
		GameOverContainer.Show();
	}

	public void ShowLevelCompleted() {
		LevelCompletedContainer.Show();
	}

	public void OnPlayAgainButtonPressed() {
		LevelCompletedContainer.Hide();
		GameOverContainer.Hide();
		EmitSignal(SignalName.PlayAgainButtonPressed);
	}
}
