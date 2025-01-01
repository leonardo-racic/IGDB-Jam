using Godot;

public partial class GiftBar : TextureRect
{
	[Export]
	public float GiftSize = 17.0f;

	private Vector2 lastWantedSize;

    public override void _Process(double delta)
    {
		// Had to implement it like this, otherwise it would not work when ready...
		if (Size != lastWantedSize)
			Size = lastWantedSize;
    }

    public void UpdateBar(float gifts)
	{
		Vector2 newSize = new Vector2(GiftSize * gifts, Size.Y);
		lastWantedSize = newSize;
	}
}
