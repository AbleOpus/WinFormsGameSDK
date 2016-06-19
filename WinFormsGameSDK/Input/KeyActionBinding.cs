using System;
using System.Windows.Forms;

namespace WinFormsGameSDK.Input
{
    /// <summary>
    /// Binds <see cref="Keys"/> to <see cref="Action"/> for local shortcuts.
    /// </summary>
    public class KeyActionBinding
    {
        private readonly Action<bool> action;

        /// <summary>
        /// Gets or sets the keys of the binding.
        /// </summary>
        public Keys Keys { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyActionBinding"/> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="action">The action to invoke when the <paramref name="keys"/> argument is processed.</param>
        /// <param name="keys">The keys of the binding.</param>
        public KeyActionBinding(Action<bool> action, Keys keys)
        {
            this.action = action;
            Keys = keys;
        }

        /// <summary>
        /// Invokes the <see cref="Action"/> of this binding.
        /// </summary>
        /// <returns>True, if action is not null and can be invoked, otherwise false.</returns>
        public bool Invoke(bool isKeyDown)
        {
            if (action != null)
            {
                action.DynamicInvoke(isKeyDown);
                return true;
            }

            return false;
        }
    }
}
