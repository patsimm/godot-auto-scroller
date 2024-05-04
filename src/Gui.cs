using Godot;

namespace Platformer;

public partial class Gui : CanvasLayer {
	[Signal]
	public delegate void PlayAgainButtonPressedEventHandler();

    [ExportGroup("GameOver")]
	[Export]
	public required BoxContainer GameOverContainer { get; set; }

    [ExportGroup("LevelCompleted")]
    [Export]
	public required BoxContainer LevelCompletedContainer { get; set; }

    [ExportGroup("LevelCompleted")]
    [Export]
    public required Label GameTimeLabel { get; set; }

	public override void _Ready() {
		GameOverContainer.Hide();
		LevelCompletedContainer.Hide();
	}

	public void ShowGameOver() {
		GameOverContainer.Show();
	}

	public void ShowLevelCompleted(double seconds) {
        GameTimeLabel.Text = GameTimeFormat.FormatGameTime(seconds);
		LevelCompletedContainer.Show();
	}

	public void OnPlayAgainButtonPressed() {
		LevelCompletedContainer.Hide();
		GameOverContainer.Hide();
		EmitSignal(SignalName.PlayAgainButtonPressed);
	}
}
