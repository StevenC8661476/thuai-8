namespace Thuai.Server.GameLogic;

public partial class Battle
{
    private Map? Map = null;

    /// <summary>
    /// If map is null, generate a map.
    /// </summary>
    /// <returns>If the map available.</returns>
    private bool GenerateMap()
    {
        Map = Map.GenerateMaps(1, 10, 10)[0];
        return false;
    }

    /// <summary>
    /// Update the map.
    /// </summary>
    private void UpdateMap()
    {
        // TODO: implement.
    }
}