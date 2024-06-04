using Godot;

namespace Platformer;

public partial class TimeCounter : Node {
    private double _startTime;
    public double ElapsedTimeSeconds { get; private set; }

    [Signal]
    public delegate void TimeChangedEventHandler(double seconds);

    public void Reset() {
        _startTime = Time.GetUnixTimeFromSystem();
    }

    public override void _Ready() {
        base._Ready();
        Reset();
    }

    public override void _Process(double delta) {
        base._Process(delta);

        ElapsedTimeSeconds = Time.GetUnixTimeFromSystem() - _startTime;
        EmitSignal(SignalName.TimeChanged, ElapsedTimeSeconds);
    }
}
