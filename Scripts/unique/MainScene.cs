using Godot;
public partial class MainScene : Node2D
{
	[Export]
	public Node2D ChristmasTree;
	[Export]
	public NavigationRegion2D NavRegion;
	[Export]
	public Player plr;

	private NavigationObstacle2D plrObstacle;

    public override void _Ready()
    {
		plrObstacle = NavRegion.GetNode<NavigationObstacle2D>("Player");
    }

    public override void _PhysicsProcess(double delta)
    {
		if (plrObstacle == null || NavRegion.IsBaking()) 
			return;
		plrObstacle.GlobalPosition = plr.GlobalPosition;
		handleObstacles();
		NavRegion.BakeNavigationPolygon(false);
    }

	// Used in order to  avoid the "Navigation map synchronisation error" coming up due to two obstacles overlapping...
	public void handleObstacles() {
		// WIP
	}

    public void OnRobberEvaded(Enemy robber)
	{
		GD.Print($"Robber {robber.Name} just evaded!");
		robber.QueueFree(); // wip
	}
}
