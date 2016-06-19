using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFormsGameSDK.Input
{
    /// <summary>
    /// Represents keyboard input.
    /// </summary>
    public class KeyInputManager
    {
        [DllImport("User32.dll")]
        private static extern short GetKeyState(Keys keys);

        /// <summary>
        /// The default instance of this class.
        /// </summary>
        public static readonly KeyInputManager Default = new KeyInputManager();

        private readonly List<KeyCommand> keyCommands = new List<KeyCommand>();

        /// <summary>
        /// Adds a <see cref="KeyCommand"/> to the manager.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="key">The key to add.</param>
        /// <param name="interval">The key down fire frequency.</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddKeyCommand(string command, Keys key, int interval)
        {
            if (HasKey(key))
                throw new ArgumentException("Key already exists.", nameof(key));

            keyCommands.Add(new KeyCommand(command, interval, key));
        }

        // For the singleton.
        private KeyInputManager() { }

        /// <summary>
        /// Gets whether this manager already has a <see cref="KeyCommand"/> instance which
        /// uses the specified key.
        /// </summary>
        private bool HasKey(Keys key)
        {
            return keyCommands.Any(k => k.Key == key);
        }


        /// <summary>
        /// Gets whether the specified key is being pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            short result = GetKeyState(key);

            switch (result)
            {
                case 0: return false;
                case 1: return false;
                default: return true;
            }
        }

        /// <summary>
        /// Gets the keys that have been depressed then invalidates the depression
        /// so the key must be pressed again or held down.
        /// </summary>
        public IEnumerable<KeyCommand> GetPendingCommands(params string[] emulatedKeyDowns)
        {
            // TODO: Emulate keydowns.
            Stack<KeyCommand> keys = new Stack<KeyCommand>();

            foreach (var TK in keyCommands.Where(TK => TK.CheckIn()))
            {
                keys.Push(TK);
            }

            return keys.ToArray();
        }
    }
}
