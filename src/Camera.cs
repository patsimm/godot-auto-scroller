using Godot;

namespace Platformer;

public partial class Camera : Camera2D {
	public required AutoScrollComponent AutoScrollComponent { get; set; }

	public override void _Ready() {
		AutoScrollComponent = GetNode<AutoScrollComponent>("AutoScrollComponent");
	}
}
