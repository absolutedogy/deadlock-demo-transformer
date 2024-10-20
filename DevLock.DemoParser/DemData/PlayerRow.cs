using System.Numerics;
using Parquet.Serialization.Attributes;

namespace DevLock.DemoParser.DemData;

public record PlayerRow()
{
    public uint Tick { get; set; }
    public ulong SteamId { get; set; }
    public Vector3 Origin { get; set; }
    public Vector3 ClientCamera { get; set; }
    public Vector3 EyeAngles { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public float HealingAmount { get; set; }
}