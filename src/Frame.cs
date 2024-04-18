using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class Frame : Node2D {
	private float _Width;
	[Export]
	public float Width {
		get => _Width;
		set {
			_Width = value;
			UpdateDimensions();
		}
	}

	private float _Height;
	[Export]
	public float Height {
		get => _Height;
		set {
			_Height = value;
			UpdateDimensions();
		}
	}

	public Area2D? Top { get; set; }
	public Area2D? Bottom { get; set; }
	public Area2D? Left { get; set; }
	public Area2D? Right { get; set; }

	public override void _Ready() {
		base._Ready();

		Top = GetNode<Area2D>("Top");
		Bottom = GetNode<Area2D>("Bottom");
		Left = GetNode<Area2D>("Left");
		Right = GetNode<Area2D>("Right");

		UpdateDimensions();
	}

	private void UpdateDimensions() {
		if (Right is null || Left is null || Top is null || Bottom is null) return;
		Top.Position = new Vector2(Width / 2, 0);
		Bottom.Position = new Vector2(Width / 2, Height);
		Right.Position = new Vector2(Width, Height / 2);
		Left.Position = new Vector2(0, Height / 2);
	}

	public bool OverlapsBody(Node2D node) {
		return GetFrameNormal(node) != Vector2.Zero;
	}

	public Vector2 GetFrameNormal(Node2D node) {
		if (Right is null || Left is null || Top is null || Bottom is null)
			throw new MissingFieldException("Frame Boundaries are null!");

		var normal = new Vector2();
		if (Top.OverlapsBody(node)) normal += Vector2.Up;
		if (Bottom.OverlapsBody(node)) normal += Vector2.Down;
		if (Left.OverlapsBody(node)) normal += Vector2.Left;
		if (Right.OverlapsBody(node)) normal += Vector2.Right;
		return normal;
	}

	public Vector2 DistanceToBody(Node2D node) {
		if (Right is null || Left is null || Top is null || Bottom is null)
			throw new MissingFieldException("Frame Boundaries are null!");

		if (node is null) throw new ArgumentNullException(nameof(node));

		var yMin = Top.GlobalPosition.Y;
		var yMax = Bottom.GlobalPosition.Y;
		var xMin = Left.GlobalPosition.X;
		var xMax = Right.GlobalPosition.X;

		var distX = new List<float> { node.GlobalPosition.X - xMax, 0f, xMin - node.GlobalPosition.X }.Max();
		var distY = new List<float> { node.GlobalPosition.Y - yMax, 0f, yMin - node.GlobalPosition.Y }.Max();

		return new Vector2(distX, distY) * GetFrameNormal(node);
	}
}
