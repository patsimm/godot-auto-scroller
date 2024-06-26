using Godot;
using System;
using System.Linq;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class MovementComponent : Component<CharacterBody2D> {
    [Export] public required VelocityComponent VelocityComponent { get; set; }

    [Export] private float _movementMaxSpeed = 400;

    [Export] private float _movementAcceleration = 400;

    public Vector2 Direction { get; set; } = Vector2.Zero;

    private Timer _reduceMovementSpeedTimer = new();

    public override void _Ready() {
        base._Ready();

        _reduceMovementSpeedTimer.OneShot = true;
        AddChild(_reduceMovementSpeedTimer);
    }

    public override string[] _GetConfigurationWarnings() {
        var errors = base._GetConfigurationWarnings().ToList();

        if (VelocityComponent is null)
            errors.Add("MovementComponent needs a linked VelocityComponent");

        return errors.ToArray();
    }

    private void PerformMovement(double delta) {
        var maxSpeedFactor = _reduceMovementSpeedTimer.TimeLeft > 0
            ? 1 / Math.Pow(
                _reduceMovementSpeedTimer.WaitTime /
                (_reduceMovementSpeedTimer.WaitTime - _reduceMovementSpeedTimer.TimeLeft), 2)
            : 1;

        var acceleration = Entity.IsOnFloor() ? _movementAcceleration : _movementAcceleration / 2;

        var targetSpeedX = Direction.X * _movementMaxSpeed * (float)maxSpeedFactor;
        var targetSpeed = new Vector2(targetSpeedX, VelocityComponent.Velocity.Y);

        acceleration = targetSpeed == Vector2.Zero ? acceleration * 2 : acceleration;

        VelocityComponent.AccelerateToTargetVelocity(targetSpeed, acceleration, delta);
    }

    public override void _PhysicsProcess(double delta) {
        if (Engine.IsEditorHint())
            return;

        PerformMovement(delta);
    }

    public void ReduceMovementSpeed(double cooldownTimeSeconds) {
        _reduceMovementSpeedTimer.WaitTime = cooldownTimeSeconds;
        _reduceMovementSpeedTimer.Start();
    }
}
