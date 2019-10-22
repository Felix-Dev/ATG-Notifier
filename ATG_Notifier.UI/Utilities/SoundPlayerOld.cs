using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ATG_Notifier.Utilities
{
    public class SoundPlayerOld
    {
        private MediaPlayer player;

        private Object SoundPlayerLock = new Object();

        private bool busy;

        #region Creation

        public SoundPlayerOld(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            player = new MediaPlayer();
            var uri = new Uri("Resources/Windows Notify Messaging.wav", UriKind.Relative);

            player.Open(uri);
            player.MediaEnded += Player_MediaEnded;
            player.BufferingStarted += Player_BufferingStarted;
            player.BufferingEnded += Player_BufferingEnded;
            player.MediaFailed += Player_MediaFailed;
        }

        private void Player_MediaFailed(object sender, ExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Player_BufferingEnded(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Player_BufferingStarted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion // Creation

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            busy = false;
        }

        public void Play()
        {
            lock(SoundPlayerLock)
            {
                if (!busy)
                {
                    player.Play();
                    busy = true;
                }
            }     
        }

        public void Stop()
        {
            if (busy)
            {
                player.Stop();
                busy = false;
            }
        }        
    }
}
