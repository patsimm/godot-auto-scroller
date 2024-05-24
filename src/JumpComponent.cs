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
    public CoyoteTimeComponent? CoyoteTimeComponent { get; set; }

    [Export]
    private float _jumpAcceleration = 4000;

    [Export]
    private double _jumpDuration = 0.3;

    [Export]
    private double _wallJumpCoolDown = 0.3;

    [Export]
    private double _jumpBufferTime = 0.15;

    private Vector2 _jumpDirection = Vector2.Zero;

    private Timer _wallJumpCoolDownDurationTimer = new();

    private Timer _jumpDurationTimer = new();

    private Timer _jumpBufferTimer = new();

    private bool _jumpActionActive;

    public override void _Ready() {
        base._Ready();

        _jumpDurationTimer.WaitTime = _jumpDuration;
        _jumpDurationTimer.OneShot = true;
        AddChild(_jumpDurationTimer);

        _wallJumpCoolDownDurationTimer.WaitTime = _wallJumpCoolDown;
        _wallJumpCoolDownDurationTimer.OneShot = true;
        AddChild(_wallJumpCoolDownDurationTimer);

        _jumpBufferTimer.WaitTime = _jumpBufferTime;
        _jumpBufferTimer.OneShot = true;
        AddChild(_jumpBufferTimer);
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
        _jumpActionActive = true;
        _jumpBufferTimer.Start();
    }

    public void JumpEnd() {
        _jumpActionActive = false;
    }

    public override void _PhysicsProcess(double delta) {
        if (Engine.IsEditorHint())
            return;

        if (Entity.IsOnCeiling()) {
            _jumpDurationTimer.Stop();
            _jumpBufferTimer.Stop();
        }

        var jumpForce = new Vector2();

        if (_jumpDurationTimer.TimeLeft > 0) {
            jumpForce = Vector2.Up * _jumpAcceleration * (float)delta;
        }

        if (_jumpBufferTimer.TimeLeft > 0 && _jumpDurationTimer.TimeLeft <= 0) {
            if (CoyoteTimeComponent?.CoyoteTimerCounter > 0f || Entity.IsOnFloor()) {
                _jumpDurationTimer.Start();
                _jumpBufferTimer.Stop();
                _wallJumpCoolDownDurationTimer.Stop();
                jumpForce = Vector2.Up * _jumpAcceleration * 0.33f;
            }

            if (Entity.IsOnWallOnly() && _wallJumpCoolDownDurationTimer.TimeLeft <= 0) {
                _jumpDurationTimer.Start();
                _jumpBufferTimer.Stop();
                _wallJumpCoolDownDurationTimer.Start();
                MovementComponent.ReduceMovementSpeed(.1);
                VelocityComponent.ResetVerticalVelocity();
                var jumpDirection = (Vector2.Up + Entity.GetWallNormal()).Normalized();
                jumpForce = jumpDirection * _jumpAcceleration * 0.4f;
            }
        }

        if (_jumpDurationTimer.TimeLeft > 0 && !_jumpActionActive) {
            _jumpDurationTimer.Stop();
        }

        VelocityComponent.AddForce(jumpForce);
    }
}
