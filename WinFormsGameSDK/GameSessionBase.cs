using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinFormsGameSDK
{
    /// <summary>
    /// Represents a single game session.
    /// </summary>
    public abstract class GameSessionBase : IDisposable
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private int ticks = 60;
        private long secondTracker, splitSecondTracker;

        /// <summary>
        /// Gets how many seconds have elapsed since the game started.
        /// </summary>
        protected int SecondsElapsed { get; private set; }

        /// <summary>
        /// Gets how many game loops complete per second.
        /// </summary>
        public static int TickRate { get; private set; } = 60;

        /// <summary>
        /// Gets the timer used to loop the game.
        /// </summary>
        private Timer UpdateTimer { get; } = new Timer();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSessionBase"/> class.
        /// </summary>
        protected GameSessionBase()
        {
            UpdateTimer.Tick += UpdateTimerTick;
            UpdateTimer.Interval = 10;
            UpdateTimer.Start();
            stopwatch.Start();
        }

        /// <summary>
        /// Implements game logic.
        /// </summary>
        protected abstract void GameLoop();

        /// <summary>
        /// Raises when a second has elapsed.
        /// </summary>
        protected virtual void OnSecondElapsed() { }

        /// <summary>
        /// Raises when half of a second has elapsed.
        /// </summary>
        protected virtual void OnSplitSecondElapsed() { }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            GameLoop();

            if (stopwatch.ElapsedMilliseconds - secondTracker >= 1000)
            {
                TickRate = ticks;
                ticks = 0;
                SecondsElapsed++;
                secondTracker = stopwatch.ElapsedMilliseconds;
                OnSecondElapsed();
            }

            if (stopwatch.ElapsedMilliseconds - splitSecondTracker >= 500)
            {
                splitSecondTracker = stopwatch.ElapsedMilliseconds;
                OnSplitSecondElapsed();
            }

            ticks++;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            UpdateTimer.Dispose();
        }
    }
}
