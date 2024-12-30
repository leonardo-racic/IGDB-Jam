using Godot;
public partial class MainScene : Node2D
{
	[Export]
	public Node2D ChristmasTree;

	public void OnRobberEvaded(Enemy robber)
	{
		GD.Print($"Robber {robber.Name} just evaded!");
		robber.QueueFree(); // wip
	}
}
