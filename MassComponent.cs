using Godot;
using System.Collections.Generic;

[Tool]
[GlobalClass]
public partial class MassComponent : Node2D
{

	[Export]
	public required VelocityComponent VelocityComponent;

	[Export]
	private float _mass = 1;

	public required CharacterBody2D CharacterBody;

	private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		CharacterBody = GetParent<CharacterBody2D>();
	}

	public override string[] _GetConfigurationWarnings()
	{
		var errors = new List<string> { };

		if (VelocityComponent is null)
			errors.Add("MassComponent needs a linked VelocityComponent");

		if (GetParent() is not CharacterBody2D)
			errors.Add("MassComponent must be child of CharacterBody2D");

		return errors.ToArray();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) return;
		if (!CharacterBody.IsOnFloor())
		{
			VelocityComponent.AddVelocity(Vector2.Down * _mass * _gravity * (float)delta);

			if (VelocityComponent.Velocity.Y > 500 && CharacterBody.IsOnWall())
			{
				VelocityComponent.Velocity = new Vector2(VelocityComponent.Velocity.X, 500);
			}

			if (VelocityComponent.Velocity.Y > 900)
			{
				VelocityComponent.Velocity = new Vector2(VelocityComponent.Velocity.X, 900);
			}


		}
	}

}
