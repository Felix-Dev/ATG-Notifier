using System.IO;
using System.Media;

namespace ATG_Notifier.Desktop.Utilities
{
    internal static class Utility
    {
        // TODO: Re-check if stream instead of path, i.e. Assets/Sound/Notification.wav, is the better solution here (path could enable XAML SoundPlayerAction use)
        public static void PlaySound(Stream stream) 
        {
            using (var soundPlayer = new SoundPlayer(stream))
            {
                soundPlayer.Play();
            }
        }
    }
}
