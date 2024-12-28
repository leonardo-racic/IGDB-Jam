using Godot;

public partial class Player : CharacterBody2D
{
  [Export]
  public AnimatedSprite2D Sprite;
  [Export]
  public Node2D Pivot;
  [Export]
  public Hitbox TackleHitbox;

  public bool Attacking = false;

  private bool tackleDebounce = false;

  private double TACKLE_DURATION = 0.25d;
  private double TACKLE_COOLDOWN = 0.3d;

  public override void _Ready()
  {
    TackleHitbox.Monitorable = false;
  }

  public override async void _Input(InputEvent inputEvent)
  {
    if (inputEvent.IsActionPressed("attack"))
    {
      if (!Attacking && !tackleDebounce)
      {
        tackleDebounce = true;
        Attacking = true;
        TackleHitbox.Monitorable = true;
        
        await ToSignal(GetTree().CreateTimer(TACKLE_DURATION), "timeout");
        Attacking = false;
        TackleHitbox.Monitorable = false;
        
        await ToSignal(GetTree().CreateTimer(TACKLE_COOLDOWN), "timeout");
        tackleDebounce = false;
      }
    }
  }

  public void PlayState(string state)
  {
    if (Sprite.Animation != state)
    {
      Sprite.Play(state);
    }
  }
}
