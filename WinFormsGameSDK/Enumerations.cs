using System;

namespace WinFormsGameSDK
{
    /// <summary>
    /// Specifies the kind of movement a sprite does.
    /// </summary>
    public enum MovementKind
    {
        /// <summary>
        /// The sprite turns and projects forwards and backwards relative to the screen.
        /// For instance, a forward movement will always result in an upwards movement
        /// on the screen.
        /// </summary>
        Global,
        /// <summary>
        /// The sprite turns and projects forwards and backwards relative to its facing degree.
        /// For instance, a forward movement will always project the sprite forwards on its vector.
        /// </summary>
        Local
    }

    /// <summary>
    /// Specifies a direction in which to turn.
    /// </summary>
    public enum TurnDirection
    {
        /// <summary>
        /// No specific direction.
        /// </summary>
        None,
        /// <summary>
        /// Indicates left turning.
        /// </summary>
        Left,
        /// <summary>
        /// Indicates right turning.
        /// </summary>
        right
    }

    /// <summary>
    /// Specifies a direction in which to move.
    /// </summary>
    [Flags]
    public enum MoveDirection
    {
        /// <summary>
        /// Indicate no movement.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicate a forwards movement.
        /// </summary>
        Forwards = 1,
        /// <summary>
        /// Indicate a backwards movement.
        /// </summary>
        Backwards = 2,
        /// <summary>
        /// Indicate a movement to the left.
        /// </summary>
        Left = 4,
        /// <summary>
        /// Indicate a movement to the right.
        /// </summary>
        Right = 8
    }
}
