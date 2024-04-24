using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

namespace Platformer;

public partial class FollowNode2DComponent : Component<Node2D> {
    [Export]
    public required VelocityComponent VelocityComponent { get; set; }

    [Export]
    public required Node2D TargetNode { get; set; }

    [Export]
    public required Frame KeepInArea { get; set; }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        var normal = KeepInArea.GetFrameNormal(TargetNode);
        if (normal == Vector2.Zero) {
            VelocityComponent.Decelerate(7, delta);
            return;
        }

        var distance = KeepInArea.DistanceToBody(TargetNode);
        var acceleration = Mathf.Min(Mathf.Abs(distance.LengthSquared()) / 100, 80);

        VelocityComponent.AccelerateToTargetSpeed(900 * normal, acceleration, delta);
    }
}
