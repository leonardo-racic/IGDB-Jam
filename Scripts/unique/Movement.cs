using Godot;

public partial class Movement : Node
{
  [Export]
  public Player plr;
  [Export]
  public float SPEED = 150.0f;
  [Export]
  public float TACKLE_SPEED = 600.0f;
  [Export]
  public float FRICTION = .5f;
  [Export]
  public float bounceFactor = 350.0f;
  [Export]
  public int stepCount = 20;

  private Vector2 lastInputDirection = Vector2.Right;

  public override void _PhysicsProcess(double delta)
  {
    if (plr.Attacking)
    {
      handleTackleMovement(delta);
    } 
    else
    {
      handleDefaultMovement(delta);
    }
    move(plr, stepCount, bounceFactor);
  }

  private void handleDefaultMovement(double delta)
  {
    Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    if (inputDirection != Vector2.Zero)
    {
      lastInputDirection = inputDirection;
      if (inputDirection.X != 0.0f)
        plr.Pivot.Scale = new Vector2(2*(inputDirection.X > 0.0f ? 1 : 0) - 1, 1);
      plr.Velocity = inputDirection * (float)(SPEED * delta);
      plr.PlayState("move");
    }
    else
    {
      plr.Velocity *= FRICTION * (float)delta;
      plr.PlayState("idle");
    }
  }

  private void handleTackleMovement(double delta) {
    plr.Velocity = lastInputDirection * (float)(TACKLE_SPEED * delta);
    plr.PlayState("tackle");
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
