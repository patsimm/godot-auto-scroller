using Godot;

namespace Platformer;

public partial class World : Node {
	[Signal]
	public delegate void LevelCompletedEventHandler();

	public void OnPlayerReachedGoal() {
		EmitSignal(SignalName.LevelCompleted);
	}
}
