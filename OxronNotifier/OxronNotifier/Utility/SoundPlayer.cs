using System.IO;
using System.Media;

namespace OxronNotifier.Utility
{
    public static class SoundHelper
    {
        public static void PlayNotification()
        {
            using (UnmanagedMemoryStream fs = Resources.Resources.Notification)
            {
                SoundPlayer player = new SoundPlayer();
                player.Stream = fs;
                player.PlaySync();
            }
        }
    }
}
