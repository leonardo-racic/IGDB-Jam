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

  void objectIsInView(Node2D body)
  {
    ShapeCast2D ray = GetNode<ShapeCast2D>("Ray");
    ray.TargetPosition = (body.GlobalPosition - ray.GlobalPosition) * 1.5f;
    ray.ForceShapecastUpdate();
    GodotObject col = ray.GetCollider(0);
    fleeing = col.GetInstanceId() == body.GetInstanceId() && body.IsInGroup("Player");
  }
}
