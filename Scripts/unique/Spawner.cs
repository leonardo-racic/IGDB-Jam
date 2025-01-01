using Godot;
using System;

public partial class Spawner : StaticBody2D
{
  [Signal]
  public delegate void SpawnCountSaturatedEventHandler();

  [Export]
  public int spawnCount;
  [Export]
  public float spawnTime = .5f;
  [Export]
  public PackedScene EnemyScene;
  [Export] // Where the enemies are spawned
  public Node2D enemiesOrigin;
  [Export]
  public int MaxEnemyCount = 5;
  [Export] // Enemy's speed and accel + or - this variable (to make them more diverse ^^)
  public float VelocityEpsilon = 0.5f;

  public int spawnedEnemyCount = 0;

  public async void SpawnEnemies(int count = -1)
  {
    if (count < 0)
      count = MaxEnemyCount;
    while (count > 0 && spawnedEnemyCount < MaxEnemyCount)
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
      newEnemy.movment.ACCEL += rand.NextSingle() * VelocityEpsilon;
      newEnemy.movment.SPEED += rand.NextSingle() * VelocityEpsilon;
      enemiesOrigin.AddChild(newEnemy);
      count--;
      spawnedEnemyCount++;
      await ToSignal(GetTree().CreateTimer(spawnTime), "timeout");
    }
    if (spawnedEnemyCount >= MaxEnemyCount)
      EmitSignal(nameof(SpawnCountSaturated));
  }

  public float nextSingle(Random rand, float mag) => rand.NextSingle() * mag;

  public bool ReadyToSpawn() => enemiesOrigin.GetChildCount() == 0;
}
