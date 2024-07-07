using Godot;

namespace Platformer;

public partial class Level : Node2D {
	[Signal]
	public delegate void LevelFailedEventHandler();

	[Signal]
	public delegate void LevelCompletedEventHandler();

	public required Area2D Floor { get; set; }

	public override void _Ready() {
		Floor = GetNode<Area2D>("Camera2D/Floor");

		Floor.BodyEntered += OnBodyEnteredFloor;
	}

	public void OnBodyEnteredFloor(Node2D node2D) {
		if (node2D is Player) {
			EmitSignal(SignalName.LevelFailed);
		}
	}

	public void OnLevelCompleted() {
		EmitSignal(SignalName.LevelCompleted);
	}
}
