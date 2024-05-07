using Godot;

namespace Platformer;

public partial class Main : Node2D {
	public required Gui Gui { get; set; }
    public required Hud Hud { get; set; }
    public required LevelLoaderComponent LevelLoaderComponent { get; set; }
    public required TimeCounter GameTimeCounter { get; set; }

	public override void _Ready() {
		Gui = GetNode<Gui>("GUI");
		Gui.PlayAgainButtonPressed += Restart;

        Hud = GetNode<Hud>("HUD");

        GameTimeCounter = GetNode<TimeCounter>("GameTimeCounter");

        LevelLoaderComponent = GetNode<LevelLoaderComponent>("LevelLoaderComponent");
		StartGame();
	}

	public void StartGame() {
        GameTimeCounter.Reset();
		Gui.Hide();
        LevelLoaderComponent.LoadLevel(1);
	}

	public void Restart() {
		StartGame();
		GetTree().Paused = false;
	}

	public void OnLevelFailed() {
		GetTree().Paused = true;
        Gui.ShowGameOver();
        Gui.Show();
    }

	public void OnLevelCompleted() {
		GetTree().Paused = true;
		Gui.ShowLevelCompleted(GameTimeCounter.ElapsedTimeSeconds);
        Gui.Show();
	}
}
