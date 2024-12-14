using LethalConstellations.PluginCore;

namespace RouteRandomRedexed;

internal class ConstellationsCompat
{
    internal static bool IsLevelInConstellation(SelectableLevel level)
    {
        return ClassMapper.IsLevelInConstellation(level);
    }
}