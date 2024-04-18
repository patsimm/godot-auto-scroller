using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Platformer;

public abstract partial class Component<T> : Node where T : Node {
    public required T Entity { get; set; }

    public override string[] _GetConfigurationWarnings() {
        var errors = base._GetConfigurationWarnings()?.ToList()
            ?? new List<string>();

        if (GetParent() is not T)
            errors.Add($"Component must be child of {typeof(T).Name}");

        return errors.ToArray();
    }

    public override void _Ready() {
        Entity = GetParent<T>();
    }
}