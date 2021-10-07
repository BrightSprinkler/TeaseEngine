using System;
using System.ComponentModel;
using System.IO;
using System.Media;

namespace TeaseEngine.Utils
{
    public class SoundService
    {
        private Logger Logger { get; } = App.Logging.GetLogger<SoundService>();

        public void PlayWavAsync(Stream stream)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(stream);
                player.LoadCompleted += delegate (object sender, AsyncCompletedEventArgs e)
                {
                    player.Play();
                };
                player.LoadAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        public void PlayFileWavAsync(string filePath)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(filePath);
                player.LoadCompleted += delegate (object sender, AsyncCompletedEventArgs e)
                {
                    player.Play();
                };
                player.LoadAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void PlayWav(Stream stream)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(stream);
                player.Play();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        public void PlayFileWav(string filePath)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(filePath);
                player.Play();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

    }
}
