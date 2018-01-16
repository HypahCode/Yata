using System.Windows.Forms;

namespace Yata.CoreNode
{
    public partial class EnvelopeEditorForm : Form
    {
        public EnvelopeEditorForm(Envelope env)
        {
            InitializeComponent();

            EnvenlopeControl envCtr = new EnvenlopeControl(env);
            envCtr.Parent = panel1;
            envCtr.Dock = DockStyle.Fill;
        }
    }
}
