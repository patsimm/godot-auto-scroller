using System;
using Godot;

namespace Platformer;

public partial class Player : CharacterBody2D {
	public required MovementComponent MovementComponent { get; set; }

	public required JumpComponent JumpComponent { get; set; }

	public override void _Ready() {
		MovementComponent = GetNode<MovementComponent>("MovementComponent");
		JumpComponent = GetNode<JumpComponent>("JumpComponent");
	}

	public override void _Input(InputEvent @event) {
		if (@event is null) throw new ArgumentNullException(nameof(@event));

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
