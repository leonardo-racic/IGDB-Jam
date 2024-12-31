using Godot;

public partial class MainScene : Node2D
{
	[Export]
	public Node2D ChristmasTree;
	[Export]
	public NavigationRegion2D NavRegion;
	[Export]
	public Player plr;
	[Export]
	public GiftBar GiftBar;
	[Export]
	public float StartingGiftsAmount = 12.0f;

	public float GiftsAmount
	{
		get => giftsAmount;
		set
		{
			giftsAmount = Mathf.Max(value, 0.0f);
			GD.Print(giftsAmount);
			if (giftsAmount == 0.0f)
			{
				GD.Print("GAME FINISHED, NO MORE GIFTS!!!");
				GiftBar.Hide();
			}
			else
			{
				GiftBar.UpdateBar(giftsAmount);
                GiftBar.Show();
			}
		}
	}
	public NavigationObstacle2D PlrObstacle; // Also used by EnemyMovment.cs
	
	private float giftsAmount;

    public override void _Ready()
    {
		GiftsAmount = StartingGiftsAmount;
		GiftBar.UpdateBar(GiftsAmount);
		PlrObstacle = NavRegion.GetNode<NavigationObstacle2D>("Player");
    }

    public override void _PhysicsProcess(double delta)
    {
		if (PlrObstacle == null || NavRegion.IsBaking()) 
			return;
		PlrObstacle.GlobalPosition = plr.GlobalPosition;
		handleObstacles();
		NavRegion.BakeNavigationPolygon(false);
    }

	// Used in order to  avoid the "Navigation map synchronisation error" coming up due to two obstacles overlapping...
	public void handleObstacles() {} // WIP

    public void OnRobberEvaded(Enemy robber)
	{
		GiftsAmount -= robber.HeldGiftsAmount;
		robber.QueueFree();
	}
}
