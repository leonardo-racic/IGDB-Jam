using Godot;

public partial class MainMenu : CanvasLayer
{
	[Export]
	public Button[] buttons;
	[Export]
	public AudioStreamPlayer music;

	public override void _Ready()
	{
		music.Finished += () => music.Play();
		foreach (Button button in buttons)
		{
			string scenePath = $"res://Stages/{button.Name.ToString().TrimSuffix("Label").ToLower()}.tscn";
			button.Pressed += () =>
			{
				GetNode<AudioStreamPlayer>("ClickedSound").Play();
				GetTree().ChangeSceneToFile(scenePath);
			};
		}
	}
}
