using System;
using System.Collections.Generic;
using Godot;

[Tool]
[GlobalClass]
public partial class VelocityComponent : Node2D
{
	[Export]
	private float _maxAcceleration = 1000;

	public required CharacterBody2D CharacterBody;

	public Vector2 Velocity { get; set; } = Vector2.Zero;

	public override void _Ready()
	{
		CharacterBody = GetParent<CharacterBody2D>();
	}

	public override string[] _GetConfigurationWarnings()
	{
		var errors = new List<string> { };

		if (GetParent() is not CharacterBody2D)
			errors.Add("MovementComponent must be child of CharacterBody2D");

		return errors.ToArray();
	}

	public void AddVelocity(Vector2 force)
	{
		Velocity += force;
	}

	public bool IsFalling()
	{
		return Velocity.Y > 0;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) return;
		CharacterBody.Velocity = Velocity;
		CharacterBody.MoveAndSlide();
		Velocity = CharacterBody.Velocity;
	}

}
