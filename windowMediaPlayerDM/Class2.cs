using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Windows.Forms;


//how to implement
//use dictionary to hold the url as key and list of the movie urls as value
//



// on download complete rename file, remove dled link from list, add file to the media listbox as local file

namespace windowMediaPlayerDM
{
    class getWebVideo
    {
        String fullcontent;
        Uri websiteLink;

        String fileUrl;
        String dlfilepath;

        String filestorage_dir;
        List<Uri> Allfiles= new List<Uri>();

        String effetiveLink;

        List<string> titles = new List<string>();
        String title;
        public getWebVideo(Uri url) {

            websiteLink = url;

            //System.Net.WebClient webclient = new System.Net.WebClient();

            loadInfo(websiteLink);
          title=  getTitle();
          titles.Add(title);
            getAllUrls();
        
        }

         public getWebVideo(Uri url , String dirpath) {

            websiteLink = url;
            filestorage_dir=dirpath;

            //System.Net.WebClient webclient = new System.Net.WebClient();

            DirectoryInfo checkxml = new DirectoryInfo(dirpath + "\\temp_xml");
            if (!checkxml.Exists) {
                checkxml.Create();
            }


           otherSiteSupport(url);

            
        }
        public void loadInfo_say(Uri url){


            using (WebClient wb = new WebClient())
            {

               wb.Encoding = Encoding.UTF8;
               fullcontent= wb.DownloadString(url);




               wb.Dispose();
            }
        
        }

         public void otherSiteSupport(Uri url) {

             String siteLink = url.OriginalString;

           //  http://himado.in/?sort=today_view_cnt&page=0
             //remove the http:// on the link
             siteLink = siteLink.Replace("http://", "");
             int end = siteLink.IndexOf("/");
             siteLink = siteLink.Substring(0, end);
             // himado.in

             switch (siteLink) {

                 case "say-move.org":

                     loadInfo_say(websiteLink);
                   title= getTitle();
                 



                 //    byte[] tempbytes = Encoding.Default.GetBytes(title);
                 //    fullcontent = Encoding.GetEncoding("shift-jis").GetString(tempbytes);
                  //  title = getTitle();
                    titles.Add(title);
                     getAllUrls_say();
                     downloadComment_say();

                     break;



                 default:

           loadInfo(websiteLink);
           title= getTitle();
           titles.Add(title);
                     getAllUrls();

                     break;
             
             
             
             }

         
         }
         void downloadComment_say() { 
         //start from getting the comment file url

             string leftbase = "http://say-move.org/comment_download.php";
             string rightbase = "\">コメントをダウンロード</a><br>";
             int start = fullcontent.IndexOf(leftbase);
             int end = fullcontent.Length;

             string link = fullcontent.Substring(start, end - start);
             start = 0;
             end = link.IndexOf("\"");
             link = link.Substring(start, end - start);

             // comment link get !


             using(WebClient wb = new WebClient()){


                 FileInfo file = new FileInfo(filestorage_dir + "\\" + title);

                 //download the .xml file as the title name +.xml into the default folder 
                 if (!file.Exists)
                 {
                     wb.DownloadFileAsync(new Uri(link), file.FullName+".xml");
                 }
                 else {
                     wb.DownloadFileAsync(new Uri(link), file.FullName+"(S).xml");
                 
                 }
                 wb.Dispose();
             
             }
          
         
         
         
         
         }

         void getAllUrls_say() { 
         

             //var ary_spare_sources = {"spare":[{"lid":"1","src":"http%3A%2F%2Ffleetupload.com%2Fmp3embed-gv3o70fbhxv2.mp3%3F"},{"lid":"2","src":"http%3A%2F%2Fwww.indishare.com%2Fmp3embed-q52air4wrwze.mp3%3F"},{"lid":"3","src":"http%3A%2F%2Fwww.indishare.com%2Fmp3embed-65a4s05kdbq2.mp3%3F"},{"lid":"4","src":"http%3A%2F%2Fsolidshelf.com%2Fmp3embed-ihu5ny2zk2e1.mp3%3F"},{"lid":"5","src":"http%3A%2F%2Fs3-us-west-1.amazonaws.com%2Fspliceproduction%2Fpreviews%2Fa9007396-0639-2680-1624-e3364fa3ecd748a6721%2Fa0221a42-0abd-6d80-73f2-a9777c63b96a461da92.mp3%3F"}]};
             //{"lid":"1","src":"http%3A%2F%2Ffleetupload.com%2Fmp3embed-gv3o70fbhxv2.mp3%3F"},{"lid":"2","src":"http%3A%2F%2Fwww.indishare.com%2Fmp3embed-q52air4wrwze.mp3%3F"},{"lid":"3","src":"http%3A%2F%2Fwww.indishare.com%2Fmp3embed-65a4s05kdbq2.mp3%3F"},{"lid":"4","src":"http%3A%2F%2Fsolidshelf.com%2Fmp3embed-ihu5ny2zk2e1.mp3%3F"},{"lid":"5","src":"http%3A%2F%2Fs3-us-west-1.amazonaws.com%2Fspliceproduction%2Fpreviews%2Fa9007396-0639-2680-1624-e3364fa3ecd748a6721%2Fa0221a42-0abd-6d80-73f2-a9777c63b96a461da92.mp3%3F"}
             //
             // count "lid":"5
             // ,"src":
             //lid":"5"
           //  getbaseUrl(fullcontent);

             String fcut = "コメントについて - FC2ヘルプ</a><br>";
             String scut = ">この動画の配信元サイトへのリンク";
             int left = fullcontent.IndexOf(fcut);
             int right = fullcontent.IndexOf(scut);
             String target;
             if (right > left && left > -1)
             {
              target = fullcontent.Substring(left, right - left);
             //>コメントについて - FC2ヘルプ</a><br><a target="_blank" href="http://video.fc2.com/content/20160710fQWKUAmS"
             left = target.IndexOf("http://");
             right = target.LastIndexOf("\"");
 
                 target = target.Substring(left, right - left);
                 target = target.Replace("%2F", "/");
                 target = target.Replace("%3A", ":");
                 target = target.Replace("%3F", "?");
                 target = target.Replace("&amp;", "&");

                 Allfiles.Add(new Uri(target));

             }
             else {

                 fcut = "FLVURL:<br>";
                 scut = "href=\"http://say-move.org/comment_download";
                 left = fullcontent.IndexOf(fcut);
                 right = fullcontent.IndexOf(scut);
                 if (right > left)
                 {
                     target = fullcontent.Substring(left, right - left);

                     left = target.IndexOf("http");
                     right = target.Length;

                     target = target.Substring(left, right - left);
                     right = target.IndexOf("\"");
                     target = target.Substring(0, right);
                     target = target.Replace("%2F", "/");
                     target = target.Replace("%3A", ":");
                     target = target.Replace("%3F", "?");
                     target = target.Replace("&amp;", "&");

                     Allfiles.Add(new Uri(target));

                 }
             
             }
             //base url obtained !!!

             if (fullcontent.Contains("flvlink"))
             {
                 fcut = "FlashVars=";
                 scut = "pluginspage=";
                 left = fullcontent.IndexOf(fcut);
                 right = fullcontent.IndexOf(scut);
                 if (right > left)
                 {
                     target = fullcontent.Substring(left, right - left);

                     left = target.LastIndexOf("flvlink");
                     right = target.Length;
                     String getcount = target.Substring(left, right - left);
                     right = getcount.IndexOf("=");
                     getcount = getcount.Substring(0, right);
                     getcount = getcount.Replace("flvlink", "");
                     int number = Int32.Parse(getcount);



                     for (int i = 0; i < number; i++)
                     {
                         left = target.IndexOf("flvlink");
                         right = target.Length;
                         target = target.Substring(left, right - left);


                         left = target.IndexOf("http");
                         right = target.IndexOf("&add");
                         if (right > left)
                         {
                             String templink = target.Substring(left, right - left);

                             templink = templink.Replace("%2F", "/");
                             templink = templink.Replace("%3A", ":");
                             templink = templink.Replace("%3F", "?");
                             templink = templink.Replace("&amp;","&");

                             Allfiles.Add(new Uri(templink));

                             target = target.Substring(right, target.Length-right);

                         }
                     }

                 }
             }
       
         
         }

        public void changeOnlineUrl(Uri url){

            websiteLink = url;

            loadInfo(websiteLink);
           title= getTitle();
           titles.Add(title);
            getAllUrls();

        
        }
        public Encoding code;
        public void changePath(string dir) {

             filestorage_dir = dir;
         }
        void loadInfo(Uri url) {
          
            
            HttpWebRequest wrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse wresponse = (HttpWebResponse)wrequest.GetResponse();

            //if you get response from the http site
            if (wresponse.StatusCode == HttpStatusCode.OK) {

                Stream recives = wresponse.GetResponseStream();
                StreamReader readers = null;

                //see if the page is unicoded
                if (wresponse.CharacterSet == null)
                {

                    readers = new StreamReader(recives);
                }
                else { 
                //if unicoded

                    readers = new StreamReader(recives, Encoding.GetEncoding(wresponse.CharacterSet));

                    code = Encoding.GetEncoding(wresponse.CharacterSet);
                }
                //could use read line to get an array of results?
                fullcontent = readers.ReadToEnd();

                wresponse.Close();
                readers.Close();

                


            }

        }
        String getTitle() {

            string result="";
            string conetent = fullcontent;
            string t = "<title>";
            int start = fullcontent.IndexOf(t);
            int end = fullcontent.LastIndexOf("</title>");

            result = fullcontent.Substring(start+t.Length,end-(start+t.Length));

            return result;
        }

         String getbaseUrl(String fcontent) {

            String result = "";
            String firsttarget = "var movie_url = ";
            String firsttemp = "var display_movie_url = ";
            if (fullcontent.Contains(firsttarget))
            {
                int length = firsttarget.Length;
                int start = fcontent.IndexOf(firsttarget);
                int end = fcontent.IndexOf(firsttemp);


                String tempresult = fcontent.Substring(start, end - start);

                start = tempresult.IndexOf("'") + 1;
                end = tempresult.LastIndexOf(";") - 1;

                result = tempresult.Substring(start, end - start);

                //http://s3.amazonaws.com/ksr/assets/012/985/562/4b9df98bfba5c3714537627d3d8ec91a_original.mp3?
                //http%3A%2F%2Fs3.amazonaws.com%2Fksr%2Fassets%2F012%2F985%2F562%2F4b9df98bfba5c3714537627d3d8ec91a_original.mp3%3F

                //basic put the gabbish data back to it's orignal form so the uri can read it
                result = result.Replace("%2F", "/");
                result = result.Replace("%3A", ":");
                result = result.Replace("%3F", "?");
                if (result != "")
                {
                    Allfiles.Add(new Uri(result));
                }

                //check if result here is online if not then go find other links in full content
            }
            return result;
        
        }
         public string saymove() {

             String key = fullcontent;
             int start = fullcontent.IndexOf("return '");
             key = key.Substring(start, key.Length - start);
             start = key.IndexOf("'")+1;
             int end = key.IndexOf("';");
             key = key.Substring(start, end - start);

             //now the key is extracted

             String video = fullcontent;
             start = video.IndexOf("http://video.fc2.com/content/");
             if (start >= 0) { 
             // if it contains the address
                 video = video.Substring(start, video.Length - start);
                 end = video.IndexOf("\"");
                 video = video.Substring(0, end);
                 //now video contains the fc2 video address
                 start = video.LastIndexOf("/")+1;
                 
                 String id = video.Substring(start,video.Length-start);

                 //http://say-move.org/fc2/api/fc2flvPath.php?gk=ewtM1aRpc5KcQ31H&up%5Fid=20160723NH71111q

                 String gatekey = saymoveKey(key, id);
               string ans=  saymovefc2(gatekey,key,id);

               //throw new Exception("view");
               return ans;


             
             }
             return "not found";
         
         }

         private string saymoveKey(string key, string id) {
             string result = "http://say-move.org/fc2/api/fc2flvPath.php?gk=&up%5Fid=";
             result = result.Replace("gk=", "gk=" + key);
             result = result.Replace("id=","id="+id);
             return result;
         
         }
         public  string saymovefc2(string url,string gk ,string id) {


             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
             //responese=wb.ResponseHeaders.ToString();
             WebHeaderCollection wa = new WebHeaderCollection();
         //    wa.Add(HttpRequestHeader.Host, "say-move.org");
             wa.Add("Host", "say-move.org");
             wa.Add("X-Request-With", "ShockwaveFlash/22.0.0.209");

             wa.Add(HttpRequestHeader.Connection, "keep-alive");
             wa.Add(HttpRequestHeader.Referer, this.websiteLink.OriginalString);
             wa.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
             wa.Add(HttpRequestHeader.Accept, "*/*");
             //  wa.Add(HttpRequestHeader.Cookie, "open_category_menu=1; fc2cnt_862=1-1468124524; __utma=64400813.163552657.1468117240.1468817630.1469435800.3; __utmc=64400813; __utmz=64400813.1469435800.3.2.utmcsr=himado.in|utmccn=(referral)|utmcmd=referral|utmcct=/; globalmode=0; lang=ja; zHADmJDY=ewtM1aRpc5KcQ31H; fc2cnt_984716=1-1469525214; fc2_analyzer_697394=1-872535462-1468117281-1469547263-21-4-1469435815; FC2ANASESSION697394=49161928; fc2cnt_861=1-1469547264; _pk_ref.4.c1cd=%5B%22%22%2C%22%22%2C1469547283%2C%22http%3A%2F%2Fhimado.in%2F%3Fsort%3Dmovie_id%26mode%3Dsearch%22%5D; _pk_id.4.c1cd=bb583a982d36ded4.1468117240.7.1469547283.1469547283.; _pk_ses.4.c1cd=*; _gat=1; _gat_generalPC=1; _ga=GA1.2.163552657.1468117240");

             wa.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch");
             wa.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,ja;q=0.6,zh-TW;q=0.4,zh;q=0.2");
             System.Collections.Specialized.NameValueCollection qstring = new System.Collections.Specialized.NameValueCollection();
             qstring.Add("gk", gk);
             qstring.Add("up_id", id);
           //  request.Headers = wa;
             using (WebClient wb = new WebClient()) {
                 wb.Headers = wa;
                 wb.QueryString = qstring;
                 wb.DownloadFile(url, "testfile");
             
             
             
             }
             
             request.Method = "GET";

             request.Host = "say-move.org";
             request.KeepAlive = true;
            // request.Connection = "keep-alive";
             //request.Referer = "http://say-move.org/comeplay.php?comeid=1620354";
             request.Referer = this.websiteLink.OriginalString;
             request.Accept = "*/*";
             request.MaximumResponseHeadersLength = 300;
             byte[] _byte = Encoding.ASCII.GetBytes(string.Concat("content=", url));
             //in case if content length need to be filled
            
             String fullc = "";
          
             //  Stream response = request.GetRequestStream();
          
             try
             {
                 HttpWebResponse wrespones = (HttpWebResponse)request.GetResponse();
                 
                 using (StreamReader reader = new StreamReader(wrespones.GetResponseStream()))
                 {

                     fullc = reader.ReadToEnd();

                     reader.Close();
                 }
             }
             catch (WebException) {
                 fullc = "404 not found";
             
             }
             return fullc;

             //if everything went well then fullc will have the text that contains the true video address
             //for the video  call dl from here
             //


         }

         void getAllUrls() {

             //var ary_spare_sources = {"spare":[{"lid":"1","src":"http%3A%2F%2Ffleetupload.com%2Fmp3embed-gv3o70fbhxv2.mp3%3F"},{"lid":"2","src":"http%3A%2F%2Fwww.indishare.com%2Fmp3embed-q52air4wrwze.mp3%3F"},{"lid":"3","src":"http%3A%2F%2Fwww.indishare.com%2Fmp3embed-65a4s05kdbq2.mp3%3F"},{"lid":"4","src":"http%3A%2F%2Fsolidshelf.com%2Fmp3embed-ihu5ny2zk2e1.mp3%3F"},{"lid":"5","src":"http%3A%2F%2Fs3-us-west-1.amazonaws.com%2Fspliceproduction%2Fpreviews%2Fa9007396-0639-2680-1624-e3364fa3ecd748a6721%2Fa0221a42-0abd-6d80-73f2-a9777c63b96a461da92.mp3%3F"}]};
             //{"lid":"1","src":"http%3A%2F%2Ffleetupload.com%2Fmp3embed-gv3o70fbhxv2.mp3%3F"},{"lid":"2","src":"http%3A%2F%2Fwww.indishare.com%2Fmp3embed-q52air4wrwze.mp3%3F"},{"lid":"3","src":"http%3A%2F%2Fwww.indishare.com%2Fmp3embed-65a4s05kdbq2.mp3%3F"},{"lid":"4","src":"http%3A%2F%2Fsolidshelf.com%2Fmp3embed-ihu5ny2zk2e1.mp3%3F"},{"lid":"5","src":"http%3A%2F%2Fs3-us-west-1.amazonaws.com%2Fspliceproduction%2Fpreviews%2Fa9007396-0639-2680-1624-e3364fa3ecd748a6721%2Fa0221a42-0abd-6d80-73f2-a9777c63b96a461da92.mp3%3F"}
             //
             // count "lid":"5
             // ,"src":
             //lid":"5"
             getbaseUrl(fullcontent);
           String  result = "";
           String  firsttarget = "var ary_spare_sources =";
           String  firsttemp = "var sendFailed =";
           if (fullcontent.Contains(firsttarget))
           {
               int start = fullcontent.IndexOf(firsttarget);
               int end = fullcontent.IndexOf(firsttemp);
               String tempresult = fullcontent.Substring(start, end - start);


               start = tempresult.IndexOf("[");
               end = tempresult.IndexOf("]");
               tempresult = tempresult.Substring(start + 1, end - start);



               start = tempresult.LastIndexOf("lid\":");
               end = tempresult.LastIndexOf(",\"src\":");
               if (end > start)
               {
                   String Urltotalcount = tempresult.Substring(start, end - start);
                   end = Urltotalcount.LastIndexOf("\"") - 1;
                   Urltotalcount = Urltotalcount.Replace("\"", "");
                   Urltotalcount = Urltotalcount.Replace("lid", "");
                   Urltotalcount = Urltotalcount.Replace(":", "");
                   int totalcount = Int32.Parse(Urltotalcount);


                   //  throw new Exception("test view"){};

                   for (int i = 0; i < totalcount; i++)
                   {

                       start = tempresult.IndexOf("http");
                       end = tempresult.IndexOf("}");

                       //http%3A%2F%2Ffleetupload.com%2Fmp3embed-gv3o70fbhxv2.mp3%3F"
                       if (end > start && start>=0 && end >=0)
                       {
                           string urlt = tempresult.Substring(start, end - 1 - start);

                           urlt = urlt.Replace("%2F", "/");
                           urlt = urlt.Replace("%3A", ":");
                           urlt = urlt.Replace("%3F", "?");

                           Uri temp = new Uri(urlt);

                           Allfiles.Add(temp);


                           //  throw new Exception("test view") { };

                           if (i != totalcount - 1)
                           {
                               tempresult = tempresult.Substring(end + 1, tempresult.Length - (end + 1));
                           }
                       }
                       else
                       {
                           if (i != totalcount - 1)
                           {
                               tempresult = tempresult.Substring(end + 1, tempresult.Length - (end + 1));
                           }
                       }
                   }
               }
           }
         }


        // other version of download file for form interaction

         public void downlaodFile(Uri filepath, WebClient wb)
         {
             //test download file code here
          //   WebClient wb = new WebClient();

         //    wb.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(wb_DownloadFileCompleted);
         //    wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wb_DownloadProgressChanged);
             try
             {
                 String fileU = filepath.AbsolutePath;

                 int fstart = fileU.LastIndexOf("/") + 1;

                 String filename = fileU.Substring(fstart, fileU.Length - fstart);



                 FolderBrowserDialog fb = new FolderBrowserDialog();


                 //if an actuall dir is set and it exist then it won't prompt up to ask user to set a dir

                 DirectoryInfo ndir = new DirectoryInfo(filestorage_dir);



                 if (!ndir.Exists)
                 {

                     //selet a folder to hold the file

                     if (fb.ShowDialog() == DialogResult.OK)
                     {
                         FileInfo newfile = new FileInfo(fb.SelectedPath + "\\" + filename);

                         // if the file exists then stop download
                         if (!newfile.Exists)
                         {
                             wb.DownloadFileAsync(filepath, @newfile.FullName);


                             dlfilepath = newfile.FullName;
                         }
                         else
                         {


                             dlfilepath = newfile.FullName;
                         }
                     }
                 }
                 else
                 {
                     filename = filename.Replace(":", "");
                     filename = filename.Replace("\\", "");
                     filename = filename.Replace("*", "");
                     filename = filename.Replace("|", "");
                     filename = filename.Replace("\"", "");
                     filename = filename.Replace("?", "");
                     filename = filename.Replace("<", "");
                     filename = filename.Replace(">", "");

                     FileInfo newfile = new FileInfo(ndir.FullName + "\\" + filename);

                     // if the file exists then stop download
                     if (!newfile.Exists)
                     {
                         wb.DownloadFileAsync(filepath, @newfile.FullName);


                         dlfilepath = newfile.FullName;

                     }
                     else
                     {

                         dlfilepath = newfile.FullName;
                     }
                 }






                 //throw a file not found exception if dlfilepath is null ?
                 wb.Dispose();
             }
             catch (NullReferenceException) { };



         }


        //make user select path if a directory is not set in default ?

         void downlaodFile(Uri filepath) {
             //test download file code here
             using (WebClient wb = new WebClient())
             {

                 wb.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(wb_DownloadFileCompleted);
                 wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wb_DownloadProgressChanged);
                 String fileU = filepath.AbsolutePath;

                 int fstart = fileU.LastIndexOf("/") + 1;

                 String filename = fileU.Substring(fstart, fileU.Length - fstart);

                 FolderBrowserDialog fb = new FolderBrowserDialog();


                 //if an actuall dir is set and it exist then it won't prompt up to ask user to set a dir

                 DirectoryInfo ndir = new DirectoryInfo(filestorage_dir);



                 if (!ndir.Exists)
                 {

                     //selet a folder to hold the file

                     if (fb.ShowDialog() == DialogResult.OK)
                     {
                         FileInfo newfile = new FileInfo(fb.SelectedPath + "\\" + filename);

                         // if the file exists then stop download
                         if (!newfile.Exists)
                         {
                             wb.DownloadFileAsync(filepath, @newfile.FullName);


                             dlfilepath = newfile.FullName;
                         }
                         else
                         {


                             dlfilepath = newfile.FullName;
                         }
                     }
                 }
                 else
                 {

                     FileInfo newfile = new FileInfo(ndir.FullName + "\\" + filename);

                     // if the file exists then stop download
                     if (!newfile.Exists)
                     {
                         wb.DownloadFileAsync(filepath, @newfile.FullName);


                         dlfilepath = newfile.FullName;

                     }
                     else
                     {

                         dlfilepath = newfile.FullName;
                     }
                 }






                 //throw a file not found exception if dlfilepath is null ?
                 wb.Dispose();
             }
         
         
         }
         int percentage = 0;
          double byte_recieved = 0;
         void wb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
         {
             //throw new NotImplementedException();
             


         }
         bool inloop = false;
         bool fileNotzero=false;
         void wb_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
         {
             //do something here update file state ?


         }
        //get and dl the file then return the local file path to play offline

         public List<Uri> getDownlist {

             get {
                   
                 return Allfiles ;
             }

             set {}
         
         }
         Uri playing;

         public Uri playDownlist {

             get { return new Uri(dlfilepath); }
             set {
                 playing = value;
                 downlaodFile(value); }
         
         }
         public String dlurl {
             get {

                     string t = getbaseUrl(fullcontent);

                     downlaodFile(new Uri(t));

                 return dlfilepath; }
             set
             {
                 websiteLink = new Uri(value);
                 loadInfo(websiteLink);
             }
         
         
         }
         bool checkFileOnline(Uri url) {
             bool result = true ;
             HttpWebResponse response=null;
             var request = (HttpWebRequest)WebRequest.Create(url.LocalPath);
             //set the request to head
             request.Method = "HEAD";

             try
             {
                  response = (HttpWebResponse)request.GetResponse();

             }
             catch (WebException ex)
             {
                 result = false;
             }
             finally {

                 if (response != null) {

                     response.Close();

                     result = true;
                 }
             }



             return result;
         }

         public string dlnameUpdate {

             get
             {
                 
                 int end = dlfilepath.LastIndexOf("\\");
                 dlfilepath = dlfilepath.Substring(0, end);
                 dlfilepath = dlfilepath + "\\" + title;
                    return dlfilepath; }
             set { dlfilepath = value; } 
         
         }


        //get the file place online
         public String getbasicUrl
         {
             get{return getbaseUrl(fullcontent);}
             set { websiteLink = new Uri(value);
             loadInfo(websiteLink);
             }

         }

        /*        List<Uri> Allfiles= new List<Uri>();

        String effetiveLink;

        List<string> titles = new List<string>();
         */

         public List<Uri> getAllfiles {
             get { return Allfiles; }
             set { Allfiles = value; }
         
         
         }
         public string getCurrentTitle {
             set { title = value; }
             get { return title; }
         
         }
         public List<string> getTitles {
             get { return titles; }
             set { titles = value; }
         
         }
    }
}
