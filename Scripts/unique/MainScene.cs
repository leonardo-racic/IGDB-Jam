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
	[Export]
	public Spawner spawner;
	[Export]
	public Label RoundLabel;

	public int Round
	{
		get => round;
		set
		{
			round = Mathf.Max(value, 1);
			RoundLabel.Text = $"Round: {round}";
		}
	}

	public float GiftsAmount
	{
		get => giftsAmount;
		set
		{
			giftsAmount = Mathf.Max(value, 0.0f);
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
	private int round = 0;
	private bool spawning = false;

    public override void _Ready()
    {
		GiftBar.UpdateBar(GiftsAmount);
		RoundLabel.Text = $"Round: {round}";
		GiftsAmount = StartingGiftsAmount;
		PlrObstacle = NavRegion.GetNode<NavigationObstacle2D>("Player");

		spawner.SpawnCountSaturated += () =>
		{
			spawning = false;
			spawner.spawnedEnemyCount = 0;
			spawner.VelocityEpsilon += 0.005f;
			if (Round % 2 == 0)
				spawner.VelocityEpsilon += 0.005f;
			if (Round % 5 == 0)
				spawner.MaxEnemyCount = (int)Mathf.Ceil(spawner.MaxEnemyCount * 2);
		};
    }

    public override void _PhysicsProcess(double delta)
    {
		// Navigation related
		if (PlrObstacle == null || NavRegion.IsBaking()) 
			return;
		PlrObstacle.GlobalPosition = plr.GlobalPosition;
		handleObstacles();
		NavRegion.BakeNavigationPolygon(false);

		// Spawning related
		if (spawner.ReadyToSpawn() && !spawning)
		{
			spawning = true;
			Round++;
			spawner.SpawnEnemies();
		}
    }

	// Used in order to  avoid the "Navigation map synchronisation error" coming up due to two obstacles overlapping...
	public void handleObstacles() {} // WIP

    public void OnRobberEvaded(Enemy robber)
	{
		GiftsAmount -= robber.HeldGiftsAmount;
		robber.QueueFree();
	}
}
