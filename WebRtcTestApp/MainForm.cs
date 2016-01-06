using System;
using System.Windows.Forms;

namespace WebRtcTestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void btnStartCall_Click(object sender, EventArgs e)
        {
            var stunservers = new Tuple<string, string, string>[]
            {
                new Tuple<string, string, string>( "stun:stun.l.google.com:19305", null, null ),
                new Tuple<string, string, string>( "stun:stun1.l.google.com:19305", null, null ),
                new Tuple<string, string, string>( "stun:stun2.l.google.com:19305", null, null ),
                new Tuple<string, string, string>( "stun:stun3.l.google.com:19305", null, null ),
                new Tuple<string, string, string>( "stun:stun4.l.google.com:19305", null, null )
            };

            var model = new WebRtcModel(stunservers);
            await model.StartCallAsync(null);
        }
    }
}
