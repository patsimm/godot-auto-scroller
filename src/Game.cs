using Godot;

namespace Platformer;

public partial class Game : Node2D {
	[Signal]
	public delegate void GameOverEventHandler();

	[Signal]
	public delegate void GameStartEventHandler();

	[Signal]
	public delegate void LevelCompletedEventHandler();

	public void OnPlayerHitFloor(Player _) {
		StopGame();
	}

	public void OnLevelCompleted() {
		EmitSignal(SignalName.LevelCompleted);
	}

	public void NewGame() {
		EmitSignal(SignalName.GameStart);
	}

	public void StopGame() {
		EmitSignal(SignalName.GameOver);
	}
}
