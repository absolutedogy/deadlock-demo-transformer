namespace DevLock.DemoParser.DemData.Entity;

public class PlayerEntity
{
    public ulong SteamId { get; set; }
    public string Name { get; set; }
    
    public Vector3 LastPosition { get; set; }
    /// <summary>
    /// Track the delta of the players position over time, meaning that this value with be the change in position
    /// relative to the last recorded tick
    /// </summary>
    public List<(uint Tick, Vector3 Delta)> DeltaPosition { get; set; } = new();


    public void UpdatePosition(uint tick, Vector3 position)
    {
        if (LastPosition == null)
        {
            LastPosition = position;
            DeltaPosition.Add((tick, position));
            return;
        }
        
        if(position == LastPosition) return;
        
        var delta = new Vector3(position.X - LastPosition.X, position.Y - LastPosition.Y, position.Z - LastPosition.Z);
        LastPosition = position;
        DeltaPosition.Add((tick, delta));
    }
}
