using System;
using System.Text.Json;
using Godot;
using Godot.Collections;

namespace Platformer;

public partial class Main : Node2D {
    private sealed record Savegame(Dictionary<int, double> LevelHighScores);

    public required Gui Gui { get; set; }
    public required Hud Hud { get; set; }
    public required LevelLoaderComponent LevelLoaderComponent { get; set; }
    public required TimeCounter GameTimeCounter { get; set; }

    private Dictionary<int, double> _levelHighScores = new();

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
        var level = LevelLoaderComponent.LoadedLevel;
        if (!level.HasValue) throw new Exception($"Unable to execute {nameof(OnLevelCompleted)} if no level is loaded.");
        if (!_levelHighScores.TryGetValue(level.Value, out var currentHighScore) || score < currentHighScore) {
            _levelHighScores[level.Value] = score;
            Save();
        }
        Gui.ShowLevelCompleted(score, _levelHighScores[level.Value]);
        Gui.Show();
    }

    private void Save() {
        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);
        var data = JsonSerializer.Serialize(new Savegame(_levelHighScores));
        saveGame.StoreLine(data);
    }

    private void Load() {
        if (!FileAccess.FileExists("user://savegame.save")) return;

        using var saveGame = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);
        var data = JsonSerializer.Deserialize<Savegame>(saveGame.GetLine());
        _levelHighScores = data?.LevelHighScores ?? new Dictionary<int, double>();
    }
}
