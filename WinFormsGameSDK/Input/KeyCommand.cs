using System.Diagnostics;
using System.Windows.Forms;

namespace WinFormsGameSDK.Input
{
    /// <summary>
    /// Represents a command that corresponds to a key. This class indicates how
    /// frequently certain commands can fire and what keys fire the commands.
    /// </summary>
    public class KeyCommand
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();
        /// <summary>
        /// Allows the key to fire when first depressed.
        /// </summary>
        private bool firstFire = true;
        /// <summary>
        /// The last time the key was down.
        /// </summary>
        private long lastTimeDown;

        static KeyCommand()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Gets the command that is fired by the set <see cref="Key"/> value.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Gets or sets how frequently this key can yield an effect.
        /// </summary>
        public int DownInterval { get; }

        /// <summary>
        /// Gets or sets the key to time.
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyCommand"/> class
        /// with the specified instance.
        /// </summary>
        /// <param name="command">the command that is fired by the provided <paramref name="key"/> value.</param>
        /// <param name="downInterval">The interval, in MS, between key down fires.
        /// This affects the rate at which this key can fire.</param>
        /// <param name="key">The key to time.</param>
        public KeyCommand(string command, int downInterval, Keys key)
        {
            DownInterval = downInterval;
            Key = key;
            Command = command;
        }

        /// <summary>
        /// Check to see if the key is depressed and how long since its last depression.
        /// </summary>
        /// <returns>True, if the key can yield an effect.</returns>
        public bool CheckIn()
        {
            long timeSinceLastDown = stopwatch.ElapsedMilliseconds - lastTimeDown;

            if (KeyInputManager.IsKeyPressed(Key) && (timeSinceLastDown >= DownInterval || firstFire))
            {
                lastTimeDown = stopwatch.ElapsedMilliseconds;
                firstFire = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Whether this instance equals the specified <see cref="KeyCommand"/> instance.
        /// </summary>
        /// <param name="other">The <see cref="KeyCommand"/> to compare to.</param>
        /// <returns>True, if equal, otherwise false.</returns>
        protected bool Equals(KeyCommand other)
        {
            return Key == other.Key;
        }
    }
}
