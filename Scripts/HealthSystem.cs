using Godot;

public partial class HealthSystem : Node3D
{
	[Export()] public float InitialHealth = 100;

	public float Health { get; private set; } = 100;


    [Signal] public delegate void DieEventHandler(Vector3 impulse);
	[Signal] public delegate void OnDamageEventHandler(int damage);

	public override void _Ready()
	{
		Health = InitialHealth;
	}

	public void Damage(float dmg, Vector3 impulse)
	{
		if (Health == 0) return;

		EmitSignal(SignalName.OnDamage, dmg);
		Health = Mathf.Max(Health - dmg, 0);

		if (Health == 0) EmitSignal(SignalName.Die, impulse);
	}

	public void Heal(float val)
	{
        Health = Mathf.Min(InitialHealth, Health + val);
	}
}