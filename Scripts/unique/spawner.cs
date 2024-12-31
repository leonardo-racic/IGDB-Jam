using Godot;
using System;

public partial class spawner : StaticBody2D
{
  [Export]
  public int spawnCount;
  [Export]
  public float spawnTime = .5f;
  [Export]
  public PackedScene EnemyScene;
  [Export]
  public Node2D main;
  [Export]
  public int MaxEnemyCount = 5;
  [Export] // Enemy's speed and accel + or - this variable (to make them more diverse ^^)
  public float VelocityEpsilon = 0.5f;

  private float deltaElapsed = 0.0f;

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (deltaElapsed >= spawnTime)
    {
      spawnEnemies(spawnCount);
      deltaElapsed = 0.0f;
    }
    deltaElapsed += (float)delta;
  }

  public void spawnEnemies(int count)
  {
    while (count > 0 && main.GetChildCount() < MaxEnemyCount)
    {
      var areas = GetChildren();
      Random rand = new Random();
      //pick a random one
      CollisionShape2D target = areas[rand.Next(GetChildCount())] as CollisionShape2D;
      //pick a random position in this rectangle (no other)
      Rect2 rec = target.Shape.GetRect();
      //now we have a rect,
      //we want to turn this into a random position
      Vector2 localPos = new Vector2(nextSingle(rand, rec.Size.X), nextSingle(rand, rec.Size.Y));
      Vector2 globalPos = localPos + target.GlobalPosition + rec.Position;

      // Enemy conditioning
      Enemy newEnemy = EnemyScene.Instantiate() as Enemy;
      newEnemy.GlobalPosition = globalPos;
      newEnemy.movment.ACCEL += (rand.NextSingle() >= 0.5f ? -1 : 1) * VelocityEpsilon;
      newEnemy.movment.SPEED += (rand.NextSingle() >= 0.5f ? -1 : 1) * VelocityEpsilon;
      main.AddChild(newEnemy);
      count--;
    }
  }
  public float nextSingle(Random rand, float mag)
  {
    return rand.NextSingle() * mag;
  }
}
