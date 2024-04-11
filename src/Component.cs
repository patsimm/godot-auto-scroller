using System.Linq;
using Godot;

public abstract partial class Component<T> : Node2D where T : Node
{
    public required T Parent;

    public override string[] _GetConfigurationWarnings()
    {
        var errors = base._GetConfigurationWarnings().ToList();

        if (GetParent() is not T)
            errors.Add($"Component must be child of {typeof(T).Name}");

        return errors.ToArray();
    }

    public override void _Ready()
    {
        Parent = GetParent<T>();
    }
}