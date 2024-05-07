using Godot;

namespace Platformer;

public partial class Level : Node2D {
	[Signal]
	public delegate void LevelFailedEventHandler();

	[Signal]
	public delegate void LevelCompletedEventHandler();

    public void OnPlayerHitFloor(Player _) {
		EmitSignal(SignalName.LevelFailed);
	}

	public void OnLevelCompleted() {
		EmitSignal(SignalName.LevelCompleted);
	}
}
