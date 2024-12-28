using Godot;
using System;

public partial class Bullet : CharacterBody2D
{

  Sprite2D sprite;
  ShaderMaterial mat;
  Vector2 vel = Vector2.Zero;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    sprite = GetNode<Sprite2D>("Sprite");
    mat = (ShaderMaterial)sprite.Material;
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    mat.SetShaderParameter("vel", vel);
  }
}
