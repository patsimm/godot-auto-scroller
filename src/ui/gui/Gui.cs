using Godot;

namespace Platformer;

public partial class Gui : CanvasLayer {
	[Signal]
	public delegate void PlayAgainButtonPressedEventHandler();

	[Signal]
	public delegate void LevelSelectedEventHandler(int level);

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

	[ExportGroup("LevelSelection")]
	[Export]
	public required BoxContainer LevelSelectionContainer { get; set; }

	public override void _Ready() {
		HideAllGUIContainer();
	}

	public void ShowLevelSelection() {
		HideAllGUIContainer();
		LevelSelectionContainer.Show();
	}

	public void ShowGameOver() {
		HideAllGUIContainer();
		GameOverContainer.Show();
	}

	public void ShowLevelCompleted(double seconds, double highscoreSeconds) {
		HideAllGUIContainer();
		GameTimeLabel.Text = GameTimeFormat.FormatGameTime(seconds);
		HighscoreLabel.Text = GameTimeFormat.FormatGameTime(highscoreSeconds);
		LevelCompletedContainer.Show();
	}

	public void OnPlayAgainButtonPressed() {
		HideAllGUIContainer();
		EmitSignal(SignalName.PlayAgainButtonPressed);
	}

	public void OnLevelSelectionButtonPressed() {
		HideAllGUIContainer();
		LevelSelectionContainer.Show();
	}

	public void OnLevelSelected(int level) {
		HideAllGUIContainer();
		EmitSignal(SignalName.LevelSelected, level);
	}

	private void HideAllGUIContainer() {
		GameOverContainer.Hide();
		LevelCompletedContainer.Hide();
		LevelSelectionContainer.Hide();
	}
}
