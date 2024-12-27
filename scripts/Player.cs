using Godot;
using System;

public partial class Player : CharacterBody2D
{
  [Export]
  public float speed { get; set; } = 600.0f;
  [Export]
  public float friction { get; set; } = 0f;
  [Export]
  public float bounceFactor { get; set; } = .5f;
  private Movement movementNode;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    movementNode = GetNode<Movement>("Movement");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    //step one figure out if the player is touching movment keys
    Vector2 Key = Vector2.Zero;
    if (Input.IsActionPressed("w"))
      Key += Vector2.Up;
    if (Input.IsActionPressed("s"))
      Key += Vector2.Down;
    if (Input.IsActionPressed("a"))
      Key += Vector2.Left;
    if (Input.IsActionPressed("d"))
      Key += Vector2.Right;
    if (Key.Length() > 1)
      Key = new Vector2(Key.X / Key.Length(), Key.Y / Key.Length());
    Key *= speed;
    Velocity += Key;
    movementNode.move(this, 10, (float)delta, bounceFactor);
    Velocity *= friction;
  }
}
