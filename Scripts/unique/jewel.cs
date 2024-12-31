using Godot;
using System;

public partial class jewel : CharacterBody2D
{
  [Export]
  public float firstDistance = 100.0f;
  [Export]
  public float acceleration = 50.0f;
  bool start = false;
  Area2D plr = null;
  float distance = 0.0f;
  float dVel = 0.0f;
  float angle = 0.0f;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (plr != null)
    {
      angle = plr.GlobalPosition.AngleToPoint(GlobalPosition);
      if (start)
      {
        distance = firstDistance;
        if (distance >= firstDistance)
          start = !start;
      }
      else
      {
        dVel -= acceleration;
      }
      distance += dVel;
      setPositionAngle();
      Position += plr.GlobalPosition;
      GD.Print(distance);
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    if (distance <= 0 && plr != null)
      CallDeferred("queue_free");
  }

  public void areaEntered(Area2D area)
  {
    angle = area.GlobalPosition.AngleToPoint(GlobalPosition);
    distance = (area.GlobalPosition - GlobalPosition).Length();
    start = true;
    plr = area;
  }

  public void setPositionAngle()
  {
    GlobalPosition = Vector2.FromAngle(angle) * distance;
  }
}
