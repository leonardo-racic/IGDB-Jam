using Godot;
using System;

public partial class destructableObstacle : Sprite2D
{
  [Export]
  private NavigationObstacle2D navOb;
  [Export]
  public NavigationRegion2D navMesh;

  [Export]
  public float health;
  [Export]
  private HealthNode healthNode;

  [Export]
  public CircleShape2D collisionShape;
  [Export]
  public CollisionShape2D[] collShape;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    healthNode.MaxHealth = health;
    healthNode.Health = health;
    foreach (CollisionShape2D i in collShape)
      i.Shape = collisionShape;
    navOb.Radius = collisionShape.Radius;
    navMesh.AddChild(navOb);
    healthNode.Dead += () => onDead();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {

  }

  void onDead()
  {
    QueueFree();
  }
}
