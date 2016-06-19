using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsGameSDK
{
    /// <summary>
    /// Provides a cloning contract to an instance Type.
    /// </summary>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Creates a clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        T Clone();
    }
}
