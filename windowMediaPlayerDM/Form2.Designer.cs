namespace windowMediaPlayerDM
{
    partial class Form2
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
            this.tab_list = new System.Windows.Forms.TabControl();
            this.Media_List = new System.Windows.Forms.TabPage();
            this.Media_List_Box = new System.Windows.Forms.ListBox();
            this.DM_List = new System.Windows.Forms.TabPage();
            this.DM_List_Box = new System.Windows.Forms.ListBox();
            this.Full_DM_box = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tab_list.SuspendLayout();
            this.Media_List.SuspendLayout();
            this.DM_List.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_list
            // 
            this.tab_list.Controls.Add(this.Media_List);
            this.tab_list.Controls.Add(this.DM_List);
            this.tab_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_list.Location = new System.Drawing.Point(0, 0);
            this.tab_list.Name = "tab_list";
            this.tab_list.SelectedIndex = 0;
            this.tab_list.Size = new System.Drawing.Size(543, 434);
            this.tab_list.TabIndex = 0;
            // 
            // Media_List
            // 
            this.Media_List.Controls.Add(this.Media_List_Box);
            this.Media_List.Location = new System.Drawing.Point(4, 22);
            this.Media_List.Name = "Media_List";
            this.Media_List.Padding = new System.Windows.Forms.Padding(3);
            this.Media_List.Size = new System.Drawing.Size(535, 408);
            this.Media_List.TabIndex = 0;
            this.Media_List.Text = "Media List";
            this.Media_List.UseVisualStyleBackColor = true;
            // 
            // Media_List_Box
            // 
            this.Media_List_Box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Media_List_Box.FormattingEnabled = true;
            this.Media_List_Box.Location = new System.Drawing.Point(3, 3);
            this.Media_List_Box.Name = "Media_List_Box";
            this.Media_List_Box.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Media_List_Box.Size = new System.Drawing.Size(529, 402);
            this.Media_List_Box.TabIndex = 0;
            // 
            // DM_List
            // 
            this.DM_List.Controls.Add(this.label2);
            this.DM_List.Controls.Add(this.label1);
            this.DM_List.Controls.Add(this.Full_DM_box);
            this.DM_List.Controls.Add(this.DM_List_Box);
            this.DM_List.Location = new System.Drawing.Point(4, 22);
            this.DM_List.Name = "DM_List";
            this.DM_List.Padding = new System.Windows.Forms.Padding(3);
            this.DM_List.Size = new System.Drawing.Size(535, 408);
            this.DM_List.TabIndex = 1;
            this.DM_List.Text = "Loaded DM_List";
            this.DM_List.UseVisualStyleBackColor = true;
            // 
            // DM_List_Box
            // 
            this.DM_List_Box.FormattingEnabled = true;
            this.DM_List_Box.Location = new System.Drawing.Point(0, 34);
            this.DM_List_Box.Name = "DM_List_Box";
            this.DM_List_Box.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.DM_List_Box.Size = new System.Drawing.Size(255, 368);
            this.DM_List_Box.TabIndex = 1;
            // 
            // Full_DM_box
            // 
            this.Full_DM_box.FormattingEnabled = true;
            this.Full_DM_box.Location = new System.Drawing.Point(259, 34);
            this.Full_DM_box.Name = "Full_DM_box";
            this.Full_DM_box.Size = new System.Drawing.Size(268, 368);
            this.Full_DM_box.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(39, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current Loaded DMs ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(367, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "All DMs";
            // 
            // Form2
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(543, 434);
            this.Controls.Add(this.tab_list);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form2";
            this.Text = "Form2";
            this.tab_list.ResumeLayout(false);
            this.Media_List.ResumeLayout(false);
            this.DM_List.ResumeLayout(false);
            this.DM_List.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_list;
        private System.Windows.Forms.TabPage Media_List;
        private System.Windows.Forms.TabPage DM_List;
        private System.Windows.Forms.ListBox Media_List_Box;
        private System.Windows.Forms.ListBox DM_List_Box;
        private System.Windows.Forms.ListBox Full_DM_box;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}