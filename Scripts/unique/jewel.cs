using Godot;
using System;

public partial class jewel : CharacterBody2D
{
  [Export]
  public float firstDistance = 100.0f;

  [Export]
  public float acceleration = 50.0f;

  [Export]
  public float friction = .9f;

  [Export]
  public Area2D detectBox;

  bool start = false;
  Area2D plr = null;

  float distance = 0.0f;
  float dVel = 0.0f;

  float angle = 0.0f;

  private int PlayerCollisionID = 2;
  private Vector2 prevPos;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    prevPos = Position;
    detectBox.Monitoring = false;
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (!detectBox.Monitoring)
      detectBox.Monitoring = !detectBox.Monitoring;
    Rotation = (prevPos - Position).Angle() + MathF.PI / 2.0f;
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
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    if (distance <= 0 && plr != null)
      CallDeferred("queue_free");
    else
    {
      MoveAndSlide();
      Velocity *= friction;
    }
  }

  public void areaEntered(Area2D area)
  {
    Velocity = Vector2.Zero;
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
