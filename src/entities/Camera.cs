using Godot;

namespace Platformer;

public partial class Camera : Camera2D {
	public required VelocityComponent VelocityComponent { get; set; }

	public override void _Ready() {
		VelocityComponent = GetNode<VelocityComponent>("VelocityComponent");
	}
}
