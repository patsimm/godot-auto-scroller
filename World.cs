using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class World : Node
{

	[Export]
	public float _speed = 100;

	public required Node2D Blocks;

	private bool _running = false;

	public override void _Ready()
	{
		Blocks = GetNode<Node2D>("Blocks");
	}

	public void Start()
	{
		_running = true;
	}

	public void Stop()
	{
		_running = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_running) return;
		Blocks.Translate(Vector2.Down * _speed * (float)delta);
	}
}
