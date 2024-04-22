using Godot;

namespace Platformer;

public partial class Main : Node2D {
	private PackedScene _gameScene = GD.Load<PackedScene>("res://scenes/game.tscn");

	public required Gui Gui { get; set; }

	private Game? _game;

	public override void _Ready() {
		Gui = GetNode<Gui>("GUI");
		Gui.PlayAgainButtonPressed += Restart;
		StartGame();
	}

	public void StartGame() {
		Gui.Hide();
		_game = (Game)_gameScene.Instantiate();
		_game.GameOver += OnGameOver;
		_game.LevelCompleted += OnLevelCompleted;
		AddChild(_game);
		_game.NewGame();
	}

	public void Restart() {
		if (_game is not null) {
			_game.GameOver -= OnGameOver;
			_game.QueueFree();
		}
		StartGame();
		GetTree().Paused = false;
	}

	public void OnGameOver() {
		GetTree().Paused = true;
		Gui.Show();
		Gui.ShowGameOver();
	}

	public void OnLevelCompleted() {
		Gui.Show();
		Gui.ShowLevelCompleted();
	}
}
