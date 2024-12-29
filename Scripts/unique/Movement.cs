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

  private Vector2 lastValidInputDir = Vector2.Zero;

  public override void _PhysicsProcess(double delta)
  {
    if (plr.Attacking)
      handleTackleMovement(delta);
    else
      handleDefaultMovement(delta);
    move(plr, stepCount, bounceFactor);
  }

  private void handleDefaultMovement(double delta)
  {
    Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    plr.Velocity += inputDirection * (float)(SPEED * delta);
    plr.Velocity *= FRICTION;
    if (inputDirection != Vector2.Zero)
    {
      lastValidInputDir = inputDirection;
      if (inputDirection.X != 0.0f)
        plr.Pivot.Scale = new Vector2(2 * (inputDirection.X > 0.0f ? 1 : 0) - 1, 1);
      plr.PlayState("move");
    }
    else
      plr.PlayState("idle");
  }

  private void handleTackleMovement(double delta)
  {
    plr.Velocity = (plr.Velocity.Length() > 1f ? plr.Velocity.Normalized() : lastValidInputDir) * TACKLE_SPEED * (float)delta;
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
        //we're in the clear
        body.Position += incVel;
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
  //so i realize this is the player movment so we have to do some...
  //other things for the enemy its gonna be similar but not quite the same
  //so i don't think its worth refactoring all ^ to work with enemies;
  //i'll just make a different script called enemyMovment or something
}
