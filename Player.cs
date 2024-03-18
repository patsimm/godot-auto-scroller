using Godot;

public partial class Player : CharacterBody2D
{

	public required MovementComponent _movementComponent;

	public override void _Ready()
	{
		_movementComponent = GetNode<MovementComponent>("MovementComponent");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("jump"))
			_movementComponent.JumpStart();
		if (@event.IsActionReleased("jump"))
			_movementComponent.JumpEnd();

		if (@event.IsActionPressed("left"))
			_movementComponent.Direction += Vector2.Left;
		if (@event.IsActionPressed("right"))
			_movementComponent.Direction += Vector2.Right;

		if (@event.IsActionReleased("left"))
			_movementComponent.Direction -= Vector2.Left;
		if (@event.IsActionReleased("right"))
			_movementComponent.Direction -= Vector2.Right;
	}
}
