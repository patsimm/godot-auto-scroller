using Godot;

namespace Platformer;

public partial class Game : Node2D {
	[Signal]
	public delegate void GameOverEventHandler();

	public void OnPlayerHitFloor(Player _) {
		GD.Print("Player hit floor");
		EmitSignal(SignalName.GameOver);
	}

	public void NewGame() {
		var scroller = GetNode<Scroller>("Scroller");
		scroller.StartScrolling();
	}

	public void StopGame() {
		GetNode<Scroller>("Scroller").StopScrolling();
	}
}
