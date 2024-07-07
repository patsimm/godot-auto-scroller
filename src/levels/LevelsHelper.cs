using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using Godot;

namespace Platformer;

public static partial class LevelsHelper {
    public static ReadOnlyCollection<int> GetLevels() {
        var levelList = new List<int>();
        foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory() + "/src/levels/scenes")) {
            var splittedPaths = file.Split("/");
            var fileWithoutPath = splittedPaths[splittedPaths.Length - 1];
            var levelNumber = LevelNumberRegex().Match(fileWithoutPath);
            levelList.Add(int.Parse(levelNumber.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture));
        }

        levelList.Sort();
        return new ReadOnlyCollection<int>(levelList);
    }

    [GeneratedRegex(@"_(\d+).")]
    private static partial Regex LevelNumberRegex();
}