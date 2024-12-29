using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
  bool fleeing = false;
  Node2D pl = null;
  [Export]
  public EnemyMovment movment;
  [Export]
  public float TargetDistance = 200.0f;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (fleeing)
    {
      //points away
      Vector2 away = (GlobalPosition - pl.GlobalPosition); //(away)
      //(not towards - away)
      float mag = TargetDistance - away.Length();
      Velocity = away.Normalized() * mag * (float)delta;
    }
    else
      Velocity = Vector2.Zero;
    GD.Print(Velocity);
  }

  public override void _PhysicsProcess(double delta)
  {
    movment.move(10, this);
  }

  void objectIsInView(Area2D area)
  {
    if (!area.IsInGroup("Player"))
      return;
    ShapeCast2D ray = GetNode<ShapeCast2D>("Ray");
    ray.TargetPosition = (area.GlobalPosition - ray.GlobalPosition);
    ray.ForceShapecastUpdate();
    if (ray.GetCollisionCount() == 0)
      return;
    GodotObject col = ray.GetCollider(0);
    fleeing = col.GetInstanceId() == area.GetInstanceId();
    if (fleeing)
      pl = area;
  }

  void objectLeftView(Area2D area)
  {
    fleeing = !area.IsInGroup("Player");
  }
}
