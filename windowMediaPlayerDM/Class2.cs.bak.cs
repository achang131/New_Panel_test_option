using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Text;

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
        public getWebVideo(String url) {
            websiteLink = new Uri(url);
            Alter_loadInfo(new Uri(url));
        
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
          // getCommentUri(url);
            
        }
        public void getCommentUri(Uri url) {
            bool lesscomment = false;

            if(url.OriginalString.Contains("himado.in")){
            String commentc = fullcontent;
            //to get the full comment need to decode this
                String tp = "\"commentdlform\"";
            int left = commentc.IndexOf("\"commentdlform\"")+tp.Length+3;
            int right = commentc.IndexOf("\"nico\"");
               
            if (left >= 0 && right >=0 && right>left)
            {
                commentc = commentc.Substring(left, right - left);



                left = commentc.IndexOf("\"id\"");
                right = commentc.Length;

                if (left >= 0 && right > 0 && right > left)
                {

                    commentc = commentc.Substring(left, right - left);

                    left = commentc.IndexOf("=\"") + 2;
                    right = commentc.IndexOf("\">");

                    String id = commentc.Substring(left, right - left);

                    commentc = commentc.Substring(right + 3, commentc.Length - (right + 3));

                    left = commentc.IndexOf("group_id\" value");
                    right = commentc.IndexOf("\">");

                    String group_id = commentc.Substring(left, right - left);
                    commentc = commentc.Substring(right + 3, commentc.Length - (right + 3));

                    left = group_id.IndexOf("=\"") + 2;
                    right = group_id.Length;

                    group_id = group_id.Substring(left, right - left);

                    left = commentc.IndexOf("key") + 5;

                    commentc = commentc.Substring(left, commentc.Length - left);

                    left = commentc.IndexOf("=\"") + 2;
                    right = commentc.IndexOf("\">");

                    String key = commentc.Substring(left, right - left);

                    //http://himado.in/?mode=comment&id=340887&limit=200000&key=23&group_id=391199,390082&start=0&ver=20100220

                    String trueUrl = "http://himado.in/?mode=comment&id=" + id + "&limit=200000&key=" + key + "&group_id=" + group_id + "&start=0";

                    Uri temp = new Uri(trueUrl);

                    //throw new Exception("check");
                    getHimadoComment(temp);
                }
                else
                {
                    int start = url.OriginalString.LastIndexOf("=")+1;
                    int end = url.OriginalString.Length;
                    String id = url.OriginalString.Substring(start, end - start);

                    left = commentc.IndexOf("group_id\" value");
                    right = commentc.IndexOf("\">");

                    String group_id = commentc.Substring(left, right - left);
                    commentc = commentc.Substring(right + 3, commentc.Length - (right + 3));

                    left = group_id.IndexOf("=\"") + 2;
                    right = group_id.Length;

                    group_id = group_id.Substring(left, right - left);

                    left = commentc.IndexOf("key") + 5;

                    commentc = commentc.Substring(left, commentc.Length - left);

                    left = commentc.IndexOf("=\"") + 2;
                    right = commentc.IndexOf("\">");

                    String key = commentc.Substring(left, right - left);

                    //http://himado.in/?mode=comment&id=340887&limit=200000&key=23&group_id=391199,390082&start=0&ver=20100220

                    String trueUrl = "http://himado.in/?mode=comment&id=" + id + "&limit=200000&key=" + key + "&group_id=" + group_id + "&start=0";

                    Uri temp = new Uri(trueUrl);

                    //throw new Exception("check");
                    getHimadoComment(temp);
                }
            }
            else {

                lesscomment = true;
            }

            }


            if (lesscomment == true)
            {

                if (url.OriginalString.Contains("himado.in"))
                {
                    //http://himado.in/?mode=comment&id=340877&limit=200000
                    //http://himado.in/340877

                    if (url.OriginalString.Contains("commentgroup"))
                    {

                        //http://himado.in/?mode=commentgroup&group_id=391236

                        int start = url.OriginalString.IndexOf("id=") + 3;

                        String id = url.OriginalString.Substring(start, url.OriginalString.Length - start);

                        id = id.Replace("?", "");

                        String commentPlace = "http://himado.in/?mode=comment&id=" + id + "&limit=200000";

                        Uri temp = new Uri(commentPlace);

                        getHimadoComment(temp);





                    }
                    else
                    {
                        Uri temp = null;
                        String original = url.OriginalString;
                        int start = original.LastIndexOf("/") + 1;
                        original = original.Substring(start, original.Length - start);
                        //just in case
                        original = original.Replace("?", "");

                        String commentPlace = "http://himado.in/?mode=comment&id=" + original + "&limit=200000";

                        temp = new Uri(commentPlace);

                        getHimadoComment(temp);
                    }
                }
            }
         
         }
         int decoder(char ch, int times)
         {
             int ans = 0;

             int abase = 36;
             int sbase = 0;
             if (times < 0)
             {
                 sbase = 1;
             }
             else
             {
                 sbase = 36;
             }
             switch (ch)
             {

                 case '1':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 1 * sbase;
                     break;
                 case '2':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 2 * sbase;
                     break;
                 case '3':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 3 * sbase;
                     break;
                 case '4':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 4 * sbase;
                     break;
                 case '5':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 5 * sbase;
                     break;
                 case '6':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 6 * sbase;
                     break;
                 case '7':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 7 * sbase;
                     break;
                 case '8':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 8 * sbase;
                     break;
                 case '9':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 9 * sbase;
                     break;
                 case 'a':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 10 * sbase;
                     break;
                 case 'b':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 11 * sbase;
                     break;
                 case 'c':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 12 * sbase;
                     break;
                 case 'd':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 13 * sbase;
                     break;
                 case 'e':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 14 * sbase;
                     break;
                 case 'f':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 15 * sbase;
                     break;
                 case 'g':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 16 * sbase;
                     break;
                 case 'h':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 17 * sbase;
                     break;
                 case 'i':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 18 * sbase;
                     break;
                 case 'j':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 19 * sbase;
                     break;
                 case 'k':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 20 * sbase;
                     break;
                 case 'l':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 21 * sbase;
                     break;
                 case 'm':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 22 * sbase;
                     break;
                 case 'n':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 23 * sbase;
                     break;
                 case 'o':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 24 * sbase;
                     break;
                 case 'p':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 25 * sbase;
                     break;
                 case 'q':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 26 * sbase;
                     break;
                 case 'r':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 27 * sbase;
                     break;
                 case 's':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 28 * sbase;
                     break;
                 case 't':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 29 * sbase;
                     break;
                 case 'u':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 30 * sbase;
                     break;
                 case 'v':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 31 * sbase;
                     break;
                 case 'w':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 32 * sbase;
                     break;
                 case 'x':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 33 * sbase;
                     break;
                 case 'y':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 34 * sbase;
                     break;

                 case 'z':
                     for (int i = 0; i < times; i++)
                     {

                         sbase = sbase * abase;

                     }
                     ans = 35 * sbase;
                     break;
                 default:

                     ans = 0;
                     break;

             }


             return ans;

         }
         int convert(String x)
         {

             int answer = 0;
             //1~10+26 char
             //int allbase = 36;

             char[] allchar = x.ToCharArray();

             for (int i = allchar.Length - 1; i >= 0; i--)
             {
                 int times = allchar.Length - (i + 1) - 1;
                 answer += decoder(allchar[i], times);

             };



             return answer;
         }

         String commentPage = "";

         void getHimadoComment(Uri url) {

             using (WebClient wb = new WebClient()) {
                 wb.Encoding = Encoding.UTF8;
                 //MessageBox.Show(url.OriginalString);
                 wb.DownloadStringAsync(url);
                 wb.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wb_DownloadStringCompleted);
             
             
             
             }
         
         
         
         
         }
         String safeFilename(String filename)
         {
             String result = filename;
             result = result.Replace("\\", " ");
             result = result.Replace("?", " ");
             result = result.Replace(":", " ");
             result = result.Replace("<", " ");
             result = result.Replace(">", " ");
             result = result.Replace("\"", " ");
             result = result.Replace("|", " ");
             result = result.Replace("*", " ");
             result = result.Replace("/", " ");




             return result;

         }
         void wb_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
         {
             //throw new NotImplementedException();
             title = safeFilename(title);
             String CommentTitle = title+".xml";
             commentPage = e.Result;
             FileInfo temp_comment = new FileInfo("temp_comment");
             if (temp_comment.Exists) {
                 temp_comment.Delete();
             }

             // title
             File.WriteAllText(temp_comment.FullName, commentPage);

             XmlReader reader = new XmlTextReader(temp_comment.FullName);
             XmlReader findID = new XmlTextReader(temp_comment.FullName);
             Dictionary<String, String> ids = new Dictionary<string, string>();
             while (findID.Read()) {

                 switch (findID.NodeType) { 
                        
                     case XmlNodeType.Text:
                         if (findID.Value.Contains("http://") || findID.Value.Contains("https://")) {
                             try
                             {
                                 Uri temp = new Uri(findID.Value);
                                 Allfiles.Add(temp);
                             }
                             catch (Exception) { }
                         
                         }

                         break;
                     case XmlNodeType.Element:
                         if(findID.Name=="d"){
                             String n = "";
                             String u = "";
                             while (findID.MoveToNextAttribute()) {

                                 if (findID.Name == "n") {
                                     n = findID.Value;
                                 
                                 }

                                 if (findID.Name == "u") {
                                     u = findID.Value;
                                 }
                             
                             
                             }
                             ids.Add(n, u);

                         
                         }

                         break;
                 
                 
                 }
             
             }
             findID.Close();

             using (XmlWriter writer = XmlWriter.Create(CommentTitle))
             {
                 writer.WriteStartDocument();
                 writer.WriteStartElement("packet");
                 while (reader.Read()) {
                     
                     switch (reader.NodeType) { 
                     
                         
                         case XmlNodeType.Element:
                             if(reader.Name=="c"){
                                 writer.WriteStartElement("chat");
                                 while (reader.MoveToNextAttribute()) {

                                     //<c p="107,bb0,44dp7e,0,0">１</c>
                                     if (reader.Name == "p") {
                                        String raw= reader.Value;
                                        int start = raw.IndexOf(",");
                                        string prevpos = raw.Substring(0, start);
                                        prevpos = prevpos.Replace(",", "");
                                         //just in case if it's including the ,;
                                        int vpos = convert(prevpos);
                                       
                                         writer.WriteAttributeString("vpos", vpos.ToString());
                                        
                                         raw = raw.Substring((start+1), raw.Length - (start+1));
                                        start = raw.IndexOf(",");
                                        raw = raw.Substring(start + 1, raw.Length - (start + 1));
                                        start = raw.IndexOf(",");
                                        String id = raw.Substring(0, start);
                                        id = id.Replace(",", "");
                                        int uid = convert(id);

                                        writer.WriteAttributeString("no", uid.ToString());
                                         
                                         //<c p="1nj0,7st,44dr0b,3y,0">超高校級の菓子職人ｗｗｗｗｗｗ</c> 
                                        raw = raw.Substring(start + 1, raw.Length - (start + 1));
                                        start = raw.LastIndexOf(",");
                                        string user = raw.Substring(0, start);
                                        user = user.Replace(",", "");
                                        String true_user = "";
                                        if (ids.ContainsKey(user))
                                        {
                                            true_user = ids[user];
                                        }
                                        if (true_user != "") {

                                            writer.WriteAttributeString("user_id", true_user);
                                        
                                        }


                                     }
                                 
                                 }
                             
                             }

                             break;

                         case XmlNodeType.Text:
                             writer.WriteString(reader.Value);
                             break;

                         case XmlNodeType.EndElement:
                             writer.WriteEndElement();
                             break;
                     
                     
                     
                     
                     
                     }
                 
                 
                 
                 }
                 writer.WriteEndDocument();
                 //writer.WriteEndElement();
                // writer.WriteEndDocument();
                 writer.Close();
                 reader.Close();

              //   MessageBox.Show("xml  complete");
             }
             FileInfo file = new FileInfo(CommentTitle);
             FileInfo basefile = new FileInfo(filestorage_dir + "\\" + CommentTitle);
             if (!basefile.Exists)
             {
                 try
                 {
                     file.CopyTo(filestorage_dir + "\\" + CommentTitle, true);
                 }
                 catch (IOException)
                 {
                     file.CopyTo(filestorage_dir + "\\temp_xml\\" + CommentTitle, true);
                 }
             }
             else {
                 //overwrite if the original file is smaller than the current file
                 //the _new is there because if the comment file is in use it can't be access/ overwrite
                 //rename if the original file is larger than the current file
                 //
                 if (basefile.Length < file.Length)
                 {
                     try
                     {
                         file.CopyTo(filestorage_dir + "\\" + CommentTitle, true);
                     }
                     catch (IOException c)
                     {
                         Console.WriteLine(c);
                         file.CopyTo(filestorage_dir + "\\temp_xml\\" + CommentTitle, true);
                     }

                 }else if(basefile.Length==file.Length){
                     try
                     {
                         file.CopyTo(filestorage_dir + "\\" + CommentTitle, true);
                     }
                     catch (IOException c)
                     {
                         Console.WriteLine(c);
                         file.CopyTo(filestorage_dir + "\\temp_xml\\" + CommentTitle, true);
                     }
                 }
                 else {
                     var msg = MessageBox.Show("The new file is smaller than the original one, Do you want to overwrite it?", "alert", MessageBoxButtons.YesNo);

                     if (msg == DialogResult.Yes) {
                         try
                         {
                             file.CopyTo(filestorage_dir + "\\" + CommentTitle, true);
                         }
                         catch (IOException)
                         {
                             file.CopyTo(filestorage_dir + "\\temp_xml\\" + CommentTitle, true);
                         }
                     }
                     else
                     {
                         try
                         {
                             file.CopyTo(filestorage_dir + "\\temp_xml\\" + CommentTitle, true);
                         }
                         catch (IOException)
                         {
                             file.CopyTo(filestorage_dir + "\\temp_xml\\" + CommentTitle, true);
                         }
                     }
                 
                 }
             
             }
           
                 file.Delete();
            

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
                 case "himado.in":


                     loadInfo_hima(url);

                     break;
                //make himado a case and make default stright dl file

                 default:
                     if (url.IsFile)
                     {
                         normal_download(url);
                     }
                     else
                     {
                         loadInfo(url);
                         title = getTitle();
                         titles.Add(title);
                         getAllUrls();
                     }

                     break;
             
             
             
             }

         
         }
         MessageWindow progressbox;
         void normal_download_ProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
             int progress = e.ProgressPercentage;
             String total = e.TotalBytesToReceive.ToString();
             String current = e.BytesReceived.ToString();
             if (progressbox == null)
             {
                 progressbox = new MessageWindow();
                 // use a new form will be easier then message box
                // progressbox = MessageBox.Show("Downloading: " + current + "/" + total + " " + progress + "%");
                 progressbox.setMessage = "Downloading: " + current + "/" + total + " " + progress + "%";
             }
             else {
                 progressbox.setMessage = "Downloading: " + current + "/" + total + " " + progress + "%";
          
             }

         
         }
         public DownloadProgressChangedEventHandler normal_dlProgress(MessageWindow msg, Uri url) {

             Action<object, DownloadProgressChangedEventArgs> action = (sender, e) =>
             {
                 int progress = e.ProgressPercentage;
                 String total = e.TotalBytesToReceive.ToString();
                 String current = e.BytesReceived.ToString();

                 msg.setMessage = "Downloading: "+url.AbsolutePath+Environment.NewLine + current + "/" + total + " " + progress + "%";

                 



             };
         return new DownloadProgressChangedEventHandler(action);
         
         }
         void normal_download(Uri url) {
             if (url.IsFile)
             {
                 String filename = url.OriginalString;
                 int left = filename.LastIndexOf("/")+1;
                 filename = filename.Substring(left,filename.Length-left);
                 filename = filestorage_dir +"\\"+ filename;
                 //if the url is a file instead of a webpage
                 using (WebClient wb = new WebClient()){
                     MessageWindow msg = new MessageWindow();
                     wb.DownloadFileAsync(url, filename);
                     wb.DownloadProgressChanged += normal_dlProgress(msg,url);
                 }


             }
             else {
                 loadInfo(url);
                 title = getTitle();
                 titles.Add(title);
                 // most likely won't work but still put it in just in case
                 getAllUrls();
             
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


             WebClient wb = new WebClient();
             


                 FileInfo file = new FileInfo(filestorage_dir + "\\" + title);

                 //download the .xml file as the title name +.xml into the default folder 
                 if (!file.Exists)
                 {
                     wb.DownloadFileAsync(new Uri(link), file.FullName + ".xml");
                 }
                 else
                 {
                     wb.DownloadFileAsync(new Uri(link), file.FullName + "(S).xml");

                 }
                 wb.Dispose();

             
          
         
         
         
         
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
        CookieCollection cookie;
        void loadInfo_hima(Uri url) {
            using (WebClient loadcontent = new WebClient())
            {
                loadcontent.Encoding = Encoding.UTF8;

                //loadcontent.DownloadStringCompleted += downloadString(url);
                fullcontent = loadcontent.DownloadString(url);

                title = getTitle();
                titles.Add(title);
                getAllUrls();
                getCommentUri(url);
                loadcontent.Dispose();
            }
          
        
        
        
        }
        public DownloadStringCompletedEventHandler downloadString(Uri url) {

            Action<object, DownloadStringCompletedEventArgs> action = (sender, e) =>
            {
           
                WebClient wb = sender as WebClient;
                if (e.Error!=null) {
                    wb.CancelAsync();
                    MessageBox.Show("Something went wrong with the website");
                }
                fullcontent = e.Result;
                if (fullcontent != null)
                {
                    title = getTitle();
                    titles.Add(title);
                    getAllUrls();


                    wb.Dispose();
                    getCommentUri(url);
                }

            };
        return new DownloadStringCompletedEventHandler(action);
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
                cookie = wresponse.Cookies;
                wresponse.Close();
                readers.Close();



            }

        }
        void Alter_loadInfo(Uri url)
        {


            HttpWebRequest wrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse wresponse = (HttpWebResponse)wrequest.GetResponse();

            //if you get response from the http site
            if (wresponse.StatusCode == HttpStatusCode.OK)
            {

                Stream recives = wresponse.GetResponseStream();
                StreamReader readers = null;

                //see if the page is unicoded
                if (wresponse.CharacterSet == null)
                {

                    readers = new StreamReader(recives);
                }
                else
                {
                    //if unicoded

                    readers = new StreamReader(recives, Encoding.GetEncoding(wresponse.CharacterSet));

                    code = Encoding.GetEncoding(wresponse.CharacterSet);
                }
                //could use read line to get an array of results?
                fullcontent = readers.ReadToEnd();
                cookie = wresponse.Cookies;
               
                alter = saymove();


                wresponse.Close();
                readers.Close();




            }

        }
        public String saymoveAlter { 
        get{return alter;}
            set { alter = value; }
        
        
        }
        String alter;
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
             string result = "http://say-move.org/fc2/api/fc2flvPath.php?gk=&up_id=";
             //string result = "http://say-move.org/fc2/api/fc2flvPath.php?gk=&up%5Fid=";
            // string result = "http://say-move.org/fc2/api/fc2flvPath.php?gk=&up5Fid=";
             
             result = result.Replace("gk=", "gk=" + key);
             result = result.Replace("id=","id="+id);
             return result;
         
         }
//GET /fc2/api/fc2flvPath.php?gk=CyszM5LQbASGxNWL&up%5Fid=20160723NH71111q HTTP/1.1
//Host: say-move.org
//Connection: keep-alive
//Cache-Control: max-age=0
//Upgrade-Insecure-Requests: 1
//User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36
//Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
//DNT: 1
//Accept-Encoding: gzip, deflate, sdch
//Accept-Language: en-US,en;q=0.8,ja;q=0.6,zh-TW;q=0.4,zh;q=0.2
//Cookie: open_category_menu=1; fc2cnt_862=1-1468124524; _pk_ref.4.c1cd=%5B%22%22%2C%22%22%2C1469435800%2C%22http%3A%2F%2Fhimado.in%2F%3Fsort%3Dmovie_id%26mode%3Dsearch%22%5D; __utma=64400813.163552657.1468117240.1468817630.1469435800.3; __utmc=64400813; __utmz=64400813.1469435800.3.2.utmcsr=himado.in|utmccn=(referral)|utmcmd=referral|utmcct=/; globalmode=0; fc2cnt_984716=1-1469435799; fc2_analyzer_697394=1-872535462-1468117281-1469436460-18-4-1469435815; FC2ANASESSION697394=46071979; fc2cnt_861=1-1469435844; _ga=GA1.2.163552657.1468117240; _pk_id.4.c1cd=bb583a982d36ded4.1468117240.4.1469436462.1469435800.; _pk_ses.4.c1cd=*; lang=ja

        public string saymoveAP2(){

            String truelink = fullcontent;

            int start = fullcontent.IndexOf("og:image");
            truelink = truelink.Substring(start, truelink.Length - start);
            start = truelink.IndexOf("http");
            truelink = truelink.Substring(start, truelink.Length - start);
            int end = truelink.IndexOf("\"");
            truelink = truelink.Substring(0, end);

            String exchange = truelink.Replace("thumb", "flv");
            exchange = exchange.Replace(".jpg", ".flv");
            string test = "";

            WebClient wb = new WebClient();
       
                //http://vip.video45000.fc2.com/up/flv/201607/23/N/20160723NH71111q.flv?mid=fbe6f6e49cde6e1ba13e68efe3b07e94
                //mid=fbe6f6e49cde6e1ba13e68efe3b07e94

                test = wb.DownloadString(exchange);



                
            
            
         




            return test;
        
        
        }

         public  string saymovefc2(string url,string gk ,string id) {

             
             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
             //responese=wb.ResponseHeaders.ToString();
             WebHeaderCollection wa = new WebHeaderCollection();
         //    wa.Add(HttpRequestHeader.Host, "say-move.org");
             String temp = "/fc2/api/fc2flvPath.php?gk=&up%5Fid=";
             temp = temp.Replace("gk=", "gk=" + gk);
             temp = temp.Replace("id=", "id=" + id);
          //   wa.Add("GET", temp);
        //     wa.Add("Host", "say-move.org");
          
             wa.Add("X-Request-With", "ShockwaveFlash/22.0.0.209");
             wa.Add("Upgrade-Insecure-Requests", "1");
             wa.Add("Cache-Control", "max-age=0");
             wa.Add("DNT", "1");
            // wa.Add("Accept-Encoding", "gzip, deflate, sdch");
        //     wa.Add("Accept-Language", "en-US,en;q=0.8,ja;q=0.6,zh-TW;q=0.4,zh;q=0.2");
           //  wa.Add("Referer", this.websiteLink.AbsolutePath);
         //    wa.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
             wa.Add("Cookie", " open_category_menu=1; fc2cnt_862=1-1468124524; _pk_ref.4.c1cd=%5B%22%22%2C%22%22%2C1469435800%2C%22http%3A%2F%2Fhimado.in%2F%3Fsort%3Dmovie_id%26mode%3Dsearch%22%5D; __utma=64400813.163552657.1468117240.1468817630.1469435800.3; __utmc=64400813; __utmz=64400813.1469435800.3.2.utmcsr=himado.in|utmccn=(referral)|utmcmd=referral|utmcct=/; globalmode=0; fc2cnt_984716=1-1469435799; fc2_analyzer_697394=1-872535462-1468117281-1469436460-18-4-1469435815; FC2ANASESSION697394=46071979; fc2cnt_861=1-1469435844; _ga=GA1.2.163552657.1468117240; _pk_id.4.c1cd=bb583a982d36ded4.1468117240.4.1469436462.1469435800.; _pk_ses.4.c1cd=*; lang=ja");
            wa.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
             //  wa.Add(HttpRequestHeader.Connection, "keep-alive");
             wa.Add(HttpRequestHeader.Referer, this.websiteLink.OriginalString);
            wa.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
             wa.Add(HttpRequestHeader.Accept, "*/*");
            //  wa.Add(HttpRequestHeader.Cookie, "open_category_menu=1; fc2cnt_862=1-1468124524; __utma=64400813.163552657.1468117240.1468817630.1469435800.3; __utmc=64400813; __utmz=64400813.1469435800.3.2.utmcsr=himado.in|utmccn=(referral)|utmcmd=referral|utmcct=/; globalmode=0; lang=ja; zHADmJDY=ewtM1aRpc5KcQ31H; fc2cnt_984716=1-1469525214; fc2_analyzer_697394=1-872535462-1468117281-1469547263-21-4-1469435815; FC2ANASESSION697394=49161928; fc2cnt_861=1-1469547264; _pk_ref.4.c1cd=%5B%22%22%2C%22%22%2C1469547283%2C%22http%3A%2F%2Fhimado.in%2F%3Fsort%3Dmovie_id%26mode%3Dsearch%22%5D; _pk_id.4.c1cd=bb583a982d36ded4.1468117240.7.1469547283.1469547283.; _pk_ses.4.c1cd=*; _gat=1; _gat_generalPC=1; _ga=GA1.2.163552657.1468117240");

             wa.Add(HttpRequestHeader.AcceptEncoding, "gzip");
             wa.Add(HttpRequestHeader.AcceptLanguage, "ja;q=0.6");
             System.Collections.Specialized.NameValueCollection qstring = new System.Collections.Specialized.NameValueCollection();
             qstring.Add("gk", gk);
             qstring.Add("up_id", id);

             CookieCollection ck = new CookieCollection();
             Cookie a = new Cookie("opencategory_menu", "1");
             ck.Add(a);
             a=new Cookie("fc2cnt_862","1-1468124524");
             //request.Headers = wa;
             
             
             using (WebClient wb = new WebClient()) {
                 wb.Headers = wa;
                 ICredentials ic = new NetworkCredential();
                 wb.Encoding = ASCIIEncoding.UTF8;
                 wb.QueryString = qstring;
                 String response = "";
                 
                 wb.BaseAddress=(this.websiteLink.OriginalString);
              response=   wb.DownloadString(temp);

                     return response;
     
             
             }
             
             request.Method = "GET";
             request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
             request.Host = "say-move.org";
             request.KeepAlive = true;
            // request.Connection = "keep-alive";
             //request.Referer = "http://say-move.org/comeplay.php?comeid=1620354";
             request.Referer = this.websiteLink.OriginalString;
             request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
             request.MaximumResponseHeadersLength = 300;
             byte[] _byte = Encoding.ASCII.GetBytes(string.Concat("content=", url));
             //in case if content length need to be filled
             //request.GetResponse();
             String fullc = "";
          
             //  Stream response = request.GetRequestStream();
             if (request.HaveResponse)
             {

                 HttpWebResponse wrespones = (HttpWebResponse)request.GetResponse();

                 using (StreamReader reader = new StreamReader(wrespones.GetResponseStream()))
                 {

                     fullc = reader.ReadToEnd();

                     reader.Close();
                 }


             }
             else {

                 return "not found 404";
             
             }
             //if everything went well then fullc will have the text that contains the true video address
             //for the video  call dl from here
             //
             return fullc;

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
         public String RenameExistFile(String dir, String filename) {
             if (checkFileExist(dir, filename))
             {
                 bool a = filename.Contains("(");
                 bool b = filename.Contains(")");
                 String newFilename = filename;
                 int number = 0;
                 if (a && b)
                 {

                     if (filename.LastIndexOf(")") == filename.Length - 1)
                     {
                         //this means that it's most likely been renamed by this program before
                         int left = filename.LastIndexOf("(") + 1;
                         int right = filename.LastIndexOf(")");
                         //find the current number and then delete the whole (*) part;
                         number = Int32.Parse(filename.Substring(left, right - left));
                         newFilename = filename.Replace(" (" + number + ")", "");
                     }
                 }

                 number++;
                 newFilename = newFilename + " (" + number + ")";
                 if (!checkFileExist(dir, newFilename))
                 {
                     return newFilename;
                 }
                 else
                 {
                     //this will keep going until it get's a unused name
                     return RenameExistFile(dir, newFilename);
                 }
             }
             else {
                 return filename;
             }
         
         }

         public bool checkFileExist(String dir, String filename) {
             

             if(File.Exists(dir+"\\"+filename)){
             return true;
             }else{
             return false;
             }
         
         }
        // other version of download file for form interaction
         public void downlaodFile(Uri filepath, WebClient wb, String titlename,List<String[]> Media_List) {

            //  String fileU = filepath.AbsolutePath;
             //this is the actual download path

             //there's no need to get the downlaod file name in this because it's going to be the title name
             DirectoryInfo maindir = new DirectoryInfo(filestorage_dir);
             String CurrentDir = "";
             //make sure the directory is not  null or empty string
             if (maindir.Exists)
             {
                 CurrentDir = maindir.FullName;
             }
             else {
                 // try to create the dir if it doesn't exist, if error then ask for a dir
                 try
                 {
                     maindir.Create();
                 }
                 catch (Exception e)
                 {
                     Console.WriteLine(e);
                     var temp = new FolderBrowserDialog();
                     if (temp.ShowDialog() == DialogResult.OK)
                     {
                         CurrentDir = new FolderBrowserDialog().SelectedPath;
                     }
                 }
             }
             if (CurrentDir != "") {
                 String filename = safeFilename(titlename);

                 if (!checkFileExist(CurrentDir, filename))
                 {
                    // filename = filename;
                     FileInfo dltemp = new FileInfo(CurrentDir + "\\" + filename);
                     wb.DownloadFileAsync(filepath,@dltemp.FullName);
                     dlfilepath = dltemp.FullName;
                 }
                 else
                 {
                     //the old file
                     FileInfo oldfile = new FileInfo(CurrentDir + "\\" + filename);
                     //if the old file is larger than 500kb then count it as dulplicate
                     if (oldfile.Length > 500000)
                     {
                         var alert = MessageBox.Show("the file with the same name already exist in the directory, are you sure you want to dl it ?", "alert", MessageBoxButtons.YesNo);
                         if (alert == DialogResult.Yes)
                         {

                             filename = RenameExistFile(CurrentDir, filename);

                             FileInfo dltemp = new FileInfo(oldfile.DirectoryName + "\\" + filename);

                             wb.DownloadFileAsync(filepath, CurrentDir+"\\"+filename);

                             dlfilepath = dltemp.FullName;
                             //throw new Exception("test");
                         }
                         else
                         {
                             // not sure dlfilepath do here so leave it empty

                         }
                     }
                     else {
                         try
                         {
                             oldfile.Delete();
                         }
                         catch (IOException) { };
                         FileInfo dltemp = new FileInfo(CurrentDir + "\\" + filename);

                         wb.DownloadFileAsync(filepath, CurrentDir + "\\" + filename);
                         dlfilepath = dltemp.FullName;
                        // throw new Exception("test");
                     }
                 }


                 wb.DownloadFileCompleted += DownloadComplete(CurrentDir,titlename,filename, Media_List);

             
             
             
             }
          
             wb.Dispose();


         
         }
         public System.ComponentModel.AsyncCompletedEventHandler DownloadComplete(String dir,String title,String newtitle,List<String[]> Media_List) {

             Action<object, System.ComponentModel.AsyncCompletedEventArgs> action = (sender, e) =>
             {
                 //do the error handling outside ?
                 String tempT = safeFilename(title);
                 FileInfo newfile = new FileInfo(dir + "\\" + newtitle);

                 if (e.Cancelled) {
                     if (newfile.Exists)
                     {
                         newfile.Delete();
                     }
                 
                 }

                 if (tempT == newtitle)
                 {
                     //if no rename happened
                     //do nothing ?
                     String[] temp = { newfile.FullName, newfile.Name };
                     if (!Media_List.Contains(temp)) {
                         Media_List.Add(temp);
                     }
                     
                 }
                 else { 
                 // if the file is renamed to other names
                 // check to see if the newest file is larger than the old file
                // if it's larger then delete the old file
                     FileInfo oldfile =new FileInfo(dir + "\\" + tempT);

                     if (oldfile.Exists)
                     {

                         if (oldfile.Length < newfile.Length)
                         {
                             try
                             {
                                 oldfile.Delete();
                                 newfile.CopyTo(oldfile.FullName);
                                 newfile.Delete();

                                 String[] temp = { oldfile.FullName, oldfile.Name };
                                 if (!Media_List.Contains(temp))
                                 {
                                     Media_List.Add(temp);
                                 }
                             }
                             catch (IOException) {
                                 String[] temp = { newfile.FullName, newfile.Name };
                                 if (!Media_List.Contains(temp))
                                 {
                                     Media_List.Add(temp);
                                 }
                             }

                         }
                         else {

                             String[] temp = { newfile.FullName, newfile.Name };
                             if (!Media_List.Contains(temp))
                             {
                                 Media_List.Add(temp);
                             }
                         
                         }
                  
                     }
                   
                 
                 }




             };

             return new System.ComponentModel.AsyncCompletedEventHandler(action);
         }
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

                 filename = filename.Replace(":", "");
                 filename = filename.Replace("\\", "");
                 filename = filename.Replace("*", "");
                 filename = filename.Replace("|", "");
                 filename = filename.Replace("\"", "");
                 filename = filename.Replace("?", "");
                 filename = filename.Replace("<", "");
                 filename = filename.Replace(">", "");
                 
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

                             //wb.DownloadFileAsync(filepath, @newfile.FullName);
                             dlfilepath = newfile.FullName;
                         }
                     }
                 }
                 else
                 {


                     FileInfo newfile = new FileInfo(ndir.FullName + "\\" + filename);

                     // if the file exists then stop download //keep download and overwrite
                     if (!newfile.Exists)
                     {
                         wb.DownloadFileAsync(filepath, @newfile.FullName);


                         dlfilepath = newfile.FullName;

                     }
                     else
                     {
                         wb.DownloadFileAsync(filepath, @newfile.FullName);
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

             get {
                 if (dlfilepath != null)
                 {
                     return new Uri(dlfilepath);
                 }
                 else {
                     return null;
                 }
             }
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
