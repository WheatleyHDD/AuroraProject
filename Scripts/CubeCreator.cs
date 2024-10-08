using Godot;
using System;

public partial class CubeCreator : RayCast3D
{
	public int TimeUse { get; private set; } = 300;
	
	[Export] private PackedScene _cubeResource;
	[Export] private AudioStreamPlayer _startSound;
	[Export] private AudioStreamPlayer _endSound;
	
	public RigidBody3D CubeCreating;

	private LevelManager _lvlMngr;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GetTree().GetNodesInGroup("LevelManager").Count > 0)
			_lvlMngr = GetTree().GetNodesInGroup("LevelManager")[0] as LevelManager;
		CubeCreating = _cubeResource.Instantiate<RigidBody3D>();
		CubeCreating = null;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (CubeCreating != null)
		{
			TimeUse = Mathf.Max(TimeUse-1, 0);

			if (IsColliding())
				GetNode<Marker3D>("Offset").Position = ToLocal(GetCollisionPoint()) - Vector3.Forward * 0.75f;
			
			CubeCreating.GlobalPosition = IsColliding() ? GetNode<Marker3D>("Offset").GlobalPosition : ToGlobal(GetTargetPosition());
			ChangeScale();
			
			CubeCreating.RotateX(0.01f);
			CubeCreating.RotateY(0.01f);
			CubeCreating.RotateZ(0.01f);
			
			if (TimeUse == 0) CancelCreate();
		}
		else TimeUse = Mathf.Min(TimeUse+1, 300);
		
		if (Input.IsActionJustPressed("create_cube"))
		{
			if (TimeUse < 300 || (_lvlMngr != null && _lvlMngr.CubesPossible == 0))
			{
				return;
			}
			
			CubeCreating = _cubeResource.Instantiate<RigidBody3D>();
			CubeCreating.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true;
			GetTree().CurrentScene.AddChild(CubeCreating);
			Engine.SetTimeScale(0.1);
			_startSound.Play();
			if (_lvlMngr != null) _lvlMngr.CubesPossible -= 1;
		}
		
		if (Input.IsActionJustReleased("create_cube"))
			CancelCreate();
	}

	public void CancelCreate()
	{
		if (CubeCreating != null)
		{
			var tpos = ToGlobal(GetTargetPosition()) - GlobalPosition;
			CubeCreating.ApplyCentralImpulse(tpos * 10);
			CubeCreating.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false;
		}
		
		CubeCreating = null;
		Engine.SetTimeScale(1.0);
		_endSound.Play();
	}

	private void ChangeScale()
	{
		var col = CubeCreating.GetNode<CollisionShape3D>("CollisionShape3D");
		var mesh = CubeCreating.GetNode<MeshInstance3D>("MeshInstance3D");
		var i = 0.05f;
		
		if (Input.IsActionJustReleased("create_cube_min") && col.Scale.X > 0.1f)
		{
			CubeCreating.Mass -= i * 5; 
			col.Scale -= (new Vector3(i, i, i));
			mesh.Scale -= (new Vector3(i, i, i));
		} else if (Input.IsActionJustReleased("create_cube_max") && col.Scale.X < 1.75f)
		{
			CubeCreating.Mass += i * 5;
			col.Scale += (new Vector3(i, i, i));
			mesh.Scale += (new Vector3(i, i, i));
		}
	}
}
