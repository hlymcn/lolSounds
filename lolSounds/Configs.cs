
namespace lolsounds
{
    public static class Configs
    {
        public static ConfigData GetConfigData()
        {
            // 这里应实现从配置文件读取数据的逻辑
            return new ConfigData();
        }

        public class ConfigData
        {
            public Dictionary<string, string> ChatCommandsToSounds { get; set; } = new Dictionary<string, string>
            {
                ["lol"] = "laugh.mp3",
                ["cao"] = "exclamation.mp3"
            };
        }
    }
}