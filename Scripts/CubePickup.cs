using Godot;
using System;

public partial class CubePickup : Node3D
{
	[Export] private RayCast3D _pickuper;
	[Export] private Node3D _joint;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pickup"))
		{
			if (!_pickuper.IsColliding()) return;
			if (!((Node3D)_pickuper.GetCollider()).IsInGroup("Pickupable")) return;
			
			_joint.Set("node_b", (_pickuper.GetCollider() as RigidBody3D)?.GetPath());
		}

		if (Input.IsActionJustReleased("pickup"))
		{
			_joint.Set("node_b", "");
		}
	}
}
