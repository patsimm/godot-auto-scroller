using System;
using Godot;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class VelocityComponent : Component<Node2D> {
	[Export]
	private float _maxAcceleration = 1000;

	[Export]
	public Vector2 InitialVelocity { get; set; } = Vector2.Zero;

	public Vector2 Velocity { get; set; } = Vector2.Zero;

	public override void _Ready() {
		base._Ready();

		Velocity = InitialVelocity;
	}

	public void AddVelocity(Vector2 force) {
		Velocity += force;
	}

	public bool IsFalling() {
		return Velocity.Y > 0;
	}

	public void ApplyVelocityAndSlide(CharacterBody2D characterBody2D) {
		if (characterBody2D is null)
			throw new ArgumentNullException(nameof(characterBody2D));

		characterBody2D.Velocity = Velocity;
		characterBody2D.MoveAndSlide();
		Velocity = characterBody2D.Velocity;
	}

	public void ApplyVelocityAndCollide(PhysicsBody2D rigidBody2D) {
		if (rigidBody2D is null)
			throw new ArgumentNullException(nameof(rigidBody2D));

		var delta = GetPhysicsProcessDeltaTime();
		var collision = rigidBody2D.MoveAndCollide(Velocity * (float)delta);
		if (collision is not null) Velocity = Vector2.Zero;
	}

	public void ApplyVelocity(Node2D node2D) {
		if (node2D is null)
			throw new ArgumentNullException(nameof(node2D));

		var delta = GetPhysicsProcessDeltaTime();
		node2D.Position += Velocity * (float)delta;
	}

	public override void _PhysicsProcess(double delta) {
		if (Engine.IsEditorHint()) return;

		switch (Entity) {
			case CharacterBody2D characterBody2D:
				ApplyVelocityAndSlide(characterBody2D);
				break;
			case PhysicsBody2D physicsBody2D:
				ApplyVelocityAndCollide(physicsBody2D);
				break;
			default:
				ApplyVelocity(Entity);
				break;
		}
	}
}
