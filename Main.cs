using Godot;
using System;

public partial class Main : Node2D
{

	private PackedScene _gameScene = GD.Load<PackedScene>("res://game.tscn");

	public required Gui Gui;

	private Game? _game;

	public override void _Ready()
	{
		Gui = GetNode<Gui>("GUI");
		Gui.PlayAgainButtonPressed += Restart;
		Gui.Hide();
		StartGame();
	}

	public void StartGame()
	{
		_game = (Game)_gameScene.Instantiate();
		_game.GameOver += OnGameOver;
		AddChild(_game);
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
		_game.StopGame();
		Gui.Show();
	}

}
