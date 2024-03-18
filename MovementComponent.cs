using Godot;
using System;
using System.Collections.Generic;

[Tool]
[GlobalClass]
public partial class MovementComponent : Node2D
{

	[Export]
	public required VelocityComponent VelocityComponent;

	[Export]
	private float _movementMaxSpeed = 400;

	[Export]
	private float _movementAcceleration = 400;

	[Export]
	private float _jumpAcceleration = 4000;

	[Export]
	private double _jumpDuration = 0.3;

	[Export]
	private double _wallJumpCoolDown = 0.3;

	public required CharacterBody2D CharacterBody;

	public Vector2 Direction = Vector2.Zero;

	private Timer _jumpDurationTimer = new();

	private Timer _wallJumpCoolDownDurationTimer = new();

	private Timer _blockMovementTimer = new();

	private Vector2 _jumpDirection = Vector2.Zero;

	public override void _Ready()
	{
		CharacterBody = GetParent<CharacterBody2D>();

		_jumpDurationTimer.WaitTime = _jumpDuration;
		_jumpDurationTimer.OneShot = true;
		AddChild(_jumpDurationTimer);

		_wallJumpCoolDownDurationTimer.WaitTime = _wallJumpCoolDown;
		_wallJumpCoolDownDurationTimer.OneShot = true;
		AddChild(_wallJumpCoolDownDurationTimer);

		_blockMovementTimer.OneShot = true;
		AddChild(_blockMovementTimer);
	}

	public override string[] _GetConfigurationWarnings()
	{
		var errors = new List<string> { };

		if (VelocityComponent is null)
			errors.Add("MovementComponent needs a linked VelocityComponent");

		if (GetParent() is not CharacterBody2D)
			errors.Add("MovementComponent must be child of CharacterBody2D");

		return errors.ToArray();
	}

	public void JumpStart()
	{
		if (CharacterBody.IsOnFloor())
		{
			_jumpDurationTimer.Start();
			_wallJumpCoolDownDurationTimer.Stop();
			VelocityComponent.AddVelocity(Vector2.Up * _jumpAcceleration * 0.33f);
		}

		if (CharacterBody.IsOnWallOnly() && _wallJumpCoolDownDurationTimer.TimeLeft <= 0)
		{
			_jumpDurationTimer.Start();
			_wallJumpCoolDownDurationTimer.Start();
			_blockMovementTimer.WaitTime = 1;
			_blockMovementTimer.Start();
			var jumpDirection = (Vector2.Up + CharacterBody.GetWallNormal()).Normalized();
			VelocityComponent.AddVelocity(jumpDirection * _jumpAcceleration * 0.4f);
		}
	}

	public void JumpEnd()
	{
		if (_jumpDurationTimer.TimeLeft > 0)
			_jumpDurationTimer.Stop();
	}

	private void PerformMovement(double delta)
	{
		var maxSpeedFactor = _blockMovementTimer.TimeLeft > 0
			? 1 / Math.Pow(_blockMovementTimer.WaitTime / (_blockMovementTimer.WaitTime - _blockMovementTimer.TimeLeft), 2)
			: 1;

		var acceleration = CharacterBody.IsOnFloor() ? _movementAcceleration : _movementAcceleration / 3;

		var targetSpeed = Direction.X * _movementMaxSpeed * (float)maxSpeedFactor;
		var speedDelta = targetSpeed - VelocityComponent.Velocity.X;

		if (speedDelta == 0) return;

		var movement = speedDelta * acceleration * (float)delta;

		VelocityComponent.AddVelocity(movement * Vector2.Right);
	}

	private void PerformJump(double delta)
	{
		var targetSpeed = Direction.X * _movementMaxSpeed;
		var speedDelta = targetSpeed - VelocityComponent.Velocity.X;

		if (speedDelta == 0) return;

		var movement = speedDelta * _movementAcceleration * (float)delta;

		VelocityComponent.AddVelocity(movement * Vector2.Right);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) return;

		PerformMovement(delta);

		if (CharacterBody.IsOnCeiling())
		{
			_jumpDurationTimer.Stop();
		}

		if (_jumpDurationTimer.TimeLeft > 0)
		{
			GD.Print(delta);
			VelocityComponent.AddVelocity(Vector2.Up * _jumpAcceleration * (float)delta);
		}
	}
}
