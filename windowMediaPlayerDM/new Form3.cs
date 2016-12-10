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



    public partial class Neo_Comment_window : Form
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


      

        public void SetFormTransparent(IntPtr Handle)
        {
          int oldWindowLong = GetWindowLong(Handle, GWL.ExStyle);
          SetWindowLong(Handle, GWL.ExStyle, Convert.ToInt32(oldWindowLong | Convert.ToInt32(WS_EX.Layered) |Convert.ToInt32( WS_EX.Transparent)));
        }

        public void SetFormNormal(IntPtr Handle)
        {
            int oldWindowLong = GetWindowLong(Handle, GWL.ExStyle);
            SetWindowLong(Handle, GWL.ExStyle, Convert.ToInt32(oldWindowLong | Convert.ToInt32(WS_EX.Layered)));
        }




        //instead of creating the comment on the main thread
        // do it here
        // and move the comments here
        // (that's if every frame runs on a differnt thread)
        // if not then have to create a thread that moves comments here
        // to fix the cross thread problem

        List<Label> comment_storage;
        int move;
        Form1 fm1 = new Form1();
        int interval;
        BackgroundWorker bk201;
        bool playing;
        int move_distance;
        int _distance; 
        int playedcomment;
        Dictionary<int, String> comments;
        int currenttime;
        int commentLimit;
       
        public Neo_Comment_window( bool uppert)
        {
            InitializeComponent();
            
            move = -1;
            this.TransparencyKey = Color.AliceBlue;
            this.BackColor = Color.AliceBlue;

            playing = false;
            move_distance = 20;
            _distance = 20;
            this.Size = fm1.currentMediaWindowSize;
            this.Location = fm1.currentMediaWindowLocation;
            this.Show();
            this.TopMost = false;
            this.BringToFront();
            this.ShowInTaskbar = false;
            comment_storage = new List<Label>();
            userColor = Color.White;
            comments = new Dictionary<int, string>();
            fullscreen = false;
            interval = 60;
            bk201 = new BackgroundWorker();
            bk201.WorkerSupportsCancellation = true;
            bk201.DoWork += new DoWorkEventHandler(bk201_DoWork);
            currenttime = 0;
            commentLimit = 20;
            playedcomment = 0;
            userOutlineColor = Color.Black;
            userOutlineWidth = 2;
            fontsize = 20;
            upper = uppert;
        }

        public Color userOutlineColor
        {
            set;
            get;
        }
        public int getplayedComment {
            get { return playedcomment; }
            set {  playedcomment=value; }
        }
        public int setCommentLimit {
            get { return commentLimit; }
            set { commentLimit = value; }
        
        }
        public Dictionary<int,String> setDictionary {

            get { return this.comments; }
            set { this.comments = value; }
        
        
        }
        String makeCCompactComponet(String comment)
        {
            String result = "";

            if (comment.Length / commentLimit > 0)
            {

                for (int j = 0; j < comment.Length / commentLimit; j++)
                {
                    //use commentLimit-1 instead commentLimit to avoid overflow
                    comment = comment.Insert((commentLimit - 1) * (j + 1), Environment.NewLine);


                }
            }

            result = comment;

            return result;


        }
        void makeCCompact(String comment)
        {


            if (comment.Length > commentLimit)
            {

                //if the comment is longer than limit and it contains newline
                if (comment.Contains(Environment.NewLine))
                {

                    string[] comments = comment.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    // now all the comments originally in newlines are now seperate

                    for (int i = 0; i < comments.Length; i++)
                    {

                        //iterate  through each strings

                        comments[i] = makeCCompactComponet(comments[i]);
                        createLabel=(comments[i]);



                    }



                }
                else
                {

                    comment = makeCCompactComponet(comment);
                    createLabel=(comment);

                }




            }
            else
            {


                createLabel=(comment);

            }





        }
        int count = 0;
        public int createLabel_extra {
            get { return currenttime; }

            set{

                if (comments.ContainsKey(value))
                {


                    makeCCompact(comments[value]);
                    currenttime = value;
        //            movecommentOnCreate();
                }
        //        else {

           //         movecommentOnCreate();
//
         //       }
            }
        
        
        }
        public bool moveC {

            set {
                movecommentOnCreate();
            }
        
        }
       public void movecommentOnCreate() {
            if (count == 5)
            {
                changeingSpeed();
                movecomment = move_distance;
                count = 0;

            }
            else
            {

                count++;
            }
        
        
        }
        void changeingSpeed() {
            int lnumber = this.comment_storage.Count * 7;
            if (lnumber > 180)
            {

                move_distance = (int)(_distance * 4);

            }
            else if (lnumber > 110)
            {

                move_distance = (int)(_distance * 3);

            }
            else if (lnumber > 80)
            {

                move_distance = (int)(_distance * 2.7);
            }
            else if (lnumber > 70)
            {

                move_distance = (int)(_distance * 2.3);
            }
            else if (lnumber > 60)
            {
                move_distance = (int)(_distance * 2.1);
            }
            else if (lnumber > 50)
            {
                move_distance = (int)(_distance * 1.7);
            }
            else if (lnumber > 40)
            {
                move_distance = (int)(_distance * 1.5);

            }
            else if (lnumber > 30)
            {

                move_distance = (int)(_distance * 1.3);

            }
            else if (lnumber > 20)
            {

                move_distance = (int)(_distance * 1.1);
            }
            else
            {

                move_distance = _distance;
            }
        
        }

        //get the move distance form main form
        public int setMoveDistance {
            get { return _distance; }
            set { _distance = value; }
        
        }

        void bk201_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
            while (playing) {


                changeingSpeed();
                movecomment = move_distance;


                System.Threading.Thread.Sleep(interval);
            }
        }
        public bool work {

            get { return playing; }
            set {
                if (value == true)
                {

                    playing = true;
                    if (!bk201.IsBusy) {

                        bk201.RunWorkerAsync();
                    
                    }
                }
                else {
                    playing = false;
                    bk201.CancelAsync();
                
                }
            
            }
        
        }
        public int setInterval {

            get { return interval; }
            set { interval = value; }
        }
        public int movecomment {

            get {return move; }
            set{
                //move comments here
                if(value != -1){
                    List<Label> remove = new List<Label>();
                    //int offset = comment_storage.Count / 5;
                    List<Label> temp_storage = new List<Label>();

                    for (int i = 0; i < comment_storage.Count; i++) {
                        temp_storage.Add(comment_storage.ElementAt(i));
                    }

                    try
                    {
                        for (int i = 0; i < temp_storage.Count; i++)
                        {

                            moveLabel2(temp_storage.ElementAt(i), value);
                            if (temp_storage.ElementAt(i).IsDisposed)
                            {
                                remove.Add(temp_storage.ElementAt(i));

                            }

                        }

                        for (int i = 0; i < remove.Count; i++)
                        {

                            comment_storage.Remove(remove.ElementAt(i));
                        }
                    }
                    catch (ArgumentOutOfRangeException) { }
                  //  remove.Clear();

            };
            }
        
        
        
        }
        delegate void moveLabel_thread(Label l, int distance);

        void moveLabel2(Label l, int distance) {
            try
            {
                if (l.InvokeRequired)
                {
                    moveLabel_thread c = new moveLabel_thread(moveLabel2);

                    this.Invoke(c, new object[] { l, distance });


                }
                else
                {

                    moveLabel(l, distance);
                }
            }
            catch (ObjectDisposedException) { }
        
        }

        void moveLabel(Label l,int distance) {
            int x = l.Location.X;
            int xend = 0 - l.Size.Width;
            if (x > xend)
            {
                //move the comment by distance if it's before xend
                l.Location = new Point(x - distance, l.Location.Y);
            }
            else {

                l.Dispose();
            
            }
        }
        bool fullscreen;
        public Color userColor{set;get;}
        int ycurrent;
        public int fontsize { set; get; }
       public float userOutlineWidth{set;get;}
       bool upper { get; set; }

        public String createLabel{
            // add the comment to the list and the controls
            // returns the newest comment ( latest set comment if read
            set
            {
                playedcomment++;
               // Label dm = new Label();
                Font_Outline dm = new Font_Outline();
                //

                dm.Text = value;
                dm.Name = value;
               // dm.TabIndex = 3;
               // dm.Visible = true;
                


                dm.Font = new Font("Microsoft Sans Serif", fontsize, FontStyle.Bold);
                
                dm.ForeColor = userColor;
                dm.OutlineForeColor = userOutlineColor;
                dm.OutlineWidth = userOutlineWidth;


                Size s = TextRenderer.MeasureText(dm.Text, dm.Font, new Size(value.Length*100 + 7, 1000),
       TextFormatFlags.VerticalCenter
       | TextFormatFlags.Left
       | TextFormatFlags.NoPadding
       | TextFormatFlags.WordBreak);

                // dm.MaximumSize= new Size(value.Length*100,0);
                dm.Size = new Size(s.Width+70,s.Height);
                

                //  dm.AutoSize = true;


                Random ypos = new Random();
                // to prevent two comments goes to close to each other


                if (!fullscreen)
                {
                    if (0 < this.Size.Height - (dm.Size.Height))
                    {
                        if (upper == false)
                        {
                            ycurrent = ypos.Next(0, this.Size.Height/2 - (dm.Size.Height));//fm3.ClientRectangle.Bottom
                            upper = true;
                        }
                        else {
                            ycurrent = ypos.Next(this.Size.Height/2, this.Size.Height - (dm.Size.Height));//fm3.ClientRectangle.Bottom
                            upper = false;
                        }
                    
                    }
                    else
                    {

                        ycurrent = 0;
                    }
                }
                else
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
                    //ycurrent = ypos.Next(this.ClientRectangle.Top, this.ClientRectangle.Bottom - dm.Size.Height - 80);

                }

                dm.Location = new Point(ClientRectangle.Right, ycurrent);

                this.comment_storage.Add(dm);
                this.Controls.Add(dm);
               
                dm.Show();
                 }
            get { return this.comment_storage.Last().Text; }
        
        
        }


        //gets the comment storage of this form 
        public List<Label> setStorage {
            set { this.comment_storage = value; }
            get { return this.comment_storage; }
        
        }
        public Point setLocation{

            set { this.Location = value; }

            get { return this.ClientRectangle.Location ;}
        
        
        
        }
        public bool setfullscreen{
            get { return fullscreen; }
            set { fullscreen = value; }
    
    }
        public Size setSize{

            set { this.Size = value; }

            get{ return this.Size;}
    
    
         }
        public bool setTopMost {

            set { this.TopMost = value; }

            get { return this.TopMost; }
        
        
        }




    }


}
