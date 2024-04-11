using Godot;
using System;

public partial class Main : Node2D
{

	private PackedScene _gameScene = GD.Load<PackedScene>("res://scenes/game.tscn");

	public required Gui Gui;

	private Game? _game;

	public override void _Ready()
	{
		Gui = GetNode<Gui>("GUI");
		Gui.PlayAgainButtonPressed += Restart;
		StartGame();
	}

	public void StartGame()
	{
		Gui.Hide();
		_game = (Game)_gameScene.Instantiate();
		_game.GameOver += OnGameOver;
		AddChild(_game);
		_game.NewGame();
	}

	public void Restart()
	{
		if (_game is not null)
		{
			_game.GameOver -= OnGameOver;
			_game.QueueFree();
		}
		StartGame();
	}

	public void OnGameOver()
	{
		GD.Print("Game Over");
		Gui.Show();
		_game?.StopGame();
	}

}
