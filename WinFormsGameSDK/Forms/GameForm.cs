using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WinFormsGameSDK.Input;

namespace WinFormsGameSDK.Forms
{
    /// <summary>
    /// Represents a Form for rendering a simple game.
    /// </summary>
    public partial class GameForm : Form
    {
        /// <summary>
        /// Gets or sets the caption to display. The caption is for debugging purposes.
        /// </summary>
        public static string DebugCaption { get; set; }

        private readonly Stopwatch stopwatch = new Stopwatch();
        private int frames;

        /// <summary>
        /// Gets the key/action bindings (or shortcut keys) for this Form.
        /// </summary>
        protected KeyActionBindingList KeyActionBindings { get; } = new KeyActionBindingList();

        /// <summary>
        /// Gets how many seconds have elapsed since application start.
        /// </summary>
        [Browsable(false)]
        public long SecondsElapsed { get; private set; }

        /// <summary>
        /// Gets or sets whether to show debug information.
        /// </summary>
        public virtual bool ShowDebugInfo { get; set; }

        /// <summary>
        /// Gets the frame rate, in frames per second.
        /// </summary>
        [Browsable(false)]
        public int FrameRate { get; private set; }

        /// <summary>
        /// Gets or sets whether this window is full-screen.
        /// </summary>
        public bool FullScreen
        {
            get { return FormBorderStyle == FormBorderStyle.None; }
            set
            {
                if (value)
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                    TopMost = false;
                }
            }
        }

        protected GameForm()
        {
            InitializeComponent();
            stopwatch.Start();
            KeyActionBindings.Add(keyDown =>
            {
                if (keyDown)
                    ShowDebugInfo = !ShowDebugInfo;
            },
            Keys.Control | Keys.D);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
#if DEBUG
            TopMost = false;
#endif
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data. </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            foreach (var binding in KeyActionBindings)
            {
                if (binding.Keys == e.KeyData)
                {
                    binding.Invoke(true);
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            foreach (var binding in KeyActionBindings)
            {
                if (binding.Keys == e.KeyData)
                {
                    binding.Invoke(false);
                }
            }
        }

        /// <summary>
        /// Raises when a second has elapsed.
        /// </summary>
        protected virtual void OnSecondElapsed() { }

        /// <summary>
        /// Implements rendering logic for the game.
        /// </summary>
        /// <param name="graphics">The surface to draw to.</param>
        protected virtual void Render(Graphics graphics) { }

        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Render(e.Graphics);

            if (ShowDebugInfo && DebugCaption != null)
            {
                e.Graphics.DrawString(DebugCaption, Font, Brushes.Lime, 10, 10);
            }

            frames++;

            if (stopwatch.ElapsedMilliseconds >= 1000)
            {
                SecondsElapsed++;
                FrameRate = frames;
                frames = 0;
                stopwatch.Restart();
                OnSecondElapsed();
            }
        }

        private void timerInvalidate_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
