using System.Linq;
using Godot;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class JumpComponent : Component<CharacterBody2D> {
    [Export]
    public required VelocityComponent VelocityComponent { get; set; }

    [Export]
    public required MovementComponent MovementComponent { get; set; }

    [Export]
    private float _jumpAcceleration = 4000;

    [Export]
    private double _jumpDuration = 0.3;

    [Export]
    private double _wallJumpCoolDown = 0.3;

    private Timer _wallJumpCoolDownDurationTimer = new();

    private Vector2 _jumpDirection = Vector2.Zero;

    private Timer _jumpDurationTimer = new();

    public override void _Ready() {
        base._Ready();

        _jumpDurationTimer.WaitTime = _jumpDuration;
        _jumpDurationTimer.OneShot = true;
        AddChild(_jumpDurationTimer);

        _wallJumpCoolDownDurationTimer.WaitTime = _wallJumpCoolDown;
        _wallJumpCoolDownDurationTimer.OneShot = true;
        AddChild(_wallJumpCoolDownDurationTimer);
    }

    public override string[] _GetConfigurationWarnings() {
        var errors = base._GetConfigurationWarnings().ToList();

        if (VelocityComponent is null)
            errors.Add("JumpComponent needs a linked VelocityComponent");

        if (MovementComponent is null)
            errors.Add("JumpComponent needs a linked MovementComponent");

        return errors.ToArray();
    }

    public void JumpStart() {
        if (Entity.IsOnFloor()) {
            _jumpDurationTimer.Start();
            _wallJumpCoolDownDurationTimer.Stop();
            VelocityComponent.AddVelocity(Vector2.Up * _jumpAcceleration * 0.33f);
        }

        if (Entity.IsOnWallOnly() && _wallJumpCoolDownDurationTimer.TimeLeft <= 0) {
            _jumpDurationTimer.Start();
            _wallJumpCoolDownDurationTimer.Start();
            MovementComponent.ReduceMovementSpeed(.2);
            var jumpDirection = (Vector2.Up + Entity.GetWallNormal()).Normalized();
            VelocityComponent.AddVelocity(jumpDirection * _jumpAcceleration * 0.4f);
        }
    }

    public void JumpEnd() {
        if (_jumpDurationTimer.TimeLeft > 0)
            _jumpDurationTimer.Stop();
    }

    public override void _PhysicsProcess(double delta) {
        if (Engine.IsEditorHint())
            return;

        if (Entity.IsOnCeiling()) {
            _jumpDurationTimer.Stop();
        }

        if (_jumpDurationTimer.TimeLeft > 0) {
            GD.Print(delta);
            VelocityComponent.AddVelocity(Vector2.Up * _jumpAcceleration * (float)delta);
        }
    }
}
