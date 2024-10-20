using System.Numerics;

namespace DevLock.DemoParser.DemData;

public class FireBulletsRow
{
    public uint Tick { get; set; }
    public Vector3 Origin { get; set; }
    public Vector3 Angles { get; set; }
    public int Ability { get; set; }
    public uint BulletsOverride {get; set;}
    public float Spread { get; set; }
    public int IgnoreEntity { get; set; }
    public float MaxRange { get; set; }
    public float PenetrationPercent { get; set; }
    public int ShooterEntity { get; set; }
    public uint ShotNumber { get; set; }
    public uint WeaponSubclassId { get; set; }
    public bool FiredFromGun { get; set; }
}