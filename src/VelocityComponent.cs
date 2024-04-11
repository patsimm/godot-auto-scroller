using Godot;

[Tool]
[GlobalClass]
public partial class VelocityComponent : Component<CharacterBody2D>
{
	[Export]
	private float _maxAcceleration = 1000;

	public Vector2 Velocity { get; set; } = Vector2.Zero;

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
		Parent.Velocity = Velocity;
		Parent.MoveAndSlide();
		Velocity = Parent.Velocity;
	}

}
