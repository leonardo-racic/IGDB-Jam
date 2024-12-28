using Godot;

public partial class Movement : Node
{
  [Export]
  public Player plr;
  [Export]
  public float speed = 150.0f;
  [Export]
  public float FRICTION = .5f;
  [Export]
  public float bounceFactor = 350.0f;
  [Export]
  public int stepCount = 20;

  public override void _PhysicsProcess(double delta)
  {
    handleMovement(delta);
    move(plr, stepCount, bounceFactor);
  }

  private void handleMovement(double delta)
  {
    Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    plr.Velocity += inputDirection * (float)(speed * delta);
    plr.Velocity *= (float)(FRICTION); //TODO: find a way to incorperate delta here so its normalized per frame rate; but _PhysicsProcess is const int framerate (i think)
    if (inputDirection != Vector2.Zero)
    {
      if (inputDirection.X != 0.0f)
        plr.Sprite.FlipH = inputDirection.X < 0.0f;
      plr.PlayState("move");
    }
    else
    {
      plr.PlayState("idle");
    }
  }

  public bool move(CharacterBody2D body, int stepCount, float bounceFactor)
  {
    bool hit = false;
    Vector2 incVel = body.Velocity / stepCount;
    while (stepCount-- > 0)
    {  //save on memory by avoiding a for (we save 4 bytes, less if we use a short)
      KinematicCollision2D col = body.MoveAndCollide(incVel, true);
      if (col == null)
      {
        //we're in the clear
        body.Position += incVel;
      }
      else
      {
        //WE HIT THE SECOND TOWER
        body.Velocity = body.Velocity.Bounce(col.GetNormal()) * bounceFactor;
        incVel = incVel.Bounce(col.GetNormal()) * bounceFactor;
        hit = true;
      }
    }
    return hit;
  }
}
