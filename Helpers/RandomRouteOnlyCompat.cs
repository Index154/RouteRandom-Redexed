using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using RouteRandomRedexed.Helpers;

namespace RouteRandomRedexed;

internal class RandomRouteOnlyCompat
{
    internal static List<CompatibleNoun> FilterRecentLevels(List<CompatibleNoun> routePlanetNodes)
    {
        List<CompatibleNoun> tempRoutePlanetNodes = [.. routePlanetNodes];
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

    internal static TerminalNode GetWeightedRandom(List<CompatibleNoun> routePlanetNodes){
        
        // Convert nodes list to list of level IDs
        List<int> levelIDs = [];
        foreach (CompatibleNoun compatibleNoun in routePlanetNodes.ToList()) {
            TerminalNode confirmNode = compatibleNoun.result.GetNodeAfterConfirmation();
            int id = StartOfRound.Instance.levels[confirmNode.buyRerouteToMoon].levelID;
            levelIDs.Add(id);
        }

        // Actual random selection process using level weights
        int weightSum = 0;
        foreach(KeyValuePair <int, ConfigEntry<int>> kvp in RandomRouteOnly.ConfigManager.levelWeights){
            if(levelIDs.Contains(kvp.Key)){
                weightSum += kvp.Value.Value;
            }
        }
        int selectionRoll = Random.Range(1, weightSum + 1);
        RouteRandomRedexed.Log.LogDebug("Level selection roll = " + selectionRoll);
        int index = 0;
        foreach(int levelID in levelIDs){
            if(selectionRoll <= RandomRouteOnly.ConfigManager.levelWeights[levelID].Value){
                RouteRandomRedexed.Log.LogInfo("Randomly selected level " + RandomRouteOnly.Helper.levels.Where(i => i.levelID == levelID).FirstOrDefault().name);
                break;
            }else{
                index++;
                selectionRoll -= RandomRouteOnly.ConfigManager.levelWeights[levelID].Value;
                RouteRandomRedexed.Log.LogDebug("Skipping level " + levelID);
            }
        }

        // Return the planet node corresponding to the selected level ID based on the order in the lists
        TerminalNode result = routePlanetNodes[index].result;
        return result;
    }

}