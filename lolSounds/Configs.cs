
namespace lolsounds
{
    public static class Configs
    {
        public static ConfigData GetConfigData()
        {
            // ����Ӧʵ�ִ������ļ���ȡ���ݵ��߼�
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