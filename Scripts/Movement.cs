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

  public void handleMovement(double delta) {
    // It's already normalized
    Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    if (inputDirection != Vector2.Zero) {
      plr.Velocity = inputDirection * (float)(speed * delta);
      // add code for animation when running
    } else {
      plr.Velocity *= (float)(FRICTION * delta);
      // add code for animation when idle
    }
  }
}
