using Godot;

public partial class Player : CharacterBody2D
{
  [Export]
  public AnimatedSprite2D Sprite;
  [Export]
  public Hitbox TackleHitbox;
  [Export]
  public float tackleShakeMag;
  [Export]
  public float tackleShakeJitness;
  [Signal]
  public delegate void sendScreenShakeEventHandler(float mag, float dir, float jitness);

  public bool Attacking = false;

  public int doubloonCount = 0;

  private bool tackleDebounce = false;

  public double TACKLE_DURATION = 0.25;
  public double TACKLE_COOLDOWN = 0.3;

  public float money = 0.0f;

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

        EmitSignal(nameof(sendScreenShake), tackleShakeMag, Velocity.Angle(), tackleShakeJitness);

        await ToSignal(GetTree().CreateTimer(TACKLE_DURATION), "timeout");
        Attacking = false;
        TackleHitbox.Monitorable = false;

        await ToSignal(GetTree().CreateTimer(TACKLE_COOLDOWN), "timeout");  //huh, i would've counted by delta but this seems much more efficient
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

  public void onJewelCollect(Area2D area)
  {
    money += (area as Hitbox).Damage;
  }
}
