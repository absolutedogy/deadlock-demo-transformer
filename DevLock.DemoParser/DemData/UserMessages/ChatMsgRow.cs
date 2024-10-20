namespace DevLock.DemoParser.DemData;

public class ChatMsgRow
{
    public uint Tick { get; set; }
    public int PlayerSlot { get; set; }
    public string Text { get; set; }
    public bool AllChat { get; set; }
}