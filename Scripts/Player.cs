using Godot;

public partial class Player : CharacterBody2D
{
  [Export]
  public AnimatedSprite2D Sprite;

  public void PlayState(string state)
  {
    if (Sprite.Animation != state)
    {
      Sprite.Play(state);
    }
  }
}
