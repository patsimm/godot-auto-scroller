using System.Text.Json;
using Godot;

namespace Platformer;

public partial class Main : Node2D {
    private sealed record Savegame(double? Highscore);

	public required Gui Gui { get; set; }
    public required Hud Hud { get; set; }
    public required LevelLoaderComponent LevelLoaderComponent { get; set; }
    public required TimeCounter GameTimeCounter { get; set; }

    private double? _highscore;

    public override void _EnterTree() {
        base._EnterTree();
        Load();
    }

	public override void _Ready() {
		Gui = GetNode<Gui>("GUI");
		Gui.PlayAgainButtonPressed += Restart;

        Hud = GetNode<Hud>("HUD");

        GameTimeCounter = GetNode<TimeCounter>("GameTimeCounter");

        LevelLoaderComponent = GetNode<LevelLoaderComponent>("LevelLoaderComponent");
		StartGame();
	}

	public void StartGame() {
        GameTimeCounter.Reset();
		Gui.Hide();
        LevelLoaderComponent.LoadLevel(1);
	}

	public void Restart() {
		StartGame();
		GetTree().Paused = false;
	}

	public void OnLevelFailed() {
		GetTree().Paused = true;
        Gui.ShowGameOver();
        Gui.Show();
    }

	public void OnLevelCompleted() {
		GetTree().Paused = true;
        var score = GameTimeCounter.ElapsedTimeSeconds;
        if (_highscore is null || score < _highscore) {
            _highscore = score;
            Save();
        }
		Gui.ShowLevelCompleted(score, _highscore ?? score);
        Gui.Show();
	}

    private void Save() {
        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);
        var data = JsonSerializer.Serialize(new Savegame(_highscore));
        saveGame.StoreLine(data);
    }

    private void Load() {
        if (!FileAccess.FileExists("user://savegame.save")) return;

        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
        var data = JsonSerializer.Deserialize<Savegame>(saveGame.GetLine());
        _highscore = data?.Highscore;
    }
}
