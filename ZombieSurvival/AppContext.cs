using System.Windows.Forms;
using WinFormsGameSDK.Input;
using ZombieSurvival.Forms;

namespace ZombieSurvival
{
    class AppContext : ApplicationContext
    {
        private readonly MainForm mainForm = new MainForm();

        public AppContext()
        {
            MainForm = mainForm;
            const int KEY_FREQ = 5;
            KeyInputManager.Default.AddKeyCommand("Move Up", Keys.W, KEY_FREQ);
            KeyInputManager.Default.AddKeyCommand("Move Down", Keys.S, KEY_FREQ);
            KeyInputManager.Default.AddKeyCommand("Move Left", Keys.A, KEY_FREQ);
            KeyInputManager.Default.AddKeyCommand("Move Right", Keys.D, KEY_FREQ);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                MainForm.Dispose();

            base.Dispose(disposing);
        }
    }
}
