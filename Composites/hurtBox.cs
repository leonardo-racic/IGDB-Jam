using Godot;
using System;

public partial class hurtBox : Area2D
{
  [Export]
  public HealthNode health;
  [Export]
  public int damage = 0;
  [Export]
  public string[] ignoreTags;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    if (health == null)
      GD.PrintErr("ERROR HEALTHNODE IS INVALID ON HURTBOX PARENT NODE IS ", GetParent().GetName());
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
  }

  public void hit(Area2D area)
  {
    foreach (string item in ignoreTags)
    {
      health.IsInGroup(item);
    }
    //from this point we know that we don't ignore them
    health.Health += -damage;
  }
}
