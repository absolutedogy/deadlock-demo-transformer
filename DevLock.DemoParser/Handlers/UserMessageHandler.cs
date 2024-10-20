using DemoFile;
using DemoFile.Game.Deadlock;
using DevLock.DemoParser.DemData;

namespace DevLock.DemoParser.Handlers;

public class UserMessageHandler
{
    public Dictionary<string, int> EventOccurrences { get; } = new Dictionary<string, int>();
    public List<ChatMsgRow> ChatMsg { get; } = new();
    public List<DamageRow> Damage { get; } = new();
    private readonly DeadlockDemoParser demParser;

    public UserMessageHandler(ref UserMessageEvents events, ref DeadlockDemoParser demo)
    {
        demParser = demo;
        
        events.AbilityNotify += AbilityNotify;
        events.AbilitiesChanged += AbilitiesChanged;
        events.AbilityInterrupted += AbilityInterrupted;
        events.AbilityPing += AbilityPing;
        events.AbilityLateFailure += AbilityLateFailure;
        
        events.BossKilled += BossKilled;
        events.BulletHit += BulletHit;

        events.ChatEvent += ChatEvent;
        events.ChatMsg += HandleChatMsg;
        events.ChatWheel += HandleChatWheel;
        events.CameraController += HandleCameraController;
        events.CurrencyChanged += HandleCurrencyChanged;
            

        events.Damage += HandleDamage;
    }

    private void HandleCurrencyChanged(CCitadelUserMessage_CurrencyChanged obj)
    {
        CountEventOccurrence("CurrencyChanged");
    }

    private void HandleCameraController(CCitadelUserMsg_CameraController obj)
    {
        CountEventOccurrence("CameraController");
    }

    private void HandleChatWheel(CCitadelUserMsg_ChatWheel obj)
    {
        CountEventOccurrence("ChatWheel");
    }
    private void HandleDamage(CCitadelUserMessage_Damage e)
    {
        CountEventOccurrence("Damage");
        Damage.Add(new DamageRow()
        {
            Tick = demParser.CurrentGameTick.Value,
            Damage = e.Damage,
            Flags = e.Flags,
            Hits = e.Hits,
            Type = e.Type,
            AbilityId = e.AbilityId,
            CitadelType = e.CitadelType,
            DamageAbsorbed = e.DamageAbsorbed,
            Origin = new Vector3(e.Origin.X, e.Origin.Y, e.Origin.Z),
            EntIndexAbility = e.EntindexAbility,
            EntIndexAttacker = e.EntindexAttacker,
            EntIndexInflictor = e.EntindexInflictor,
            EntIndexVictim = e.EntindexVictim
        });
    }

    private void BulletHit(CCitadelUserMessage_BulletHit e)
    {
        CountEventOccurrence("BulletHit");
    }

    private void HandleChatMsg(CCitadelUserMsg_ChatMsg e)
    {
        CountEventOccurrence("ChatMsg");
        ChatMsg.Add(new ChatMsgRow()
        {
            Tick = demParser.CurrentGameTick.Value,
            Text = e.Text,
            AllChat = e.AllChat,
            PlayerSlot = e.PlayerSlot,
        });
    }

    private void AbilitiesChanged(CCitadelUserMsg_AbilitiesChanged e)
    {
        CountEventOccurrence("AbilitiesChanged");
    }

    private void ChatEvent(CCitadelUserMsg_ChatEvent e)
    {
        CountEventOccurrence("ChatEvent");
    }

    /// <summary>
    /// I don't really know what this event is, it only has 3 data points, the ability ID and then an attacker and victim.
    /// Perhaps it is to notify that you got hit by an ability?
    /// 
    /// </summary>
    /// <param name="e"></param>
    private void AbilityNotify(CCitadelUserMessage_AbilityNotify e)
    {
        CountEventOccurrence("AbilityNotify");
    }

    private void AbilityInterrupted(CCitadelUserMsg_AbilityInterrupted e)
    {
        CountEventOccurrence("AbilityInterrupted");
    }

    private void AbilityPing(CCitadelUserMsg_AbilityPing e)
    {
        CountEventOccurrence("AbilityPing");
    }

    private void AbilityLateFailure(CCitadelUserMsg_AbilityLateFailure e)
    {
        CountEventOccurrence("AbilityLateFailure");
    }

    private void BossKilled(CCitadelUserMsg_BossKilled e)
    {
        CountEventOccurrence("BossKilled");
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