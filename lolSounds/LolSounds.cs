using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace lolsounds
{
    [MinimumApiVersion(164)]
    public class LoLSounds : BasePlugin
    {
        internal static IStringLocalizer? StringLocalizer;
        public override string ModuleName => "LoL Sounds";
        public override string ModuleVersion => "1.0.0";
        public override string ModuleAuthor => "Your Name";
        public override string ModuleDescription => "Plays sounds on specific chat commands.";
        internal static IStringLocalizer? Stringlocalizer;

        public override void Load(bool hotReload)
        {
            Stringlocalizer = Localizer;
            RegisterEventHandler<EventPlayerChat>(OnPlayerChat, HookMode.Post);
        }

        private HookResult OnPlayerChat(EventPlayerChat @event, GameEventInfo info)
        {

            var player = Utilities.GetPlayerFromUserid(@event.Userid);
            if (player == null || !player.IsValid)
                return HookResult.Continue;

            var message = @event.Text.Trim().ToLower();
            if (Configs.GetConfigData().ChatCommandsToSounds.ContainsKey(message))
            {
                player.ExecuteClientCommand("play " + Configs.GetConfigData().ChatCommandsToSounds[message]);
            }

            return HookResult.Continue;
        }
    }
}