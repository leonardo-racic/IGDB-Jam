using Godot;
using System;

public partial class camera : Camera2D
{
  [Export]
  public float dampening { get; set; } = .9f;
  float mag = .1f;
  float phase = 0;
  float jitness;
  Vector2 shakeVector = Vector2.Zero;
  Vector2 endPos = Vector2.Zero;
  Random rand = new Random();
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (Input.IsActionJustPressed("space"))
      applyScreenShake(9, 0, 1);
    //i was gonna do some trig stuff
    //but this is too dumb to ignore
    //(its 1:45 am btw)
    Vector2 randBox = new Vector2(randSingle(rand, jitness), randSingle(rand, jitness));
    shakeVector += randBox;
    randBox = new Vector2(randSingle(rand, jitness), randSingle(rand, jitness));
    Position = endPos + (randBox + shakeVector) * mag;
    jitness += -jitness * dampening * (float)delta;
    mag += -mag * dampening * (float)delta;
  }

  public void applyScreenShake(float magnitude, float dirRads, int jitterness)
  {
    mag = magnitude;
    shakeVector = Vector2.FromAngle(dirRads);  //thers a function for that :O
    jitness = jitterness;
  }

  public float randSingle(Random rand, float mag)
  {
    float scalar = (rand.NextSingle() - .5f) * 2.0f;
    return scalar * mag;
  }
}
