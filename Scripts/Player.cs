using Godot;

public partial class Player : CharacterBody3D
{
	[ExportGroup("Settings")]
	[Export()] public float Speed = 5.0f;
	[Export()] public float Acceleration = 15f;
	[Export()] public float JumpVelocity = 4.5f;

	[ExportGroup("Main Setups")]
	[Export()] public Node3D Head;
	[Export()] private RayCast3D _foot;
	[Export] public HealthSystem Health;
	[Export] public CameraInterpolate Camera;
	[Export] private PackedScene _deadBody;
	
	[ExportGroup("Sounds")]
	[Export()] private AudioStreamPlayer3D _footstep;
	[Export()] private AudioStreamPlayer3D _jump;
	[Export()] private AudioStreamPlayer3D _land;
	[Export()] private Timer _footstepDelay;

	bool _landSoundPlayed = true;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	public Vector3 Direction;
	
	private Vector2 _lookVector;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		
		Health.Connect(HealthSystem.SignalName.OnDamage, Callable.From<int>(dmg =>
		{
			Camera.Shake(0.1f, 0.3f);
		}));
		Health.Connect(HealthSystem.SignalName.Die, Callable.From<Vector3>((_) =>
		{
			var dead = _deadBody.Instantiate<Node3D>();
			GetParent().AddChild(dead);
			dead.GlobalPosition = GlobalPosition + Vector3.Up * 0.4f;
			dead.Rotation = GlobalRotation;
			QueueFree();
		}));
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = new Vector3(Velocity.X, 0, Velocity.Z);
		Vector3 gravVec = new Vector3(0, Velocity.Y, 0);

		// гравитация
		if (!IsOnFloor())
			gravVec += Vector3.Down * Gravity * (float)delta;
		else gravVec = Vector3.Zero;

		if ((_foot.IsColliding() || IsOnFloor()) && !_landSoundPlayed && velocity.Y <= 0)
		{
			//Input.StartJoyVibration(0, 0.85f, 0f, 0.1f);
			_landSoundPlayed = true;
			_land.Play();
		}

		Vector2 inputDir = new Vector2(0, 0);

		// прыжок
		if (Input.IsActionJustPressed("move_jump") && (_foot.IsColliding() || IsOnFloor()))
		{
			_jump.Play();
			gravVec = Vector3.Up * JumpVelocity;

			var t = GetTree().CreateTween();
			t.TweenInterval(0.2f);
			t.TweenCallback(Callable.From(() => _landSoundPlayed = false));
		}

		// движение вроде
		inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		
		if (inputDir != Vector2.Zero && gravVec.Y == 0f && !_footstep.Playing && _footstepDelay.IsStopped())
		{
			_footstep.Play();
			_footstepDelay.Start();
		}

		Direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		velocity = velocity.Lerp(Direction * Speed, Acceleration * (float)delta);
		Velocity = velocity + gravVec;

		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * GetNode<CanvasLayer>("/root/Overlays/Settings").Get("sensitivity").AsSingle()));
			Head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * GetNode<CanvasLayer>("/root/Overlays/Settings").Get("sensitivity").AsSingle()));
			Vector3 rotDeg = Head.RotationDegrees;
			rotDeg.X = Mathf.Clamp(rotDeg.X, -89f, 89f);
			Head.RotationDegrees = rotDeg;
		}
	}

	public void Hurt()
	{
		Health.Damage(1, Vector3.Zero);
	}
	
	/*
	public override void _Process(double delta)
	{
		if (Current)
		{
			_lookVector.Y = Input.GetActionStrength("look_up") - Input.GetActionStrength("look_down");
			_lookVector.X = Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left");
			GpCameraRotation();
		}
	}

	private void GpCameraRotation() {
		if (Input.MouseMode != Input.MouseModeEnum.Captured) return;
		if (!(_lookVector.Length() > 0)) return;
		
		var horizontal = -_lookVector.X * (Sensivity * 5);
		var vertical = _lookVector.Y * (Sensivity * 5);
		
		_lookVector = Vector2.Zero;
		
		RotateY(Mathf.DegToRad(horizontal));
		Head.RotateX(Mathf.DegToRad(vertical)); 
			
		// Clamp mouse rotation
		var rotDeg = Head.RotationDegrees;
		rotDeg.X = Mathf.Clamp(rotDeg.X, -89f, 89f);
		Head.RotationDegrees = rotDeg;
	}
	*/
}
