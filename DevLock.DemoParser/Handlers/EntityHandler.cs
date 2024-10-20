using DemoFile;
using DemoFile.Game.Deadlock;
using DevLock.DemoParser.DemData.Entity;

namespace DevLock.DemoParser.Handlers;

public class EntityHandler
{
    private readonly DeadlockDemoParser _demParser;
    public Dictionary<ulong, PlayerEntity> PlayerEntities { get; } = new Dictionary<ulong, PlayerEntity>();
    public EntityHandler(ref EntityEvents demoEntityEvents, ref DeadlockDemoParser demo)
    {
        _demParser = demo;
        demoEntityEvents.CCitadelPlayerPawn.Create += PlayerPawn_Create;
        demoEntityEvents.CCitadelPlayerPawn.Delete += PlayerPawn_Delete;
        demoEntityEvents.CCitadelPlayerPawn.PostUpdate += PlayerPawn_Updated;
    }

    private void PlayerPawn_Updated(CCitadelPlayerPawn obj)
    {
        if (obj?.Controller?.SteamID == null) return;
        if (PlayerEntities.TryGetValue(obj.Controller.SteamID, out var playerEntity))
        {
            playerEntity.UpdatePosition(_demParser.CurrentGameTick.Value, new Vector3(obj.Origin.X, obj.Origin.Y, obj.Origin.Z));
        }
    }
    
    private void PlayerPawn_Delete(CCitadelPlayerPawn obj)
    {
        Console.WriteLine("Player Deleted");
    }
    private void PlayerPawn_Create(CCitadelPlayerPawn obj)
    {
        if (obj?.Controller?.SteamID == null) return;
        var steamId = obj.Controller.SteamID;
        var newPlayerEntity = new PlayerEntity()
        {
            SteamId = steamId,
            Name = obj?.Controller?.PlayerName
        };
        PlayerEntities.Add(steamId, newPlayerEntity);
        newPlayerEntity.UpdatePosition(_demParser.CurrentGameTick.Value, new Vector3(obj.Origin.X, obj.Origin.Y, obj.Origin.Z));
    }
}