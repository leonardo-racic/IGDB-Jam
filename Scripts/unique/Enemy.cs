using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
  bool fleeing = false;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    GD.Print(fleeing);
  }

  void objectIsInView(Area2D area)
  {
    if (!area.IsInGroup("Player"))
      return;
    ShapeCast2D ray = GetNode<ShapeCast2D>("Ray");
    Sprite2D debug = GetNode<Sprite2D>("Icon");
    ray.TargetPosition = (area.GlobalPosition - ray.GlobalPosition);
    debug.GlobalPosition = ray.TargetPosition + ray.GlobalPosition;
    ray.ForceShapecastUpdate();
    if (ray.GetCollisionCount() == 0)
      return;
    GodotObject col = ray.GetCollider(0);
    fleeing = col.GetInstanceId() == area.GetInstanceId();
  }

  void objectLeftView(Area2D area)
  {
    if (area.IsInGroup("Player"))
      return;
  }
}
