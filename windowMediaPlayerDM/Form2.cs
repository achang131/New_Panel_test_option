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
    public partial class Form2 : Form
    {
        public List<String[]> DM_L = new List<string[]>();
        public List<String[]> Media_L = new List<string[]>();
        public List<String[]> FullDM_L = new List<string[]>();

        public Form2()
        {
            InitializeComponent();

           

            this.ClientSizeChanged += new EventHandler(Form2_SizeChanged);


            this.ShowInTaskbar = false;
            this.Media_List_Box.MouseWheel += new MouseEventHandler(Media_List_Box_MouseWheel);
            this.Full_DM_box.MouseWheel +=new MouseEventHandler(Media_List_Box_MouseWheel);
            this.DM_List_Box.MouseWheel +=new MouseEventHandler(Media_List_Box_MouseWheel);
            this.Media_List_Box.Select();
        }
        
        public void  Media_List_Box_MouseWheel(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            ListBox box = sender as ListBox;
            HandledMouseEventArgs handle = e as HandledMouseEventArgs;
            handle.Handled = true;

          //  ((HandledMouseEventArgs)e).Handled = true;
            if (box.SelectedItem == null)
            {
                
                box.ClearSelected();
                box.SelectedItem=box.Items[0];
            }
            else {
                int temp = box.SelectedIndex;
                switch(e.Delta){
                case -120:
                        if (box.Items.Count-1 > temp)
                        {
                            box.ClearSelected();
                            box.SelectedItem = box.Items[temp + 1];
                        }
                break;
                case 120:
                if (0 < temp)
                {
                    box.ClearSelected();
                    box.SelectedItem = box.Items[temp - 1];
                }
                       
                break;
            }
            }
        }



        // when the size of the window is changed the tab page will autofill the whole form
        void Form2_SizeChanged(object sender, EventArgs e)
        {
            tab_list.Size = new Size(ClientRectangle.Right, ClientRectangle.Bottom);
            Media_List_Box.Size = new Size(Media_List.ClientRectangle.Right,Media_List.ClientRectangle.Bottom);
            DM_List_Box.Size = new Size(DM_List.ClientRectangle.Right, DM_List.ClientRectangle.Bottom);
        }
        /*
        public void setDMList(List<String[]> s) {
            DM_L = s;
            foreach (string[] c in DM_L)
            {
                DM_List_Box.Items.Add(c[1]);

            }
        
        }
         */
        public ListBox setFullDMBox {
            set { this.Full_DM_box = value; }
            get { return this.Full_DM_box; }
        
        }

        public ListBox setMListBox {
            set { this.Media_List_Box = value; }
            get { return this.Media_List_Box; }
        
        }
        public ListBox setDMListBox
        {
            set { this.DM_List_Box = value; }
            get { return this.DM_List_Box; }

        }

        public List<String[]> setDMList {

            set
            {
                DM_List_Box.Items.Clear();
                DM_L = value;
                for (int i = 0; i < value.Count();i++ ) {

                    DM_List_Box.Items.Add(value.ElementAt(i)[1]);
                
                }
            
            }
            get { return DM_L; }
        
        
        
        }

        public List<String[]> setFullDMList {
            set
            {
                Full_DM_box.Items.Clear();
                FullDM_L = value;
                for (int i = 0; i < value.Count(); i++)
                {

                    Full_DM_box.Items.Add(value.ElementAt(i)[1]);

                }

            }
            get { return FullDM_L; }
        
        }


        public List<String[]> setMediaList
        {

            set
            {
                Media_List_Box.Items.Clear();
                Media_L = value;
                for (int i = 0; i < value.Count(); i++)
                {

                    Media_List_Box.Items.Add(value.ElementAt(i)[1]);

                }

            }
            get { return Media_L; }



        }

        public List<String[]> getDMList() {

            return DM_L;
        
        }
        /*
        public void setMediaList(List<String[]> s)
        {
            Media_L = s;
            foreach (string[] c in Media_L) {
                Media_List_Box.Items.Add(c[1]);
            
            }

        }
         */
        public List<String[]> getMediaList()
        {

            return Media_L;

        }

        public void writeMediadir(string c) {
            Media_List_Box.Items.Add(c);
        }
        public void writeDMdir(string c){
            DM_List_Box.Items.Add(c);
        
        }

    }
}
