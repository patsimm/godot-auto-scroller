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

    [ExportGroup("LevelCompleted")]
    [Export]
    public required Label HighscoreLabel { get; set; }

	public override void _Ready() {
		GameOverContainer.Hide();
		LevelCompletedContainer.Hide();
	}

	public void ShowGameOver() {
		GameOverContainer.Show();
	}

	public void ShowLevelCompleted(double seconds, double highscoreSeconds) {
        GameTimeLabel.Text = GameTimeFormat.FormatGameTime(seconds);
        HighscoreLabel.Text = GameTimeFormat.FormatGameTime(highscoreSeconds);
		LevelCompletedContainer.Show();
	}

	public void OnPlayAgainButtonPressed() {
		LevelCompletedContainer.Hide();
		GameOverContainer.Hide();
		EmitSignal(SignalName.PlayAgainButtonPressed);
	}
}
