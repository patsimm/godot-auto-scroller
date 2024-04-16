using Godot;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class AutoScrollComponent : Component<Camera2D> {
	[Export]
	private float _speed = 100;

	private bool _running = true;

	private float _startPosition;

	public override void _Ready() {
		base._Ready();
		_startPosition = Position.X;
	}

	public void StartScrolling() {
		_running = true;
	}

	public void StopScrolling() {
		_running = false;
	}

	public override void _PhysicsProcess(double delta) {
		if (!_running || Engine.IsEditorHint())
			return;

		Entity.Translate(Vector2.Up * (float)delta * _speed);
	}
}
