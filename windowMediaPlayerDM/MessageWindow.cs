using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace windowMediaPlayerDM
{
    public partial class MessageWindow : Form
    {
        public MessageWindow()
        {
            InitializeComponent();
        }

        public String setMessage {
            get { return Message_text.Text; }
            set { Message_text.Text = value; }
        }
        public Label MessageBox {
            get { return Message_text; }
            set { Message_text = value; }
        }
        private void ok_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
