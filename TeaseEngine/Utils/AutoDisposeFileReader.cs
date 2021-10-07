using NAudio.Wave;

namespace TeaseEngine.Utils
{
    internal class AutoDisposeFileReader
    {
        private AudioFileReader input;

        public AutoDisposeFileReader(AudioFileReader input)
        {
            this.input = input;
        }
    }
}