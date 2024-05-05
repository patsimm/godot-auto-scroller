using Godot;

namespace Platformer;

public partial class Main : Node2D {
	private PackedScene _gameScene = GD.Load<PackedScene>("res://scenes/game.tscn");

	public required Gui Gui { get; set; }
    public required Hud Hud { get; set; }
    public required TimeCounter GameTimeCounter { get; set; }

	private Game? _game;

	public override void _Ready() {
		Gui = GetNode<Gui>("GUI");
		Gui.PlayAgainButtonPressed += Restart;

        Hud = GetNode<Hud>("HUD");

        GameTimeCounter = GetNode<TimeCounter>("GameTimeCounter");

		StartGame();
	}

	public void StartGame() {
        GameTimeCounter.Reset();
		Gui.Hide();
		_game = (Game)_gameScene.Instantiate();
		_game.GameOver += OnGameOver;
		_game.LevelCompleted += OnLevelCompleted;
		AddChild(_game);
		_game.NewGame();
	}

    public void Restart() {
        _game?.QueueFree();
		StartGame();
		GetTree().Paused = false;
	}

    public void OnGameOver() {
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
