using Godot;

namespace Platformer;

public partial class Level : Node2D {
	[Signal]
	public delegate void LevelFailedEventHandler();

	[Signal]
	public delegate void LevelCompletedEventHandler();

    public void OnBodyEnteredFloor(Node2D node2D) {
        if (node2D is Player) {
            EmitSignal(SignalName.LevelFailed);
        }
	}

	public void OnLevelCompleted() {
		EmitSignal(SignalName.LevelCompleted);
	}
}
