using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;



namespace windowMediaPlayerDM
{



    public partial class newMoveEngine_Panel : Form
    {
        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Transparent = 0x20,
            Layered = 0x80000
        }

        public enum LWA
        {
            ColorKey = 0x1,
            Alpha = 0x2
        }


        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);


        private List<Panel> CommentHolders = new List<Panel>();
        private Dictionary<int, String> comments = new Dictionary<int, string>();
        public void SetFormTransparent(IntPtr Handle)
        {
            int oldWindowLong = GetWindowLong(Handle, GWL.ExStyle);
            SetWindowLong(Handle, GWL.ExStyle, Convert.ToInt32(oldWindowLong | Convert.ToInt32(WS_EX.Layered) | Convert.ToInt32(WS_EX.Transparent)));
        }

        public void SetFormNormal(IntPtr Handle)
        {
            int oldWindowLong = GetWindowLong(Handle, GWL.ExStyle);
            SetWindowLong(Handle, GWL.ExStyle, Convert.ToInt32(oldWindowLong | Convert.ToInt32(WS_EX.Layered)));
        }

        private bool upper { set; get; }
        public newMoveEngine_Panel()
        {
            InitializeComponent();
            this.TransparencyKey = Color.AliceBlue;
            this.BackColor = Color.AliceBlue;
            SetFormTransparent(this.Handle);
            this.TopMost = true;

            /*
            Label testlable = new Label();
            testlable.Text = "TEST";
            testlable.Location = new Point(1000, 1000);
            testlable.Show();
            this.Controls.Add(testlable);
              
             */
            played_comments = 0;
            move_distance = 25;
            current_time = -1;
            userColor = Color.White;
            userOutLineColor = Color.Black;
            userOutLineWidth = 2;
            fontsize = 20;
            upper = true;


        }
        private void createPanel() {
            Panel p1 = new Panel();
            p1.BackColor = Color.AliceBlue;
            SetFormTransparent(p1.Handle);
            p1.Size = this.Size;
            p1.Location = new Point(ClientRectangle.Right, this.Location.Y);
            this.Controls.Add(p1);
            CommentHolders.Add(p1);


        
        }
        public int fontsize
        {
            get;
            set;
        }
        public Color userColor { get; set; }
        public Color userOutLineColor { get; set; }
        public int userOutLineWidth { get; set; }
        private void createLabel(String comment){
        
        //    if(comments.ContainsKey(time)){
                played_comments++;
                Font_Outline dm = new Font_Outline();
                dm.Text = comment;
                dm.Name = dm.Text;
                dm.Font = new Font("Microsoft Sans Serif", fontsize, FontStyle.Bold);

                dm.ForeColor = userColor;
                dm.OutlineForeColor = userOutLineColor;
                dm.OutlineWidth = userOutLineWidth;

                Size s = TextRenderer.MeasureText(dm.Text, dm.Font, new Size(dm.Text.Length * 100 + 7, 1000),
TextFormatFlags.VerticalCenter
| TextFormatFlags.Left
| TextFormatFlags.NoPadding
| TextFormatFlags.WordBreak);

                // dm.MaximumSize= new Size(value.Length*100,0);
                dm.Size = new Size(s.Width + 70, s.Height);

                int ycurrent = 0;
                Random ypos = new Random();

                if (0 < this.Size.Height - (dm.Size.Height))
                {
                    if (upper == false)
                    {
                        ycurrent = ypos.Next(0, this.Size.Height / 2 - (dm.Size.Height));//fm3.ClientRectangle.Bottom
                        upper = true;
                    }
                    else
                    {
                        ycurrent = ypos.Next(this.Size.Height / 2, this.Size.Height - (dm.Size.Height));//fm3.ClientRectangle.Bottom
                        upper = false;
                    }

                }
                else
                {

                    ycurrent = 0;
                }

                dm.Location = new Point(ClientRectangle.Right, ycurrent);


                if ((dm.Size.Width + dm.Location.X) > (CommentHolders[0].Size.Width + CommentHolders[0].Location.X))
                {
                    if (CommentHolders.Count < 2)
                    {
                        if (CommentHolders[0].Location.X <= 0)
                        {
                            createPanel();
                            CommentHolders[1].Controls.Add(dm);
                        }
                        else
                        {

                            CommentHolders[0].Size = new Size(CommentHolders[0].Size.Width + dm.Size.Width, CommentHolders[0].Size.Height);
                            CommentHolders[0].Controls.Add(dm);
                        }
                    }

                }
                else {
                    CommentHolders[1].Controls.Add(dm);
                }

       //     }
        
        }
        public int move_distance { get; set; }
        public int played_comments { get; set; }

        public int current_time
        {
            get { return current_time; }
            set
            {
                if (current_time != value)
                {
                    current_time = value;

                }
                movePanel();

            }
        }
        private void movePanel() {
            List<Panel> removelist = new List<Panel>();
            if (CommentHolders.Count > 0) {
                foreach (Panel p in CommentHolders) {
                    p.Location = new Point(p.Location.X-move_distance,p.Location.Y);

                    if (p.Location.X < (0 - p.Size.Width)) {
                        removelist.Add(p);
                    
                    }
                }

                foreach (Panel p in removelist) {
                    CommentHolders.Remove(p);
                    p.Dispose();
                }
            }

        
        }
        public Dictionary<int, String> setDictionary {
            get { return this.comments; }
            set {
                this.comments = value;
            }
        }
        public Size ChangeSize {
            get { return this.Size ; }
            set{
                this.Size = value;
                foreach (Panel i in CommentHolders) {
                    i.Size = value;
                }
            }
        }
        public Point ChangeLocation {
            get { return this.Location; }
            set { this.Location = value;
            foreach (Panel i in CommentHolders)
            {
                i.Location = value;
            }
            }
        }
    }
}
