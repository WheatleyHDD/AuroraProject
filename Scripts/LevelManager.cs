using Godot;
using System;

public partial class LevelManager : Node
{
	[Export] public int CubesPossible = 0;
	public float TimeElapsed = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		TimeElapsed += (float)delta;
	}
}
