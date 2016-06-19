using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinFormsGameSDK.Input
{
    /// <summary>
    /// Represents a list of <see cref="KeyActionBinding"/>s.
    /// </summary>
    public class KeyActionBindingList : List<KeyActionBinding>
    {
        /// <summary>
        /// Adds a <see cref="KeyActionBinding"/> to the list.
        /// </summary>
        /// <param name="action">The action to add.</param>
        /// <param name="keys">The keys to add.</param>
        public void Add(Action<bool> action, Keys keys)
        {
            Add(new KeyActionBinding(action, keys));
        }
    }
}
