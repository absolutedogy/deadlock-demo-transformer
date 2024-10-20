using System.ComponentModel;
using System.IO.Compression;
using System.Text.Json;
using DemoFile;
using DemoFile.Game.Deadlock;
using DevLock.DemoParser.DemData;
using DevLock.DemoParser.Handlers;
using Parquet;
using Parquet.Schema;
using Parquet.Serialization;

namespace DevLock.DemoParser;

class Program
{
    public static DemParserConfig Config = new();

    static async Task Main(string[] args)
    {
        Config.CompressionMethod = CompressionMethod.Snappy;
        Config.CompressionLevel = CompressionLevel.SmallestSize;
        var demoPath = @"C:\tmp\DeadlockDemos\T001_M22863262_C129_S900955290.dem";

        var demo = new DeadlockDemoParser();

        demo.DemoEvents.DemoFileInfo += async e =>
        {
            Console.WriteLine($"==== Processing Demo ====");
            Console.WriteLine($"\tPlayback Frames: {e.PlaybackFrames}");
            Console.WriteLine($"\tPlayback Time: {e.PlaybackTime}");
            Console.WriteLine($"\tPlayback Ticks: {e.PlaybackTicks}");
        };
        


        // NOTE: This was my original thought, but it is likely not great in terms of space saving, so breaking
        // down to more files and using deltas will be better 
        Dictionary<ulong, List<PlayerRow>> playerData = new();
        demo.EntityEvents.CCitadelPlayerPawn.PostUpdate += async e =>
        {
            if (e?.Controller?.SteamID != null)
            {
                var steamId = e.Controller.SteamID;
                var newRow = new PlayerRow()
                {
                    Tick = demo.CurrentGameTick.Value,
                    SteamId = e.Controller.SteamID,
                    Origin = new Vector3(e.Origin.X, e.Origin.Y, e.Origin.Z),
                    ClientCamera = new Vector3(e.ClientCamera.Pitch, e.ClientCamera.Roll, e.ClientCamera.Yaw),
                    EyeAngles = new Vector3(e.EyeAngles.Pitch, e.EyeAngles.Roll, e.EyeAngles.Yaw),
                    Health = e.Health,
                    MaxHealth = e.MaxHealth,
                    HealingAmount = e.CurrentHealingAmount
                };


                if (playerData.ContainsKey(steamId))
                {
                    if (playerData[steamId] == null)
                        playerData[steamId] = new List<PlayerRow>();
                    playerData[steamId].Add(newRow);
                }
                else
                {
                    playerData[steamId] = new List<PlayerRow>();
                    playerData[steamId].Add(newRow);
                }
            }
        };
        
        UserMessageHandler userMessageHandler = new(ref demo.UserMessageEvents, ref demo);
        GameEventHandler gameEventHandler = new(ref demo.GameEvents, ref demo);
        EntityHandler entityHandler = new(ref demo.EntityEvents, ref demo);
        
        var reader = DemoFileReader.Create(demo, File.OpenRead(demoPath));
        await reader.ReadAllAsync();


        Console.WriteLine("==== Event Occurrences ====");
        Console.WriteLine("#### USER MESSAGES #### ");
        foreach (var occ in userMessageHandler.EventOccurrences)
        {
            Console.WriteLine($"\t{occ.Key,-24}: {occ.Value,6}");
        }

        Console.WriteLine("#### GAME EVENTS #### ");
        foreach (var occ in gameEventHandler.EventOccurrences)
        {
            Console.WriteLine($"\t{occ.Key,-24}: {occ.Value,6}");
        }
        
        // This is all commented out to focus on other parts, events are easy as they don't require any transformation... entities are a pain
        
       // foreach (var player in playerData)
       // {
       //     await SerializeList(player.Value, @$"C:\tmp\DemoParserResults\PlayerData\PlayerEntities{player.Key}.prq");
       // }

       foreach (var player in entityHandler.PlayerEntities)
       {
           await SerializeList( player.Value.DeltaPosition.Select(obj => new PlayerPositionRow()
           {
               SteamId = player.Key,
               DeltaX = obj.Delta.X,
               DeltaY = obj.Delta.Y,
               DeltaZ = obj.Delta.Z,
               Tick = obj.Tick,
           }), @$"C:\tmp\DemoParserResults\PlayerData\Position_{player.Key}.prq");
       }
       // await SerializeList(gameEventHandler.FireBullets, @$"C:\tmp\DemoParserResults\PlayerData\evt_FireBullets.prq");
       // await SerializeList(userMessageHandler.Damage, @$"C:\tmp\DemoParserResults\PlayerData\evt_Damage.prq");
       // await SerializeList(userMessageHandler.ChatMsg, @$"C:\tmp\DemoParserResults\PlayerData\evt_ChatMsg.prq");
            
    }


    static async Task SerializeList<T>(IEnumerable<T> list, string filePath)
    {
        await ParquetSerializer.SerializeAsync(list, filePath, new ParquetSerializerOptions()
        {
            CompressionLevel = Config.CompressionLevel,
            ParquetOptions = new ParquetOptions()
                { },
            CompressionMethod = Config.CompressionMethod
        });
    }
}