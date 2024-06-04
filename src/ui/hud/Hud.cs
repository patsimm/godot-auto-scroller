using Godot;

namespace Platformer;

public partial class Hud : CanvasLayer
{
    [Export]
    public required Label GameTimeLabel { get; set; }

    public void UpdateGameTime(double time) {
        GameTimeLabel.Text = GameTimeFormat.FormatGameTime(time);
    }
}
