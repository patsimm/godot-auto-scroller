using Godot;

namespace Platformer;

public partial class FollowNode2DComponent : Component<Node2D> {
    [Export]
    public required Node2D TargetNode { get; set; }

    [Export]
    public required Frame KeepInArea { get; set; }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        var normal = KeepInArea.GetFrameNormal(TargetNode);
        if (normal != Vector2.Zero) {
            var distance = KeepInArea.DistanceToBody(TargetNode);
            GD.Print(distance.ToString());
            Entity.Position += distance;
        }
    }
}
