using Godot;
using System;

public partial class EnemyMovment : Node
{
  [Export]
  public Enemy e;
  [Export]
  public float SPEED = 7.0f;
  [Export]
  public float ACCEL = 10.0f;
  [Export]
  public float MAX_HELD_GIFTS_AMOUNT = 4.0f;
  [Export]
  public float MIN_HELD_GIFTS_AMOUNT = 0.3f;

  const float giftOffsetX = 3.0f;
  const float giftOffsetY = 1.5f;

  public override void _Ready()
  {
    MainScene scene = GetTree().CurrentScene as MainScene;
    Node2D markers = scene.ChristmasTree.GetNode<Node2D>("Markers");
    int childIndex = new Random().Next(markers.GetChildCount());
    Marker2D target = markers.GetChild<Marker2D>(childIndex);
    e.Nav.SetDeferred("target_position", target.GlobalPosition);

    // main path logic
    e.Nav.NavigationFinished += () =>
    {
      if (e.Nav.TargetPosition == target.GlobalPosition)
        handleGiftGrabbing(scene);
      else
        scene.OnRobberEvaded(e);
    };
  }

  public override void _PhysicsProcess(double delta)
  {
    Vector2 goal = e.Nav.GetNextPathPosition();
    Vector2 direction = e.GlobalPosition.DirectionTo(goal);
    e.Velocity = e.Velocity.Lerp(direction * SPEED, ACCEL * (float)delta);
    if (goal.X != 0.0f)
      e.Pivot.Scale = new Vector2(e.Velocity.X < 0.0f ? -1 : 1, 1);
    move(20, e);
  }

  private async void handleGiftGrabbing(MainScene scene)
  {
    float heldGiftsAmount;
    do
      heldGiftsAmount = (float)new Random().NextDouble() * MAX_HELD_GIFTS_AMOUNT;
    while (heldGiftsAmount < MIN_HELD_GIFTS_AMOUNT);
    e.HeldGiftsAmount = heldGiftsAmount;

    e.Hand.Play("gift");
    // Let's assume the gift's a square, e.Hand.Scale.X = e.Hand.Scale.Y
    float epsilon = 1.0f;
    if (heldGiftsAmount < epsilon)
      e.Hand.Scale = new Vector2(heldGiftsAmount * e.Hand.Scale.X, heldGiftsAmount * e.Hand.Scale.Y);
    else if (heldGiftsAmount > epsilon)
    {
      AnimatedSprite2D previousHand = e.Hand;
      bool spawnUpwards = true;
      while (heldGiftsAmount > epsilon)
      {
        AnimatedSprite2D d = previousHand.Duplicate() as AnimatedSprite2D;
        previousHand.GetParent().CallDeferred("add_child", d);
        await ToSignal(d, "ready");

        d.Position = new Vector2(previousHand.Position.X + giftOffsetX, e.Hand.Position.Y + (spawnUpwards ? -1 : 1) * giftOffsetY);
        if (heldGiftsAmount < epsilon)
          d.Scale = new Vector2(heldGiftsAmount * e.Hand.Scale.X, heldGiftsAmount * e.Hand.Scale.Y);
        else
        {
          d.Scale = new Vector2(e.Hand.Scale.X, e.Hand.Scale.Y);
          spawnUpwards = !spawnUpwards;
          previousHand = d;
        }
        heldGiftsAmount -= epsilon;
      }
    }

    Node2D doors = scene.GetNode<Node2D>("Doors");
    int doorIndex = new Random().Next(doors.GetChildCount());
    Node2D door = doors.GetChild<Node2D>(doorIndex);
    Marker2D doorMarker = door.GetNode<Marker2D>("Marker2D");
    
    e.Nav.TargetPosition = doorMarker.GlobalPosition;
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
