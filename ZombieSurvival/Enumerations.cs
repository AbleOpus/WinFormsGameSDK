namespace ZombieSurvival
{
    /// <summary>
    /// Specifies what kind of effect to use for a contrail.
    /// </summary>
    enum ContrailKind
    {
        /// <summary>
        /// Indicates that a blood effect is used.
        /// </summary>
        Blood,
        /// <summary>
        /// Indicates that a bullet effect is used.
        /// </summary>
        Bullet,
        /// <summary>
        /// Indicates that a defined effect is used for debugging.
        /// </summary>
        Debug
    }

    /// <summary>
    /// Specifies one of the states for a shooting animation.
    /// </summary>
    public enum ShootingAnimationState
    {
        /// <summary>
        /// The animation is idle.
        /// </summary>
        None,
        /// <summary>
        /// The animation is retracting back (occurs right after firing).
        /// </summary>
        Retracting,
        /// <summary>
        /// The animation is moving back to its idle state.
        /// </summary>
        Restoring
    }
}
