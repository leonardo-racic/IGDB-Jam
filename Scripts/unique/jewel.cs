using Godot;
using System;

public partial class jewel : CharacterBody2D
{
  [Export]
  public float dampening = .04f;
  [Export]
  public float pushFactor = 1.0f;
  bool collecting = false;
  Area2D plr = null;
  float t = 0.0f;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (collecting)
    {
      Velocity += GlobalPosition.Lerp(plr.GlobalPosition, t) - GlobalPosition;
      GD.Print(t);
      t += (float)delta;
      t = t > 1 ? 1 : t;
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    if (MoveAndSlide())
      CallDeferred("queue_free");
  }

  public void areaEntered(Area2D area)
  {
    if (!collecting)
      Velocity += (Position - area.Position) * pushFactor;
    collecting = true;
    plr = area;
  }
}
