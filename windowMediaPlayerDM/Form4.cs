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
    public partial class Settings : Form
    {
        int comment_speed;
        bool auto_match, comment_switch;
        int time_offset;
        int auto_offset;
        int endpoint;

        

        public Settings()
        {
            InitializeComponent();


            
            
        }

        public void formSetup(){
        
        
        
        
        }
        public Button getconfirm {

            set { this.confirm = value; }

            get { return this.confirm; }
        
        }
        public Button getcancel
        {

            set { this.Cancel = value; }

            get { return this.Cancel; }

        }
        public Button getapply
        {

            set { this.apply_button = value; }

            get { return this.apply_button; }

        }
        public Button getdefault
        {

            set { this.default_button = value; }

            get { return this.default_button; }

        }
        public ComboBox select_commentswitch {

            set { this.comment_switch_box = value; }
            get { return this.comment_switch_box; }
        
        
        }

        public Button getAudio_up {

            set { this.audio_up = value; }

            get { return this.audio_up; }
        
        }
        public Button getAudio_down
        {

            set { this.audio_down = value; }

            get { return this.audio_down; }

        }
        public CheckBox debug_mode {
            get { return this.Debug_mode; }
            set { this.Debug_mode = value; }
        
        }
        public TextBox setFontSize { get { return font_size_box;} set {font_size_box=value ;} }
        public Label getCurrentAudio {

            set { this.current_audio = value; }

            get { return this.current_audio; }
        
        
        }
        public Label getTotalAudio
        {

            set { this.total_audio = value; }

            get { return this.total_audio; }


        }
        public Label getAudioInfo
        {

            set { this.audio_info = value; }

            get { return this.audio_info; }


        }
        public CheckBox auto_mode_check {

            set { this.offset_check = value; }
            get { return this.offset_check; }
        
        
        }

        public TextBox timeoffset {

            set { //this.time_offset =Int32.Parse(value.Text);
            this.offset_box = value;
            }
            get { return this.offset_box; }
        }
        public TextBox Cspeed {

            set { this.comment_speed_box = value; }

            get { return this.comment_speed_box; }
        
        
        }
        public TrackBar setBrightness {
            set { this.Brightness_bar = value; }
            get { return this.Brightness_bar; }
        
        }
        public TrackBar setContrast
        {
            set { this.Contrast_bar = value; }
            get { return this.Contrast_bar; }

        }
        public TrackBar setGamma
        {
            set { this.Gamma_bar = value; }
            get { return this.Gamma_bar; }

        }
        public TrackBar setHue
        {
            set { this.Hue_bar = value; }
            get { return this.Hue_bar; }

        }
        public Button setSubtitleUP {
            set { this.subtitle_button_u = value; }
            get{return this.subtitle_button_u;}
        
        }
        public Button setSubtitleDown
        {
            set { this.subtitle_button_d = value; }
            get { return this.subtitle_button_d; }

        }

        public Label currentSubtitleText {
            set { this.currentSubtitle_text = value; }
            get { return this.currentSubtitle_text; }
        
        
        }
        public Label totalSubtitleText {
            set { this.totalSubtitle_text = value; }
            get { return this.totalSubtitle_text; }
        
        }
        public Label SubtitleText
        {
            set { this.Subtitle_text = value; }
            get { return this.Subtitle_text; }

        }
        public TrackBar setSaturation
        {
            set { this.Saturation_bar = value; }
            get { return this.Saturation_bar; }

        }
        public CheckBox clipboard {
            get { return Clicpboard_check; }
            set { Clicpboard_check = value; }
        
        }
        public TextBox Cend
        {

            set { this.comment_end_box = value; }

            get { return this.comment_end_box; }


        }
        public TextBox setCommentLimit {

            get { return this.commentLimit_box; }
            set { this.commentLimit_box = value; }
        
        }


        /// /////////////////

        private void confirm_Click(object sender, EventArgs e)
        {
            // save all changes here


       
        }

        private void Cancel_Click(object sender, EventArgs e)
        {


    
        }

        private void apply_button_Click(object sender, EventArgs e)
        {

        }

        private void default_button_Click(object sender, EventArgs e)
        {
            //reload data from fm1?  apply button click in fm1 for reloading data?
        }
    }
}
/*
            speed_control = 1;

            time_offset = 0;

            move_distance = 3;

            timer1.Interval = 1;

            commentdestroy = -200;

            sommentswitch = true;

            threading_mode = false;

            vpos = -1;

            userColor = Color.DarkGray;

            replacetimer1_interval = 5;

            auto_TimeMatch = true;

            offset_auto = -1400;
*/