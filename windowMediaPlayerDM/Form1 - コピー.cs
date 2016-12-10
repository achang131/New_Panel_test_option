using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;
using System.Net.Http;
using System.Net;
using ShockwaveFlashObjects;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Windows.Interop;
using System.Diagnostics;

namespace windowMediaPlayerDM
{
    public partial class Form1 : Form{
        String media_dir;
         String danmoku_dir;
        OpenFileDialog media;
        OpenFileDialog danmoku;
        int ycurrent;
        int xstart;
        int time_counter;
        int speed_control;
        Color userColor;
        List<Label> comment_storage = new List<Label>();
        XmlTextReader dm_comment;
        List<String[]> comment = new List<String[]>();
      //  List<Label> remove_List = new List<Label>();
        int time_offset;
        int move_distance;
        int playedcomment;
        int counter_check;
        List<String[]> Media_List = new List<String[]>();
        List<String[]> DM_List = new List<String[]>();
        List<String[]> FullDM_List = new List<String[]>();
        FileInfo currentfile;
        int _duplicates;

        bool _first_load;
        WMPLib.IWMPPlaylist Media_Playlist;
        bool sommentswitch;

        Settings fm4;
        int commentdestroy;
        int auto_base;
        int vpos;

        int currentLanguage;

        int vpos_end;

        int vpos_video;

        bool threading_mode;

        int offset_auto;
        int panel_numbers;

        delegate void tmovelabel(Label l,int x, int y);

        delegate void exmovelabel(Label l);

        delegate void commentEnginecomp();

        delegate void setTrack_t(int time);

        delegate void setDouble(double d);

        delegate void setInt(int i);

        delegate void setString(String s);

        delegate void setUPCommentMoveEngine(comment_move_engine cme,List<Label> storage, int moved);

        Dictionary<int, String> comment2 = new Dictionary<int, String>();

        List<Neo_Comment_window> Comment_Windows;

        TaskFactory th1 = new TaskFactory();

        bool videoloop;

        Form2 fm2;

  //      public DispatcherTimer newtimer = new DispatcherTimer();

    //    public DispatcherTimer newTimer2 = new DispatcherTimer();

//        public System.Timers.Timer newTimer2 = new System.Timers.Timer();

      //  Form2 fm2;

        //



        double replacetimer1_interval;

        BackgroundWorker replacetimer1;

        bool _isPlaying;

        Neo_Comment_window fm3;

        bool multi_dm_mode;

        bool auto_TimeMatch;

        String audioinfo;

        BackgroundWorker replacetimer2;

        double replacetimer2_interval;

        BackgroundWorker replacetimer3;
        double replacetimer3_interval;

        double _timer1, _timer3;
        int lcount;

        int _distance;
        // Next try add setting (new window)  and Play/DM List(new window possible tabs ?)

        int choose_player; //1 for Windows media player 2, for vlc player


        int current_volume;

        bool mousedown;

        int vlcTIme;

        bool autoMlist;

        int vVolume;

        bool startautotime;

        int commentLimit;

        int commentmethod;
        String Program_location;
        DllImportAttribute dla;
        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        IntPtr nextClipboardViewer;
        [DllImport("kernel32.dll", SetLastError=true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        bool debug;           
        public Form1()
        {
            InitializeComponent();

            Program_location = Environment.GetCommandLineArgs()[0];

            this.Shown += new EventHandler(Form1_Shown);

            Vlc.DotNet.Core.VlcMediaPlayer vlc = new Vlc.DotNet.Core.VlcMediaPlayer(new DirectoryInfo("C:\\Users\\Alan\\Documents\\GitHub\\DM_Player\\lib\\x86"));
            

            media = new OpenFileDialog();
            danmoku = new OpenFileDialog();
            danmoku.Filter = "xml |*.xml";
            media.Filter = "";

          
            Media_Player.ClickEvent += new AxWMPLib._WMPOCXEvents_ClickEventHandler(Media_Player_ClickEvent);

            Media_Player.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Media_Player_PlayStateChange);


            this.ClientSizeChanged += new EventHandler(Form1_ClientSizeChanged);


            Comment_Windows = new List<Neo_Comment_window>();

            Media_Player.windowlessVideo = true;

            Media_Player.stretchToFit = true;

            audioinfo = "test";

            //maybe add a 3rd backgroundworker to move comment will make the programe even more smoother ?

            BackgroundworkerSetup();

            this.LocationChanged += new EventHandler(Form1_LocationChanged);

            if (choose_player == 1)
            {

                Media_Playlist = Media_Player.playlistCollection.newPlaylist("My_List");

                //replacetimer1.WorkerSupportsCancellation = true;

                Media_Player.settings.autoStart = true;

            }

            this.MouseWheel+=new MouseEventHandler(Form1_MouseWheel);

            vlcPlayer.MouseWheel+=new MouseEventHandler(Form1_MouseWheel);
            vlcPlayer.MouseClick += new MouseEventHandler(vlcPlayer_MouseClick);
            vlcPlayer.Playing += new EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs>(vlcPlayer_Playing);
            vlcPlayer.Stopped += new EventHandler<Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs>(vlcPlayer_Stopped);
            vlcPlayer.Paused += new EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs>(vlcPlayer_Paused);
            vlcPlayer.MediaChanged += new EventHandler<Vlc.DotNet.Core.VlcMediaPlayerMediaChangedEventArgs>(vlcPlayer_MediaChanged);
           // vlcPlayer.TimeChanged += new EventHandler<Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs>(vlcPlayer_TimeChanged);
            vlcPlayer.Opening += new EventHandler<Vlc.DotNet.Core.VlcMediaPlayerOpeningEventArgs>(vlcPlayer_Opening);
            
            
            
            vlcPlayer.AllowDrop = true;
            vlcPlayer.DragEnter += new DragEventHandler(f_DragEnter);
            
            

            VLC_track.MouseDown += new MouseEventHandler(VLC_track_MouseDown);
            VLC_track.MouseUp += new MouseEventHandler(VLC_track_MouseUp);
            VLC_track.ValueChanged += new EventHandler(VLC_track_ValueChanged);
            VLC_track.MouseMove += new MouseEventHandler(VLC_track_MouseMove);

            sound_trackbar.Maximum = 100;
            sound_trackbar.Minimum = 0;
            sound_trackbar.Value = vVolume;
            sound_trackbar.ValueChanged += new EventHandler(sound_trackbar_ValueChanged);


            this.KeyUp += new KeyEventHandler(Form1_KeyUp);

            vlcPlayer.KeyUp += new KeyEventHandler(vlcPlayer_KeyUp);
            vlcPlayer.MouseDoubleClick += new MouseEventHandler(vlcPlayer_MouseDoubleClick);
            this.MouseDoubleClick += new MouseEventHandler(Form1_MouseDoubleClick);
            statusStrip1.DragEnter += new DragEventHandler(f_DragEnter);

            fullscreen = false;
            if (Comment_Windows.Count>0) {
                for (int i = 0; i < Comment_Windows.Count; i++) {

                    Comment_Windows.ElementAt(i).setfullscreen = false;
                }
            
            }

            if (choose_player == 1)
            {

                vlcPlayer.Dispose();
            }
            else {

                Media_Player.Dispose();
            }

            

          //  this.Disposed += new EventHandler(Form1_Disposed);

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            //CommentEngineSetup(cme1, comment_storage3, move_distance);
            //CommentEngineSetup(cme2, comment_storage4, move_distance);

            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
            debug = true;
          cme1 = new comment_move_engine(move_distance);
          cme2 = new comment_move_engine(move_distance);

          this.DoubleBuffered = true;

          DragDropSetup(this);
          CSettings();
 
          if (debug) {
              AllocConsole();
          }
         // commentWindowSetup();

        }
  
        void Form1_Shown(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (fm7 == null && clipboardswitch == true)
            {

                if (current_dir_url == null || current_dir_url == "")
                {
                    var t = MessageBox.Show("No Default Folder is selected, Do you want to select it now ?", "Alert", MessageBoxButtons.YesNo);
                    if (t == DialogResult.Yes)
                    {

                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        fbd.RootFolder = Environment.SpecialFolder.MyComputer;

                        if (fbd.ShowDialog() == DialogResult.OK)
                        {

                            current_dir_url = fbd.SelectedPath;

                        }



                    }
                    else
                    {

                        clipboardswitch = false;

                    }

                }
                else
                {

                    URL_menuloapup_clipboard();
                    fm7.Hide();
                    fm7.hide = true;
                }

            }
            if (Environment.GetCommandLineArgs() != null)
            {
                String[] test = Environment.GetCommandLineArgs();
                if (test.Length > 1)
                {
                    //commentWindowSetup();
                    FileInfo f = new FileInfo(test[1]);
                    vlcSetMedia(f);
                   // clipboard_label.Text = test[1];
                    if (vlcPlayer.GetCurrentMedia() != null)
                    {
                        vlcPlayer.Play();
                    }
                }
            }
        }
        Neo_Comment_window fm3_1, fm3_2, fm3_3,fm3_4,fm3_5,fm3_6,fm3_7;
        comment_move_engine cme1;
        comment_move_engine cme2;
        int threadNumber;

        void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FullScreenSwitch();
        }

        void vlcPlayer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FullScreenSwitch();
        }

        void vlcPlayer_DragEnter(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException(;)

            
            //printvlc(e.Data.GetDataPresent(DataFormats.Locale).ToString());

        }

        int temp_sound;

        
        
        
        void sound_trackbar_ValueChanged(object sender, EventArgs e)
        {
            
            
                vVolume = sound_trackbar.Value;
                vlcPlayer.Audio.Volume = vVolume;
                printvlc("Volume: " + vVolume);
                if (vVolume == 0)
                {

                    vlcSound_button.BackgroundImage = Properties.Resources.mute;


                }
                else {

                    vlcSound_button.BackgroundImage = Properties.Resources.sound;
                }

        }


        bool fullscreen;
        Size playersize;
        Point playerlocation;
        Size commentPanelsize;
        Point commentPanelLocation;
        bool firstFullscreen = false;
        void FullScreenSwitch() {
            if (fullscreen)
            {
  
                show_all_fm1();
            }
            else
            {
                if (firstFullscreen == false)
                {
                    //to solve the weird first time fullscreen bug, just a temp fix
                    hide_All_fm1();
                    show_all_fm1();
                    firstFullscreen = true;
                }
                hide_All_fm1();
            }
        
        
        }
        protected override void WndProc(ref Message m)
        {



            if (clipboardswitch)
            {
                //base.WndProc(ref m);
                const int WM_DRAWCLIPBOARD = 0x308;
                const int WM_CHANGECBCHAIN = 0x030D;
                switch (m.Msg)
                {
                    case WM_DRAWCLIPBOARD:


                        showClipboard();

                        SendMessage(nextClipboardViewer, m.Msg, m.WParam,
                                     m.LParam);
                        break;

                    case WM_CHANGECBCHAIN:


                        if (m.WParam == nextClipboardViewer)
                            nextClipboardViewer = m.LParam;
                        else
                            SendMessage(nextClipboardViewer, m.Msg, m.WParam,
                                        m.LParam);
                        break;

                    default:
                        base.WndProc(ref m);
                        break;






                }
            }
        }
        String ClipBoardText = "";
        bool clipstartup = false;
        bool fm7_firsttime = true;
        bool clipboardswitch = true;
        
        
        void showClipboard() {

            String text="";
            try{
            IDataObject data = new DataObject();
            data = Clipboard.GetDataObject();
            
            if (data.GetDataPresent(DataFormats.Rtf)) {

                RichTextBox tb = new RichTextBox();
                //tb.Rtf = data.GetData(DataFormats.Text) as String;
                tb.Rtf = data.GetData(DataFormats.Rtf) as String;
                
                //  clipboard_label.Text = tb.Text;
                if (tb.Rtf != null)
                {
                    text = tb.Text;
                }
            
            } else if (data.GetDataPresent(DataFormats.Text)){
            
           // clipboard_label.Text = data.GetData(DataFormats.Text) as string;
            text = data.GetData(DataFormats.Text) as string;
            
            }else{
         //   clipboard_label.Text="clipbaord data is not rtf or ascii";
            }
            }catch(Exception e){
           // MessageBox.Show(e.ToString());
                Console.WriteLine(e.ToString());
            
            }
  //the startup is to prevent the player run the get link from start up 
            if (ClipBoardText != text &&clipstartup)
            {

               // if (!fm7_firsttime)
                //{

                    clipaddlink(text);

                
             //   }
             //   else {
             //       fm7_firsttime = false;
             //       cliploadfm7(text);
                 //   clipaddlink(text);
                
                
              //  }



            }
            if (!clipstartup)
            {

                clipstartup = true;
            }
            ClipBoardText = text;
        
        }
        void clipaddlink(string text) {

            if ( this.fm7 != null)
            {



                if (text.Contains("himado.in") || text.Contains("say-move.org"))
                {
                   /* 
                   if (fm7.hide == true)
                   {
                        fm7.hide = false;
                        fm7.Show();


                    }
                    */
                    ClipBoardText = text;
                    loadFromClipboard(ClipBoardText);
                  



                }



            }
        }
        void cliploadfm7(string text) {
            if (this.fm7 == null&& linkaddress==null)
            {

                if (text.Contains("himado.in") || text.Contains("say-move.org"))
                {
                    ClipBoardText = text;
                    linkaddress = new Uri(ClipBoardText);

                    URL_menuloapup_clipboard();



                }

            }
        }
        void KeyUpActions(KeyEventArgs e) {
           
            //Danmoku_status.Text = e.KeyValue.ToString();

            switch (e.KeyValue) { 
                    //esc key
                case 27:

                    if (fullscreen) {

                        show_all_fm1();
                    
                    }

                    break;
            
                    //F11 key
                case 122:
                    //hide if false and show if true
                    FullScreenSwitch();

                    break;
            
            
            }
        
        }
        void vlcPlayer_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpActions(e);
        }

        void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpActions(e);
        }

        void show_all_fm1() {
          //  if (Windowstate == FormWindowState.Maximized)
          //  {
               this.WindowState = FormWindowState.Maximized;
         //   }
         //   else {

         ////       this.Size = originalsize;
         //       this.Location = originalLocation;
            
            
         //   }

               if (fullscreenBottom == true)
               {
                   hidePlayMenu();
               }
               for (int i = 0; i < trackbars.Count; i++)
               {
                   trackbars.ElementAt(i).BringToFront();
               }
                   for (int i = 0; i < buttons.Count; i++) {
                       buttons.ElementAt(i).BringToFront();
                   }
              
                   statusStrip1.BringToFront();
              
             
            for (int i = 0; i < this.Controls.OfType<Button>().Count(); i++)
            {
                this.Controls.OfType<Button>().ElementAt(i).Show();
            }
            for (int i = 0; i < this.Controls.OfType<Label>().Count(); i++)
            {
                this.Controls.OfType<Label>().ElementAt(i).Show();

            }
            this.sound_trackbar.Show();
            this.menuStrip1.Show();

            this.statusStrip1.Show();
          //  this.statusStrip1.BringToFront();

            this.VLC_track.Show();
         //   this.VLC_track.BringToFront();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            fullscreen = false;
            if (Comment_Windows.Count > 0)
            {
                for (int i = 0; i < Comment_Windows.Count; i++)
                {

                    Comment_Windows.ElementAt(i).setfullscreen = false;
                }

            }

            vlcPlayer.Size = playersize;
            vlcPlayer.Location = playerlocation;

          //  this.TopMost = false;

            if (Comment_Windows.Count>0) {
       
               Comment_Windows.ElementAt(0).setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
                Comment_Windows.ElementAt(0).Size = new Size(Media_Player.ClientSize.Width, Media_Player.ClientSize.Height - 45);
             //   fm3.TopMost = false;

             //   ///////fm3.Owner = this;
            }

           // Cursor.Show();
            hideCurosr = false;

        }

        void hideCursor() {

            if (vlcPlayer.IsPlaying)
            {
                if (mousemoving)
                {

                    hideCurosr = false;
                }
                else
                {
                  //  Cursor.Hide();
                }
            }
            else
            {

                hideCurosr = false;
            }
        
        }


        FormWindowState Windowstate;
        Size originalsize;
        Point originalLocation;
        void hide_All_fm1() {

            fullscreen = true;

            originalsize = this.currentMediaWindowSize;
            originalLocation = this.currentMediaWindowLocation;

            this.WindowState = FormWindowState.Normal;

            playersize = vlcPlayer.Size;
            playerlocation = vlcPlayer.Location;

            if( Comment_Windows.Count>0)
            {
                commentPanelsize = Comment_Windows.ElementAt(0).Size;
                commentPanelLocation = Comment_Windows.ElementAt(0).Location;


            }
            //hide all buttons
            for (int i = 0; i < this.Controls.OfType<Button>().Count();i++ ){
                this.Controls.OfType<Button>().ElementAt(i).Hide();

            }
            for (int i = 0; i < this.Controls.OfType<Label>().Count(); i++) {
                this.Controls.OfType<Label>().ElementAt(i).Hide();
            
            }
            this.sound_trackbar.Hide();
            this.menuStrip1.Hide();

            this.statusStrip1.Hide();

            this.VLC_track.Hide();

                this.FormBorderStyle = FormBorderStyle.None;


              

                if (Comment_Windows.Count > 0)
                {
                    for (int i = 0; i < Comment_Windows.Count; i++)
                    {

                        Comment_Windows.ElementAt(i).setfullscreen = true;
                    }

                }


           //     if (this.ClientRectangle.Size.Height < 1028)
          //      {
             //       vlcPlayer.Size = this.ClientRectangle.Size;
             //       vlcPlayer.Location = new Point(this.ClientRectangle.Location.X, this.ClientRectangle.Location.Y);
          //      }
          //      else {

                   // vlcPlayer.Size = new Size(this.ClientRectangle.Width+8, this.ClientRectangle.Height+30);
//

                this.Size = new Size(1920, 1080);
                    vlcPlayer.Size = new Size(1920,1080);
         //           vlcPlayer.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width,Screen.PrimaryScreen.WorkingArea.Height);
        //           vlcPlayer.Location = new Point(this.ClientRectangle.Location.X+8, this.ClientRectangle.Location.Y+8);
                   vlcPlayer.Location = new Point(0, 0);
         //       }
           
               // vlcPlayer.Location = new Point(this.ClientRectangle.Location.X, this.ClientRectangle.Location.Y);




                if (Comment_Windows.Count>0) {
                    setCommentWS(Comment_Windows, new Size(1920, 1080), new Point(0, 0));

                }

               // this.MaximizedBounds = Screen.PrimaryScreen.Bounds;
                this.Bounds = Screen.PrimaryScreen.Bounds;
               // Cursor.Hide();
                hideCurosr = true;
               // 620 559 form.size.y-61
            
           //      statusStrip1.Location = new Point(0, this.Size.Height - statusStrip1.Height);
           //      statusStrip1.ImageScalingSize = new Size(this.Size.Width, 23) ;

            statusStrip1.Location = new Point(0, vlcPlayer.Size.Height - statusStrip1.Height);
            statusStrip1.ImageScalingSize = new Size(vlcPlayer.Size.Width, 23) ;

            vlcPlay_button.Location = new Point(32, statusStrip1.Location.Y - 19);

                vlcStop_button.Location = new Point(77, statusStrip1.Location.Y - 19);

                vlcSound_button.Location = new Point(ClientRectangle.Right - 182, statusStrip1.Location.Y - 19);

                loop_button.Location = new Point(ClientRectangle.Right - 218, statusStrip1.Location.Y - 19);

                fullscreen_button.Location = new Point(loop_button.Location.X - fullscreen_button.Size.Width - 5, loop_button.Location.Y);
                last_track.Location = new Point(145, statusStrip1.Location.Y - 19);
                next_track.Location = new Point(194, statusStrip1.Location.Y - 19);

                VLC_track.Size = new Size(this.Size.Width - 171, 45);
                VLC_track.Location = new Point(0, statusStrip1.Location.Y - 46);
                sound_trackbar.Location = new Point(VLC_track.Size.Width, statusStrip1.Location.Y - 46);
 
            }
        void setCommentWS(List<Neo_Comment_window>l,Size s, Point t) {

            for (int i = 0; i < l.Count; i++)
            {
                l.ElementAt(i).Size = s;
                l.ElementAt(i).Location = t;
            }


        }

            void Form1_FormClosing(object sender, FormClosingEventArgs e)
            {
                saveSettings();

            }

            //read the settings form the xml
            void form1_loadSetting_XML() {


                FileInfo config = new FileInfo(@Application.StartupPath + "\\settings.DMconfig");

                if(config.Exists){
            
                XmlTextReader reader =new XmlTextReader(config.FullName);

                try
                {
                    String currentElement = "";
                    while (reader.Read())
                    {

                        switch (reader.NodeType)
                        {

                            case XmlNodeType.Element:

                                currentElement = reader.Name;

                                break;

                            case XmlNodeType.Text:

                                switch(currentElement){
                            
                                    case "Default_Path":

                                        current_dir_url= reader.Value;


                                        break;

                                    case "Comment_Speed":
                                        _distance = Int32.Parse(reader.Value);

                                        break;

                                    case "Font_Size":
                                        fontsize = Int32.Parse(reader.Value);
                                        break;

                                    case "volume":

                                        vVolume = Int32.Parse(reader.Value);


                                        break;

                                    case "CommentLimit":

                                        commentLimit = Int32.Parse(reader.Value);

                                        break;
                                    case "Panel_number":
                                        panel_numbers = Int32.Parse(reader.Value);
                                        break;
                                    case "useClipboard":
                                        clipboardswitch = bool.Parse(reader.Value);

                                        break;
                                    case "debug_mode":

                                        debug = bool.Parse(reader.Value);
                                        break;
                            
                                }

                                break;

                            case XmlNodeType.EndElement:

                                currentElement = "";
                                break;


                        }




                    }
                    reader.Close();

                }
                catch (XmlException e) { Console.WriteLine(e.ToString()); }
        
                }
        
        
        
            }

            void saveSettings() {

                //throw new NotImplementedException();
                //save the settings on disposed
               // FileInfo settings = new FileInfo(Directory.GetCurrentDirectory() + "\\settings.DMconfig");
                FileInfo settings = new FileInfo(Application.StartupPath + "\\settings.DMconfig");
                if (settings.Exists)
                {
                    try
                    {
                        settings.Delete();
                    }
                    catch (IOException e) { Console.WriteLine(e.ToString()); }
                }

                using (XmlWriter writer = XmlWriter.Create(Application.StartupPath+"\\settings.DMconfig"))
                {

                    writer.WriteStartDocument();
                    writer.WriteStartElement("settings");
                    writer.WriteElementString("Default_Path", current_dir_url);
                    writer.WriteElementString("Comment_Speed", _distance.ToString());
                    writer.WriteElementString("volume", vVolume.ToString());
                    writer.WriteElementString("CommentLimit", commentLimit.ToString());
                    writer.WriteElementString("Panel_number", panel_numbers.ToString());
                    writer.WriteElementString("Font_Size", fontsize.ToString());
                    writer.WriteElementString("useClipboard", clipboardswitch.ToString());
                    writer.WriteElementString("debug_mode", debug.ToString());
                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                    writer.Close();

                
                }
            }

            //writes settings into xml file
            void Form1_Disposed(object sender, EventArgs e)
            {


            


            }
        
            void vlcPlayer_Opening(object sender, Vlc.DotNet.Core.VlcMediaPlayerOpeningEventArgs e)
            {

            //    if (currentfile != null)
            //    {
            //        setVLCname(currentfile);

           //     }
        }
        bool mousemoving;
        Point mouseLocation;
        void VLC_track_MouseMove(object sender, MouseEventArgs e)
        {

            mouseLocation = System.Windows.Forms.Cursor.Position;
                mousemoving = true;

            

        }

        void vlcPlayer_Paused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
            test_label.Text = vlcPlayer.State.ToString();




            timerStop();

        }



        void VLC_track_ValueChanged(object sender, EventArgs e)
        {

            changetime();

        }
        int vlc_track_time = -2;
        void changetime() {


           // if ( mousedown == true && VLC_track.Value != time_counter)
           if(mousedown==true)
            {
             //   timerStop();
             //   vlcPlayer.Time = VLC_track.Value * 10;

                if (vlcPlayer.IsPlaying)
                {
                    vlc_track_time = VLC_track.Value * 10;

                }
                else {

                    vlcPlayer.Time = VLC_track.Value * 10;
                    time_counter = VLC_track.Value;
                
                
                }
               // if (VLC_track.Value * 10 >= vlcPlayer.Length) {

                //    manual_checkend((int)vlcPlayer.Length,VLC_track.Value);
                
                    //diable this for now since already have timer to check

                    //it it doesn't work out maybe disable the one in timer will be better ?
                
             //   }


            }

        
        }

        void VLC_track_MouseUp(object sender, MouseEventArgs e)
        {

            changetime();

            mousedown = false;


        }

        void VLC_track_MouseDown(object sender, MouseEventArgs e)
        {
            mousedown = true;
        }
/*
        void vlcPlayer_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            vlc_display.Text = "end reached";
            vlcPlayer.Time = 0;
            time_counter = 0;
            timerStop();
            if (vlcPlayer.IsPlaying)
            {
                vlcPlayer.Stop();
            }

        }
        */
        void manual_checkend(int end, int current) {

            //set video loops here

                if (current >=
                    end && end !=0)
                {

                   // vlc_display.Text = vlc_display.Text+ "end reached";
                    if (videoloop == false)
                    {
                        //vlc_track_time = 0;
                        
                        time_counter = 0;
                        if (vlcPlayer.IsPlaying)
                        {
                            vlcPlayer.Stop();
                        }
                        timerStop();

                       

                    }
                    else
                    {
                        if (Media_List.Count > 1)
                        {
                            int currentinx = 0;
                            try
                            {
                              //  Uri nuri = new Uri(vlcPlayer.GetCurrentMedia().Mrl);
                                Uri nuri = new Uri(currentfile.FullName);
                                FileInfo nfile = new FileInfo(nuri.LocalPath);
                                String currentv = currentfile.Name;

                                for (int i = 0; i < Media_List.Count(); i++)
                            {

                                if (Media_List.ElementAt(i)[1].Equals(currentv))
                                {

                                    currentinx = i;

                                }

                            }

                            if (currentinx >= Media_List.Count - 1)
                            {

                                currentinx = 0;
                            }
                            else
                            {
                                currentinx++;
                            }//werid stopping problem happens here probably because it can't get media form current video in vlc ?
                            FileInfo nv = new FileInfo(Media_List.ElementAt(currentinx)[0]);
           

                            vlcSetMedia(nv);

                            vlcPlayer.Play();
                            timerStart();

                            this.Text = "DM Player " + nv.Name;
                            }
                            catch (ArgumentException e) { Console.WriteLine(e.ToString()); };

                        }
                        else
                        {
                            time_counter = 0;
                            vlc_track_time = 0;
                            playedcomment = 0;
                            CWplayedcomments(Comment_Windows, 0);


                        }


                    
                }
            }
        }

        void adjust_interval() {


            int ctime = (int)(vlcPlayer.Time) / 10;

            if (ctime == 0)
            {
                missed = 0;
                counter_check = -9999;

            }
            missed = (int)replacetimer2_interval;
            //instead of inputting the value directly change the speed to catch up so won't miss
            if (ctime + 5 < time_counter)
            {
                if (replacetimer2_interval > 10)
                {
                    replacetimer2_interval++;
                }
                else
                {
                    replacetimer2_interval = 11;
                };

            }
            else if (ctime == time_counter)
            {
                replacetimer2_interval = 10;

            }
            else if (ctime - 5 > time_counter && replacetimer2_interval > 1)
            {
                if (replacetimer2_interval < 10)
                {
                    replacetimer2_interval--;
                }
                else
                {
                    replacetimer2_interval = 9;
                }

            }
        }
        void vlcPlayer_TimeChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs e)
        {
         //   time_counter =(int) e.NewTime;
         //   print2("time: "+e.ToString()+" / "+e.NewTime.ToString());
          //  vlcTime = (int)e.NewTime;

            /*
            if (ctime + 10 < time_counter || ctime - 10 > time_counter)
            {
                time_counter = ((int)(e.NewTime) / 10);
            }
             * */



            


        }
        void setTime_track(int t) {

            if (InvokeRequired) {

                setInt st = new setInt(setTime_track);

                try
                {
                    this.Invoke(st, new object[] { t });
                }
                catch (Exception e){Console.WriteLine(e.ToString()); }
            }


            else {
             try{
                    VLC_track.Value = t;
                }
                catch (Exception e){Console.WriteLine(e.ToString()); }
            }
        
        
        
        }
        void vlcPlayer_MediaChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerMediaChangedEventArgs e)
        {
            timerStop();
            test_label.Text = vlcPlayer.State.ToString();
            printvlc("media changed");

            _first_load = false;

            playedcomment = 0;
            CWplayedcomments(Comment_Windows, 0);
            time_counter = 0;

            vlc_track_time = -2;

            VLC_track.Value = 0;



        
            DM_List.Clear();
            CWRemoveComments(Comment_Windows);
            removeAllComments();
            

            setVLCname(currentfile);
            autoLoadByName(currentfile);
            string[] tp = { currentfile.FullName, currentfile.Name };
            autoLoadDMlist(tp);
           
        }

        void vlcPlayer_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            test_label.Text = vlcPlayer.State.ToString();

            playedcomment = 0;
            CWplayedcomments(Comment_Windows, 0);
            CWRemoveComments(Comment_Windows);

            time_counter = 0;

            vlc_track_time = -2;

            VLC_track.Value = 0;

            timerStop();

       

           // removeAllComments();
        }

        void vlcPlayer_Playing(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            onLoadUp();
            if(_first_load==false){
                vlc_videoSetup();
            }
            print_test_label(vlcPlayer.State.ToString());
          

            timerStart();
            vlcPlayer.Audio.Volume = vVolume;
            
        }
        void print_test_label(string t) {
            try
            {
                test_label.Text = t;
            }
            catch (Exception e){Console.WriteLine(e.ToString());
                if (this.InvokeRequired)
                {
                    setString s = new setString(print_test_label);
                    this.Invoke(s, new object[] { t});

                }
                else {

                    test_label.Text = t;
                }
            
            }
        }
        void vlcPlayer_MouseClick(object sender, MouseEventArgs e)
        {
            Media_Player_ClickAction();
           
        



        }

        void CSettings() {



            multi_dm_mode = false;
            if (multi_dm_mode)
            {

                setDMsToolStripMenuItem.Text = setDMsToolStripMenuItem.Text + " Multi On";

            }
            else
            {

                setDMsToolStripMenuItem.Text = setDMsToolStripMenuItem.Text + "Multi Off";
            }

            playedcomment = 0;

            speed_control = 1;

            time_offset = 0;

            move_distance = 10;

            _distance = 10;

            timer1.Interval = 1;

            commentdestroy = -100;

            sommentswitch = true;

            threading_mode = false;

            vpos = -1;

            _first_load = false;

            auto_base = 0;

            userColor = Color.DarkGray;

            replacetimer1_interval = 59;
            replacetimer3_interval = 60;
            replacetimer2_interval = 10;


            fontsize = 20;

            cme1.setInterval = 64;
            cme2.setInterval = 63;

            threadNumber = 4;

            auto_TimeMatch = true;


            //-1400 for gp movie
            offset_auto = -500;



            _duplicates = 0;

            _timer1 = replacetimer1_interval;
            _timer3 = replacetimer3_interval;

            //   newtimer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            _isPlaying = true;

            //         newtimer.Tick += new EventHandler(newtimer_Tick);

            //    newTimer2.Tick += new EventHandler(newTimer2_Tick);

            //          newTimer2.Elapsed += new System.Timers.ElapsedEventHandler(newTimer2_Elapsed);

            //          newtimer.Interval= TimeSpan.FromMilliseconds(.1);

            //    newTimer2.Interval = TimeSpan.FromMilliseconds(.1);

            //         newTimer2.Interval = .1;



            //timer1.interval default at 100 milli second
            time_counter = 0;


            vpos_end = 0;
            vpos_video = 0;


            //initialize the y value to 40 so it will default start at 40
            ycurrent = 40;
            xstart = ClientRectangle.Right;


            //Media_Player.enableContextMenu = true;


            //


            choose_player = 2;

            switchPlayer(choose_player);

            current_volume = vlcPlayer.Audio.Volume;

            mousedown = false;

            vVolume = 50;


            //decide if it's going to load all the same media/xml file under the same directory into the playlist

            autoMlist = true;

            videoloop = true;


            //change this to auto start time match
            startautotime = false;

            commentLimit = 65;


            //set the comment method  0 = default 1 = using list to move labels
            commentmethod = 1;

            panel_numbers = 8;
            form1_loadSetting_XML();


       

            counter_check = -9999;

            int bt =this.Controls.OfType<Button>().Count();

            for (int i = 0; i < bt; i++) {
                buttons.Add(Controls.OfType<Button>().ElementAt(i));
            }
            int tb = this.Controls.OfType<TrackBar>().Count();

            for (int i = 0; i < tb; i++)
            {
                trackbars.Add(Controls.OfType<TrackBar>().ElementAt(i));
            }

            statusStrip1.BackColor = this.BackColor;


        }
        void switchPlayer(int c)
        {

            switch (c) { 
            
                case 1:
                    vlcPlayer.Hide();
                    VLC_track.Hide();

                    vlcPlay_button.Hide();
                    vlcSound_button.Hide();
                    vlcStop_button.Hide();


                    break;


                case 2:
                    Media_Player.Hide();

                    break;
            
            
            
            }
        
        
        }

        void BackgroundworkerSetup() {
            
            replacetimer1 = new BackgroundWorker();
            replacetimer1.DoWork += new DoWorkEventHandler(replacetimer1_DoWork);
            replacetimer1.WorkerSupportsCancellation = true;
            replacetimer2 = new BackgroundWorker();
            replacetimer2.DoWork += new DoWorkEventHandler(replacetimer2_DoWork);
            replacetimer2.WorkerSupportsCancellation = true;

         //   replacetimer3 = new BackgroundWorker();
         //   replacetimer3.DoWork += new DoWorkEventHandler(replacetimer3_DoWork);
         //   replacetimer3.WorkerSupportsCancellation = true;
        
        
        }
        int totalComments(List<Neo_Comment_window> l){
            int result = 0;

            for (int i = 0; i < l.Count-1; i++) {

                result += l.ElementAt(i).setStorage.Count;
            
            }

            return result;
    
    }
        void changingSpeedontime()
        {

            if (Comment_Windows.Count>0)
            {
                // int lnumber = fm3.setStorage.Count + fm3_1.setStorage.Count + fm3_2.setStorage.Count + fm3_3.setStorage.Count + fm3_4.setStorage.Count + fm3_5.setStorage.Count + fm3_6.setStorage.Count + fm3_7.setStorage.Count;
                int lnumber = totalComments(Comment_Windows);

                //  int lnumber = comment_storage.Count + comment_storage2.Count + cme1.setStorage.Count + cme2.setStorage.Count;
                if(lnumber >200){

                    move_distance = (int)(_distance * 6);

                }else if(lnumber > 120){

                    move_distance = (int)(_distance * 3.3);
                
                }else if(lnumber >90){

                    move_distance = (int)(_distance * 2.8);
                }else if(lnumber > 80) {

                    move_distance = (int)(_distance * 2.5);
                }
                else if (lnumber > 70)
                {
                    move_distance = (int)(_distance * 2.1);
                }
                else if (lnumber > 60)
                {
                    move_distance = (int)(_distance * 1.7);
                }
                else if (lnumber > 50)
                {
                    move_distance = (int)(_distance * 1.5);

                }
                else if (lnumber > 40)
                {

                    move_distance = (int)(_distance * 1.3);

                }
                else if (lnumber > 30)
                {

                    move_distance = (int)(_distance * 1.1);
                }
                else
                {

                    move_distance = _distance;
                }
                if (cme1 != null) {

                    cme1.setMoveDistance = move_distance;
                
                }
                if (cme2 != null) {

                    cme2.setMoveDistance = move_distance;
                }

            }

        }
        void replacetimer3_DoWork(object sender, DoWorkEventArgs e)
        {
            //while (!replacetimer3.CancellationPending)
            while(_isPlaying)
            {

                changingSpeedontime();

                //moveComment_thread2();

                //commentEngine_thread();
                fm3_2.movecomment = move_distance;
                fm3_3.movecomment = move_distance;
                fm3_4.movecomment = move_distance;
                fm3_5.movecomment = move_distance;

                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(replacetimer3_interval));

            }
        }

        void replacetimer2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!replacetimer2.CancellationPending) {

               // moveComment_thread();

                double temp = replacetimer2_interval;
                commentEngine_thread();

                changingSpeedontime();

                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(temp));
            
            }
        }
        void replacetimer3_start()
        {
            if (!replacetimer3.IsBusy)
            {
                replacetimer3.RunWorkerAsync();

            }

        }
        void replacetimer3_stop()
        {
            //only stops if they are running to avoid freeze ?

        //    if (replacetimer3.IsBusy)
        //    {
   
                    replacetimer3.CancelAsync();


          //  }
        }
        void replacetimer2_start() {
            if(!replacetimer2.IsBusy)
            replacetimer2.RunWorkerAsync();
        
        }
        void replacetimer2_stop() {
          //  if (replacetimer2.IsBusy)
          //  {

                    replacetimer2.CancelAsync();
  
           // }
        }

        void printvlc(String s) {
            this.vlc_display.Text = s;
        
        }
        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (vVolume < 0) {

                vVolume = 50;
            }

       //    Danmoku_status.Text=e.Delta.ToString();
            switch (e.Delta)
            {
                case 120:

                    if (choose_player == 1)
                    {
                        Media_Player.settings.volume++;
                    }
                    else {
                        if (vVolume < sound_trackbar.Maximum)
                        {
                            vVolume++;
                            sound_trackbar.Value = vVolume;
                            vlcPlayer.Audio.Volume = vVolume;
                            //vlcPlayer.Audio.Volume++;
                            printvlc("Volume: " + vlcPlayer.Audio.Volume);
                        }
                       // throw new Exception("get here");
                    }

                    break;


                case -120:
                    if (choose_player == 1)
                    {
                        Media_Player.settings.volume--;
                    }
                    else
                    {
                        if(vVolume!=0){
                        vVolume--;
                        };
                        sound_trackbar.Value = vVolume;
                        vlcPlayer.Audio.Volume = vVolume;
                        //vlcPlayer.Audio.Volume--;
                        printvlc("Volume: " + vlcPlayer.Audio.Volume);
                    }

                    break;

            }
        }
        void setCWLoaction(List<Neo_Comment_window> l, Point t) {

            for (int i = 0; i < l.Count; i++) {

                l.ElementAt(i).setLocation = t;
            }
        
        }
        void Form1_LocationChanged(object sender, EventArgs e)
        {
            if (Comment_Windows.Count>0)
            {

                setCWLoaction(Comment_Windows, new Point(this.Location.X + 8, this.Location.Y + 59));
              
                /*
                fm3.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
               // fm3.Size = new Size(Media_Player.ClientSize.Width, Media_Player.ClientSize.Height - 45);

                fm3_1.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
                fm3_2.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
                fm3_3.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
                fm3_4.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
                fm3_5.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
                fm3_6.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
                fm3_7.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
            
                 * 
                 */
            }
        }
        public Size currentMediaWindowSize {
            
           
            get {
                Size temp =new Size ( Media_Player.ClientSize.Width, Media_Player.ClientSize.Height-40 );
                
                
                return temp ;}
        
        
        
        }

        public  Point currentMediaWindowLocation {

            get { 
            
                Point temp = new Point(this.ClientRectangle.Left,this.ClientRectangle.Bottom);

                return temp;
            
            }
        
        }


        void replacetimer1_start() {

            if (replacetimer1.IsBusy != true) {

                replacetimer1.RunWorkerAsync();
            }
        
        
        }
        void replacetimer1_stop() {

    
                    replacetimer1.CancelAsync();

            
        
        }
        void CWmoveComment(List<Neo_Comment_window> l,int move) {

            for (int i = 0; i < l.Count-1; i++) {

                l.ElementAt(i).movecomment = move;
            }
        
        }
        private void replacetimer1_DoWork(object sender, DoWorkEventArgs e)
        {


           // while (!replacetimer1.CancellationPending)
            while (_isPlaying)
            {

                if (comment2.Count > 0)
                {
                    changingSpeedontime();


                    CWmoveComment(Comment_Windows, move_distance);



                }
              //  System.Threading.Thread.Sleep(replacetimer1_interval);
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(replacetimer1_interval));
            }
        
        
        }





        //load xml into a List and foreach addcomment ?
        void addComment(int time,String comment) { 


        //time counter dependent
            if (time == time_counter) {

                //createLabel(comment);
                makeCCompact(comment);
            
            }
        
        
        }
        void addComment(int currenttime,Dictionary<int,String> d) {

            if (currenttime!=vpos && d.ContainsKey(currenttime))
            {
                playedcomment++;
               // createLabel(d[currenttime]);
                makeCCompact(d[currenttime]);
                vpos = currenttime;
            }
        
        }

        void addComment(int currenttime, int time, String comment) {

            if (currenttime == time) {

                playedcomment++;
               // createLabel(comment);

                makeCCompact(comment);
                
            }
        }

        void comment_windowSLsetup(List<Neo_Comment_window> l) {

            for (int i = 0; i < l.Count; i++)
            {
                l.ElementAt(i).Size = new Size(Media_Player.ClientSize.Width, Media_Player.ClientSize.Height - 45);
                l.ElementAt(i).setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);

            }
        }

        void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            Media_Player.Size = new System.Drawing.Size(ClientRectangle.Width,ClientRectangle.Height-49);

            if (Comment_Windows.Count>0)
            {
                if (fullscreen == false)
                {
                    comment_windowSLsetup(Comment_Windows);
                    /*
                    fm3.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
                    fm3.Size = new Size(Media_Player.ClientSize.Width, Media_Player.ClientSize.Height - 45);
                    comment_windowSLsetup(fm3_1);
                    comment_windowSLsetup(fm3_2);
                    comment_windowSLsetup(fm3_3);
                    comment_windowSLsetup(fm3_4);
                    comment_windowSLsetup(fm3_5);
                    comment_windowSLsetup(fm3_6);
                    comment_windowSLsetup(fm3_7);
                    */
                }
            
            }

            if (fullscreen == false)
            {
                vlcPlayer.Size = new Size(ClientRectangle.Width, ClientRectangle.Height - 94);
                vlcPlayer.Location = new Point(0, ClientRectangle.Top + 29);

                statusStrip1.Location = new Point(0, this.Size.Height - 61);
                statusStrip1.ImageScalingSize = new Size(this.Size.Width, 23);
            }
            else {
             //   statusStrip1.Location = new Point(0, 1500 - statusStrip1.Height);
             //   statusStrip1.ImageScalingSize = new Size(this.Size.Width, 23);
            }

     
            
            vlcPlay_button.Location = new Point(32, statusStrip1.Location.Y - 19);

            vlcStop_button.Location = new Point(77, statusStrip1.Location.Y - 19);

            vlcSound_button.Location = new Point(ClientRectangle.Right - 182, statusStrip1.Location.Y - 19);
        
            loop_button.Location = new Point(ClientRectangle.Right - 218, statusStrip1.Location.Y - 19);
            fullscreen_button.Location = new Point(loop_button.Location.X - fullscreen_button.Size.Width - 5, loop_button.Location.Y);
            last_track.Location = new Point(145, statusStrip1.Location.Y - 19);
            next_track.Location = new Point(194, statusStrip1.Location.Y - 19);

            VLC_track.Size = new Size(this.Size.Width-171, 45);
            VLC_track.Location = new Point(0,statusStrip1.Location.Y-46);
            sound_trackbar.Location = new Point(VLC_track.Size.Width, statusStrip1.Location.Y-46);
            
        }
        
        // form1 ends
        void autoTimesetup()
        {
   
                if (choose_player == 1)
                {
                    var tconsol = ((WMPLib.IWMPControls3)Media_Player.Ctlcontrols);

                    currentLanguage = tconsol.currentAudioLanguageIndex;

                    lcount = tconsol.audioLanguageCount;



                    vpos_video = (int)(Media_Player.Ctlcontrols.currentItem.duration * 100);

                }
                else {


                    vlc_videoSetup();
                 //   vlcPlayer.GetCurrentMedia().StateChanged += new EventHandler<Vlc.DotNet.Core.VlcMediaStateChangedEventArgs>(Form1_StateChanged);
         
                
                }


                cautoSet();

              //  vpos_video = (int)(tconsol.currentItem.duration * 100);
                

                print3("Video Length: " + vpos_video + "/" + "Comment time: " + vpos_end + "  CL: " + currentLanguage + " total: " + lcount);

            

              
    
        }


       
        void cautoSet() {


            if (vpos_end > vpos_video * 1.3 || startautotime==false)
            {

                auto_TimeMatch = false;

            }
            else
            {

                auto_TimeMatch = true;
            }


            if (auto_TimeMatch==true && vpos_video > 0)
            {
                auto_base = vpos_end - vpos_video;

            }


        }
        void vlc_videoSetup()
        {




            vpos_video = (int)vlcPlayer.Length/ 10;

          //  vpos_video = (int)vlcPlayer.Length / 10;

            currentLanguage = -1;



            for (int i = 0; i < vlcPlayer.Audio.Tracks.Count; i++)
            {
                try
                {
                    if (vlcPlayer.Audio.Tracks.All.ElementAt(i).ID.Equals(vlcPlayer.Audio.Tracks.Current.ID))


                        currentLanguage = i;
                }
                catch (ArgumentOutOfRangeException e) { Console.WriteLine(e.ToString()); };

            }


            lcount = vlcPlayer.Audio.Tracks.Count - 1;

            //  vlcPlayer.Audio.Tracks.Current = vlcPlayer.Audio.Tracks.All.ElementAt(2);

            //VLC_track.Maximum = vpos_video;

            setTrackMax(vpos_video);
            VLC_track.Minimum = 0;

           // vlcPlayer.Audio.Volume = 50;



        }
        int CWplayedcomments(List<Neo_Comment_window> l)
        {
            int result = 0;
            for (int i = 0; i < l.Count; i++)
            {
               result += l.ElementAt(i).getplayedComment;
            }
            return result;
        }
        void CWplayedcomments(List<Neo_Comment_window> l,int value) {

            for (int i = 0; i < l.Count; i++) {
                l.ElementAt(i).getplayedComment = value;
            }
        }
        void CWRemoveComments(List<Neo_Comment_window> l) {

            for (int i = 0; i < l.Count; i++) {
                List<Label> list = l.ElementAt(i).setStorage;
                for (int j = 0; j < list.Count; j++)
                {
                    list.ElementAt(j).Text = "";
                    list.ElementAt(j).Dispose();
                }
                l.ElementAt(i).setStorage.Clear();
            }
            
        
        }


        void Form1_StateChanged(object sender, Vlc.DotNet.Core.VlcMediaStateChangedEventArgs e)
        {
          /*
          switch(e.State.ToString()){
          
              case "Stopped":
                  time_counter = 0;
                  playedcomment = 0;

                  CWplayedcomments(Comment_Windows, 0);
                  
                  resetComment();

                  timerStop();


                  break;
              case "Playing":

                  timerStart();

                  break;
              case "Paused":

                  timerStop();

                  break;



              default:

                  timerStop();

                  break;
                
          
          }
        */
        }


        void setTrackMax(int time)
        {

            if (this.InvokeRequired)
            {

                setTrack_t temp = new setTrack_t(setTrackMax);


                this.Invoke(temp, new object[] { time });



            }
            else
            {

                VLC_track.Maximum = time-10;
                VLC_track.Minimum = 0;
                VLC_track.TickFrequency = 1;
            }


        }

        void fmStart(Neo_Comment_window f) {


            f.work = true;
        }
            void timerStart(){
          
                _isPlaying = true;
                replacetimer1_start();

                replacetimer2_start();
            
              //  replacetimer3_start();

                /*
                if (cme1 != null && cme2 != null)
                {
                    cme1.Start();
                    cme2.Start();
                }
      
                /*
                replacetimer2_start();
                if (fm3 != null)
                {
                    fmStart(fm3);
                    fmStart(fm3_1);
                    fmStart(fm3_2);
                    fmStart(fm3_3);
                    fmStart(fm3_4);
                    fmStart(fm3_5);
                    fmStart(fm3_6);
                    fmStart(fm3_7);
                }
                 */
            
            
        }
            void fmStop(Neo_Comment_window f) {

                f.work = false;
            }
        void timerStop(){
        
             
            _isPlaying = false;
                
            replacetimer1_stop();

            
            replacetimer2_stop();

//            replacetimer3_stop();
            /*
            if (cme1 != null)
            {
                cme1.Stop();
            }
            if (cme2 != null)
            {
                cme2.Stop();
            }
                   * */
            /*
            replacetimer2_stop();
            if (fm3 != null)
            {
                fmStop(fm3);
                fmStop(fm3_1);
                fmStop(fm3_2);
                fmStop(fm3_3);
                fmStop(fm3_4);
                fmStop(fm3_5);
                fmStop(fm3_6);
                fmStop(fm3_7);
            }
             */

        }
        void srCommentWindow() {

            for (int i = 0; i < Comment_Windows.Count; i++) {

                Comment_Windows.ElementAt(i).setDictionary.Clear();
            
            }
        
        }
        void srCommentWindow(Dictionary<int,String> dic)
        {
            
            for (int i = 0; i < Comment_Windows.Count-1; i++)
            {

                Comment_Windows.ElementAt(i).setDictionary=dic;

            }

        }
        
        void removeAllComments()
        {


            comment.Clear();
            comment2.Clear();
            srCommentWindow();
           
            _duplicates = 0;


        
        }
        void Media_Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            test_label.Text = Media_Player.playState.ToString();

            onLoadUp();

            switch(Media_Player.playState.ToString()){
            
            
                case "wmppsPlaying":

  //                  onLoadUp();

                    timerStart();
                    break;
                case "wmppsStopped":

                  
                    playedcomment = 0;
                    CWplayedcomments(Comment_Windows, 0);
                    CWRemoveComments(Comment_Windows);
                    time_counter = 0;

  //                  onLoadUp();

                    timerStop();
                
  //                  resetComment();

                  //  removeAllComments();
                    break;
                
                case "wmppsReady":

   //                 onLoadUp();

                    break;

                default:

                    
                    timerStop();

                    break;
            
            }
          
        }
        String makeCCompactComponet(String comment) {
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

        void makeCCompact(String comment) {


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
                        createLabel(comments[i]);



                    }



                }
                else {

                    comment = makeCCompactComponet(comment);
                    createLabel(comment);
                
                }




            }
            else {


                createLabel(comment);
            
            }
            




        }

        bool core0 = false;
        bool core1 = false;
        bool core2 = false;
        bool core3 = false;


        void createLabel(String comment) {
            //instead of filtering out the comment make it more compact on the screen ?

          //  comment = comment.Insert(commentLimit, Environment.NewLine);


        //    if (comment.Length < commentLimit || comment.Contains(Environment.NewLine))
        //    {



             //   Label dm = new Label();

                //
            /*
                dm.Text = comment;
                dm.Name = comment;
                dm.TabIndex = 3;
                dm.Visible = true;
                dm.AutoSize = true;

                dm.Font = new Font("Microsoft Sans Serif", 24, FontStyle.Bold);
                dm.ForeColor = userColor;
                dm.MouseClick += new MouseEventHandler(dm_MouseClick);


                Random ypos = new Random();

                if (!fullscreen)
                {
                    if (40 < fm3.Size.Height - (80+dm.Size.Height))
                    {
                        ycurrent = ypos.Next(40, fm3.Size.Height - (80+dm.Size.Height));//fm3.ClientRectangle.Bottom
                    }
                    else
                    {

                        ycurrent = 40;
                    }
                }
                else {


                    ycurrent = ypos.Next(fm3.ClientRectangle.Top, fm3.ClientRectangle.Bottom - dm.Size.Height-80);
                
                }
                dm.Location = new Point(ClientRectangle.Right, ycurrent);

            
            
                if (commentmethod == 1)
                {

                    //basically splits the comments into two lists



                    switch (playedcomment % threadNumber) { 
                    
                        case 1:
                            if (core3 == false)
                            {
                                fm3_1.createLabel = comment;
                            //    comment_storage2.Add(dm);
                            //    fm3_1.Controls.Add(dm);
                                core3 = !core3;
                            }
                            else
                            {
                            //    comment_storage2.Add(dm);
                            //    fm3_7.Controls.Add(dm);
                                fm3_7.createLabel = comment;
                                core3 = !core3;
                            }

                            break;
                        case 2:
                            if (cme1 != null)
                            {
                                if (core2 == false)
                                {
                                    fm3_2.createLabel = comment;
                                 //   cme1.setStorage.Add(dm);
                                //    fm3_2.Controls.Add(dm);
                                    core2 = !core2;
                                }
                                else
                                {
                                    fm3_6.createLabel = comment;
                                   // cme1.setStorage.Add(dm);
                                   // fm3_6.Controls.Add(dm);
                                    core2 = !core2;
                                }
                            }
                            break;
                        case 3:
                            if (cme2 != null)
                            {
                                if (core1 == false)
                                {
                                    fm3_3.createLabel = comment;
                                 //   cme2.setStorage.Add(dm);
                                  //  fm3_3.Controls.Add(dm);
                                    core1 = !core1;
                                }
                                else
                                {
                                    fm3_5.createLabel = comment;
                                    //cme2.setStorage.Add(dm);
                                    //fm3_5.Controls.Add(dm);
                                    core1 = !core1;
                                }
                                break;
                            }
                            break;
                    
                        default:
                            if (core0 == false)
                            {
                                fm3.createLabel = comment;
                                //comment_storage.Add(dm);
                                //fm3.Controls.Add(dm);
                                core0 = !core0;
                            }
                            else {
                                fm3_4.createLabel = comment;
                                //comment_storage.Add(dm);
                                //fm3_4.Controls.Add(dm);
                                core0 = !core0;
                            }
                            break;
                    
                    }




                }
                dm.BringToFront();
                dm.Show();
             * 
              
         //   }
             */
            if (Comment_Windows.Count > 0) {

                if (seleter < Comment_Windows.Count - 1)
                {
                    Comment_Windows.ElementAt(seleter).createLabel = comment;

                    seleter++;
                }
                else {
                    Comment_Windows.ElementAt(seleter).createLabel = comment;

                    seleter = 0;
                
                }

            }
        
        }
        int seleter = 0;
        void safecontrol(Label dm) {

            if (this.InvokeRequired)
            {


            }
            else {

                fm3.Controls.Add(dm);
            }
        
        
        }

       public void Media_Player_ClickAction() {

           if (choose_player == 1)
           {
               test_label.Text = Media_Player.playState.ToString();


               switch (Media_Player.playState.ToString())
               {

                   case "wmppsStopped":
                       Media_Player.Ctlcontrols.play();



                       timerStart();

                       break;

                   case "wmppsPaused":
                       Media_Player.Ctlcontrols.play();

                       timerStart();

                       break;

                   case "wmppsPlaying":
                       Media_Player.Ctlcontrols.pause();

                       timerStop();
                       break;
                   case "wmppsReady":
                       //          onLoadUp();

                       Media_Player.Ctlcontrols.play();

                       timerStart();


                       break;

               }
           }
           else {

               test_label.Text = vlcPlayer.State.ToString();

               switch (vlcPlayer.State.ToString())
               {

                   case "Stopped":

                       test_label.Text = vlcPlayer.State.ToString();
                       vlcPlay_button.BackgroundImage = Properties.Resources.pause;
                       vlcPlayer.Play();
                       if (fullscreen && hideCurosr == false && activeForms == 0)
                       {
                           hideCurosr = true;
                       }
                     

                       timerStart();

                       break;

                   case "Paused":
                       

                       vlcPlay_button.BackgroundImage = Properties.Resources.pause;
                       vlcPlayer.Play();

                       test_label.Text = vlcPlayer.State.ToString();

                      timerStart();
                      if (fullscreen && hideCurosr == false && activeForms==0) {
                          hideCurosr = true;
                      }

                       break;

                   case "Playing":
                      

                       vlcPlay_button.BackgroundImage = Properties.Resources.start;
                       vlcPlayer.Pause();

                       test_label.Text = vlcPlayer.State.ToString();

                       timerStop();
                       if (fullscreen&&hideCurosr) {
                           hideCurosr = false;
                       
                       }
                       break;

                   default:
                      // if (vlcPlayer.GetCurrentMedia() != null) {
                       if(currentfile !=null){
                           vlcPlay_button.BackgroundImage = Properties.Resources.pause;
                           vlcPlayer.Play();
                           vlcPlayer.Audio.Volume = 50;
                           test_label.Text = vlcPlayer.State.ToString();

                           timerStart();
                       
                       }


                       break;


               }
           
           
           }
        
        }
        void dm_MouseClick(object sender, MouseEventArgs e)
        {

            //check whick button is pressed on label , if right then bring it to front else stop/continue video
            switch(e.Button.ToString()){
          
              
            
            case "Right":
                    //turn off for now, since it's now multilayer and need anothing method to work

                    /*
                        Label temp = (Label)sender;
                        temp.BringToFront();
                     */
    
            break;

            default:

            Media_Player_ClickAction();
            
            break;

        }

            
                        
        }
        void resetLabelPos(Label l) {
            l.Location = new Point(ClientRectangle.Right, l.Location.Y);
        
        }
        

        void MoveLabel(Label l) {
            
            // where the DM start to hit
            int xstart = ClientRectangle.Right;
           // int xend = commentdestroy;

            int xend = 0 - l.Size.Width;
            //where the DM ends


            //adjest height position range from 0 to height using random to generate ?
            // not needed for now will use this when creating new TransparentLabels for DM
            // int ydown = this.Height;
            // int yup = 0;

            int initialx = l.Location.X;
            if (initialx > xend)
            {
                l.Location = new Point(initialx - move_distance, l.Location.Y);
              
            }
            else {
                //need to de comment this when the xml actually loads since it's going to add the comments on depending on the time counter real time


              //  comment_storage.Remove(l);
             //   remove_List.Add(l);
                //remove_List.AddLast(l);

          
                l.Dispose();
               
            
            }
        
        }

        void MoveLabel_thread(Label l) {

            // where the DM start to hit
            int xstart = ClientRectangle.Right;
            int xend = commentdestroy;
            //where the DM ends


            //adjest height position range from 0 to height using random to generate ?
            // not needed for now will use this when creating new TransparentLabels for DM
            // int ydown = this.Height;
            // int yup = 0;

            int initialx = l.Location.X;
            if (initialx > xend)
            {
             //   l.Location = new Point(initialx - move_distance, l.Location.Y);
                LabelLocation_thread(l, initialx - move_distance, l.Location.Y);
            }
            else
            {
                //need to de comment this when the xml actually loads since it's going to add the comments on depending on the time counter real time


                //  comment_storage.Remove(l);
                
                //remove_List.AddLast(l);
                l.Dispose();


            }
        
        
        }
        void MoveLabel_threadEX(Label l){

            if (l.InvokeRequired) {
                exmovelabel n = new exmovelabel(MoveLabel_threadEX);
                this.Invoke(n, new object[] { l });
            
            } else {

                // where the DM start to hit
                int xstart = ClientRectangle.Right;

             //   int xend = commentdestroy;

                int xend = 0 - l.Size.Width;
                //where the DM ends


                //adjest height position range from 0 to height using random to generate ?
                // not needed for now will use this when creating new TransparentLabels for DM
                // int ydown = this.Height;
                // int yup = 0;

                int initialx = l.Location.X;
                
                if (initialx > xend)
                {
                    l.Location = new Point(initialx - move_distance, l.Location.Y);
                }
                else
                {
                    //need to de comment this when the xml actually loads since it's going to add the comments on depending on the time counter real time


                    //  comment_storage.Remove(l);
                   // remove_List.AddLast(l);

                    l.Dispose();


                }
            
            }
        
        
        }
        void LabelLocation_thread(Label l,int x,int y) {

            if (l.InvokeRequired)
            {
                tmovelabel n = new tmovelabel(LabelLocation_thread);

                this.Invoke(n, new object[] { l,x,y });
            }
            else {
                l.Location = new Point(x, y);
            
            }
        
        }
        void resetComment() {
            foreach (Label l in fm3.Controls.OfType<Label>()) {

                resetLabelPos(l);
            }
        
        
        }
        void resetComment(List<Label> l) {

            foreach (Label comment in l) {
                resetLabelPos(comment);
            
            }
        
        }
         void Media_Player_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            Media_Player_ClickAction();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            replacetimer1.Dispose();
            this.Close();

            

        }
        private void MediaPlayerClick(object sender, MouseEventArgs e) {
            test_label.Text = Media_Player.playState.ToString();
        
        }
        void setMedia_Multi(List<String[]> medias) {
            switch(choose_player){
                case 1: 
                    
                    
                    for (int i = 0; i < medias.Count(); i++)
            {
                Media_Playlist.appendItem(Media_Player.newMedia(medias.ElementAt(i)[0]));
                if(!Media_List.Contains(medias.ElementAt(i))){
                Media_List.Add(medias.ElementAt(i));
                }

            }
            media_dir = medias.ElementAt(0)[0];
            Media_status.Text = "Playlist Set";
            Media_Player.currentPlaylist = Media_Playlist;

             Media_Player.Ctlcontrols.stop();


                break;

                case 2:
                    for (int i = 0; i < medias.Count(); i++)
            {
                
                        if(!Media_List.Contains(medias.ElementAt(i))){
                Media_List.Add(medias.ElementAt(i));
                }
            }
            media_dir = medias.ElementAt(0)[0];
            Media_status.Text = "Playlist Set";

            FileInfo temp = new FileInfo(medias.ElementAt(0)[0]);

            vlcSetMedia(temp);

            vlcPlayer.Play();
            timerStart();
            //onLoadUp();
            // need to manulally create a method to play the list of files in media_linklist after the first loaded video done playing
            
            break;
                
            
            }

            
        }

        void autoLoadMlist(String[] mediadirs) {

            Media_List.Clear();

        int end = mediadirs[0].LastIndexOf("\\");
        String temp_dir = mediadirs[0].Substring(0, end);

        DirectoryInfo fdir = new DirectoryInfo(temp_dir);

          //if the file doesn't contain the . extension (meaning it's most likely dled by this player)
        // or if the current file dir is the same as the dl file dir
            //loads all files dispite file extension type
        if (!mediadirs[1].Contains(".")||mediadirs[1].Contains(".mp")||fdir.FullName==current_dir_url)
        {
            FileInfo[] file = fdir.GetFiles();
            for (int i = 0; i < file.Count(); i++)
            {

                String[] ml = { file[i].FullName, file[i].Name };

                //only load if it's not xml file do this to avoid reading xml in wrong place
                if(!file[i].Name.Contains(".xml") && !file[i].Name.Contains(".ass")){
                    if (!Media_List.Contains(ml))
                    {
                        Media_List.Add(ml);
                    }
                }



            }
            if (fm2 != null)
            {
                fm2.setMediaList = Media_List;

            }

        }
        else
        {

            end = mediadirs[1].LastIndexOf(".");

            String extension = mediadirs[1].Substring(end, mediadirs[1].Length - end);

            FileInfo[] file = fdir.GetFiles("*" + extension);

            for (int i = 0; i < file.Count(); i++)
            {

                String[] ml = { file[i].FullName, file[i].Name };
                if (!Media_List.Contains(ml))
                {
                    Media_List.Add(ml);
                }

         

            }
            if (fm2 != null)
            {
                fm2.setMediaList = Media_List;

            }
        }        
        
        }

        //loaded after setmedia(single or multi all applied) && loaded after set dm
        //for this list instead of clearing everything maybe just checking duplicates will be fine ?
       
        void autoLoadDMlist(String[] dmdirs) {

            int end = dmdirs[0].LastIndexOf("\\");
            String temp_dir = dmdirs[0].Substring(0, end);

            DirectoryInfo ddir = new DirectoryInfo(temp_dir);

            //no need to check for extensions here since all are reading .xml. . . for now

            String extension = ".xml";

            FileInfo[] file = ddir.GetFiles("*" + extension);

            //now all the files under the dir with .xml are now in files array

            for (int i = 0; i < file.Count(); i++) {

                String[] results = { file[i].FullName, file[i].Name };
                bool exists=false;

                for (int l = 0; l < FullDM_List.Count(); l++) {
                  //  if (FullDM_List.Count > 0)
                 //   {
                        if (FullDM_List.ElementAt(l)[0].Equals(results[0]))
                        {
                            exists = true;

                        }
                //    }
                
                }


                    if (!exists)
                    {
                        FullDM_List.Add(results);

                        //if the list menu windows is left opened when video/comment is loaded through dropdown menu maynot be need ? 
                        if (fm2 != null)
                        {
                            fm2.setFullDMBox.Items.Add(results[1]);
                        }

                    }
            
            
            }

        
        
        
        
        }

        void setMedia_Single(List<String[]> medias)
        {

            if (medias != null)
            {
                media_dir = medias.ElementAt(0)[0];
                Media_status.Text = "Media Set";
                if(!Media_List.Contains(medias.ElementAt(0))){
                Media_List.Add(medias.ElementAt(0));
                }



                if (autoMlist == true) {

                    autoLoadMlist(medias.ElementAt(0));
                
                }

            }
            else
            {
                Media_status.Text = "No Media";
            }
            //   Media_Player.settings.autoStart=false;

            switch(choose_player){
                case 1:
                Media_Player.URL = media_dir;
            Media_Player.Ctlcontrols.stop();

            break;
            case 2:
            if (media_dir != null)
            {
                FileInfo temp = new FileInfo(media_dir);

                vlcSetMedia(temp);
                vlcPlayer.Play();
                timerStart();
            }
           // onLoadUp();
            break;
        }




        }


        void PlayMedia(List<String[]> medias) {

            bool multi_media;
            if (medias != null && medias.Count > 1)
            {
                //multiple media file is selected;
                multi_media = true;


            }
            else
            {

                multi_media = false;
            }

            if (multi_media)
            {

                setMedia_Multi(medias);

            }
            else
            {

                setMedia_Single(medias);


            }
            
           
            _first_load = false;
        
        }

        void setMedia() {




            List<String[]> medias = setFile(media);

            if (medias != null)
            {
                if (vlcPlayer.IsPlaying) {
                    vlcPlayer.Stop();
                    comment2.Clear();
                    
                    srCommentWindow();
                    
                    comment.Clear();
                    _duplicates = 0;
                
                }
                PlayMedia(medias);
                autoLoadDMlist(medias.ElementAt(0));
            }

            
        
        
        }
        private void setDMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setMedia();


            this.Focus();
            if (Comment_Windows.Count==0) {
                commentWindowSetup();
            
            }
        }
        void print3(String s) {
            try
            {
                Video_comment.Text = s;
            }
            catch (Exception e){Console.WriteLine(e.ToString());
                if (this.InvokeRequired)
                {
                    setString sa = new setString(print3);
                    this.Invoke(sa, new object[] { s });

                }
                else {
                    Video_comment.Text = s;
                }
            
            
            }
        }
        private void readXML(String danmoku_dir)
        {
            String[] temp_comment = new String[2];

            using (dm_comment = new XmlTextReader(danmoku_dir))
            {

                while (dm_comment.Read())
                {

                    switch (dm_comment.NodeType)
                    {

                        case XmlNodeType.Element:



                            while (dm_comment.MoveToNextAttribute())
                            {

                                //get the time value if the attribute is vpos
                                if (dm_comment.Name == "vpos")
                                {
                                    temp_comment[0] = dm_comment.Value;

                                }

                            }
                            break;

                        case XmlNodeType.Text:
                            temp_comment[1] = dm_comment.Value;
                            comment.Add(temp_comment);
                            int vpost = Int32.Parse(temp_comment[0]);

                            //this will ends with the final vpos in the xml files
                            if (vpost > vpos_end)
                            {

                                vpos_end = vpost;
                            }


                            if (comment2.ContainsKey(vpost))
                            {

                                String tempc = comment2[vpost] + Environment.NewLine + temp_comment[1];
                                _duplicates++;
                                comment2.Remove(vpost);
                                comment2.Add(vpost, tempc);


                            }
                            else
                            {
                                comment2.Add(vpost, temp_comment[1]);
                            }
                            //temp_comment = new String[2];


                            break;

                        case XmlNodeType.EndElement:
                            if (dm_comment.Name == "</chat>")
                            {

                                temp_comment = new String[2];

                            }

                            break;



                    }
                }
                srCommentWindow(comment2);

                //every time xml loads


                _first_load = false;
                //          onLoadUp();
                dm_comment.Close();
                
            }
        }

        void setDM_Multi() { 
        
        
        
        
}

        void setDM_Menu() {

            if (multi_dm_mode == false)
            {
                comment.Clear();
                comment2.Clear();

                srCommentWindow();

                _duplicates = 0;

                //clears all the dm that is current loaded
                DM_List.Clear();

            }
            /*
            if (Comment_Windows.Count>0)
            {
                this.Owner = Comment_Windows.ElementAt(0);
                    this.Owner = fm3;
 
            }
             * */


            List<String[]> danmokus = setFile(danmoku);

            if (danmokus != null)
            {
                if (danmokus.Count > 1)
                {

                    multi_dm_mode = true;
                    setDMsToolStripMenuItem.Text = "Set DMs" + " Multi On ";
                }





                if (danmokus.Count == 1)
                {
                    danmoku_dir = danmokus.ElementAt(0)[0];
                    Danmoku_status.Text = "DM Set";
                    if (!DM_List.Contains(danmokus.ElementAt(0)))
                    {
                        DM_List.Add(danmokus.ElementAt(0));

                    }
                    //reader.name is the name of the element/attribute
                    //reader.value is the value of the attribute/text

                    readXML(danmoku_dir);

                    danmoku_dir = null;
                    Danmoku_status.Text = "DM Ready";

                }
                else
                {

                    for (int i = 0; i < danmokus.Count(); i++)
                    {


                        //add only if it's not in the list to prevent dulplicate from being loaded
                        if (!DM_List.Contains(danmokus.ElementAt(i)))
                        {
                            danmoku_dir = danmokus.ElementAt(i)[0];
                            DM_List.Add(danmokus.ElementAt(i));
                            readXML(danmoku_dir);

                        }


                    }

                    danmoku_dir = null;
                    Danmoku_status.Text = "DM Ready";


                }
                if (Comment_Windows.Count == 0)
                {
                    commentWindowSetup();
                }
                else
                {
                    ///////fm3.Owner = this;

                }

                autoLoadDMlist(danmokus.ElementAt(0));

            }
            else
            {
                Danmoku_status.Text = "No DM";

            }
        
        
        }
        private void setDMToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            setDM_Menu();
        }
        bool cw = false;
        void multicommentwindowsetup() {

            if (cw == false)
            {
                cw = true;
            }
            else {
                cw = false;
            }
            
           Neo_Comment_window f = new Neo_Comment_window(cw);
            f.setLocation = new Point(this.Location.X + 8, this.Location.Y + 59);
            f.Size = new Size(vlcPlayer.Size.Width, vlcPlayer.Size.Height); // - 45
            f.fontsize = fontsize;

            f.Show();
      
            f.setInterval = winterval;
            f.setMoveDistance = move_distance;
            f.setCommentLimit = commentLimit;
            winterval++;
            Comment_Windows.Add(f);
        
        }

        void f_MouseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            int x = e.X;
            int y = e.Y;
            //during fullscreen mode
            if (fullscreen)
            {
                if (Comment_Windows.Count > 0)
                {
                    int borderY = VLC_track.Location.Y;
                    int borderUY = menuStrip1.Size.Height;
                    if (y > borderY)
                    {
                        showPlayMenuDown();


                    }else if(y<borderUY){
                        showPlayMenuUp();
                    }
                    else
                    {
                        hidePlayMenu();

                    }

                }
            }
            else {

                hidePlayMenu();
            }


        }
        List<Button> buttons = new List<Button>();
        List<TrackBar> trackbars = new List<TrackBar>();
        List<ToolStripStatusLabel> sslabel = new List<ToolStripStatusLabel>();
        StatusStrip sstrip = new StatusStrip();
        bool fullscreenUp = false;
        void showPlayMenuUp() {
            int cw = Comment_Windows.Count - 1;
            if(fullscreenUp==false){

                this.Controls.Remove(menuStrip1);
                Comment_Windows.ElementAt(cw).Controls.Add(menuStrip1);
                menuStrip1.Show();

                hideCurosr = false;
                fullscreenUp = true;
            }
        
        }

        void  Clone<T>(T n, T o) where T: Control
        {
            try
            {
                n.Location = o.Location;
                n.Size = o.Size;
                n.Text = o.Text;
                n.BackgroundImage = o.BackgroundImage;
            }
            catch (Exception e){Console.WriteLine(e.ToString()); 
            

            }
 
          
            //the code here clones all events
            var eventsField = typeof(Component).GetField("events", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var eventHandlerList = eventsField.GetValue(o);
            eventsField.SetValue(n, eventHandlerList);
            

    
    }
        bool fullscreenBottom = false;
        void showPlayMenuDown() {
            int cw= Comment_Windows.Count-1;
            if (fullscreenBottom==false && Comment_Windows.ElementAt(cw).Controls.OfType<Button>().Count() == 0)
            {
                //if the layer doen't contain the component, make them
                for (int i = 0; i < buttons.Count; i++)
                {
                    /*
                    Button a = new Button();
                    Clone<Button>(a,this.Controls.OfType<Button>().ElementAt(i));
                    Comment_Windows.ElementAt(cw).Controls.Add(a);
                    a.Show();

                    buttons.Add(a);
                     */
                    this.Controls.Remove(buttons.ElementAt(i));
                    Comment_Windows.ElementAt(cw).Controls.Add(buttons.ElementAt(i));
                    buttons.ElementAt(i).Show();
                }

                for (int i = 0; i < trackbars.Count(); i++)
                {
                    /*
                    TrackBar a = new TrackBar();
                    Clone<TrackBar>(a,this.Controls.OfType<TrackBar>().ElementAt(i));
                    Comment_Windows.ElementAt(cw).Controls.Add(a);
                    a.Show();

                    trackbars.Add(a);
                    */
                    this.Controls.Remove(trackbars.ElementAt(i));
                    Comment_Windows.ElementAt(cw).Controls.Add(trackbars.ElementAt(i));
                    trackbars.ElementAt(i).Show();
                }
                /*
                Clone<StatusStrip>(sstrip,statusStrip1);
                Comment_Windows.ElementAt(cw).Controls.Add(sstrip);

                sstrip.Show();

                int countsslabel = this.Controls.OfType<ToolStripStatusLabel>().Count();
                for (int i = 0; i < countsslabel; i++) {


                    ToolStripStatusLabel s = new ToolStripStatusLabel();
                    s.Text = this.Controls.OfType<ToolStripStatusLabel>().ElementAt(i).Text;
                    sstrip.Items.Add(s);
                    sslabel.Add(s);

                }
                 */
                this.Controls.Remove(statusStrip1);
                Comment_Windows.ElementAt(cw).Controls.Add(statusStrip1);
                statusStrip1.Show();
                
                fullscreenBottom = true;
                //hideCurosr = false;

            }
            else if (fullscreenBottom == false)
            {
                /*
                for (int i = 0; i < buttons.Count; i++) {
                    buttons.ElementAt(i).Show();
                }
                for (int i = 0; i < trackbars.Count; i++) {
                    trackbars.ElementAt(i).Show();
                }
                 */
                int countbutton = this.Controls.OfType<Button>().Count();
                int counttrack = this.Controls.OfType<TrackBar>().Count();
                for (int i = 0; i < buttons.Count(); i++)
                {
                 
                    buttons.ElementAt(i).Show();
                }
                for (int i = 0; i < trackbars.Count(); i++)
                {
                  
                    trackbars.ElementAt(i).Show();
                }

                statusStrip1.Show();

                fullscreenBottom = true;
            }
            hideCurosr = false;

        }
        void hidePlayMenu()
        {
            if ((fullscreenBottom == true ||fullscreenUp==true) && fullscreen==true) {
               /*
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons.ElementAt(i).Hide();
                }
                for (int i = 0; i < trackbars.Count; i++)
                {
                    trackbars.ElementAt(i).Hide();
                }
                if (fullscreenBottom == true && fullscreen==true)
                {
                    statusStrip1.Hide();
                }
                */
                /*
                int countbutton = this.Controls.OfType<Button>().Count();
                int counttrack = this.Controls.OfType<TrackBar>().Count();
                for (int i = 0; i < this.Controls.OfType<Button>().Count(); i++)
                {
                    Controls.OfType<Button>().ElementAt(i).Hide();
                }
                for (int i = 0; i <  this.Controls.OfType<TrackBar>().Count(); i++)
                {
                    Controls.OfType<TrackBar>().ElementAt(i).Hide();
                }
                statusStrip1.Hide();
                 */ int cw = Comment_Windows.Count - 1;
                if (fullscreenBottom == true)
                {
                   
                    for (int i = 0; i < trackbars.Count; i++)
                    {

                        Comment_Windows.ElementAt(cw).Controls.Remove(trackbars.ElementAt(i));
                        this.Controls.Add(trackbars.ElementAt(i));
                    }

                    for (int i = 0; i < buttons.Count; i++)
                    {
                        Comment_Windows.ElementAt(cw).Controls.Remove(buttons.ElementAt(i));
                        this.Controls.Add(buttons.ElementAt(i));

                    }

                    Comment_Windows.ElementAt(cw).Controls.Remove(statusStrip1);

                    this.Controls.Add(statusStrip1);
                   
                }
                else
                {
                    Comment_Windows.ElementAt(cw).Controls.Remove(menuStrip1);
                    this.Controls.Add(menuStrip1);
                }

                fullscreenBottom = false;
                fullscreenUp = false;
                if (activeForms == 0 && vlcPlayer.State.ToString()=="Playing")
                {
                    hideCurosr = true;
                }
            
            }
         //   Cursor.Hide();
          
        
        }
        int winterval = 57;
        void cwSetOwner(List<Neo_Comment_window> l)
        {

            for (int i = 0; i < l.Count; i++)
            {
                if (i - 1 >= 0)
                {
                    l.ElementAt(i).Owner = l.ElementAt(i - 1);
                }
                else
                {
                    l.ElementAt(i).Owner = this;
                }
            }
        }
        void ChangeCWnumber(){



          for (int i = 0; i < Comment_Windows.Count; i++) {
                Comment_Windows.ElementAt(i).Dispose();
            
            
            }

            Comment_Windows.Clear();
            addcomment = 0;
            commentWindowSetup();
        
        
        
        }
        void changeCommentWindowsNumber() {
            int number = panel_numbers;

            if (currentfile!=null) {

                DialogResult dr = MessageBox.Show("Doing this will pause the current playing media, are you sure ","alert",MessageBoxButtons.YesNo);
                
               
               if(dr ==DialogResult.Yes ){
               
               vlcPlayer.Pause();

               ChangeCWnumber();


              

               }  
                
            
            }else{
            
            ChangeCWnumber();
            
            }





      
        
        
        }
        void commentControlSetup(Neo_Comment_window f) {

            f.MouseClick += new MouseEventHandler(dm_MouseClick);
            f.MouseDoubleClick += new MouseEventHandler(fm3_MouseDoubleClick);

            f.KeyUp += new KeyEventHandler(fm3_KeyUp);
            f.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            DragDropSetup(f);

            f.MouseMove += new MouseEventHandler(f_MouseMove);


            
        }
        void commentWindowSetup()
        {


            for (int i = 0; i < panel_numbers+1; i++) {

                multicommentwindowsetup();
            }
            int cw = Comment_Windows.Count - 1;
            commentControlSetup(Comment_Windows.ElementAt(cw));

            // maybe will add this in a list to reduce lines

            cwSetOwner(Comment_Windows);

            /*
                fm3_1.Owner = this;
                fm3_2.Owner = fm3_1;
                fm3_3.Owner = fm3_2;
                fm3_4.Owner = fm3_3;
                fm3_5.Owner = fm3_4;
                fm3_6.Owner = fm3_5;
                fm3_7.Owner = fm3_6;
                fm3.Owner = fm3_7;
            */


               // ///////fm3.Owner = this;
            if (comment2.Count > 0) {

                srCommentWindow(comment2);
            }
        }
        void DragDropSetup(Form f) {

            f.AllowDrop = true;

            f.MouseUp += new MouseEventHandler(f_MouseUp);
            f.DragEnter += new DragEventHandler(f_DragEnter);
        
        
        
        
        
        }
        bool mouseup = false;
        void f_MouseUp(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            mouseup = true;
            


        }

        void f_DragEnter(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
            //set up drag drop here
            String[] files = e.Data.GetData(DataFormats.FileDrop, false) as String[];
            e.Effect = DragDropEffects.Move;
            

                if (files != null) {

                    for (int i = 0; i < files.Length; i++) {

                        FileInfo file = new FileInfo(files[i]);
                        String[] temp = { file.FullName, file.Name };
                        if (!Media_List.Contains(temp))
                        {
                            Media_List.Add(temp);
                        }
                  //      printvlc(file.FullName);
                    
                    }
                    FileInfo playfile = new FileInfo(files[0]);


                    vlcSetMedia(playfile);
                    vlcPlayer.Play();

                
                    






                mouseup = false;
            }

        }
        void fm3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FullScreenSwitch();
        }

        void fm3_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpActions(e);
        }
        //modified from String[] to List so multiple files can be selected at once.

        private List<String[]> setFile(OpenFileDialog ofd){
            ofd.Multiselect = true;
        
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

                int end = ofd.FileNames.ElementAt(0).LastIndexOf("\\");
                String dir = ofd.FileNames.ElementAt(0).Substring(0, end);
                ofd.InitialDirectory = dir;
                List<String[]> temp = new List<String[]>();
                for (int i = 0; i < ofd.FileNames.Count(); i++)
                {
                    String[] result = { ofd.FileNames.ElementAt(i), ofd.SafeFileNames.ElementAt(i) };
                    temp.Add(result);


                }
                return temp;

            
            }else{
            
                return null;
            }
        
        
        }


        void change_timeoffset(int s) {
            time_offset = s;
        
        }
        void change_move_distance(int s) {

            _distance = s;
        
        }

        void print(String s) {

            debugtab.Text = s;
        
        }
        void print2(String s) {

            debugtab2.Text = s;
        }

        void autoSetVolumn() {
            try
            {
                if (vlcPlayer.Audio.Volume != vVolume)
                {
                    vlcPlayer.Audio.Volume = vVolume;
                    try
                    {
                        sound_trackbar.Value = vVolume;
                    }
                    catch (ArgumentOutOfRangeException) {
                        sound_trackbar.Value = 100;
                        vVolume = 100;
                    }
                };
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.ToString()); 
            
            }
        
        }

        void autoSetTrackTime() {


            if (vlc_track_time != -2)
            {

                vlcPlayer.Time = vlc_track_time;
                time_counter = vlc_track_time / 10;

                vlc_track_time = -2;

            }
        
        }
        int comment_time;

        void makeComment() {

            adjust_interval();
            //sets the player volume if it's not equal to the current user volume
            autoSetVolumn();

            //component for track bar, checks if the track value has changed if yes then set
            autoSetTrackTime();

            if (choose_player != 1)
            {

                    if (vlcPlayer.IsPlaying)
                    {
                        //checks if the video reaches the end of the video
                        manual_checkend((int)vlcPlayer.Length / 10, time_counter);
                    }
               
            }

           

            
            try
            {
                switch(choose_player){
                    case 1:
                        if (auto_TimeMatch == false)
                {
                    comment_time = (int)(Media_Player.Ctlcontrols.currentPosition * 100) + time_offset;
                }
                else {

                    comment_time = (int)(Media_Player.Ctlcontrols.currentPosition * 100) + auto_base+offset_auto;
                
                }

                        break;


                    default:

                       
                        //if auto match is on then offset the comments by the comment's largest vpos-video time
                        if (auto_TimeMatch == false)
                        {


                            //second important element in the engine, basiclly time_counter with user offset to get the comments to the right video position
                            comment_time = time_counter+ time_offset;
                        }
                        else
                        {

                            comment_time = time_counter + auto_base + offset_auto; 

                        }
                    break;
            }

            }
            catch (Exception e){Console.WriteLine(e.ToString()); comment_time = 0; }

            

            //comment_time is time_counter + the offset
            print(showTime(((int)(vlcPlayer.Time)) )+" "+showTime(time_counter*10) + "/" + comment_time + "/" + time_counter + " missed: "+missed);


            if (Comment_Windows.Count>0)
            {
                if (commentmethod != 1)
                {
                    print2(showTime((int)vlcPlayer.Length) + "  L: " +fm3.Controls.OfType<Label>().Count()+ " " + (playedcomment) + "/" + (comment.Count()));
                }
                else {
       //             print2(showTime((int)vlcPlayer.Length) + "L: " + (comment_storage.Count+comment_storage2.Count) + "  L1: " + (comment_storage.Count) + " L2: " + comment_storage2.Count  + " " + (playedcomment + _duplicates) + "/" + (comment.Count()));
      
              //      print2(showTime((int)vlcPlayer.Length) + "L: "+(fm3.Controls.OfType<Label>().Count())+ "  L1: " + (comment_storage.Count)+ " L2: "+comment_storage2.Count+" L3 "+cme1.setStorage.Count+" L4: "+cme2.setStorage.Count + " " + (playedcomment + _duplicates) + "/" + (comment.Count()));
                  //  print2(showTime((int)vlcPlayer.Length) + "L: " + (cme1.setStorage.Count+cme2.setStorage.Count+comment_storage.Count+comment_storage2.Count) + "  L1: " + (comment_storage.Count) + " L2: " + comment_storage2.Count + " L3 " + cme1.setStorage.Count + " L4: " + cme2.setStorage.Count + " " + (playedcomment + _duplicates) + "/" + (comment.Count()));

                    print2(showTime((int)vlcPlayer.Length) + " L: " +totalComments(Comment_Windows) +showEachCount(Comment_Windows)+ " " + (CWplayedcomments(Comment_Windows)) + "/" + (comment.Count()));

                
                }
                }
            else {

                print2(playedcomment + "/" + (comment.Count() + _duplicates));
            
            }

            //only runs if there's comments avaliable;
            
            if (comment2.Count > 0 && Comment_Windows.Count>0)
            {
               // addComment(comment_time, comment2);
                CWaddComment(Comment_Windows, comment_time);

            }
            if (counter_check != -9999 && counter_check != time_counter - 1) {


           //     missed++;
            
            }else if(time_counter ==0){
                //reset missed and counter check if time_counter is zero;
                missed = 0;
                counter_check = -9999;
            }

            counter_check = time_counter;

            //super important the main clock for the comment engine;
           
            time_counter++;
            if (time_counter < VLC_track.Maximum)
            {
                setTime_track(time_counter);
            }
        
        }
        int missed = 0;
        int addcomment = 0;
        int addctime = -9999;
        void CWaddComment(List<Neo_Comment_window>l,int time) {

            if (addctime != time)
            {
                if (addcomment < l.Count - 1)
                {

                    l.ElementAt(addcomment).createLabel_extra = time;
                   /*

                    for (int i = 0; i < l.Count; i++) {
                        if (i != addcomment) {

                            l.ElementAt(i).movecommentOnCreate();
                        
                        }
                    
                    }*/
                    addcomment++;
                }
                else
                {
                    l.ElementAt(addcomment).createLabel_extra = time;
                 /*  for (int i = 0; i < l.Count; i++)
                    {
                        if (i != addcomment)
                        {

                            l.ElementAt(i).movecommentOnCreate();

                        }

                   }*/
                    addcomment = 0;

                }
                addctime = time;
            }
        
        
        }
        String showEachCount(List<Neo_Comment_window> l)
        {
            String result = "";

            for (int i = 0; i < l.Count-1; i++) {
               result+=" L"+i+": "+ l.ElementAt(i).setStorage.Count;
            
            }

            return result;
        }
        String showTime(int time) {

            String result="";

            int totalsec = time / 1000;
            int sec = totalsec % 60;
            int min,hour;

            if (totalsec / 60 < 60)
            {
                min = totalsec / 60;
                hour = 0;
            }
            else {

                min = (totalsec / 60) % 60;
                hour = (totalsec / 60) / 60;
            }

            result = String.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
            return result;
        }
        void moveComment() {


            if (time_counter % speed_control == 0)
            {
 /*               foreach (Label l in Controls.OfType<Label>())
                {

                    MoveLabel(l);


                }
                
*/

                    switch(commentmethod){


                        case 1:
                            List<Label> removetemp = new List<Label>();
                            for (int i = 0; i < comment_storage.Count; i++) {


                                MoveLabel(comment_storage.ElementAt(i));

                                if (comment_storage.ElementAt(i).IsDisposed) {

                                    removetemp.Add(comment_storage.ElementAt(i));
                                }
                            
                            }

                            if (removetemp.Count > 0)
                            {
                                for (int i = 0; i < removetemp.Count; i++)
                                {

                                    comment_storage.Remove(removetemp.ElementAt(i));

                            
                                
                                }

                            }
                            //clear removelist here ?    but it should be empty by the end of the code
                           

                                break;
                 
                        
                    default:
                        for (int i = 0; i < fm3.Controls.OfType<Label>().Count(); i++)
                    {


                        MoveLabel(fm3.Controls.OfType<Label>().ElementAt(i));



                    }

                    break;

                
                
                }

                
                
                
                
                /*
                foreach (Label l in remove_List)
                {
                    Controls.Remove(l);
                  //  comment_storage.Remove(l);
                }

                remove_List.Clear();
                  */
            }
        
        }

        List<Label> comment_storage2 = new List<Label>();
        void moveComment_thread2()
        {
            //   if (time_counter % speed_control == 0 && fm3!=null)
            if (fm3 != null)
            {
                //not sure how fast this is compare to actual label need to do test on this
                switch (commentmethod)
                {


                    case 1:
                        try
                        {

                            List<Label> removetemp2 = new List<Label>();


                            for (int i = 0; i < comment_storage2.Count; i++)
                            {


                                MoveLabel_threadEX(comment_storage2.ElementAt(i));

                                if (comment_storage2.ElementAt(i).IsDisposed)
                                {
                                    removetemp2.Add(comment_storage2.ElementAt(i));
                                }

                            }

                            if (removetemp2.Count > 0)
                            {
                                for (int i = 0; i < removetemp2.Count; i++)
                                {

                                    comment_storage2.Remove(removetemp2.ElementAt(i));



                                }


                            }
                        }
                        catch (Exception e){Console.WriteLine(e.ToString()); };
                        //clear removelist here ?    but it should be empty by the end of the code


                        break;


                    default:
                        for (int i = 0; i < fm3.Controls.OfType<Label>().Count(); i++)
                        {


                            MoveLabel_threadEX(fm3.Controls.OfType<Label>().ElementAt(i));



                        }

                        break;



                }

            }


        }

        void moveComment_thread() {
         //   if (time_counter % speed_control == 0 && fm3!=null)
           if(fm3!=null)
           {
               //not sure how fast this is compare to actual label need to do test on this
                               switch(commentmethod){


                        case 1:
                                       try
                                       {
                                           List<Label> removetemp = new List<Label>();
                               

                                           for (int i = 0; i < comment_storage.Count; i++)
                                           {


                                               MoveLabel_threadEX(comment_storage.ElementAt(i));

                                               if (comment_storage.ElementAt(i).IsDisposed)
                                               {
                                                   removetemp.Add(comment_storage.ElementAt(i));
                                               }

                                           }

                                  
                                           
                                               for (int i = 0; i < removetemp.Count; i++)
                                               {

                                                   comment_storage.Remove(removetemp.ElementAt(i));



                                               }


                                           


                                       }
                                       catch (Exception e){Console.WriteLine(e.ToString()); };
                            //clear removelist here ?    but it should be empty by the end of the code
                           

                                break;
                 
                        
                    default:
                        for (int i = 0; i < fm3.Controls.OfType<Label>().Count(); i++)
                    {


                        MoveLabel_threadEX(fm3.Controls.OfType<Label>().ElementAt(i));



                    }

                    break;

                
                
                }

            }
            
        
        }
        void commentEngine() {

            if(sommentswitch ==true){

                makeComment();

                //moveComment();

            }
             
        
        
        }

        void commentEngine_thread() {

            if (this.InvokeRequired)
            {

                commentEnginecomp c = new commentEnginecomp(commentEngine_thread);
                try
                {
                    this.Invoke(c, new object[] { });

                }
                catch (Exception e){Console.WriteLine(e.ToString()); }


            }
            else {

                if (sommentswitch == true)
                {

                    makeComment();

                    //moveComment();

                }
             
        
            
            }
        
        
        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
       //   commentEngine(); 
            if (!threading_mode)
            {
                   moveComment();

            }
            
        }
      void  removebyDel_DM(List<String[]> dm,ListBox mbox,bool readxml){



          for (int i = 0; i < dm.Count(); i++)
                        {
                            foreach (object l in mbox.SelectedItems)
                            {
                                if (dm.ElementAt(i)[1].Equals(l.ToString()))
                                {

                                    dm.Remove(dm.ElementAt(i));
                                }
                            }

                        }
                        if (mbox.SelectedItems.Count > 1) {

                            List<object> removel = new List<object>();
                            foreach (object i in mbox.SelectedItems) {

                                removel.Add(i);
                            
                            }
                            for (int i = 0; i < removel.Count; i++) {

                                mbox.Items.Remove(removel.ElementAt(i));
                            }

                        }
                        else
                        {
                            mbox.Items.Remove(mbox.SelectedItem);
                        }

                 // after comment is removed  from the listbox and the list
    
                    if (readxml)
                    {

                        srCommentWindow();
                        comment2.Clear();
                        comment.Clear();
                        _duplicates = 0;

                        for (int i = 0; i < dm.Count(); i++)
                        {
                            readXML(dm.ElementAt(i)[0]);


                        }
                    }
}
        private void Media_DM_menu_Click(object sender, EventArgs e)
        {
            List_menu_setup();
        }

        void DMMlistsetup() {
            fm2 = new Form2();

            if (Comment_Windows.Count>0)
            {
                fm2.Owner = Comment_Windows.ElementAt(Comment_Windows.Count-1);


            }
            fm2.Disposed += new EventHandler(fm2_Disposed);


            if (DM_List != null)
            {
                fm2.setDMList = DM_List;
                //   fm2.setDMList(DM_List);
            }
            if (Media_List != null)
            {
                fm2.setMediaList = Media_List;
                //fm2.setMediaList(Media_List);
            }
            if (FullDM_List != null)
            {
                fm2.setFullDMList = FullDM_List;
            }
            fm2.setMListBox.DoubleClick += new EventHandler(setMListBox_DoubleClick);
            fm2.setDMListBox.DoubleClick += new EventHandler(setDMListBox_DoubleClick);
            fm2.setFullDMBox.DoubleClick += new EventHandler(setFullDMBox_DoubleClick);
            fm2.setMListBox.KeyUp += new KeyEventHandler(setMListBox_KeyUp);
            fm2.setDMListBox.KeyUp += new KeyEventHandler(setDMListBox_KeyUp);
            fm2.setFullDMBox.KeyUp += new KeyEventHandler(setFullDMBox_KeyUp);
            //fm2.FormClosed += new FormClosedEventHandler(Form_FormClosed);
           // fm2.MouseMove += new MouseEventHandler(Form_MourseMove);

            // if (vlcPlayer.GetCurrentMedia() != null) {
            if(currentfile !=null){  

                selectPlaying_box(currentfile.Name);
            }

            fm2.Show();
            if (fullscreen == true) {
                hideCurosr = false;
            }
            activeForms++;
        
        }
        int activeForms = 0;
        void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            form_closing_action();
        }
        void form_closing_action() {

            activeForms--;
            //throw new NotImplementedException();
            if (fullscreen == true && activeForms == 0 && vlcPlayer.State.ToString() == "Playing")
            {
                hideCurosr = true;
            }
        }
        void Form_MourseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            Form test = sender as Form;
            if (fullscreen == true) {
                int mouseX = e.X;
                int mouseY = e.Y;
                Point defaultLocation = test.DesktopLocation;
                int boundX = test.Size.Width + defaultLocation.X;
                int boundY = test.Size.Height + defaultLocation.Y;

                if (mouseX < boundX && mouseX > defaultLocation.X && mouseY<boundY&&mouseY >defaultLocation.Y)
                {

                    hideCurosr = false;
                }
                else {

                    hideCurosr = true;
                }
            
            
            
            
            
            
            }
        }

        void setFullDMBox_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            ListBox mbox = (ListBox)sender;
            switch (e.KeyValue) { 
            
                case 46:
                    removebyDel_DM(FullDM_List, mbox,false);
                    break;
            
            
            }
        }

        void setDMListBox_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            //bascally do exact the same thing as mlist key up

            ListBox mbox = (ListBox)sender;

            switch (e.KeyValue) { 
            

            
                case 46:



                    removebyDel_DM(DM_List,mbox,true);

                        break;
            
            
            }
        }

        void List_menu_setup() {

            if (fm2 == null)
            {
                DMMlistsetup();

            }
            else {

                fm2.Dispose();
            }
        
        
        
        
        
        
        }

        void setMListBox_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            // set del key action here if del is pressed with an item selected then remove the selected item from the Media List and media list box

            //also probably add a function to play the media selected if enter key is pressed instead of double clicking it ?

            ListBox mbox = (ListBox)sender;



            switch (e.KeyValue) { 
            
            
                case 13:
                    //when enter key is let go
                    runSelectedVido(sender,e);
                    break;


                case 46:
                    //when del key is let go
        
                        for (int i = 0; i < Media_List.Count(); i++)
                        {
                            foreach (object l in mbox.SelectedItems)
                            {
                                if (Media_List.ElementAt(i)[1].Equals(l.ToString()))
                                {

                                    Media_List.Remove(Media_List.ElementAt(i));
                                }
                            }

                        }
                        if (mbox.SelectedItems.Count > 1) {

                            List<object> removel = new List<object>();
                            foreach (object i in mbox.SelectedItems) {

                                removel.Add(i);
                            
                            }
                            for (int i = 0; i < removel.Count; i++) {

                                mbox.Items.Remove(removel.ElementAt(i));
                            }

                        }
                        else
                        {
                            mbox.Items.Remove(mbox.SelectedItem);
                        }

                 

                    break;
            
            
            
            
            
            }
        }



        void setFullDMBox_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            
            //basiclly the same build as Mlistbox double click, but do it with dm
            //goes into the current media directory and fetch all .xml files avaliable add to the full dmlistbox and list
            //check if the item clicked is already in the DMlistbox if not then 
            //add item to the left DMlistbox and load load comments with readXML
            //if it's already in there then do nothing !!!

            ListBox cbox = (ListBox)sender;
            bool has = false;
            for (int i = 0; i < DM_List.Count(); i++) {

                
                if (DM_List.ElementAt(i)[1].Equals(cbox.SelectedItem.ToString())) {

                    has = true;
                
                }
            
            
            }
            if (!has) {

                for (int i = 0; i < FullDM_List.Count(); i++) {

                    if (cbox.SelectedItem.ToString().Equals(FullDM_List.ElementAt(i)[1])) {

                        DM_List.Add(FullDM_List.ElementAt(i));
                        fm2.setDMListBox.Items.Add(cbox.SelectedItem.ToString());
                        readXML(FullDM_List.ElementAt(i)[0]);
                    
                    }
                
                }
            
            }
            if (Comment_Windows.Count == 0) {

                commentWindowSetup();
                fm2.Owner = Comment_Windows.ElementAt(Comment_Windows.Count - 1);
                fm2.Dispose();
                DMMlistsetup();

            }

        }

        void removeDMitem(object sender, EventArgs e) {

            //throw new NotImplementedException();

            //Unload the DMs 
            // remove the selected dm from the listbox, the list and then do a reload all dms

            ListBox dmbox = (ListBox)sender;

            srCommentWindow();
            CWRemoveComments(Comment_Windows);
            comment2.Clear();
            comment.Clear();
            _duplicates = 0;

            //loop through to find the selected item if found then remove if not then load to comment2 using read xml
            try
            {
                for (int i = 0; i < DM_List.Count(); i++)
                {

                    if (DM_List.ElementAt(i)[1].Equals(dmbox.SelectedItem.ToString()))
                    {

                        DM_List.Remove(DM_List.ElementAt(i));
                    }
                    else
                    {

                        readXML(DM_List.ElementAt(i)[0]);
                    }

                }
                //dmbox.Items.Remove(dmbox.SelectedItem);
                fm2.setDMList = DM_List;

            }
            catch (NullReferenceException a) {
                Console.WriteLine(a.ToString());
                Console.WriteLine("in removeDMitem"); };
          
        }
        ProcessStartInfo ps = new ProcessStartInfo();
        void restartApplication() {
            /*
            ps.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            ps.CreateNoWindow = true;
            ps.FileName = "cmd.exe";
            Process.Start(ps);
            Application.Exit();
        */
            Application.Restart();
        }
        void setDMListBox_DoubleClick(object sender, EventArgs e)
        {
            removeDMitem(sender, e);

        }
        //only use when trying menual double click
        String mdclick;

        void runSelectedVido(object sender, EventArgs e)
        {

            //throw new NotImplementedException();

            //implement set media by click method here
            String result = "";

            ListBox mtemp = (ListBox)sender;

            if (vlcPlayer.IsPlaying)
            {

                vlcPlayer.Stop();
            }

            for (int i = 0; i < Media_List.Count(); i++)
            {

                if (Media_List.ElementAt(i)[1].Equals(mtemp.SelectedItem.ToString()))
                {

                    result = Media_List.ElementAt(i)[0];

                }


            }
            if (result != "")
            {
                FileInfo newMedia = new FileInfo(result);
                String current = null;
                if (currentfile!=null)
                {
                   current = currentfile.FullName;
                }
                //unload the loaded dm files, to avoid using the wrong dm on different media files?


                if (fm2 != null)
                {
                    fm2.setDMListBox.Items.Clear();

                }
                vlcSetMedia(newMedia);

                fm2.setDMList = DM_List;

                vlcPlayer.Play();
                timerStart();



             //   autoLoadByName(newMedia);



                if (Comment_Windows.Count == 0) {

                    commentWindowSetup();
                    if (fm2 != null)
                    {
                        fm2.Owner = Comment_Windows.ElementAt(Comment_Windows.Count - 1);
                    }
                    // fm2.Dispose();
                    DMMlistsetup();
                }

                this.Text = "DM Player " + newMedia.Name;
            }
            /*

            //this is manual double click code
            if (mdclick == mtemp.SelectedValue) { 
            
            
            
            }
            
            
            mdclick = mtemp.SelectedValue.ToString();

            */


        }
        void setMListBox_DoubleClick(object sender, EventArgs e)
        {

            runSelectedVido(sender,e);

        }
        void fm2_Disposed(object sender, EventArgs e)
        {
            activeForms--;
            fm2 = null;
            if (fullscreen && activeForms == 0 && vlcPlayer.State.ToString()=="Playing") {
                hideCurosr = true;
            }
        }
       
        
        private void openMeidaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void setDMsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // loads multiple XML files here
            if (multi_dm_mode == false)
            {
                multi_dm_mode = true;
                setDMsToolStripMenuItem.Text = "Set DMs" + " Multi On";
            }
            else {
                multi_dm_mode = false;
                setDMsToolStripMenuItem.Text = "Set DMs" + "Multi Off";
                
            }

        }

        private void Settings_menu_Click(object sender, EventArgs e)
        {
            if (fm4 == null)
            {
                setting_setup();
            }
            else {
                fm4.Dispose();
            }

        }

        void setting_setup() {

            if (fm4 == null)
            {
                // open up a new form for displaying settings options

                if (choose_player == 1)
                {
                    var tconsol = ((WMPLib.IWMPControls3)Media_Player.Ctlcontrols);

                    currentLanguage = tconsol.currentAudioLanguageIndex;

                    lcount = tconsol.audioLanguageCount;
                    if (lcount > 0)
                    {

                        audioinfo = tconsol.getAudioLanguageDescription(currentLanguage);

                    }
                }
                else {



                    lcount = vlcPlayer.Audio.Tracks.Count-1;
                   /*
                    int countup = 0;
                    
                    foreach (object c in vlcPlayer.Audio.Tracks.All) {
                      
                        var b = (Vlc.DotNet.Core.TrackDescription)c;
                        if (b.ID == vlcPlayer.Audio.Tracks.Current.ID)
                        {

                            currentLanguage = countup;


                        }
                        else {
                            countup++;
                        }
                        

                    }
                    */
                  
                    for (int i = 0; i < lcount; i++) {

                        if (vlcPlayer.Audio.Tracks.Current.ID.Equals(vlcPlayer.Audio.Tracks.All.ElementAt(i))) {

                            currentLanguage = i;
                        
                        }
                    
                    
                    }
                     
                    try
                    {
                        if (currentfile != null)
                        {
                            audioinfo = vlcPlayer.Audio.Tracks.Current.Name;
                        }
                        else {
                            audioinfo = "No Media Loaded";
                        }
                       
                    }
                    catch (NullReferenceException e) {
                        Console.WriteLine(e.ToString());
                        Console.Write("in void settingup"); }
                
                
                }
                fm4 = new Settings();
                fm4.Disposed += new EventHandler(fm4_Disposed);
                fm4.getapply.Click += new EventHandler(fm4_apply);
                fm4.getconfirm.Click += new EventHandler(getconfirm_Click);
                fm4.getcancel.Click += new EventHandler(getcancel_Click);
                fm4.getdefault.Click += new EventHandler(getdefault_Click);
                fm4.getAudio_down.Click += new EventHandler(getAudio_down_Click);
                fm4.getAudio_up.Click += new EventHandler(getAudio_up_Click);
                fm4.auto_mode_check.CheckStateChanged += new EventHandler(auto_mode_check_CheckStateChanged);
                fm4.select_commentswitch.SelectedValueChanged += new EventHandler(select_commentswitch_SelectedValueChanged);
                fm4.clipboard.CheckStateChanged += new EventHandler(clipboard_CheckStateChanged);
                fm4.FormClosed += new FormClosedEventHandler(Form_FormClosed);
                fm4.debug_mode.Click += new EventHandler(debug_mode_Click);
                VideoAdjuestAction(fm4.setBrightness);
                VideoAdjuestAction(fm4.setContrast);
                VideoAdjuestAction(fm4.setGamma);
                VideoAdjuestAction(fm4.setHue);
                VideoAdjuestAction(fm4.setSaturation);
                subTittlebuttons(fm4.setSubtitleUP);
                subTittlebuttons(fm4.setSubtitleDown);

                if (fullscreen == true)
                {
                    hideCurosr = false;
                }
            
                this.loadAll();
                if (Comment_Windows.Count>0)
                {
                    fm4.Owner = Comment_Windows.ElementAt(Comment_Windows.Count - 1);
                }
                activeForms++;
                fm4.Show();

            }
       
        }

        void debug_mode_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            var msg = MessageBox.Show("Changing this will require a restart to take effect, Do you want a restart now?", "alert", MessageBoxButtons.YesNo);
            if(msg==DialogResult.Yes){
            debug=fm4.debug_mode.Checked;
                restartApplication();
            }else{
            debug=fm4.debug_mode.Checked;
            }
        }

        void clipboard_CheckStateChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            CheckBox ck = sender as CheckBox;
            clipboardswitch=ck.Checked;
            if (Comment_Windows.Count == 0) {

                commentWindowSetup();
                if (fm7 == null)
                {
                    URL_menuloapup_clipboard();
                }
            }
        }

        void select_commentswitch_SelectedValueChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ComboBox a = sender as ComboBox;
            switch (a.SelectedItem.ToString())
            {

                case "ON":
                    this.sommentswitch = true;
                    break;

                case "OFF":
                    this.sommentswitch = false;
                    break;
            }
        }

        void subTittlebuttons(Button b) {

            b.Click += new EventHandler(b_Click);
        
        }

        void b_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Button b = sender as Button;
            switch (b.Text)
            {

                case "↑":
                    if (Subttile_index != -1 && Subttile_index < vlcPlayer.SubTitles.Count - 1)
                    {
                        Subttile_index++;
                        vlcPlayer.SubTitles.Current = vlcPlayer.SubTitles.All.ElementAt(Subttile_index);

                        fm4.currentSubtitleText.Text = Subttile_index.ToString();
                        fm4.SubtitleText.Text = vlcPlayer.SubTitles.Current.Name;
                    }
                    break; ;
                case "↓":
                    if (Subttile_index != -1 && Subttile_index > 0)
                    {
                        Subttile_index--;
                        vlcPlayer.SubTitles.Current = vlcPlayer.SubTitles.All.ElementAt(Subttile_index);
                        fm4.currentSubtitleText.Text = Subttile_index.ToString();
                        fm4.SubtitleText.Text = vlcPlayer.SubTitles.Current.Name;
                    }
                    break;

            }
        }
        int Subttile_index=-1;
        void findSubtitleIndex() {
            if (currentfile != null) {
                int totalsubtitles = vlcPlayer.SubTitles.Count-1; // probably need to -1
                for (int i = 0; i < vlcPlayer.SubTitles.Count; i++) {
                    if (vlcPlayer.SubTitles.Current.ID == vlcPlayer.SubTitles.All.ElementAt(i).ID) {

                        Subttile_index = i;
                    
                    }

                }

                fm4.currentSubtitleText.Text = Subttile_index.ToString();
                fm4.totalSubtitleText.Text = totalsubtitles.ToString();
                if (vlcPlayer.SubTitles.Current != null)
                {
                    fm4.SubtitleText.Text = vlcPlayer.SubTitles.Current.Name;
                }
            }
        
        
        }
        void VideoAdjuestAction(TrackBar a) {

            a.ValueChanged += new EventHandler(a_ValueChanged);
            a.MouseDown += new MouseEventHandler(a_MouseDown);
        }
        bool VideoMouse = false;
        void a_MouseDown(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            VideoMouse = true;
        }

        void a_ValueChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            TrackBar a = (TrackBar)sender;
            if (VideoMouse == true)
            {
                switch (a.Name)
                {

                    case "Brightness_bar":

                        vlcPlayer.Video.Adjustments.Brightness = ((float)a.Value) / 10;

                      
                        break;

                    case "Contrast_bar":
                        vlcPlayer.Video.Adjustments.Contrast = ((float)a.Value) / 10;
                        break;

                    case "Gamma_bar":
                        vlcPlayer.Video.Adjustments.Gamma = ((float)a.Value) / 10;
                        break;

                    case "Hue_bar":
                        vlcPlayer.Video.Adjustments.Hue = ((float)a.Value) / 10;
                        break;

                    case "Saturation_bar":
                        vlcPlayer.Video.Adjustments.Saturation = ((float)a.Value) / 10;
                        break;




                };

            }
            VideoMouse = false;
        }
        void auto_mode_check_CheckStateChanged(object sender, EventArgs e)
        {
            auto_TimeMatch = fm4.auto_mode_check.Checked;
            CheckBox temp = (CheckBox)sender;

            if (temp.Checked == true &&vpos_video>0 &&vpos_end>0) {
                auto_base = vpos_end - vpos_video;
            
            }
            loadCheck();
        }

        void getAudio_up_Click(object sender, EventArgs e)
        {
            if (lcount > currentLanguage) {

                if (choose_player == 1)
                {
                    var tconsol = ((WMPLib.IWMPControls3)Media_Player.Ctlcontrols);
                    tconsol.currentAudioLanguageIndex = currentLanguage + 1;
                    currentLanguage++;
                    audioinfo = tconsol.getAudioLanguageDescription((Int32)currentLanguage);
                }
                else {
                    currentLanguage++;
                    vlcPlayer.Audio.Tracks.Current=vlcPlayer.Audio.Tracks.All.ElementAt(currentLanguage);

                    audioinfo = vlcPlayer.Audio.Tracks.Current.Name;

                }
                loadAudio();
            }
        }

        void getAudio_down_Click(object sender, EventArgs e)
        {
            if ( currentLanguage>1)
            {
                if (choose_player == 1)
                {
                    var tconsol = ((WMPLib.IWMPControls3)Media_Player.Ctlcontrols);
                    tconsol.currentAudioLanguageIndex = currentLanguage - 1;
                    currentLanguage--;
                    audioinfo = tconsol.getAudioLanguageDescription((Int32)currentLanguage);

                }
                else {
                    currentLanguage--;
                    vlcPlayer.Audio.Tracks.Current = vlcPlayer.Audio.Tracks.All.ElementAt(currentLanguage);
                    

                    audioinfo = vlcPlayer.Audio.Tracks.Current.Name;
                
                }
                loadAudio();
            }
        }

        void getdefault_Click(object sender, EventArgs e)
        {
            loadAll();
        }

        void getcancel_Click(object sender, EventArgs e)
        {


                fm4.Dispose();
                fm4 = null;
           
        }

        void getconfirm_Click(object sender, EventArgs e)
        {
            applyAll();
            if (fm4 != null)
            {
                fm4.Dispose();
                fm4 = null;
            }


        }
        void VideoSetup(TrackBar a, int value) {

            a.Minimum = 0;
            a.Maximum = 30;
            a.Value = value;
        }
        int fontsize;
        void loadAll() {
            if (fm4 != null) {
                switch (this.sommentswitch) { 
                
                    case true:
                        fm4.select_commentswitch.SelectedItem = "ON";

                        break;


                    case false:
                        fm4.select_commentswitch.SelectedItem = "OFF";

                        break;
                
                }
                fm4.auto_mode_check.Checked = this.auto_TimeMatch;

                loadCheck();
                fm4.Cspeed.Text = this._distance.ToString();
                fm4.Cend.Text = this.panel_numbers.ToString();
                fm4.setCommentLimit.Text = commentLimit.ToString();
                loadAudio();
                fm4.clipboard.Checked = clipboardswitch;
                VideoSetup(fm4.setBrightness, (int)vlcPlayer.Video.Adjustments.Brightness*10);
                VideoSetup(fm4.setContrast, (int)vlcPlayer.Video.Adjustments.Contrast*10);
                VideoSetup(fm4.setGamma, (int)vlcPlayer.Video.Adjustments.Gamma*10);
                VideoSetup(fm4.setHue, (int)vlcPlayer.Video.Adjustments.Hue*10);
                VideoSetup(fm4.setSaturation, (int)vlcPlayer.Video.Adjustments.Saturation*10);
                fm4.debug_mode.Checked = debug;
                fm4.setFontSize.Text = fontsize.ToString();
                findSubtitleIndex();
            }
        
        
        }
        void loadAudio() {

            fm4.getCurrentAudio.Text = this.currentLanguage.ToString();
            fm4.getTotalAudio.Text = this.lcount.ToString();
            fm4.getAudioInfo.Text = this.audioinfo;
            print3("Video Length: " + vpos_video.ToString() + "/" + "Comment time: " + vpos_end + "  CL: " + currentLanguage.ToString() + " total: " + lcount.ToString());
                
        
        }
        void loadCheck() {

            fm4.auto_mode_check.Checked = this.auto_TimeMatch;

            if (auto_TimeMatch==true)
            {
                fm4.timeoffset.Text = offset_auto.ToString();

            }
            else
            {

                fm4.timeoffset.Text = time_offset.ToString();
            }
        
        }

        void applyAll() {
            if (fm4 != null) {

                try
                {
                    switch (fm4.select_commentswitch.SelectedItem.ToString()) { 
                    
                        case "ON":
                            this.sommentswitch = true;
                            break;

                        case "OFF":
                            this.sommentswitch = false;
                            break;
                    
                    
                    
                    }


                    this.auto_TimeMatch = fm4.auto_mode_check.Checked;
                    if (fm4.auto_mode_check.Checked==true)
                    {
                        this.auto_TimeMatch = true;
                        this.offset_auto = Int32.Parse(fm4.timeoffset.Text);
                    
                    }
                    else
                    {
                        this.auto_TimeMatch = false;
                        this.time_offset = Int32.Parse(fm4.timeoffset.Text);

                    }
                    this._distance = Int32.Parse(fm4.Cspeed.Text);
                    this.move_distance = Int32.Parse(fm4.Cspeed.Text);

                    if (this.panel_numbers != Int32.Parse(fm4.Cend.Text))
                    {
                        this.panel_numbers = Int32.Parse(fm4.Cend.Text);
                        changeCommentWindowsNumber();
                    }

                    commentLimit = Int32.Parse(fm4.setCommentLimit.Text);
                    if (Comment_Windows.Count > 0) {
                        for (int i = 0; i < Comment_Windows.Count; i++) {

                            Comment_Windows.ElementAt(i).setCommentLimit = Int32.Parse(fm4.setCommentLimit.Text);
                        
                        }
                    
                    }
                    for (int i = 0; i < Comment_Windows.Count - 1; i++)
                    {
                        fontsize = Int32.Parse(fm4.setFontSize.Text);
                        Comment_Windows.ElementAt(i).fontsize = fontsize;
                    }
                    clipboardswitch = fm4.clipboard.Checked;

                }
                catch (Exception e){Console.WriteLine(e.ToString()); }
            
            
            
            }
        }
        void fm4_apply(object sender, EventArgs e) {
            applyAll();
        
        }
        
        void fm4_Disposed(object sender, EventArgs e)
        {
            // save all the changes here
          //  activeForms--;
         //   if (activeForms == 0&&fullscreen) {
         //       hideCurosr = true;
           // }
            fm4.Close();
            fm4.Dispose();
            fm4 = null;


        }



        void onLoadUp()
        {

            if (comment2.Count > 0 && Media_List.Count > 0)
            {



                if (_first_load == false)
                {
                    playedcomment = 0;
                    CWplayedcomments(Comment_Windows, 0);
                    time_counter = 0;

                    autoTimesetup();

                    if (choose_player == 1)
                    {
                        this.Text = "DM PLayer   :" + Media_Player.Ctlcontrols.currentItem.name;
                        _first_load = true;
                    }
                    else
                    {
        
                        setDMtitle(currentfile.Name);
                        _first_load = true;
                    }



                }



            }
        }
        void setDMtitle(String s) {

            if (InvokeRequired)
            {
                setString temp = new setString(setDMtitle);

                this.Invoke(temp, new object[] {s });

            }
            else {

                this.Text = "DM PLayer   :" + s;
            }
        
        
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

            VLC_test test = new VLC_test();
            test.Show();
        }

        private void vlcPlay_button_Click(object sender, EventArgs e)
        {
            if (!vlcPlayer.IsPlaying)
            {

                vlcPlay_button.BackgroundImage = Properties.Resources.pause;
                vlcPlayer.Play();


            }
            else {
                vlcPlay_button.BackgroundImage = Properties.Resources.start;
                vlcPlayer.Pause();
            
            }
        }

        private void vlcStop_button_Click(object sender, EventArgs e)
        {
            if (vlcPlayer.State.ToString()!="Stopped")
            {

                vlcPlayer.Stop();
                VLC_track.Value = 0;

                vlcPlay_button.BackgroundImage = Properties.Resources.start;

            }

        }

        private void vlcSound_button_Click(object sender, EventArgs e)
        {
            if (!vlcPlayer.Audio.IsMute)
            {
                if (vVolume!=0)
                {
                    vlcSound_button.BackgroundImage = Properties.Resources.mute;
                    vlcPlayer.Audio.IsMute = true;
                }

            }
            else
            {
                if (vVolume!=0)
                {
                    vlcSound_button.BackgroundImage = Properties.Resources.sound;

                    vlcPlayer.Audio.IsMute = false;

                }
                //vlcPlayer.Audio.Volume = current_volume;

            }
        }

        private void loop_button_Click(object sender, EventArgs e)
        {
            videoloop = !videoloop;

            if (videoloop) 
            {
                loop_button.BackgroundImage = Properties.Resources.repeat;
            } 
            else
            {
                loop_button.BackgroundImage = Properties.Resources.stop;
            }


            vlc_display.Text = "loop playlist: "+ videoloop.ToString();
        }

        private void vlcPlayer_Click(object sender, EventArgs e)
        {
            Media_Player_ClickAction();
        }
        private void vlcSetMedia(FileInfo file) {
            
            FileInfo playfile = file;
            _duplicates = 0;
            if (file.Name.Contains(".xml")) {

                playfile = new FileInfo(file.FullName.Replace(".xml", ""));
                if (!playfile.Exists) {
                    playfile = file;
                }
            }

          
           
            if (vlcPlayer.IsPlaying)
            {
                timerStop();
                time_counter = 0;


            }
            if (Comment_Windows.Count == 0)
            {

                commentWindowSetup();
            }
            string[] tp = { playfile.FullName, playfile.Name };


                autoLoadMlist(tp);



            Media_status.Text = "Media Set";
            autoLoadByName(playfile);
            currentfile = playfile;
            if (fm2 != null) {
                if (currentfile != null)
                {

                    selectPlaying_box(currentfile.Name);
                }
            
            }
            vlcPlayer.SetMedia(playfile);
        
        }
        private void next_track_Click(object sender, EventArgs e)
        {
            if (Media_List.Count > 1) {

                String current = currentfile.Name;
                int currentindex = -2;

                for (int i = 0; i < Media_List.Count; i++) {

                    if (Media_List.ElementAt(i)[1].Equals(current)) {
                        currentindex = i;
                    
                    }
                
                
                }

                if (currentindex + 1 < Media_List.Count)
                {

                    currentindex++;

                }
                else {
                    currentindex = 0;
                
                }

                FileInfo nfile2 = new FileInfo(Media_List.ElementAt(currentindex)[0]);

                vlcSetMedia(nfile2);

                vlcPlayer.Play();
                timerStart();
   

            }

        }
        void setVLCname(FileInfo file) {

            String temp = file.Name;
            setVLCname_thread("DM Player " + file.Name);

           // Media_status.Text = currentfile.Name;
            
        }

        void setVLCname_thread(string s) {
            if (InvokeRequired) {
                setString st = new setString(setVLCname_thread);
                this.Invoke(st, new object[] { s });
            
            
            } else {

                this.Text = s;
            }
        
        
        
        }

        void selectPlaying_box(String filename) {
            //search for names in listbox equals to filename and selects it
            if (fm2 != null)
            {
                int currentinx  =-2;
                for (int i = 0; i < Media_List.Count; i++) {
                    if (Media_List.ElementAt(i)[1].Equals(filename)) { 
                    
                    currentinx = i;
                    }
                
                }
                if(currentinx>0){
                    
                    setfm2MBoxindex(currentinx);
                }


            }
        
        }
        void setfm2MBoxindex(int index) {

            if (InvokeRequired)
            {
                setInt i = new setInt(setfm2MBoxindex);
                this.Invoke(i, new object[] { index });
            }
            else {
                fm2.setMListBox.ClearSelected();
                fm2.setMListBox.SelectedIndex = index;
            
            }
        
        }
        private void last_track_Click(object sender, EventArgs e)
        {
            if (Media_List.Count > 1)
            {

                String current = currentfile.Name;
                int currentindex = -2;

                for (int i = 0; i < Media_List.Count; i++)
                {

                    if (Media_List.ElementAt(i)[1].Equals(current))
                    {
                        currentindex = i;

                    }


                }

                if (currentindex - 1 > -1)
                {

                    currentindex--;

                }
                else
                {
                    currentindex = Media_List.Count-1;

                }

                FileInfo nfile2 = new FileInfo(Media_List.ElementAt(currentindex)[0]);

                vlcSetMedia(nfile2);

                //timerStop();
                vlcPlayer.Play();
                timerStart();

            }
        }
        // the orginal source page is call links
        // the media file urls are called url

        List<Uri> urls = new List<Uri>();
        List<Uri> Links = new List<Uri>();
        List<string> urlTitles=new List<string>();
        String current_dir_url;
        Uri linkaddress;
        add_link_form fm6;
        Dictionary<Uri, List<Uri>> UrlDictionary= new Dictionary<Uri,List<Uri>>();
        Url_menu fm7;
       // getWebVideo gb;
        private void setFromURL_menu_Click(object sender, EventArgs e)
        {
            if (current_dir_url == null || linkaddress == null)
            {
                if (fm6 == null)
                {
                    fm6 = new add_link_form();
                    fm6.setCacnel.Click += new EventHandler(setCacnel_Click);
                    fm6.setConfirm.Click += new EventHandler(setConfirm_Click);
                    fm6.Disposed += new EventHandler(fm6_Disposed);
                    fm6.FormClosed += new FormClosedEventHandler(Form_FormClosed);
                    activeForms++;

                    if (fullscreen == true)
                    {
                        hideCurosr = false;
                    }
                    if (current_dir_url != null ) {


                        fm6.getDri = current_dir_url;
                    
                    
                    }
                    if (Comment_Windows.Count > 0)
                    {
                        fm6.Owner = Comment_Windows.ElementAt(Comment_Windows.Count - 1);
                    }
                    fm6.Show();




                }
                else {
                 
                    fm6.Dispose();
                
                }
            }
            else {

                URL_menuLoadup();
            
            }


        }


        //   ・xml
        int fm7_number = 0;
        void URL_menuloapup_clipboard() {
            fm7_firsttime = false;
            if (this.fm7 == null)
            {

             
                this.fm7 = new Url_menu();
                if (linkaddress != null)
                {
                    this.fm7.setLinkbox.Text = linkaddress.AbsoluteUri;
                }

                //load the list with the urls if exists
                if (Links.Count > 0)
                {
                    for (int i = 0; i < Links.Count; i++)
                    {
                        fm7.getTitle.Items.Add(urlTitles.ElementAt(i));
                        fm7.getLinks.Items.Add(Links.ElementAt(i));
                        //the url list only displays when an items is selected in title or links
                    }
                }
                else
                {
                    //if links is empty that means it's first time run





                }
                fm7.FormClosing += new FormClosingEventHandler(fm7_FormClosing);
                fm7.getTitle.Click += new EventHandler(getTitle_Click);
                fm7.getTitle.DoubleClick += new EventHandler(getTitle_DoubleClick);
                fm7.getLinks.Click += new EventHandler(getLinks_Click);
                fm7.getloadURLbutton.Click += new EventHandler(getloadURLbutton_Click);
                fm7.getFileUrl.DoubleClick += new EventHandler(getFileUrl_DoubleClick);
                fm7.getFileUrl.MouseHover += new EventHandler(setLinkbox_MouseHover);
                fm7.getFileUrl.MouseMove += new MouseEventHandler(setLinkbox_MouseMove);
                fm7.getFileUrl.MouseLeave += new EventHandler(getFileUrl_MouseLeave);
                fm7.FormClosed += new FormClosedEventHandler(Form_FormClosed);
                fm7.reload.Click += new EventHandler(reload_Click);
              //  activeForms++;

                if (fullscreen == true)
                {
                    hideCurosr = false;
                }

                if (fullscreen == true)
                {
                    hideCurosr = false;
                }
             //   fm7.Disposed += new EventHandler(fm7_Disposed);
                if (Comment_Windows.Count > 0)
                {
                    fm7.Owner = Comment_Windows.ElementAt(Comment_Windows.Count - 1);
                }
                else {

                    commentWindowSetup();
                    fm7.Owner = Comment_Windows.ElementAt(Comment_Windows.Count - 1);
                
                
                }
                fm7.Show();

            }
            else
            {
              //  activeForms++;

                if (fullscreen == true)
                {
                    hideCurosr = false;
                }
                    fm7.hide = false;
                    fm7.Show();



            }
        }

        void fm7_FormClosing(object sender, FormClosingEventArgs e)
        {
            //throw new NotImplementedException();
            form_closing_action();
        }
      
        void URL_menuLoadup() {
            fm7_firsttime = false;
            if (fm7 == null)
            {

              
                fm7 = new Url_menu();
                fm7.setLinkbox.Text = linkaddress.AbsoluteUri;
                

                //load the list with the urls if exists
                if (Links.Count > 0)
                {
                    for (int i = 0; i < Links.Count; i++)
                    {
                        fm7.getTitle.Items.Add(urlTitles.ElementAt(i));
                        fm7.getLinks.Items.Add(Links.ElementAt(i));
                        //the url list only displays when an items is selected in title or links
                    }
                }
                else
                {
                    //if links is empty that means it's first time run



                    load_Url_from(linkaddress);


                }
                fm7.getTitle.Click += new EventHandler(getTitle_Click);
                fm7.getTitle.DoubleClick += new EventHandler(getTitle_DoubleClick);
                fm7.getLinks.Click += new EventHandler(getLinks_Click);
                fm7.getloadURLbutton.Click += new EventHandler(getloadURLbutton_Click);
                fm7.getFileUrl.DoubleClick += new EventHandler(getFileUrl_DoubleClick);
                fm7.getFileUrl.MouseHover += new EventHandler(setLinkbox_MouseHover);
                fm7.getFileUrl.MouseMove += new MouseEventHandler(setLinkbox_MouseMove);
                fm7.getFileUrl.MouseLeave += new EventHandler(getFileUrl_MouseLeave);
                fm7.reload.Click += new EventHandler(reload_Click);
                fm7.FormClosing +=new FormClosingEventHandler(fm7_FormClosing);
               // fm7.Disposed += new EventHandler(fm7_Disposed);
                fm7.Shown += new EventHandler(fm7_Shown);
                if (Comment_Windows.Count > 0)
                {
                    fm7.Owner = Comment_Windows.ElementAt(Comment_Windows.Count - 1);
                }
               // activeForms++;
                fm7.Show();
            }
            else {

                if (fm7.hide == false)
                {
                    fm7.Hide();
                    fm7.hide = true;
                    activeForms--;
                    if (activeForms == 0 && fullscreen) {
                        hideCurosr = true;
                    }
                }
                else {
                    fm7.hide = false;
                    fm7.Show();
                    activeForms++;
                    if (activeForms > 0) {
                        hideCurosr = false;
                    }
                
                }

            
            }
        
        
        }

        void fm7_Shown(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            activeForms++;
        }
        void loadFromClipboard(String urlt) {

           
            Uri url = new Uri(urlt);

            if (url != null)
            {

                if (fm7.getLinks.Items.Contains(url))
                {
                    
                    int place = fm7.getLinks.Items.IndexOf(url);
                    fm7.getLinks.Items.RemoveAt(place);
                    fm7.getTitle.Items.RemoveAt(place);
                    //     UrlDictionary.Remove(url);
                    //no longer needed since already updated the part to adapt update links

                    Links.Remove(url);

                    load_Url_from(url);
                     
                   // MessageBox.Show("The link is already in the list");
                }
                else
                {

                    load_Url_from(url);

                }
            }

            linkaddress = url;
        
        }
        void reload_click() {
            if (fm7 != null)
            {
                string temp = fm7.setLinkbox.Text;
                Uri url = new Uri(temp);
                if (url != null)
                {

                    if (fm7.getLinks.Items.Contains(url))
                    {
                        int place = fm7.getLinks.Items.IndexOf(url);
                        fm7.getLinks.Items.RemoveAt(place);
                        fm7.getTitle.Items.RemoveAt(place);
                        //     UrlDictionary.Remove(url);
                        //no longer needed since already updated the part to adapt update links

                        Links.Remove(url);

                        load_Url_from(url);
                    }
                    else
                    {

                        load_Url_from(url);

                    }
                }
            }
        
        }
        void reload_Click(object sender, EventArgs e)
        {
            //Change the name to set directory

            //throw new NotImplementedException();
            //implement reload the whole page remove the link from the list first then do original load page
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder= Environment.SpecialFolder.MyComputer;
            fbd.SelectedPath = current_dir_url;
            
            if (fbd.ShowDialog() == DialogResult.OK) {
              
                current_dir_url= fbd.SelectedPath;
            
            
            }

           

        }

        void getFileUrl_MouseLeave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            tipMouseHover = false;
        }
        ToolTip tip = new ToolTip();
        int mousex = 0;
        int mousey = 0;
        bool tooltip = false;
        void setLinkbox_MouseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();

            
            if (tipMouseHover == true&& fm7.getFileUrl.SelectedItem!=null&&tooltip==false) {

            
                 mousex = e.X;
                 mousey = e.Y;
                String fullurl = fm7.getFileUrl.SelectedItem.ToString();

                tip.Show(fullurl,fm7,new Point(e.X+fm7.getFileUrl.Location.X,e.Y+fm7.getFileUrl.Location.Y));

                

                tooltip=true;

                //tipMouseHover = false;
            
            }
            if (mousex != e.X || mousey != e.Y|| tipMouseHover ==false) {

                tip.Hide(fm7);
            
            
            tooltip=false;
            }


        }
        bool tipMouseHover = false;
        void setLinkbox_MouseHover(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //when mouse hover over a link have a pop up show the full link to the hover item

            tipMouseHover = true;
           



        }
        bool renamed = false;
        /*
        void renametoXML() {
            //throw new NotImplementedException();
            //rename the shattered xml file to the current file name

            DirectoryInfo dif = new DirectoryInfo(current_dir_url);

            DirectoryInfo chk = new DirectoryInfo(dif.FullName + "\\temp_xml");
            if (!chk.Exists) {
                chk.Create();
            
            }
            FileInfo[] files = chk.GetFiles("*xml");

            int xmlnumber = files.Count(); 

            FileInfo newest= null;

            for (int i = 0; i < files.Count(); i++) {


                if (newest == null || newest.LastWriteTime.Ticks < files.ElementAt(i).LastWriteTime.Ticks) {

                    newest = files.ElementAt(i);
                
                }
            //get the smallest creation time which means the newest file 
            
            }

                //only do this if there's only 1 file that suits the condition 
                if (newest!=null && xmlnumber!=0)
                {

                 //   FileInfo xml = files.ElementAt(0);
                   
                    byte[] tempbytes = Encoding.Default.GetBytes(gb.getCurrentTitle);
                    string transtemp = Encoding.GetEncoding("shift-jis").GetString(tempbytes);
                   // transtemp = transtemp.Replace("?", " ");
                    transtemp = safeFilename(transtemp);
                    newest.CopyTo(dif.FullName + "\\" + transtemp + ".xml", true);

                    newest.Delete();

                    fm7.getDownloadstatus1.Text = "xml file renamed to : " + transtemp + ".xml";

                    renamed = true;
                }
        
        }
         */
        void getTitle_DoubleClick(object sender, EventArgs e)
        {
            //renametoXML();
        }

        void fm7_Disposed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (fm7_number != 0)
            {
                fm7_number=0;
            }
            fm7.Hide();
            fm7.hide = true;
        }

        void getFileUrl_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //download files here

            //・xml

            //renametoXML();

            DirectoryInfo dif = new DirectoryInfo(current_dir_url);

            ListBox box = (ListBox)sender;
            FileInfo file = new FileInfo(current_dir_url+"\\" + fm7.getTitle.SelectedItem.ToString());

            if (file.Exists)
            {
            
            DialogResult dg = MessageBox.Show("Already file existed in the directory are you sure you want to dl the file?","Alert",MessageBoxButtons.YesNo);
            if (dg == DialogResult.Yes)
            {
                downloadFile(sender);
            }
            else {
                if (!vlcPlayer.IsPlaying)
                {
                    if (currentfile==null || currentfile.Name != file.Name)
                    {
                        string[] temp = { file.FullName, file.Name };
                     //   if (!Media_List.Contains(temp))
                     //   {
                       //     Media_List.Add(temp);
                     //   }
                        autoLoadMlist(temp);
                        autoLoadDMlist(temp);

                        // vlcSetMedia(file);
                        vlc_track_time = -2;
                        autoLoadByName(file);
                    }
                }
            }
            }else{
            
            downloadFile(sender);
            }

          //  if (fm7 != null) {

          //      fm7.Focus();
          //  }
        }
        public DownloadProgressChangedEventHandler DownloadProgessChange(ProgressBar pbar,Label percent,String title, Button dlcancel) {

            Action<object, DownloadProgressChangedEventArgs> action = (sender, e) =>
            {

                var _pbar = pbar;

                //throw new NotImplementedException();
                //show dl progress
                String currentbyte = e.BytesReceived.ToString();
                String totalbyte = e.TotalBytesToReceive.ToString();
                int progress = e.ProgressPercentage;
                if (fm7 != null)
                {
                    if (pbar.IsDisposed) {
                        ProgressBar npbar = new ProgressBar();
                        Label status = new Label();
                        downloadstatusSetup(npbar, status, dlcancel);

                        pbar = npbar;
                        percent = status;

                    
                    
                    }

                    pbar.Value = progress;
                    percent.Text = title+" Downloading: " + progress + "% completed " + currentbyte + "/" + totalbyte;
                    

                }
           
            








            };
            return new DownloadProgressChangedEventHandler(action);
        
        
        
        }
        public AsyncCompletedEventHandler DownloadFileCompleted2(Uri filename, ProgressBar pbar, Label status, Button dlcancel)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {

                var _filename = filename;

                if (fm7 != null)
                {

                    //fm7.Size = new Size(fm7.Width, fm7.Height - pbar.Height);
                    pbar.Dispose();
                    status.Dispose();
                    dlcancel.Dispose();

                    //only changes the size back to normal if there's no task is downloading
                    if (fm7.Controls.OfType<ProgressBar>().Count() == 0)
                    {
                        fm7.Size = new Size(fm7.Width, 475);
                    }


                }

                if (e.Error != null)
                {
                    fm7.getDownloadstatus2.Text = "server error";


                }

                try
                {
                    if (e.Cancelled)
                    {


                        fm7.getDownloadstatus2.Text = "server error";


                    }
                }
                catch (Exception a)
                {
                    Console.WriteLine(a.ToString());

                    fm7.getDownloadstatus2.Text = "server error";

                    WebClient wb = (WebClient)sender;
                    wb.Dispose();
                }


            };
            return new AsyncCompletedEventHandler(action);





        }
        public AsyncCompletedEventHandler DownloadFileCompleted(Uri filename,ProgressBar pbar,Label status,Button dlcancel)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                BackgroundWorker bk = new BackgroundWorker();
                bk.DoWork += DownloadDoWork(filename, pbar, status,dlcancel);

                var _filename = filename;

                if (e.Error != null)
                {
                    fm7.getDownloadstatus2.Text = "server error";

                    if (fm7 != null)
                    {

                        //fm7.Size = new Size(fm7.Width, fm7.Height - pbar.Height);
                        pbar.Dispose();
                        status.Dispose();
                        dlcancel.Dispose();

                        //only changes the size back to normal if there's no task is downloading
                        if (fm7.Controls.OfType<ProgressBar>().Count() == 0)
                        {
                            fm7.Size = new Size(fm7.Width, 475);
                        }

                        String filenames = filename.OriginalString;
                        int start = filenames.LastIndexOf("/") + 1;
                        filenames = filenames.Substring(start, filenames.Length - start);
                        filenames = safeFilename(filenames);
                        filenames = current_dir_url + "\\" + filenames;
                        FileInfo file = new FileInfo(filenames);
                       // bk.CancelAsync();
                        bk.Dispose();
                        /*
                        if (file.Exists) {
                            file.Delete();
                        }
                         */ 
                    }
                  //  throw e.Error;
                }

                try
                {
                    if (e.Cancelled)
                    {


                        fm7.getDownloadstatus2.Text = "server error";
              

                    }
                }
                catch (Exception a){
                    Console.WriteLine(a.ToString());

                    fm7.getDownloadstatus2.Text = "server error";

                    WebClient wb = (WebClient)sender;
                    wb.Dispose();
                }
            //    if (!gb.downlaodFile.Any())
           //     {
            //        complited = true;
            //    }
            //    downloadFile(sender);
                try{
              
          
                if (bk!=null && !bk.IsBusy) {
                    bk.RunWorkerAsync();
                
                }
                   // wb_DownloadFileCompleted(filename,pbar,status);
                }
                catch (WebException) { if (fm7 != null) { fm7.getDownloadstatus2.Text = "server error"; } }

            };
            return new AsyncCompletedEventHandler(action);





        }
        public DoWorkEventHandler DownloadDoWork(Uri filename, ProgressBar pbar, Label status,Button dlcancel) {


            Action<object, DoWorkEventArgs> action = (sender, e) =>
            {
                BackgroundWorker bk = sender as BackgroundWorker;
                if (e.Cancel)
                {

                    bk.CancelAsync();
                }



                downloadfileCompleted_thread(filename, pbar, status,dlcancel);

            };
        return new DoWorkEventHandler(action);
        

        
        }


        // write a method contains load mlist and set vlcplayer media , replace all the current vlcplayer.setmedia

        Dictionary<Uri, String> MultiDownloadLinks = new Dictionary<Uri, string>();

        void downloadstatusSetup(ProgressBar pbar, Label status,Button dlCancel) {



            fm7.Size = new Size(fm7.Width, fm7.Height + pbar.Height + 20);
            pbar.Maximum = 100;
            pbar.Minimum = 0;
            pbar.Size = new Size(fm7.Width / 2 - 30, 23);
            pbar.Location = new Point(fm7.Width / 2, fm7.Height - 120 + pbar.Height);
            status.Location = new Point(0, fm7.Height - 120+5);
            status.AutoSize = true;
            dlCancel.Text = "Cancel Download";
            int width = dlCancel.Size.Width;
            dlCancel.Location = new Point(pbar.Location.X - width-20, pbar.Location.Y);
            fm7.Controls.Add(pbar);
            fm7.Controls.Add(status);
            fm7.Controls.Add(dlCancel);
            pbar.Show();
            status.Show();
            dlCancel.Show();

        }

        void downloadFile(object sender) {
            ListBox box = (ListBox)sender;
            if (box.SelectedItem != null)
            {
                using (WebClient wb = new WebClient())
                {
                    getWebVideo gb = new getWebVideo(new Uri(fm7.getLinks.SelectedItem.ToString()), current_dir_url);
                    wb.Encoding = Encoding.UTF8;
                            
                    Uri filename = (Uri)box.SelectedItem;

                    //download status related items here
                    ProgressBar pbar = new ProgressBar();
                    Label status = new Label();
                    Button dlCancel = new Button();
                    downloadstatusSetup(pbar, status,dlCancel);
                    String title = fm7.getTitle.SelectedItem.ToString();

                    wb.BaseAddress = box.SelectedItem.ToString();

                    dlCancel.Click += cancelDownload(wb) ;
                    
                    // wb.DownloadFileCompleted += new AsyncCompletedEventHandler(wb_DownloadFileCompleted);
                  //  wb.DownloadFileCompleted += DownloadFileCompleted(filename, pbar, status,dlCancel);
                    wb.DownloadFileCompleted += DownloadFileCompleted2(filename, pbar, status, dlCancel);
                    //  wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wb_DownloadProgressChanged);
                    wb.DownloadProgressChanged += DownloadProgessChange(pbar, status, title, dlCancel);
                    
                    gb.downlaodFile((Uri)box.SelectedItem, wb,title,Media_List);
                    

                    //add the used link as a key for getting the title of the current file
                    if (!MultiDownloadLinks.ContainsKey(box.SelectedItem as Uri))
                    {
                        MultiDownloadLinks.Add((Uri)box.SelectedItem, fm7.getTitle.SelectedItem.ToString());
                    }
                    else {
                        var t = MessageBox.Show("The file has already been download once, are you sure you want to download it again?", "alert", MessageBoxButtons.YesNo);
                        if (t == DialogResult.Yes) {

                            MultiDownloadLinks.Remove(box.SelectedItem as Uri);
                            MultiDownloadLinks.Add((Uri)box.SelectedItem, fm7.getTitle.SelectedItem.ToString());
         
                        }
                    }
                    //vlcSetMedia(gb.playDownlist);
                    vlc_track_time = -2;
                    //autoLoadByName(gb.playDownlist);
                    string _file2 = gb.playDownlist.OriginalString;
                    if (_file2 != null)
                    {
                        FileInfo file2 = new FileInfo(gb.playDownlist.OriginalString);

                        FileInfo[] findDM = file2.Directory.GetFiles(fm7.getTitle.SelectedItem.ToString());

                        for (int i = 0; i < findDM.Count(); i++)
                        {
                            if (findDM[i].Name.Contains("*.xml"))
                            {
                                string[] tdm = { findDM[i].FullName, findDM[i].Name };
                                DM_List.Add(tdm);
                                readXML(tdm[0]);

                                Danmoku_status.Text = "DM set";


                            }


                        }
                        if (Comment_Windows.Count == 0)
                        {
                            commentWindowSetup();
                        }
                        else
                        {
                            ///////fm3.Owner = this;


                        }





                        //set the media in vlc player
                        urls.Remove((Uri)box.SelectedItem);
                        FileInfo file = new FileInfo(gb.playDownlist.OriginalString);
                        String[] temp = { file.FullName, file.Name };
                        //Media_List.Add(temp);
                        autoLoadMlist(temp);
                        autoLoadDMlist(temp);

                        UrlDictionary[(Uri)fm7.getLinks.SelectedItem].Remove((Uri)box.SelectedItem);
                        box.Items.Remove((Uri)box.SelectedItem);





                    }
                    wb.Dispose();

                }
            }
        }
        String safeFilename(String filename) {
            String result = filename;
            result = result.Replace("\\", " ");
            result = result.Replace("?", " ");
            result = result.Replace(":", " ");
            result = result.Replace("<", " ");
            result = result.Replace(">", " ");
            result = result.Replace("\""," ");
            result = result.Replace("|", " ");
            result = result.Replace("*", " ");
            result = result.Replace("/", " ");




            return result;
        
        }
         public EventHandler cancelDownload(WebClient wb){


             Action<object, EventArgs> action = (sender, e) =>
             {
                 Button cancel = sender as Button;
                 String filename = wb.BaseAddress;
                 int start = filename.LastIndexOf("/")+1;
                 filename = filename.Substring(start, filename.Length - start);
                 filename = safeFilename(filename);
                 filename = current_dir_url + "\\" + filename;


                 FileInfo file = new FileInfo(filename);

                 if (wb != null)
                 {
                     var alert = MessageBox.Show("Are you sure you want to cancel this download?", "alert", MessageBoxButtons.YesNo);
                     if (alert == DialogResult.Yes)
                     {
                         cancel.Dispose();
                         wb.CancelAsync();

                         wb.Dispose();


                         
                         if (file.Exists)
                         {
                             try
                             {
                              //   file.Refresh();
                                 file.Delete();
                             }
                             catch (Exception a){Console.WriteLine(a.ToString()); };
                         }
                     }
                     
                 }
             };
         
         return new EventHandler (action);
         
         
         
         } 

        void autoLoadByName(FileInfo file)
        {

            srCommentWindow();
            comment2.Clear();
            comment.Clear();
            playedcomment = 0;
            CWplayedcomments(Comment_Windows, 0);
            _duplicates = 0;
            DM_List.Clear();


          //  FileInfo file = new FileInfo(media.LocalPath);

          //  throw new Exception("just to see") { };
            //search the parent dir of the file to see if there's other files has the same name
            FileInfo[] files = file.Directory.GetFiles("*.xml");

            //get all xml files in the directory
            // if the file doesn't contain a extension
            if (!file.Name.Contains(".")|| file.Name.IndexOf(".")<file.Name.Length*2/3)
            {
                //if the file has extension
                for (int i = 0; i < files.Length; i++)
                {

                    if (files[i].Name.Contains(file.Name))
                    {
                        string[] tempstring = { files[i].FullName, files[i].Name };

                        DM_List.Add(tempstring);
                        readXML(files[i].FullName);
                        Danmoku_status.Text = "DM Set";

                    }


                }

            }
            else
            {

                for (int i = 0; i < files.Length; i++)
                {
                    int end = file.Name.LastIndexOf(".");
                    string newName = file.Name.Substring(0, end);
                    if (files[i].Name.Contains(newName))
                    {
                        string[] tempstring = { files[i].FullName, files[i].Name };

                        DM_List.Add(tempstring);
                        readXML(files[i].FullName);
                        Danmoku_status.Text = "DM Set";

                    }


                }


            }


            if (Comment_Windows.Count == 0)
            {
                commentWindowSetup();
            }
            else
            {
                ///////fm3.Owner = this;

            }
        
        }

        void autoLoadByName(Uri media) {
            FileInfo file = new FileInfo(media.LocalPath);

         //   throw new Exception("just to see"){};
            //search the parent dir of the file to see if there's other files has the same name
            FileInfo[] files = file.Directory.GetFiles("*.xml");

            //get all xml files in the directory
                
            
            if (!file.Name.Contains("."))
            {
                //if the file has extension
                for (int i = 0; i < files.Length; i++) {

                    if (files[i].Name.Contains(file.Name)) {
                        string[] tempstring = { files[i].FullName, files[i].Name };

                        DM_List.Add(tempstring);
                        readXML(files[i].FullName);
                        Danmoku_status.Text = "DM Set";
                    
                    }
                
                
                }

            }
            else {

                for (int i = 0; i < files.Length; i++)
                {
                    int end = file.Name.LastIndexOf(".");
                    string newName = file.Name.Substring(0,end);
                    if (files[i].Name.Contains(newName))
                    {
                        string[] tempstring = { files[i].FullName, files[i].Name };

                        DM_List.Add(tempstring);
                        readXML(files[i].FullName);
                        Danmoku_status.Text = "DM Set";

                    }


                }

            
            }


            if (Comment_Windows.Count == 0)
            {
                commentWindowSetup();
            }
            else
            {
                ///////fm3.Owner = this;

            }
        
        
        }
        /*
        void wb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            //show dl progress
            String currentbyte = e.BytesReceived.ToString();
            String totalbyte = e.TotalBytesToReceive.ToString();
            int progress = e.ProgressPercentage;
            if (fm7 != null) {

                fm7.getProgressbar.Maximum = 100;
                fm7.getProgressbar.Minimum = 0;
                fm7.getProgressbar.Value = progress;
                fm7.getDownloadstatus1.Text = currentbyte + "/" + totalbyte;
            
            }
            downlaod_status.Text ="downloading"+gb.getCurrentTitle+" "+ currentbyte + "/" + totalbyte +"  "+progress +"% completed";

            



        }
         */
        bool _cursor = false;
        //this is so that the same method won't be called twice in a row since it's counted
        public bool hideCurosr{
            get { return _cursor; }
            set { if (_cursor != value) {

                if (value == true)
                {
                    Cursor.Hide();
                }
                else {
                    Cursor.Show();
                }
                
                
                _cursor = value;
                  } 
            }
    
    
        }
        delegate void downlaod_complete_thread(Uri filename, ProgressBar pbar, Label status,Button dlcancel);

        void downloadfileCompleted_thread(Uri ufilename, ProgressBar pbar, Label status,Button dlcancel){

            if (fm7 != null) {

                if (fm7.InvokeRequired)
                {

                    downlaod_complete_thread dl = new downlaod_complete_thread(downloadfileCompleted_thread);


                    fm7.Invoke(dl, new object[] { ufilename, pbar, status,dlcancel });




                }
                else {

                    wb_DownloadFileCompleted(ufilename, pbar, status,dlcancel);
                
                }
            
            
            
            
            
            }
        
        
        
        
        }
        void wb_DownloadFileCompleted(Uri ufilename,ProgressBar pbar, Label status,Button dlcancel)
        {

            if (fm7 != null)
            {

                //fm7.Size = new Size(fm7.Width, fm7.Height - pbar.Height);
                pbar.Dispose();
                status.Dispose();
                dlcancel.Dispose();
                //only changes the size back to normal if there's no task is downloading
                if (fm7.Controls.OfType<ProgressBar>().Count() == 0)
                {
                    fm7.Size = new Size(fm7.Width, 475);
                }
            }

            //throw new NotImplementedException();
            //remove urls from url list
            //change dled file name
            //update name in gb dlfile 

            String dllink = ufilename.LocalPath;



            string title = MultiDownloadLinks[ufilename];

            int start = dllink.LastIndexOf("/") + 1;
            int end = dllink.Length;

            String filename = dllink.Substring(start, end - start);
            filename = safeFilename(filename);

            FileInfo file = new FileInfo(current_dir_url + "\\" + filename);
            string[] tempstring = { file.FullName, file.Name };
            Media_List.Remove(tempstring);
            if (file.Exists && file.Length > 0)
            {
                if (fm7 != null)
                {

                    fm7.getProgressbar.Value = 0;
                    fm7.getDownloadstatus1.Text = "downlaod completed, renaming file now";

                }
                downlaod_status.Text = "download completed, Renaming file now";

                title = safeFilename(title);

                //パス 'A:\video\hima test video\F
                FileInfo temp = new FileInfo(file.Directory + "\\" + title);


                if (!temp.Exists || file.Length > temp.Length)
                {
                    file.CopyTo(file.Directory + "\\" + title, true);
                }
                else
                {

                    file.CopyTo(file.Directory + "\\" + title + "(1)", false);
                }

             //   gb.dlnameUpdate = temp.FullName;
                tempstring = new String[] { file.FullName, file.Name };
        
                if (!Media_List.Contains(tempstring))
                {
                    Media_List.Add(tempstring);
                }
                if (vlcPlayer.IsPlaying)
                {


                    String[] m = {temp.FullName,temp.Name };
                    if (!Media_List.Contains(m))
                    {
                        Media_List.Add(m);
                    }

                }
                else
                {


                    String[] m = { temp.FullName, temp.Name };
                    if (!Media_List.Contains(m))
                    {
                        Media_List.Add(m);
                    }
                  //  vlcSetMedia(temp);
                  //  autoLoadByName(temp);
                    //           vlcPlayer.Play();

                }
                file.Delete();

            }
            else
            {
                if (file.Exists)
                {
                    file.Delete();
                }

            }
            if (fm7 != null)
            {
                fm7.getDownloadstatus1.Text = "";
            }
            downlaod_status.Text = "";

            if (fm7.getDownloadstatus2.Text == "server error" && !file.Exists) {

                fm7.getDownloadstatus2.Text = "Downlaod Cancelled";
            
            };
              
             

        }

        void wb_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            //remove urls from url list
            //change dled file name
            //update name in gb dlfile 

            string dllink = "";

            
            WebClient wb = (WebClient)sender;
            
                
            
                dllink=wb.BaseAddress;

            

            string title = MultiDownloadLinks[new Uri(dllink)];
            
            int start = dllink.LastIndexOf("\\")+1;
            int end = dllink.Length;

            String filename = dllink.Substring(start, end - start);

            FileInfo file = new FileInfo(current_dir_url+"\\"+filename);
            string[] tempstring = { file.FullName, file.Name };
            Media_List.Remove(tempstring);
            if (file.Length > 0)
            {
                if (fm7 != null)
                {

                    fm7.getProgressbar.Value = 0;
                    fm7.getDownloadstatus1.Text = "downlaod completed, renaming file now";

                }
                downlaod_status.Text = "download completed, Renaming file now";

                title = title.Replace("\\", " ");
                title = title.Replace("/", " ");


                //パス 'A:\video\hima test video\Fate/kaleid liner プリズマ☆イリヤ ドライ！ 第01話 吸収さん高画質「銀色に沈む街」 - ひまわり動画' の一部が見つかりませんでした。
                FileInfo temp = new FileInfo(file.Directory + "\\" + title);
               
                
                if (!temp.Exists || file.Length > temp.Length)
                {
                    file.CopyTo(file.Directory + "\\" + title, true);
                }
                else {

                    file.CopyTo(file.Directory + "\\" + title + "(1)", false);
                }

          //      gb.dlnameUpdate = temp.FullName;
                tempstring= new String[]{file.FullName,file.Name};
                if (!Media_List.Contains(tempstring))
                {
                    Media_List.Add(tempstring);
                }
                if (vlcPlayer.IsPlaying)
                {
                    long temptime = vlcPlayer.Time;
                 //   vlcPlayer.Stop();

                    vlcSetMedia(temp);
                    vlcPlayer.Play();
                    timerStart();
                    vlcPlayer.Time = temptime;

                }
                else {

                    FileInfo[] findDM = file.Directory.GetFiles(temp.Name);

                    for (int i = 0; i < findDM.Count(); i++)
                    {
                        if (findDM[i].Name.Contains("*.xml"))
                        {
                            string[] tdm = { findDM[i].FullName, findDM[i].Name };
                            DM_List.Add(tdm);
                            readXML(tdm[0]);

                            Danmoku_status.Text = "DM set";

                            if (Comment_Windows.Count==0)
                            {
                                commentWindowSetup();
                            }
                            else
                            {
                                ///////fm3.Owner = this;

                            }


                        }

                    }
                    timerStop();

                    vlcSetMedia(temp);

                    vlcPlayer.Play();
                    timerStart();
         //           vlcPlayer.Play();
                
                }
                file.Delete();

            }
            else {

                file.Delete();
            
            }
            if (fm7 != null) {
                fm7.getDownloadstatus1.Text = "";
            }
            downlaod_status.Text = "";

           

        }


        void load_Url_from(Uri address) {

            if (!Links.Contains(address))
            {


                    
                

                fm7.getFileUrl.Items.Clear();
                getWebVideo gb = new getWebVideo(address, current_dir_url);
                Links.Add(address);
                // :/\?<>*|
                gb.getCurrentTitle = safeFilename(gb.getCurrentTitle);

                if (!urlTitles.Contains(gb.getCurrentTitle))
                {
                    urlTitles.Add(gb.getCurrentTitle);
                }
                else {
                    //the reason for doing this is to regulate the order in the list, with out this step all the titles in the list will mess up
                    urlTitles.Remove(gb.getCurrentTitle);
                    urlTitles.Add(gb.getCurrentTitle);
                
                }
                urls = gb.getAllfiles;
                if (UrlDictionary.ContainsKey(address)) {
                    //instaed of doing this check if there's dulplicate in links if not then add it

    
                        for (int j = 0; j < gb.getAllfiles.Count; j++) {

                        if(!UrlDictionary[address].Contains(gb.getAllfiles.ElementAt(j))){
                            
                            UrlDictionary[address].Add(gb.getAllfiles.ElementAt(j));
                        }


                        }
    
                    /*
                    if (gb.getAllfiles.Count > 0)
                    {
                        //if the updated page contains at least 1 links then update otherwise do nothing

                        UrlDictionary.Remove(address);
                        UrlDictionary.Add(address, gb.getAllfiles);


                    }
                    else { 
                    // do nothing if the new updated links are all dead or removed 
                        //maybe can change this part to  add new links only ?
                    
                    
                    }
                */
                
                }
                else
                {
                    UrlDictionary.Add(address, gb.getAllfiles);
                }

                //load all the info onto the box
                fm7.getLinks.Items.Add(address);
                fm7.getTitle.Items.Add(gb.getCurrentTitle);
                //load url itembox with all the links


         //       BackgroundWorker bk = new BackgroundWorker();
         //       bk.WorkerSupportsCancellation = true;

             //  bk.DoWork += browserdowork(address);
              //  bk.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bk_RunWorkerCompleted);
                //only do this if it's on himado.in since this only works on there

           //     if (address.OriginalString.Contains("himado.in"))
           //     {
                    
                 //   if (!bk.IsBusy)
                 //   {
                 //       bk.RunWorkerAsync();
                //    }
                      
                   // runbrowser(address);
                }


            }
        
        

        void bk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            renamed = false;

            BackgroundWorker bk = sender as BackgroundWorker;
            bk.Dispose();
        }
        delegate void browser_thread(Uri address);
        void runbrowser_thread(Uri address){

            if (fm7.InvokeRequired)
            {

                browser_thread bt = new browser_thread(runbrowser_thread);

                fm7.Invoke(bt, new object[] { address });




            }
            else {

                runbrowser(address);
            }
    
    
    
        }


        void runbrowser(Uri address) {
          //  gb.getCommentUri(address);
            /*   WebBrowser wb = new WebBrowser();

            //  wb.TopLevelControl.Visible = false;
            wb.Visible = false;
            //wb.Document.Encoding = "UTF-8";
            wb.ScriptErrorsSuppressed = true;
              // printvlc(wb.Version.ToString());

            wb.Url = address;
          
            // wb.Navigated +=new WebBrowserNavigatedEventHandler(wb_Navigated);

            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);


            for (int i = 0; i < urls.Count; i++)
            {
                fm7.getFileUrl.Items.Add(urls.ElementAt(i));


            }

            fm7.getLinks.SelectedIndex = fm7.getLinks.Items.Count - 1;
            fm7.getTitle.SelectedIndex = fm7.getTitle.Items.Count - 1;
        
          */ 
        }


        public DoWorkEventHandler browserdowork(Uri address) { 
        
        
        Action<object,DoWorkEventArgs> action =(sender,e) =>{
        BackgroundWorker bk = sender as BackgroundWorker;

            if(e.Cancel){
            
            bk.CancelAsync();
            
            
            }



            runbrowser_thread(address);
         
        };
        
      
        
         return  new DoWorkEventHandler (action);
        
        }
        /*
        void bk_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();


        }
         * */

        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {


            renamed = false;

            fm7.getDownloadstatus2.Text = "document loaded";
            
            


            WebBrowser wb = (WebBrowser)sender;

            fm7.getDownloadstatus2.Text = wb.Document.DefaultEncoding;
            wb.Document.Encoding =Encoding.Unicode.WebName;

            //wb.Document.Encoding = "UTF-8";

       //     List<String> viewer = new List<string>();
      //      List<String> viewer2 = new List<string>();
            if (wb.Document.GetElementsByTagName("select").GetElementsByName("limit").Count>0)
            {
            for (int i = 0; i < wb.Document.GetElementsByTagName("select").GetElementsByName("limit")[0].GetElementsByTagName("option").Count; i++) {

               
              //  viewer.Add(wb.Document.GetElementsByTagName("select").GetElementsByName("limit")[0].GetElementsByTagName("option")[i].GetAttribute("value"));

                wb.Document.GetElementsByTagName("select").GetElementsByName("limit")[0].GetElementsByTagName("option")[i].SetAttribute("value","200000");
             //   viewer2.Add(wb.Document.GetElementsByTagName("select").GetElementsByName("limit")[0].GetElementsByTagName("option")[i].GetAttribute("value"));

            
            }
           // throw new Exception("view value");





                for (int i = 0; i < wb.Document.GetElementsByTagName("input").Count; i++)
                {



                    if (wb.Document.GetElementsByTagName("input")[i].GetAttribute("value").Equals("ダウンロード"))
                    {
                        wb.Document.GetElementsByTagName("input")[i].InvokeMember("click");

                        wb.Dispose();
                        break;

                    }
                }
            }
            wb.Dispose();
           
        }

        void wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {

            fm7.getDownloadstatus2.Text = "document loaded";

            WebBrowser wb = (WebBrowser)sender;
            for (int i = 0; i < wb.Document.GetElementsByTagName("input").Count; i++)
            {
                
             

                if (wb.Document.GetElementsByTagName("input")[i].GetAttribute("type").Equals("submit"))
                {
                    wb.Document.GetElementsByTagName("input")[i].InvokeMember("click");

                    
                }
            }

 
            wb.Dispose();
            
        }
        void getloadURLbutton_Click(object sender, EventArgs e)
        {

           // <input type="submit" value="ダウンロード">
            //throw new NotImplementedException();
            //when the set url button is clicked
            /*
            Uri cb;
            try
            {
                 cb = new Uri(fm7.setLinkbox.Text);
            }
            catch (Exception e){Console.WriteLine(e.ToString());
                 cb = null;
            }
            if (cb != null)
            {
                load_Url_from(cb);
            }
             * */
            reload_click();

        }

        void getLinks_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //basiclly the same as get title with a few lines chagned


            //clear the uri box
            fm7.getFileUrl.Items.Clear();
            ListBox temp = (ListBox)sender;

            int selected = temp.SelectedIndex;
            fm7.getTitle.SelectedIndex = selected;
            setTextasTitle();

            //if the link has been read before add if not in else area read the links and load it on dictionary
            if (selected >= 0 && selected < temp.Items.Count)
            {
                if (UrlDictionary.ContainsKey(Links.ElementAt(selected)))
                {
                    for (int i = 0; i < UrlDictionary[Links.ElementAt(selected)].Count(); i++)
                    {

                        fm7.getFileUrl.Items.Add(UrlDictionary[Links.ElementAt(selected)].ElementAt(i));

                    }

                }
                else
                {
                    // not doing anything here for now since it might conflict with later code
                    // since everything in the links are supposed to be loaded and add into dictionary
                    // so here is just to prevent the unexcepted error


                }


            }
        }
        void setTextasTitle() {


            if (fm7 != null) {

                if (fm7.getLinks.SelectedItem != null)
                {
                    String title = fm7.getLinks.SelectedItem.ToString();
                    fm7.setLinkbox.Text = title;
                }
            
            }
        
        
        }
        void getTitle_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            // when the name of the title is clicked

            //clear the uri box
            fm7.getFileUrl.Items.Clear();

            ListBox temp = (ListBox)sender;

            int selected = temp.SelectedIndex;
            fm7.getLinks.SelectedIndex = selected;
            setTextasTitle();

            //if the link has been read before add if not in else area read the links and load it on dictionary
            if (selected >= 0)
            {
                if (UrlDictionary.ContainsKey(Links.ElementAt(selected)))
                {
                    for (int i = 0; i < UrlDictionary[Links.ElementAt(selected)].Count(); i++)
                    {

                        fm7.getFileUrl.Items.Add(UrlDictionary[Links.ElementAt(selected)].ElementAt(i));

                    }

                }
                else
                {
                    // not doing anything here for now since it might conflict with later code
                    // since everything in the links are supposed to be loaded and add into dictionary
                    // so here is just to prevent the unexcepted error


                }

            }


        }

        void fm6_Disposed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //when fm6 gets disposed;
            fm6.Close();
            if (current_dir_url == null || linkaddress == null)
            {

                //do nothing if one fo the two is not set


                // may modiefied  current_dir_url if you want to leave the directory out for online streaming instead of dled to local then play but for now it's all dled local
            }
            else { 
            
            //opens out the url menu that displays the list of links to dl or maybe past urls ?
            //loads content of the url and then save the url as key the list of links in content as values
              
                
                URL_menuLoadup();
            
            
            }
            fm6 = null;


        }

        void setConfirm_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if confirm button is clicked
            if (fm6.getLink != null)
            {
                linkaddress = fm6.getLink;
            }
            if (fm6.getDri != null)
            {
                current_dir_url = fm6.getDri;
            }

            fm6.Dispose();
        }

        void setCacnel_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //when cancel buttun is click on the add link form

            fm6.Dispose();

        }

        private void fullscreen_button_Click(object sender, EventArgs e)
        {
            FullScreenSwitch();          
        }


        //write to XML file to save user changes to settings?



    }

    
}
