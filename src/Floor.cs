using Godot;

public partial class Floor : Area2D
{

	[Signal]
	public delegate void PlayerHitFloorEventHandler(Player player);

	public override void _EnterTree()
	{
		BodyEntered += OnBodyEntered;
	}

	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;
	}

	public void OnBodyEntered(Node2D node2D)
	{
		if (node2D is Player player)
		{
			EmitSignal(SignalName.PlayerHitFloor, player);
		}
	}

}
