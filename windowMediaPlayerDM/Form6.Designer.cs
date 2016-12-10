namespace windowMediaPlayerDM
{
    partial class add_link_form
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
            this.link_box = new System.Windows.Forms.TextBox();
            this.dir_box = new System.Windows.Forms.TextBox();
            this.dir_label = new System.Windows.Forms.Label();
            this.link_label = new System.Windows.Forms.Label();
            this.confirm_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // link_box
            // 
            this.link_box.Location = new System.Drawing.Point(212, 63);
            this.link_box.Name = "link_box";
            this.link_box.Size = new System.Drawing.Size(543, 20);
            this.link_box.TabIndex = 0;
            // 
            // dir_box
            // 
            this.dir_box.Location = new System.Drawing.Point(212, 23);
            this.dir_box.Name = "dir_box";
            this.dir_box.Size = new System.Drawing.Size(543, 20);
            this.dir_box.TabIndex = 0;
            // 
            // dir_label
            // 
            this.dir_label.AutoSize = true;
            this.dir_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dir_label.Location = new System.Drawing.Point(13, 23);
            this.dir_label.Name = "dir_label";
            this.dir_label.Size = new System.Drawing.Size(116, 17);
            this.dir_label.TabIndex = 1;
            this.dir_label.Text = "Set File Directory";
            // 
            // link_label
            // 
            this.link_label.AutoSize = true;
            this.link_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.link_label.Location = new System.Drawing.Point(13, 63);
            this.link_label.Name = "link_label";
            this.link_label.Size = new System.Drawing.Size(114, 17);
            this.link_label.TabIndex = 1;
            this.link_label.Text = "Set Link address";
            // 
            // confirm_button
            // 
            this.confirm_button.Location = new System.Drawing.Point(101, 96);
            this.confirm_button.Name = "confirm_button";
            this.confirm_button.Size = new System.Drawing.Size(75, 23);
            this.confirm_button.TabIndex = 2;
            this.confirm_button.Text = "Confirm";
            this.confirm_button.UseVisualStyleBackColor = true;
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(565, 96);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 2;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // add_link_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 121);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.confirm_button);
            this.Controls.Add(this.link_label);
            this.Controls.Add(this.dir_label);
            this.Controls.Add(this.dir_box);
            this.Controls.Add(this.link_box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "add_link_form";
            this.Text = "Add link";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox link_box;
        private System.Windows.Forms.TextBox dir_box;
        private System.Windows.Forms.Label dir_label;
        private System.Windows.Forms.Label link_label;
        private System.Windows.Forms.Button confirm_button;
        private System.Windows.Forms.Button cancel_button;
    }
}