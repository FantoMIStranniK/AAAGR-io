using SFML.Audio;

namespace AAAGR_io.Engine.Audio
{
    public static class AudioSystem
    {
        public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();

        private static Random rand = new Random();

        public static void InitAudioFiles()
        {
            string pathToSounds = Game.PathToProject + @"\Sounds";

            string[] soundNames = Directory.GetFiles(pathToSounds);

            foreach (string soundName in soundNames) 
            { 
                var buffer = new SoundBuffer(soundName);

                Sound sound = new Sound(buffer);

                var editedSoundName = soundName.Replace(pathToSounds + @"\", "");

                Sounds.Add(editedSoundName, sound);
            }
        }
        public static void PlaySound(string soundName, bool isRandom = false, int countOfRandomVariants = 0, float soundVolume = 1f, float soundPitch = 1f, bool doLoop = false)
        {
            if (isRandom)
                soundName += rand.Next(1, countOfRandomVariants);

            soundName += ".wav";

            if (!Sounds.ContainsKey(soundName))
                return;

            var sound = Sounds[soundName];

            sound.Volume = soundVolume;
            sound.Pitch = soundPitch;
            sound.Loop = doLoop;

            sound.Play();
        }
    }
}
