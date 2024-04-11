using Godot;

public partial class Player : CharacterBody2D
{

	public required MovementComponent MovementComponent;

	public required JumpComponent JumpComponent;

	public override void _Ready()
	{
		MovementComponent = GetNode<MovementComponent>("MovementComponent");
		JumpComponent = GetNode<JumpComponent>("JumpComponent");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("jump"))
			JumpComponent.JumpStart();
		if (@event.IsActionReleased("jump"))
			JumpComponent.JumpEnd();

		if (@event.IsActionPressed("left"))
			MovementComponent.Direction += Vector2.Left;
		if (@event.IsActionPressed("right"))
			MovementComponent.Direction += Vector2.Right;

		if (@event.IsActionReleased("left"))
			MovementComponent.Direction -= Vector2.Left;
		if (@event.IsActionReleased("right"))
			MovementComponent.Direction -= Vector2.Right;
	}
}
