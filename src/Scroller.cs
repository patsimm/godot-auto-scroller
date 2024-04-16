using Godot;

namespace Platformer;

public partial class Scroller : Node2D {
	[Export]
	private float _speed = 100;

	private bool _running = true;

	private float _startPosition;

	public override void _Ready() {
		_startPosition = Position.X;
	}

	public void StartScrolling() {
		_running = true;
	}

	public void StopScrolling() {
		_running = false;
	}

	public override void _PhysicsProcess(double delta) {
		if (!_running)
			return;
		Translate(Vector2.Up * (float)delta * _speed);
	}
}
