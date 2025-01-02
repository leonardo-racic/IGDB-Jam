using Godot;

public partial class GameUI : CanvasLayer
{
	[Export]
	public VBoxContainer ResultsContainer;
	[Export]
	public Label ResultsLabel;
	[Export]
	public Button RetryButton;
	[Export]
	public Button BackButton;
	[Export]
	public GiftBar CurrentGiftBar;
	[Export]
	public Label RoundLabel;


    public override void _Ready()
    {
		Show();
		RoundLabel.Show();
		ResultsContainer.Hide();
		CurrentGiftBar.Show();

		RetryButton.Pressed += () =>
		{
			GetNode<AudioStreamPlayer>("ClickedSound").Play();
			GetTree().Paused = false;
			GetTree().ReloadCurrentScene();
		};
		BackButton.Pressed += () => 
		{
			GetNode<AudioStreamPlayer>("ClickedSound").Play();
			GetTree().Paused = false;
			GetTree().ChangeSceneToFile("res://UI/main_menu.tscn");
		};
    }
}
