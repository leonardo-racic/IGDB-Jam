using Godot;
using System;

public partial class destructableObstacle : StaticBody2D
{
  [Export]
  public CircleShape2D collisionShape;
  [Export]
  public Rect2 textureOffset;
  [Export]
  public float health;
  [Export]
  public HealthNode HealthNode;

  [Export]
  private CollisionShape2D[] collisionShapes;
  [Export]
  private Sprite2D spriteNode;
  [Export]
  private NavigationObstacle2D navOb;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    foreach (CollisionShape2D item in collisionShapes)
    {
      item.Shape = collisionShape as Shape2D;
    }
    navOb.Radius = collisionShape.Radius;
    HealthNode.startHealth = health;
    spriteNode.RegionRect = textureOffset;
    HealthNode.Dead += () => onDead();
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
