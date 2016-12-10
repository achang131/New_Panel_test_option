using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;
using System.Data;



namespace windowMediaPlayerDM
{
    class comment_move_engine
    {
        List<Label> comment_storage = new List<Label>();
        BackgroundWorker bk201 = new BackgroundWorker();
        delegate void tLabel(Label t);
        bool _playing;
        int interval;
        int move_distance;
        Thread tb;
        Form display;
        public comment_move_engine(int moved) {

         //   comment_storage = comments;
            _playing = false;
            interval = 50;
            move_distance = moved;
            bk201.WorkerSupportsCancellation = true;
            bk201.DoWork += new DoWorkEventHandler(bk201_DoWork);
           // tb = new Thread(threadstart);

            

            
        }

        void bk201_DoWork(object sender, DoWorkEventArgs e)
        {
           // throw new NotImplementedException();
            //while (!bk201.CancellationPending) {
            while(_playing){  
            
            //  MethodInvoker mti = new MethodInvoker(moveComment_part1);
               // mti.Invoke();
                moveComment_part1();





                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(interval));
            
            }

        }
        delegate void invokeLabel(Label l);
        void moveComment_part2(Label l) {

            if (l.InvokeRequired)
            {
                invokeLabel e = new invokeLabel(moveComment_part2);
                try
                {
                    l.Invoke(e, new object[] { l });
                }
                catch (Exception) { }

            }
            else {


                int xend = 0 - l.Size.Width;
                int xinitial = l.Location.X;

                if (xinitial > xend)
                {
                    l.Location = new System.Drawing.Point(xinitial - move_distance, l.Location.Y);
                }
                else
                {
                    try
                    {
                        l.Dispose();
                    }
                    catch (Exception) { };
                }
            
            
            }


            
        
        }
        void threadstart() {
            while (_playing)
            {
                //  MethodInvoker mti = new MethodInvoker(moveComment_part1);
                // mti.Invoke();
                moveComment_part1();





                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(interval));

            }
        
        }

        void moveComment_part1() {

            List<Label> remove = new List<Label>();

            for (int i = 0; i < comment_storage.Count; i++) {


                moveComment_part2(comment_storage.ElementAt(i));


                try
                {
                    if (comment_storage.ElementAt(i).IsDisposed)
                    {


                        remove.Add(comment_storage.ElementAt(i));

                    }
                }
                catch (Exception) { };
            }

            for (int i = 0; i < remove.Count; i++) {

                comment_storage.Remove(remove.ElementAt(i));
            
            }
        
        
        
        
        
        }


        public void Start() {
            if(method ==0){
            this._playing = true;

            if (!bk201.IsBusy) {

                bk201.RunWorkerAsync();
            }
            }else{
           tb = new Thread(threadstart); ;

           tb.IsBackground = true;
            
            
            this._playing = true;
            tb.Start();
        }
        
        }
        int method = 0;
        public void Stop() {

            if (method == 1)
            {
                if (tb != null && tb.IsAlive)
                {
                    tb.Abort();
                }
            }
            else
            {
                _playing = false;
                bk201.CancelAsync();
            }
        
        }
        public int setMoveDistance {

            get { return move_distance; }
            set { move_distance = value; }
        }

        public int setInterval{

            get { return interval; }
            set { this.interval = value; }
    
    }

        public List<Label> setStorage {
            get {
                if (!bk201.IsBusy) {
                    _playing = true;
                    Start();
                }
                return comment_storage; }


            set { comment_storage = value; }
        
        }



    }
}
