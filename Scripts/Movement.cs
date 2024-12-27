using Godot;

public partial class Movement : Node
{
  [Export]
  public Player plr;
  [Export]
  public float speed = 20_000.0f;

  public const float FRICTION = 0.1f;

  public override void _PhysicsProcess(double delta)
  {
    handleMovement(delta);
    plr.MoveAndSlide();
  }

  private void handleMovement(double delta)
  {
    Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    if (inputDirection != Vector2.Zero)
    {
      plr.Velocity = inputDirection * (float)(speed * delta);
      if (inputDirection.X != 0.0f)
        plr.Sprite.FlipH = inputDirection.X < 0.0f;
      plr.PlayState("move");
    } 
    else
    {
      plr.Velocity *= (float)(FRICTION * delta);
      plr.PlayState("idle");
    }
  }
}
