using Godot;

namespace Platformer;

[Tool]
[GlobalClass]
public partial class LevelLoaderComponent : Component<Node> {
    [Signal]
    public delegate void LevelFailedEventHandler();

    [Signal]
    public delegate void LevelCompletedEventHandler();

    private Level? _currentLevel;

    private void UnloadLevel() {
        if (_currentLevel == null || !IsInstanceIdValid(_currentLevel.GetInstanceId())) return;
        _currentLevel.QueueFree();
        _currentLevel = null;
    }

    public void LoadLevel(int levelNumber) {
        UnloadLevel();
        var levelScene = GD.Load<PackedScene>($"res://scenes/levels/level_{levelNumber}.tscn");
        var instantiatedLevel = levelScene.Instantiate();
        if (instantiatedLevel is not Level level) {
            instantiatedLevel.QueueFree();
            return;
        }

        _currentLevel = level;
        Entity.AddChild(_currentLevel);
        _currentLevel.LevelCompleted += () => EmitSignal(SignalName.LevelCompleted);
        _currentLevel.LevelFailed += () => EmitSignal(SignalName.LevelFailed);
    }
}
