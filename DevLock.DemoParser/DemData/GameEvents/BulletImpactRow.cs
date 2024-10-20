namespace DevLock.DemoParser.DemData;

public class BulletImpactRow
{
    public uint Tick { get; set; }
    public uint WeaponSubclassId { get; set; }
    public uint SurfaceType { get; set; }
    public uint Damage { get; set; }
    public int AbilityEndIndex { get; set; }
    public int ImpactedEntIndex { get; set; }
    public uint ImpactedHitBox { get; set; }
    public Vector3 ImpactOrigin { get; set; }
    public int ShooterEntIndex { get; set; }
}