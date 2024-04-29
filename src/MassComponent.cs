using Godot;
using System.Linq;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class MassComponent : Component<CharacterBody2D> {
	[Export]
	public required VelocityComponent VelocityComponent { get; set; }

	[Export]
	private float _mass = 1;

	private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override string[] _GetConfigurationWarnings() {
		var errors = base._GetConfigurationWarnings().ToList();

		if (VelocityComponent is null)
			errors.Add("MassComponent needs a linked VelocityComponent");

		return errors.ToArray();
	}

	public override void _PhysicsProcess(double delta) {
		if (Engine.IsEditorHint())
			return;

        if (Entity.IsOnFloor()) return;

        var gravity = VelocityComponent.IsFalling() ? _gravity * 2 : _gravity;

        VelocityComponent.AddForce(Vector2.Down * _mass * gravity * (float)delta);

        if (Entity.IsOnWall()) {
            VelocityComponent.LimitVerticalSpeed(500f);
        }
        else {
            VelocityComponent.LimitVerticalSpeed(900f);
        }
    }
}
