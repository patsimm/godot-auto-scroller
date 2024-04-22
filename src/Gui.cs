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
		VisibilityChanged += OnVisibilityChanged;
		GameOverContainer.Hide();
		LevelCompletedContainer.Hide();
	}

	public void ShowGameOver() {
		GameOverContainer.Show();
	}

	public void ShowLevelCompleted() {
		LevelCompletedContainer.Show();
	}

	public void OnVisibilityChanged() {
		if (!Visible) {
			GameOverContainer.Hide();
			LevelCompletedContainer.Hide();
		}
	}

	public void OnPlayAgainButtonPressed() {
		_ = EmitSignal(SignalName.PlayAgainButtonPressed);
	}
}
