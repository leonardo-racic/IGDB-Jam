using Godot;

public partial class Hitbox : Area2D
{
    [Export]
    public float damage = 10.0f;
    
    public void onAreaEntered(Area2D area) {
        if (area is Hurtbox) {
            (area as Hurtbox).TakeDamage(damage);
        }
    }
}
