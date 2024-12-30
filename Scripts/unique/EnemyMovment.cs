using Godot;

public partial class EnemyMovment : Node
{
  [Export]
  public Enemy e;
  [Export]
  public float SPEED = 7.0f;
  [Export]
  public float ACCEL = 5.0f;

  public override void _Ready()
  {
    e.Nav.SetDeferred("target_position", (GetTree().CurrentScene as MainScene).ChristmasTree.GlobalPosition);
  }

  public override void _PhysicsProcess(double delta)
  {
    Vector2 direction = e.GlobalPosition.DirectionTo(e.Nav.GetNextPathPosition());
    e.Velocity = e.Velocity.Lerp(direction * SPEED, ACCEL * (float)delta);
    move(20, e);
  }

  public bool move(int stepCount, CharacterBody2D body)
  {
    Vector2 preVel = body.Velocity;
    body.Velocity *= 1.0f / stepCount;
    bool hit = false;
    while (stepCount-- > 0)
    {
      //our first step is to move for x
      KinematicCollision2D collision = body.MoveAndCollide(new Vector2(body.Velocity.X, 0), true);
      if (collision == null)
        body.Position += new Vector2(body.Velocity.X, 0);
      else
      {
        hit = true;
        body.Velocity *= new Vector2(0, 1); //clears x keeps y
        preVel *= new Vector2(0, 1);
      }
      collision = body.MoveAndCollide(new Vector2(0, body.Velocity.Y), true);
      if (collision == null)
        body.Position += new Vector2(0, body.Velocity.Y);
      else
      {
        hit = true;
        body.Velocity *= new Vector2(1, 0);
        preVel *= new Vector2(1, 0);
      }
    }
    return hit;
  }
}
