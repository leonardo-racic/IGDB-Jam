using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
  [Export]
  public EnemyMovment movment;
  [Export] // used by EnemyMovment
  public NavigationAgent2D Nav;
  [Export]
  public Node2D Pivot;
  [Export]
  public int stepCount = 3;
  [Export]
  public HealthNode healthNode;
  [Export]
  public AnimatedSprite2D Hand;
  [Export]
  public AnimatedSprite2D Sprite;
  [Export]
  public int jewelDropCount;
  [Export]
  public Vector2 minMaxJewelSpeed;
  [Export]
  public PackedScene jewelScene;

  public float HeldGiftsAmount;

  MainScene main;

  private Node2D pl = null;
  private int PlayerCollisionID = 2;

  public override void _Ready()
  {
    if (minMaxJewelSpeed.X > minMaxJewelSpeed.Y)
      GD.PrintErr("ERROR: minimum jewel speed is higher than maximum on enemy");
    healthNode.Dead += () => onDead();
    Sprite.Play("default");
    Hand.Play("hand");
    main = GetTree().CurrentScene as MainScene;
  }

  public override void _PhysicsProcess(double delta)
  {
    movment.move(stepCount, this);
  }
  
  public void onDead()
  {
    //WE SPAWN THEM DOUBLOONS
    while (jewelDropCount > 0)
    {
      jewel instance = jewelScene.Instantiate() as jewel;
      main.CallDeferred("add_child", instance);
      RandomNumberGenerator rand = new RandomNumberGenerator();
      float angle = rand.Randf() * MathF.PI;
      instance.GlobalPosition = GlobalPosition;
      instance.Velocity = Vector2.FromAngle(angle);
      float mag = (rand.Randf() * (minMaxJewelSpeed.Y - minMaxJewelSpeed.X)) + minMaxJewelSpeed.X;
      instance.Velocity *= mag;
      jewelDropCount--;
    }
    QueueFree();
  }
}
