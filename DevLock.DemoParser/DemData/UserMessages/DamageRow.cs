namespace DevLock.DemoParser.DemData;

public class DamageRow
{
    public uint Tick { get; set; }
    public int Damage { get; set; }
    public int CitadelType { get; set; }
    public uint AbilityId { get; set; }
    public int Type { get; set; }
    public int Hits { get; set; }
    public ulong Flags { get; set; }
    public int DamageAbsorbed { get; set; }
    public Vector3 Origin { get; set; }
    public int EntIndexAbility { get; set; }
    public int EntIndexAttacker { get; set; }
    public int EntIndexInflictor { get; set; }
    public int EntIndexVictim { get; set; }
}