using System.IO.Compression;
using Parquet;

namespace DevLock.DemoParser;

public class DemParserConfig
{
    public CompressionMethod CompressionMethod { get; set; }
    public CompressionLevel CompressionLevel { get; set; }
    
    
}