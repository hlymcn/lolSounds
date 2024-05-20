using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lolSounds
{
    [MinimumApiVersion(164)]
    public class LolSoundsPlugin : BasePlugin
    {
        private ConfigData _configData;

        public override string ModuleName => "[Custom] lolSounds";
        public override string ModuleVersion => "0.0.1 [Beta]";
        public override string ModuleAuthor => "hlymcn";
        public override string ModuleDescription => "Plays sounds based on chat commands defined in a configuration file.";

        public override void Load(bool hotReload)
        {
            Logger.LogInformation("lolSounds loaded.");
            string configPath = Path.Combine(ModuleDirectory, "lolSoundsConfig.json");

            if (File.Exists(configPath))
            {
                string jsonConfig = File.ReadAllText(configPath);
                _configData = JsonConvert.DeserializeObject<ConfigData>(jsonConfig);
                Logger.LogInformation($"Config loaded from {configPath}");
            }
            else
            {
                Logger.LogWarning($"lolSoundsConfig.json not found at {configPath}. Generating default configuration.");
                _configData = GenerateDefaultConfig();
                SaveConfig(configPath);
                Logger.LogInformation($"Default config saved to {configPath}");
            }

            RegisterEventHandler<EventPlayerChat>(OnPlayerChat, HookMode.Post);
        }

        private void SaveConfig(string filePath)
        {
            string configJson = JsonConvert.SerializeObject(_configData, Formatting.Indented);
            File.WriteAllText(filePath, configJson);
            Logger.LogInformation($"Config saved to {filePath}");
        }

        private ConfigData GenerateDefaultConfig()
        {
            var defaultAudioCommands = new List<AudioCommand>
            {
                new AudioCommand { Command = "!lol", AudioPath = "audio/lol.mp3" }
            };
            return new ConfigData { AudioCommands = defaultAudioCommands };
        }

        private HookResult OnPlayerChat(EventPlayerChat chatEvent, GameEventInfo eventInfo)
        {
            if (chatEvent == null) return HookResult.Continue;

            string message = chatEvent.Text.Trim().ToLower();
            var matchingCommand = _configData.AudioCommands.FirstOrDefault(cmd => cmd.Command == message);
            if (matchingCommand != null)
            {
                var player = Utilities.GetPlayerFromUserid(chatEvent.Userid);
                if (player != null && player.IsValid && !player.IsBot && !player.IsHLTV)
                {
                    string audioCommand = $"play {matchingCommand.AudioPath}";
                    Task.Run(() => player.ExecuteClientCommand(audioCommand));
                    Logger.LogInformation($"Playing audio '{matchingCommand.AudioPath}' for {player.PlayerName}");
                }
            }

            return HookResult.Continue;
        }
    }

    public class ConfigData
    {
        public List<AudioCommand> AudioCommands { get; set; } = new List<AudioCommand>();
    }

    public class AudioCommand
    {
        public string Command { get; set; }
        public string AudioPath { get; set; }
    }
}