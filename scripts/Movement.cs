using Godot;

public partial class Movement : Node
{
  public bool move(CharacterBody2D body, int stepCount, float delta, float bounceFactor)
  {
    Vector2 preVel = body.Velocity;
    body.Velocity = (preVel / stepCount) * delta;
    while (stepCount-- > 0)
    {
      KinematicCollision2D col = body.MoveAndCollide(body.Velocity, true);
      if (col == null)
      {
        body.Position += body.Velocity;
      }
      else
      {
        //THEY HIT THE SECOND TOWER
        body.Velocity = preVel * bounceFactor;
        body.Position = col.GetPosition();
        return true;
      }
    }
    body.Velocity = preVel;
    return false;
  }
}
