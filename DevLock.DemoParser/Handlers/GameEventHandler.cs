using DemoFile;
using DemoFile.Game.Deadlock;
using DevLock.DemoParser.DemData;

namespace DevLock.DemoParser.Handlers;

public class GameEventHandler
{
    public List<FireBulletsRow> FireBullets { get; } = new();
    public List<BulletImpactRow> BulletImpact { get; } = new();
    public Dictionary<string, int> EventOccurrences { get; } = new Dictionary<string, int>();
    private readonly DeadlockDemoParser demParser;

    public GameEventHandler(ref GameEvents events, ref DeadlockDemoParser demo)
    {
        demParser = demo;
        events.FireBullets += HandleFireBullets;
        events.BulletImpact += HandleBulletImpacts;
    }
    


    private void HandleBulletImpacts(CMsgBulletImpact e)
    {
        CountEventOccurrence("BulletImpact");
        
        BulletImpact.Add(new BulletImpactRow()
        {
            Tick = demParser.CurrentGameTick.Value,
            WeaponSubclassId = e.WeaponSubclassId,
            SurfaceType = e.SurfaceType,
            Damage = e.Damage,
            AbilityEndIndex = e.AbilityEntindex,
            ImpactedEntIndex = e.ImpactedEntindex,
            ImpactedHitBox = e.ImpactedHitbox,
            ImpactOrigin = new Vector3(e.ImpactOrigin.X,e.ImpactOrigin.Y,e.ImpactOrigin.Z),
            ShooterEntIndex = e.ShooterEntindex,
        });
    }

    private void HandleFireBullets(CMsgFireBullets e)
    {
        CountEventOccurrence("FireBullets");
        FireBullets.Add(new FireBulletsRow()
        {
            Origin = new Vector3(e.Origin.X, e.Origin.Y, e.Origin.Z),
            Tick = demParser.CurrentGameTick.Value,
            Ability = e.Ability,
            Angles = new Vector3(e.Angles.X, e.Angles.Y, e.Angles.Z),
            BulletsOverride = e.BulletsOverride,
            Spread = e.Spread,
            IgnoreEntity = e.IgnoreEntity,
            MaxRange = e.MaxRange,
            PenetrationPercent = e.PenetrationPercent,
            ShooterEntity = e.ShooterEntity,
            ShotNumber = e.ShotNumber,
            WeaponSubclassId = e.WeaponSubclassId,
            FiredFromGun = e.FiredFromGun
        });
    }
    
    /// <summary>
    /// Checks the dictionary if an event is being tracked, and adds it if needed, otherwise
    /// increments the occruances by one
    /// </summary>
    /// <param name="eventName"></param>
    private void CountEventOccurrence(string eventName)
    {
        if (!EventOccurrences.TryAdd(eventName, 1))
            EventOccurrences[eventName]++;
    }
}