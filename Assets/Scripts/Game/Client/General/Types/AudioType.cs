using System.ComponentModel;

namespace Game.Client
{
    public enum AudioType
    {
        [Description("void")] Void,
        [Description("music_lobby")] MusicLobby,
        [Description("music_battle")] MusicBattle,
    }
}