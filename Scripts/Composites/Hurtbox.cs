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

  public void onAreaEntered(Area2D area) {
    if (area is Hitbox) {
      health.Health -= (area as Hitbox).Damage;
    }
  }
}
