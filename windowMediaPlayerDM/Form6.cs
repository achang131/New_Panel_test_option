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
    public partial class add_link_form : Form
    {
        string dir;
        string link;
        
        
        public add_link_form()
        {
            InitializeComponent();
        }

        public Button setConfirm {
            get { return confirm_button; }
            set { confirm_button = value; }
        
        
        }

        public Button setCacnel {
            get { return cancel_button; }

            set { cancel_button = value; }
        
        
        }

        public string getDri {

            get
            {
                 dir=dir_box.Text;
                 if (dir != "")
                 {
                     return dir;
                 }
                 else
                 {
                     return null;
                 }
            }
            set
            {
                dir = value; 
                dir_box.Text = value; }
        }

        public Uri getLink {

            get { 
                link = link_box.Text;
                if (link != "")
                {
                    return new Uri(link);
                }
                else {
                    return null;
                }
            }
            set { link = value.LocalPath;
            link_box.Text = link;
                    }
        
        }

    }
}
