using Godot;
using Godot.Collections;
using System;

public partial class EnemyMovment : Node
{
  [Export]
  public Enemy e;
  [Export]
  public float SPEED = 7.0f;
  [Export]
  public float ACCEL = 5.0f;

  public override void _Ready()
  {
    Node2D markers = (GetTree().CurrentScene as MainScene).ChristmasTree.GetNode<Node2D>("Markers");
    int childIndex = new Random().Next(markers.GetChildCount());
    Marker2D target = markers.GetChild<Marker2D>(childIndex);
    e.Nav.SetDeferred("target_position", target.GlobalPosition);

    // main path logic
    e.Nav.NavigationFinished += () =>
    {
      if (e.Nav.TargetPosition == target.GlobalPosition)
      {
        Node2D doors = GetTree().CurrentScene.GetNode<Node2D>("Doors");
        int doorIndex = new Random().Next(doors.GetChildCount());
        Node2D door = doors.GetChild<Node2D>(doorIndex);
        e.Nav.TargetPosition = door.GlobalPosition;
      }
      else
      {
      }
    };
  }

  public override void _PhysicsProcess(double delta)
  {
    Vector2 goal = e.Nav.GetNextPathPosition();
    GD.Print(goal);
    Vector2 direction = e.GlobalPosition.DirectionTo(goal);
    e.Velocity = e.Velocity.Lerp(direction * SPEED, ACCEL * (float)delta);
    if (goal.X != 0.0f)
      e.Pivot.Scale = new Vector2(e.Velocity.X < 0.0f ? -1 : 1, 1);
    move(20, e);
  }

  public bool move(int stepCount, CharacterBody2D body)
  {
    Vector2 preVel = body.Velocity;
    body.Velocity *= 1.0f / stepCount;
    bool hit = false;
    while (stepCount-- > 0)
    {
      //our first step is to move for x
      KinematicCollision2D collision = body.MoveAndCollide(new Vector2(body.Velocity.X, 0), true);
      if (collision == null)
        body.Position += new Vector2(body.Velocity.X, 0);
      else
      {
        hit = true;
        body.Velocity *= new Vector2(0, 1); //clears x keeps y
        preVel *= new Vector2(0, 1);
      }
      collision = body.MoveAndCollide(new Vector2(0, body.Velocity.Y), true);
      if (collision == null)
        body.Position += new Vector2(0, body.Velocity.Y);
      else
      {
        hit = true;
        body.Velocity *= new Vector2(1, 0);
        preVel *= new Vector2(1, 0);
      }
    }
    return hit;
  }
}
