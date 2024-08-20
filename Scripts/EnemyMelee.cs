using Godot;

public partial class EnemyMelee : EnemyAI
{
    [ExportCategory("Melee Settings")]
    [Export] private Area3D _hurtArea;
    [Export] public float DamageAmount;
    [Export] private AnimationPlayer _animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        _hurtArea.Connect(Area3D.SignalName.BodyEntered, Callable.From<Node3D>((b) => {
            if (!(b is Player)) return;
            if (DamageAmount == 0) return;
            
            Player.Hurt();
        }));

        Health.Connect(HealthSystem.SignalName.OnDamage, Callable.From<int>(_ => _animationPlayer.Play("hurt")));

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!IsInstanceValid(Player)) return;

        if (HurtTimer.IsStopped() && IsSeePlayer && EyesRaycast.TargetPosition.LengthSquared() <= SightRangeMin + 1)
            _animationPlayer.Play("attack");
    }
}