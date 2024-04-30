using Godot;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class CoyoteTimeComponent : Component<CharacterBody2D> {
    [Export]
    public required VelocityComponent VelocityComponent { get; set; }

    [Export]
    private float _coyoteTime = 0.08f;

    public float CoyoteTimerCounter { get; private set; }

    public override void _PhysicsProcess(double delta) {
        if (Engine.IsEditorHint()) return;

        if (VelocityComponent.Velocity.Y != 0) {
            CoyoteTimerCounter = 0f;
        }

        if (Entity.IsOnFloor()) {
            CoyoteTimerCounter = _coyoteTime;
        }

        if (!Entity.IsOnFloor()) {
            CoyoteTimerCounter -= (float)delta;
        }
    }
}
