using Godot;
using System;

namespace Platformer;

public partial class MovingPlatform : Node2D {
	[Export]
	private float _speedScale = 1.0f;

	[Export]
	private bool _closedPath;

	public required PathFollow2D PathFollow { get; set; }

	public required AnimationPlayer AnimationPlayer { get; set; }

	public override void _Ready() {
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		PathFollow = GetNode<PathFollow2D>("PathFollow2D");

		AnimationPlayer.SpeedScale = _speedScale;

		AnimationPlayer.Play(!_closedPath ? "moveOpenPath" : "moveClosedPath");
	}
}
