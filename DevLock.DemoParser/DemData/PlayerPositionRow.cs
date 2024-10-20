namespace DevLock.DemoParser.DemData;

public class PlayerPositionRow
{
    public uint Tick { get; set; }
    public ulong SteamId { get; set; }
    public float DeltaX { get; set; }
    public float DeltaY { get; set; }
    public float DeltaZ { get; set; }
}