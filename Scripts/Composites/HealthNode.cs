using Godot;

public partial class HealthNode : Node
{
  [Signal]
  public delegate void DeadEventHandler();

  [Export]
  public float MaxHealth
  {
	get => maxHealth;
	set => maxHealth = Mathf.Max(1.0f, value);
  }

  [Export]
  //if its negative it'll start at maxHealth
  public float startHealth = -1;

  public float Health
  {
	get => health;
	set
	{
	  health = Mathf.Clamp(value, 0.0f, maxHealth);

	  if (health <= 0.0f)
		EmitSignal(nameof(Dead));
	}
  }

  private float maxHealth;
  private float health;

  public override void _Ready()
  {
	  Health = startHealth < 0 ? maxHealth : startHealth;
  }
}
