using Godot;

public partial class Hurtbox : Area2D
{
  [Export]
  public HealthNode health;

  public override void _Ready()
  {
    if (health == null)
      GD.PrintErr("ERROR HEALTHNODE IS INVALID ON HURTBOX PARENT NODE IS ", GetParent().GetName());
  }

  public void TakeDamage(float damage)
  {
    health.Health -= damage;
  }
}
