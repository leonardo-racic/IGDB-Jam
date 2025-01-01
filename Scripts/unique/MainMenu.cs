using Godot;

public partial class MainMenu : CanvasLayer
{
	[Export]
	public Button[] buttons;

	public override void _Ready()
	{
		foreach (Button button in buttons)
		{
			string scenePath = $"res://Stages/{button.Name.ToString().TrimSuffix("Label").ToLower()}.tscn";
			button.Pressed += () =>
			{
				GetTree().ChangeSceneToFile(scenePath);
			};
		}
	}
}
