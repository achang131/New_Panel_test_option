using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace windowMediaPlayerDM
{
    public class Font_Outline:System.Windows.Forms.Label
    {

        public Font_Outline() {

            OutlineForeColor = Color.Black;
            OutlineWidth = 2;
        
        
        }

        public Color OutlineForeColor
        {
            set;
            get;

        }
        public float OutlineWidth
        {
            set;
            get;
        }
       
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            using (GraphicsPath gp = new GraphicsPath())


            using (Pen outline = new Pen(OutlineForeColor, OutlineWidth)
            {
                LineJoin = LineJoin.Round
            })
            using (StringFormat st = new StringFormat())
                
                using (Brush foreBrush = new SolidBrush(ForeColor))
                {
                    st.LineAlignment = StringAlignment.Near;
                    st.Trimming = StringTrimming.None;
                    st.Alignment = StringAlignment.Near;
                    //
                    gp.AddString(Text, Font.FontFamily, (int)Font.Style, Font.Size, ClientRectangle, st);
                    e.Graphics.ScaleTransform(1.3f, 1.35f);
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    e.Graphics.DrawPath(outline, gp);
                    e.Graphics.FillPath(foreBrush, gp);

                    

                }
            
            
        }
    }
}
