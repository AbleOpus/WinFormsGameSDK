using System.Drawing;
using System.Windows.Forms;
using WinFormsGameSDK.Sprites;

namespace ZombieSurvival.Forms
{
    /// <summary>
    /// For doing isolated test on various things.
    /// </summary>
    public partial class TestForm : Form
    {
        private readonly BoundarySprite boundary;

        public TestForm()
        {
            InitializeComponent();
            var rect = ClientRectangle;
            rect.Inflate(-50, -50);
            boundary = new BoundarySprite(rect, 20);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.Text = boundary.MovementCollision.ContainsPoint(e.Location).ToString();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawPath(Pens.Red, boundary.MovementCollision.Path);
        }
    }
}
