using System.IO;
using System.Media;

namespace ATG_Notifier.Desktop.Utilities
{
    internal static class Utility
    {
        public static void PlaySound(Stream stream) 
        {
            using (var soundPlayer = new SoundPlayer(stream))
            {
                soundPlayer.Play();
            }
        }
    }
}
