using Godot;

public partial class Enemy : CharacterBody2D
{
  [Export]
  public EnemyMovment movment;
  [Export] // used by EnemyMovment
  public NavigationAgent2D Nav;
  [Export]
  public float TargetDistance = 200.0f;
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
  public PackedScene jewelScene;

  MainScene main;

  private bool fleeing = false;
  private Node2D pl = null;
  private int PlayerCollisionID = 2;

  public override void _Ready()
  {
    healthNode.Dead += () => onDead();
    Sprite.Play("default");
    Hand.Play("hand");
    main = GetTree().CurrentScene as MainScene;
  }

  public override void _Process(double delta)
  {
    if (fleeing && pl != null)
    {
      //points away
      Vector2 away = GlobalPosition - pl.GlobalPosition; //(away)
      float mag = TargetDistance - away.Length();
      Velocity = away.Normalized() * mag * (float)delta;
      Pivot.Scale = new Vector2(Velocity.X > 0.0f ? 1 : -1, 1);
    }
    else
      Velocity = Vector2.Zero;
  }

  public override void _PhysicsProcess(double delta)
  {
    movment.move(stepCount, this);
  }

  void objectIsInView(Area2D area)
  {
    if (!area.GetCollisionLayerValue(PlayerCollisionID))
      return;
    ShapeCast2D ray = GetNode<ShapeCast2D>("Ray");
    ray.TargetPosition = area.GlobalPosition - ray.GlobalPosition;
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
    fleeing = area.GetCollisionLayerValue(PlayerCollisionID) ? false : fleeing;
  }

  public void onDead()
  {
    //WE SPAWN THEM DOUBLOONS
    while (jewelDropCount > 0)
    {
      Node2D instance = jewelScene.Instantiate() as Node2D;
      main.CallDeferred("add_child", instance);
      instance.Position = Vector2.Zero;
      jewelDropCount--;
    }
    QueueFree();
  }
}
