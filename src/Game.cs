using Godot;

public partial class Game : Node2D
{

	[Signal]
	public delegate void GameOverEventHandler();

	public void OnPlayerHitFloor(Player player)
	{
		GD.Print("Player hit floor");
		EmitSignal(SignalName.GameOver);
	}

	public override void _Ready()
	{
		NewGame();
	}

	public void NewGame()
	{
		GetNode<World>("World").Start();
	}

	public void StopGame()
	{
		GetNode<World>("World").Stop();
	}

}
