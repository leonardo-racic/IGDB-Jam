using Godot;

public partial class destructableObstacle : Sprite2D
{
  [Export]
  private NavigationObstacle2D navOb;

  [Export]
  public float health;
  [Export]
  private HealthNode healthNode;

  [Export]
  public Shape2D collisionShape;
  [Export]
  public CollisionShape2D[] collShape;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    healthNode.MaxHealth = health;
    healthNode.Health = health;
    foreach (CollisionShape2D i in collShape)
      i.Shape = collisionShape;
    healthNode.Dead += () => onDead();
  }

  void onDead()
  {
    (GetTree().CurrentScene as MainScene).ObstacleBrokenSound.Play();
    navOb.QueueFree();
    QueueFree();
  }
}
