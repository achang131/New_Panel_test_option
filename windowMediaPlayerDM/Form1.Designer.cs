namespace windowMediaPlayerDM
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {

            try
            {
                ChangeClipboardChain(this.Handle, nextClipboardViewer);
            }
            catch (System.ObjectDisposedException) { 
            
            }
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
    
            base.Dispose(disposing);

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openMeidaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setDMsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Media_DM_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFromURL_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Media_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.Danmoku_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.test_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.debugtab = new System.Windows.Forms.ToolStripStatusLabel();
            this.debugtab2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Video_comment = new System.Windows.Forms.ToolStripStatusLabel();
            this.vlc_display = new System.Windows.Forms.ToolStripStatusLabel();
            this.downlaod_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.download_status2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.clipboard_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.VLC_track = new System.Windows.Forms.TrackBar();
            this.next_track = new System.Windows.Forms.Button();
            this.last_track = new System.Windows.Forms.Button();
            this.loop_button = new System.Windows.Forms.Button();
            this.vlcSound_button = new System.Windows.Forms.Button();
            this.vlcStop_button = new System.Windows.Forms.Button();
            this.vlcPlay_button = new System.Windows.Forms.Button();
            this.sound_trackbar = new System.Windows.Forms.TrackBar();
            this.fullscreen_button = new System.Windows.Forms.Button();
            this._VLCMediaPlayer = new System.Windows.Forms.Panel();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VLC_track)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sound_trackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMeidaToolStripMenuItem,
            this.Media_DM_menu,
            this.Settings_menu,
            this.testToolStripMenuItem,
            this.setFromURL_menu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(996, 26);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openMeidaToolStripMenuItem
            // 
            this.openMeidaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setDMToolStripMenuItem,
            this.setDMToolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.openMeidaToolStripMenuItem.Name = "openMeidaToolStripMenuItem";
            this.openMeidaToolStripMenuItem.Size = new System.Drawing.Size(50, 22);
            this.openMeidaToolStripMenuItem.Text = "Open";
            this.openMeidaToolStripMenuItem.Click += new System.EventHandler(this.openMeidaToolStripMenuItem_Click);
            // 
            // setDMToolStripMenuItem
            // 
            this.setDMToolStripMenuItem.Name = "setDMToolStripMenuItem";
            this.setDMToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.setDMToolStripMenuItem.Text = "Set Meida";
            this.setDMToolStripMenuItem.Click += new System.EventHandler(this.setDMToolStripMenuItem_Click);
            // 
            // setDMToolStripMenuItem1
            // 
            this.setDMToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setDMsToolStripMenuItem});
            this.setDMToolStripMenuItem1.Name = "setDMToolStripMenuItem1";
            this.setDMToolStripMenuItem1.Size = new System.Drawing.Size(134, 22);
            this.setDMToolStripMenuItem1.Text = "Set DM";
            this.setDMToolStripMenuItem1.Click += new System.EventHandler(this.setDMToolStripMenuItem1_Click);
            // 
            // setDMsToolStripMenuItem
            // 
            this.setDMsToolStripMenuItem.Name = "setDMsToolStripMenuItem";
            this.setDMsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.setDMsToolStripMenuItem.Text = "Set DMs";
            this.setDMsToolStripMenuItem.Click += new System.EventHandler(this.setDMsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Media_DM_menu
            // 
            this.Media_DM_menu.Name = "Media_DM_menu";
            this.Media_DM_menu.Size = new System.Drawing.Size(104, 22);
            this.Media_DM_menu.Text = "Media/DM List";
            this.Media_DM_menu.Click += new System.EventHandler(this.Media_DM_menu_Click);
            // 
            // Settings_menu
            // 
            this.Settings_menu.Name = "Settings_menu";
            this.Settings_menu.Size = new System.Drawing.Size(68, 22);
            this.Settings_menu.Text = "Settings";
            this.Settings_menu.Click += new System.EventHandler(this.Settings_menu_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(43, 22);
            this.testToolStripMenuItem.Text = "test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // setFromURL_menu
            // 
            this.setFromURL_menu.Name = "setFromURL_menu";
            this.setFromURL_menu.Size = new System.Drawing.Size(100, 22);
            this.setFromURL_menu.Text = "Set from URL";
            this.setFromURL_menu.Click += new System.EventHandler(this.setFromURL_menu_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Media_status,
            this.Danmoku_status,
            this.test_label,
            this.debugtab,
            this.debugtab2,
            this.Video_comment,
            this.vlc_display,
            this.downlaod_status,
            this.download_status2,
            this.clipboard_label});
            this.statusStrip1.Location = new System.Drawing.Point(0, 559);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1012, 23);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Media_status
            // 
            this.Media_status.Name = "Media_status";
            this.Media_status.Size = new System.Drawing.Size(62, 18);
            this.Media_status.Text = "No Media";
            // 
            // Danmoku_status
            // 
            this.Danmoku_status.Name = "Danmoku_status";
            this.Danmoku_status.Size = new System.Drawing.Size(47, 18);
            this.Danmoku_status.Text = "No DM";
            // 
            // test_label
            // 
            this.test_label.Name = "test_label";
            this.test_label.Size = new System.Drawing.Size(0, 18);
            // 
            // debugtab
            // 
            this.debugtab.Name = "debugtab";
            this.debugtab.Size = new System.Drawing.Size(0, 18);
            // 
            // debugtab2
            // 
            this.debugtab2.Name = "debugtab2";
            this.debugtab2.Size = new System.Drawing.Size(0, 18);
            // 
            // Video_comment
            // 
            this.Video_comment.Name = "Video_comment";
            this.Video_comment.Size = new System.Drawing.Size(0, 18);
            // 
            // vlc_display
            // 
            this.vlc_display.Name = "vlc_display";
            this.vlc_display.Size = new System.Drawing.Size(0, 18);
            // 
            // downlaod_status
            // 
            this.downlaod_status.Name = "downlaod_status";
            this.downlaod_status.Size = new System.Drawing.Size(0, 18);
            // 
            // download_status2
            // 
            this.download_status2.Name = "download_status2";
            this.download_status2.Size = new System.Drawing.Size(0, 18);
            // 
            // clipboard_label
            // 
            this.clipboard_label.Name = "clipboard_label";
            this.clipboard_label.Size = new System.Drawing.Size(0, 18);
            // 
            // VLC_track
            // 
            this.VLC_track.Cursor = System.Windows.Forms.Cursors.Default;
            this.VLC_track.LargeChange = 3000;
            this.VLC_track.Location = new System.Drawing.Point(0, 513);
            this.VLC_track.Name = "VLC_track";
            this.VLC_track.Size = new System.Drawing.Size(824, 45);
            this.VLC_track.SmallChange = 1000;
            this.VLC_track.TabIndex = 1;
            this.VLC_track.TabStop = false;
            this.VLC_track.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // next_track
            // 
            this.next_track.BackgroundImage = global::windowMediaPlayerDM.Properties.Resources.next;
            this.next_track.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.next_track.FlatAppearance.BorderSize = 0;
            this.next_track.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.next_track.Location = new System.Drawing.Point(194, 540);
            this.next_track.Name = "next_track";
            this.next_track.Size = new System.Drawing.Size(30, 19);
            this.next_track.TabIndex = 7;
            this.next_track.UseVisualStyleBackColor = true;
            this.next_track.Click += new System.EventHandler(this.next_track_Click);
            // 
            // last_track
            // 
            this.last_track.BackgroundImage = global::windowMediaPlayerDM.Properties.Resources.rewand;
            this.last_track.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.last_track.FlatAppearance.BorderSize = 0;
            this.last_track.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.last_track.Location = new System.Drawing.Point(145, 540);
            this.last_track.Name = "last_track";
            this.last_track.Size = new System.Drawing.Size(30, 19);
            this.last_track.TabIndex = 7;
            this.last_track.UseVisualStyleBackColor = true;
            this.last_track.Click += new System.EventHandler(this.last_track_Click);
            // 
            // loop_button
            // 
            this.loop_button.BackgroundImage = global::windowMediaPlayerDM.Properties.Resources.repeat;
            this.loop_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.loop_button.FlatAppearance.BorderSize = 0;
            this.loop_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.loop_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loop_button.Location = new System.Drawing.Point(794, 540);
            this.loop_button.Name = "loop_button";
            this.loop_button.Size = new System.Drawing.Size(30, 19);
            this.loop_button.TabIndex = 6;
            this.loop_button.UseVisualStyleBackColor = true;
            this.loop_button.Click += new System.EventHandler(this.loop_button_Click);
            // 
            // vlcSound_button
            // 
            this.vlcSound_button.BackgroundImage = global::windowMediaPlayerDM.Properties.Resources.sound;
            this.vlcSound_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vlcSound_button.FlatAppearance.BorderSize = 0;
            this.vlcSound_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vlcSound_button.Location = new System.Drawing.Point(830, 540);
            this.vlcSound_button.Name = "vlcSound_button";
            this.vlcSound_button.Size = new System.Drawing.Size(30, 19);
            this.vlcSound_button.TabIndex = 5;
            this.vlcSound_button.UseVisualStyleBackColor = true;
            this.vlcSound_button.Click += new System.EventHandler(this.vlcSound_button_Click);
            // 
            // vlcStop_button
            // 
            this.vlcStop_button.BackgroundImage = global::windowMediaPlayerDM.Properties.Resources.stop;
            this.vlcStop_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vlcStop_button.FlatAppearance.BorderSize = 0;
            this.vlcStop_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vlcStop_button.Location = new System.Drawing.Point(77, 540);
            this.vlcStop_button.Name = "vlcStop_button";
            this.vlcStop_button.Size = new System.Drawing.Size(30, 19);
            this.vlcStop_button.TabIndex = 5;
            this.vlcStop_button.UseVisualStyleBackColor = true;
            this.vlcStop_button.Click += new System.EventHandler(this.vlcStop_button_Click);
            // 
            // vlcPlay_button
            // 
            this.vlcPlay_button.BackgroundImage = global::windowMediaPlayerDM.Properties.Resources.start;
            this.vlcPlay_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vlcPlay_button.FlatAppearance.BorderSize = 0;
            this.vlcPlay_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vlcPlay_button.Location = new System.Drawing.Point(32, 540);
            this.vlcPlay_button.Name = "vlcPlay_button";
            this.vlcPlay_button.Size = new System.Drawing.Size(30, 19);
            this.vlcPlay_button.TabIndex = 5;
            this.vlcPlay_button.UseVisualStyleBackColor = true;
            this.vlcPlay_button.Click += new System.EventHandler(this.vlcPlay_button_Click);
            // 
            // sound_trackbar
            // 
            this.sound_trackbar.Location = new System.Drawing.Point(825, 513);
            this.sound_trackbar.Name = "sound_trackbar";
            this.sound_trackbar.Size = new System.Drawing.Size(159, 45);
            this.sound_trackbar.TabIndex = 8;
            this.sound_trackbar.TabStop = false;
            this.sound_trackbar.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // fullscreen_button
            // 
            this.fullscreen_button.BackgroundImage = global::windowMediaPlayerDM.Properties.Resources.fullscreen;
            this.fullscreen_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.fullscreen_button.Location = new System.Drawing.Point(758, 540);
            this.fullscreen_button.Name = "fullscreen_button";
            this.fullscreen_button.Size = new System.Drawing.Size(30, 19);
            this.fullscreen_button.TabIndex = 9;
            this.fullscreen_button.UseVisualStyleBackColor = true;
            this.fullscreen_button.Click += new System.EventHandler(this.fullscreen_button_Click);
            // 
            // _VLCMediaPlayer
            // 
            this._VLCMediaPlayer.Location = new System.Drawing.Point(0, 30);
            this._VLCMediaPlayer.Name = "_VLCMediaPlayer";
            this._VLCMediaPlayer.Size = new System.Drawing.Size(996, 477);
            this._VLCMediaPlayer.TabIndex = 10;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(996, 582);
            this.Controls.Add(this._VLCMediaPlayer);
            this.Controls.Add(this.fullscreen_button);
            this.Controls.Add(this.vlcSound_button);
            this.Controls.Add(this.sound_trackbar);
            this.Controls.Add(this.next_track);
            this.Controls.Add(this.last_track);
            this.Controls.Add(this.loop_button);
            this.Controls.Add(this.vlcStop_button);
            this.Controls.Add(this.vlcPlay_button);
            this.Controls.Add(this.VLC_track);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "DM Player";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VLC_track)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sound_trackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openMeidaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDMToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel Media_status;
        private System.Windows.Forms.ToolStripStatusLabel Danmoku_status;
        private System.Windows.Forms.ToolStripStatusLabel test_label;
        private System.Windows.Forms.ToolStripStatusLabel debugtab;
        private System.Windows.Forms.ToolStripStatusLabel debugtab2;
        private System.Windows.Forms.ToolStripMenuItem Media_DM_menu;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem setDMsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel Video_comment;
        private System.Windows.Forms.ToolStripMenuItem Settings_menu;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.TrackBar VLC_track;
        private System.Windows.Forms.Button vlcPlay_button;
        private System.Windows.Forms.Button vlcStop_button;
        private System.Windows.Forms.Button vlcSound_button;
        private System.Windows.Forms.ToolStripStatusLabel vlc_display;
        private System.Windows.Forms.Button loop_button;
        private System.Windows.Forms.Button last_track;
        private System.Windows.Forms.Button next_track;
        private System.Windows.Forms.ToolStripMenuItem setFromURL_menu;
        private System.Windows.Forms.ToolStripStatusLabel downlaod_status;
        private System.Windows.Forms.ToolStripStatusLabel download_status2;
        private System.Windows.Forms.TrackBar sound_trackbar;
        private System.Windows.Forms.ToolStripStatusLabel clipboard_label;
        private System.Windows.Forms.Button fullscreen_button;
        private System.Windows.Forms.Panel _VLCMediaPlayer;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
    }
}

