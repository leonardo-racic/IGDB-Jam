using Godot;
using System;

public partial class camera : Camera2D
{
  [Export]
  public float dampening { get; set; } = .9f;
  [Export]
  public Player plr { get; set; }
  [Export]
  public float restingRadius { get; set; } = 250.0f;
  [Export]
  public Vector2 restingPos { get; set; } = Vector2.Zero;
  [Export]
  public float transitionSpeed { get; set; } = 0.02f;
  [Export]
  public float outsideAreaShakeMag { get; set; } = 2;
  [Export]
  public float outsideAreaShakeJit { get; set; } = 2;

  public bool transitioningToPlayer;

  float mag = .0f;
  float phase = 0;
  float jitness;
  Vector2 shakeVector = Vector2.Zero;
  Vector2 endPos = Vector2.Zero;
  Random rand = new Random();
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    if (plr == null)
      GD.PrintErr("Error camera Player target is not set");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    //step one git if the player is far away
    transitioningToPlayer = plr.GlobalPosition.DistanceTo(restingPos) > restingRadius;
    if (transitioningToPlayer)
      endPos += (plr.GlobalPosition - endPos) * transitionSpeed;
    else
      endPos += (restingPos - endPos) * transitionSpeed;

    if (Input.IsActionJustPressed("space") || transitioningToPlayer)
      if (mag <= outsideAreaShakeMag)
        setScreenShake(outsideAreaShakeMag, plr.Position.Angle(), outsideAreaShakeJit);
    //i was gonna do some trig stuff
    //but this is too dumb to ignore
    //(its 1:45 am btw)
    Vector2 randBox = new Vector2(randSingle(rand, jitness), randSingle(rand, jitness));
    shakeVector += randBox;
    randBox = new Vector2(randSingle(rand, jitness), randSingle(rand, jitness));
    Position = endPos + (randBox * mag / 2.0f) + shakeVector;
    jitness += -jitness * dampening * (float)delta;
    mag += -mag * dampening * (float)delta;
  }

  public void applyScreenShake(float magnitude, float dirRads, float jitterness)
  {
    mag += magnitude;
    shakeVector = Vector2.FromAngle(dirRads);  //thers a function for that :O
    jitness = jitness + jitterness;
  }

  public void setScreenShake(float magnitude, float dirRads, float jitterness)
  {
    mag = magnitude;
    shakeVector = Vector2.FromAngle(dirRads) * mag;
    jitness = jitterness;
  }

  public float randSingle(Random rand, float mag)
  {
    float scalar = (rand.NextSingle() - .5f) * 2.0f;
    return scalar * mag;
  }
}
