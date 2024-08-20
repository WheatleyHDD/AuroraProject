using Godot;
using Godot.Collections;

public partial class EnemyAI : CharacterBody3D
{
	[ExportCategory("Main Settings")]
	[Export] private NavigationAgent3D _navAgent;
	[Export] protected RayCast3D EyesRaycast;
    [Export] private Timer _waitTimer;
	[Export] protected HealthSystem Health;

    [Export] private PackedScene _deadBody;

    [Export] public float SightRangeMax;
    [Export] public float SightRangeMin;
    [Export] public float MoveSpeed;
	
    /*
    [ExportCategory("Sound Settings")]
    [Export] private RandomSoundsPlayer3D DamageSounds;
    */

    public bool IsSeePlayer = false;

    protected Timer HurtTimer;
	protected Vector3 TargetPosition;
    protected Vector3 StartPosition;
    protected Player Player;
    protected float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        HurtTimer = new()
        {
            WaitTime = 0.2f,
            OneShot = true
        };
        AddChild(HurtTimer);

        StartPosition = GlobalPosition;

        _navAgent.Connect(NavigationAgent3D.SignalName.VelocityComputed, Callable.From<Vector3>((safe) =>
        {
            Velocity = new Vector3(safe.X, Velocity.Y, safe.Z);
            MoveAndSlide();
        }));

		_waitTimer.Connect(Timer.SignalName.Timeout, Callable.From(() => IsSeePlayer = false));
		
		Health.Connect(HealthSystem.SignalName.Die, Callable.From<Vector3>((i) =>
		{
            var body = _deadBody.Instantiate<RigidBody3D>();
			GetParent().AddChild(body);
            body.GlobalPosition = GlobalPosition;
            body.GlobalRotation = GlobalRotation;
			body.ApplyCentralImpulse(i);
            QueueFree();
        }));

        if (GetTree().GetNodesInGroup("player").Count > 0)
			if (GetTree().GetNodesInGroup("player")[0] is Player)
				Player = GetTree().GetNodesInGroup("player")[0] as Player;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        Vector3 velocity = Vector3.Zero;

        Vector3 grav_vec = new(0, Velocity.Y, 0);
        grav_vec += Vector3.Down * Gravity * (float)delta;

        if (IsInstanceValid(Player))
		{
			if (CheckSight())
				IsSeePlayer = true;
			else if (IsSeePlayer && _waitTimer.IsStopped())
				_waitTimer.Start();

			if (IsSeePlayer) TargetPosition = Player.GlobalPosition;
			else TargetPosition = StartPosition;
			_navAgent.TargetPosition = TargetPosition;


            if (GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) > SightRangeMin)
			{
                Vector3 newNavPoint = _navAgent.GetNextPathPosition();
                Vector3 newVel = (newNavPoint - GlobalPosition).Normalized() * MoveSpeed;
				if (HurtTimer.IsStopped())
                    velocity = new Vector3(newVel.X, 0, newVel.Z);
			}

			var lookAtTarget = new Vector3(TargetPosition.X, GlobalPosition.Y, TargetPosition.Z);
            if (GlobalPosition.DistanceSquaredTo(lookAtTarget) > 0.5f)
				LookAt(lookAtTarget, Vector3.Up);
        }

		_navAgent.Velocity = velocity;
        Velocity = new Vector3(Velocity.X, grav_vec.Y, Velocity.Z);
    }

	public bool CheckSight()
	{
        if (GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) < SightRangeMax)
		{
			EyesRaycast.TargetPosition = ToLocal(Player.GlobalPosition) - EyesRaycast.Position;
			EyesRaycast.Enabled = true;
			EyesRaycast.ForceRaycastUpdate();

			if (EyesRaycast.IsColliding() && EyesRaycast.GetCollider() is Player)
                return true;

            return false;
        }
        
		EyesRaycast.Enabled = false;
        return false;
    }

	public void Damage(float val, Vector3 impulse)
    {
        IsSeePlayer = true;
        //DamageSounds.PlayRandom();
        
        HurtTimer.Start();
        Health.Damage(val, impulse);
	}
}