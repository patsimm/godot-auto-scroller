using Godot;

namespace Platformer;

public partial class LevelGoal : Area2D {
	[Signal]
	public delegate void PlayerReachedGoalEventHandler();

	public override void _EnterTree() {
		BodyEntered += OnBodyEntered;
	}

	public override void _ExitTree() {
		BodyEntered -= OnBodyEntered;
	}

	public override void _Process(double delta) {
	}

	public void OnBodyEntered(Node2D node2D) {
		if (node2D is Player) {
			EmitSignal(SignalName.PlayerReachedGoal);
		}
	}
}
