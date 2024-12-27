using Godot;
using System;

public partial class camera : Camera2D
{
  [Export]
  public float smoothingRatio { get; set; } = .1f;
  float mag;
  float phase = 0;
  float jitness;
  Vector2 shakeVector = Vector2.Zero;
  Vector2 endPos = Vector2.Zero;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (Input.IsActionJustPressed("space"))
      applyScreenShake(15, 0, 9);
    //i was gonna do some trig stuff
    //but this is too dumb to ignore
    //(its 1:45 am btw)
    Random rand = new Random();
    Vector2 randBox = new Vector2(rand.Next(-(int)jitness, (int)jitness), rand.Next(-(int)jitness, (int)jitness));  //this will be copied
    shakeVector += randBox * mag;
    randBox = new Vector2(rand.Next(-(int)jitness, (int)jitness), rand.Next(-(int)jitness, (int)jitness));  //this will be copied
    Position += endPos - (Position + randBox + shakeVector);
    jitness += -jitness * smoothingRatio * (float)delta;
    mag += -mag * smoothingRatio * (float)delta;
  }

  public void applyScreenShake(float magnitude, float dirRads, int jitterness)
  {
    mag = magnitude;
    shakeVector = Vector2.FromAngle(dirRads) * mag;  //thers a function for that :O
    jitness = jitterness;
  }
}
