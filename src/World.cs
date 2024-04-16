using Godot;

namespace Platformer;

public partial class World : Node {
	[Export]
	public float Speed { get; set; } = 100;

	public required Node2D Blocks { get; set; }

	private bool _running;

	public override void _Ready() {
		Blocks = GetNode<Node2D>("Blocks");
	}

	public void Start() {
		_running = true;
	}

	public void Stop() {
		_running = false;
	}

	public override void _PhysicsProcess(double delta) {
		if (!_running)
			return;
		Blocks.Translate(Vector2.Down * Speed * (float)delta);
	}
}
