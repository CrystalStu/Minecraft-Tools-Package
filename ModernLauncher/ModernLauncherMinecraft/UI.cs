using System.Runtime.InteropServices;

namespace ModernLauncherMinecraft
{
    public static class UI
    {
        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand,
        string lpstrReturnString, uint uReturnLength, uint hWndCallback);
        public static void PlayBGM()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.ResourceManager.GetStream("misc"));
            player.PlayLooping();
        }
    }
}
