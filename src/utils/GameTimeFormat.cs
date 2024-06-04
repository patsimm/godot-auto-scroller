using System;
using Godot;

namespace Platformer;

public static class GameTimeFormat {
    public static string FormatGameTime(double seconds) {
        var timeSpan = TimeSpan.FromSeconds(seconds);
        return $"{Mathf.FloorToInt(timeSpan.TotalMinutes):D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}";
    }
}
