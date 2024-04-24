using System;
using Godot;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class VelocityComponent : Component<Node2D> {
    [Export]
    private float _maxVelocity = 850;

    [Export]
	private Vector2 _initialVelocity = Vector2.Zero;

    public Vector2 Velocity { get; set; } = Vector2.Zero;

	public override void _Ready() {
		base._Ready();

		Velocity = _initialVelocity;
	}

	public void AddForce(Vector2 force) {
		Velocity += force;
	}

    public void AccelerateToTargetSpeed(Vector2 targetSpeed, float acceleration, double delta) {
        var speedDelta = targetSpeed.LimitLength(_maxVelocity) - Velocity;

        if (speedDelta == Vector2.Zero)
            return;

        var movement = speedDelta * acceleration * (float)delta;

        AddForce(movement);
    }

    public void Decelerate(float acceleration, double delta) {
        if (_initialVelocity == Velocity) return;
        AccelerateToTargetSpeed(_initialVelocity, acceleration, delta);
    }

	public bool IsFalling() {
		return Velocity.Y > 0;
	}

    private void ApplyVelocityAndSlide(CharacterBody2D characterBody2D) {
		if (characterBody2D is null)
			throw new ArgumentNullException(nameof(characterBody2D));

		characterBody2D.Velocity = Velocity;
		characterBody2D.MoveAndSlide();
		Velocity = characterBody2D.Velocity;
	}

    private void ApplyVelocityAndCollide(PhysicsBody2D rigidBody2D) {
		if (rigidBody2D is null)
			throw new ArgumentNullException(nameof(rigidBody2D));

		var delta = GetPhysicsProcessDeltaTime();
		var collision = rigidBody2D.MoveAndCollide(Velocity * (float)delta);
		if (collision is not null) Velocity = Vector2.Zero;
	}

    private void ApplyVelocity(Node2D node2D) {
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
