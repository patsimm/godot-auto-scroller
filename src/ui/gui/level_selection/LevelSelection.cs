using Godot;
using System.Collections.ObjectModel;

namespace Platformer;

public partial class LevelSelection : BoxContainer {
	[Signal]
	public delegate void LevelSelectedEventHandler(int level);

	public required GridContainer gridContainer { get; set; }

	public required ReadOnlyCollection<int> levels { get; set; }

	public override void _Ready() {
		gridContainer = GetNode<GridContainer>("%LevelGrid");
		levels = LevelsHelper.GetLevels();
		var buttonStyleBoxNormal = GD.Load<StyleBox>("res://resources/button_styles.tres");
		var buttonStyleBoxHover = GD.Load<StyleBox>("res://resources/button_styles_hover.tres");

		foreach (var level in levels) {
			var button = new Button();
			button.Pressed += () => OnLevelButtonPressed(level);
			button.Text = "Level " + level;
			button.AddThemeFontSizeOverride("font_size", 64);
			button.AddThemeStyleboxOverride("normal", buttonStyleBoxNormal);
			button.AddThemeStyleboxOverride("hover", buttonStyleBoxHover);
			gridContainer.AddChild(button);
			button.Dispose();
		}
	}

	public void OnLevelButtonPressed(int level) {
		EmitSignal(SignalName.LevelSelected, level);
	}
}
