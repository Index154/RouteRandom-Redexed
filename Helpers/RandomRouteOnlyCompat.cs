using System.Collections.Generic;
using System.Linq;
using RouteRandomRedexed.Helpers;

namespace RouteRandomRedexed;

internal class RandomRouteOnlyCompat
{
    internal static List<CompatibleNoun> FilterRecentLevels(List<CompatibleNoun> routePlanetNodes)
    {
        List<CompatibleNoun> tempRoutePlanetNodes = new List<CompatibleNoun>(routePlanetNodes);
        foreach (CompatibleNoun compatibleNoun in tempRoutePlanetNodes.ToList()) {
            TerminalNode confirmNode = compatibleNoun.result.GetNodeAfterConfirmation();
            SelectableLevel moonLevel = StartOfRound.Instance.levels[confirmNode.buyRerouteToMoon];
            if (RandomRouteOnly.Helper.recentLevels.Contains(moonLevel.levelID)) {
                tempRoutePlanetNodes.Remove(compatibleNoun);
            }
        }
        if(tempRoutePlanetNodes.Count > 0){
            routePlanetNodes = tempRoutePlanetNodes;
        }else{
            // If there would be no moons left to choose, clear the recent moons list and use the full list of nodes instead
            RandomRouteOnly.Helper.recentLevels.Clear();
        }
        return routePlanetNodes;
    }
}