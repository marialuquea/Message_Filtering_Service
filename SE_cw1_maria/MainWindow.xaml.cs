
using System.Collections.Generic;
using System.Windows;
using System.IO;
using Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Linq;
using System.Windows.Input;
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SE_cw1_maria
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> abb = new List<string>();
        List<string> def = new List<string>();
        List<string> quarantineList = new List<string>();
        Dictionary<string, string> SIR = new Dictionary<string, string>();
        List<string> mentions = new List<string>();
        Dictionary<string, int> hashtags = new Dictionary<string, int>();
        List<Message> data_out = new List<Message>();
        List<Message> data_in = new List<Message>();

        public MainWindow()
        {
            InitializeComponent();
            
            // ABREVIATIONS - read file
            using (var reader = new StreamReader(@"../../../textwords.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    abb.Add(values[0]);
                    def.Add(values[1]);
                }
            }
        }

        // UPLOAD FILE TO LISTBOX
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string ext = Path.GetExtension(openFileDialog.FileName);

                    if (ext.Equals(".json"))
                    {
                        System.IO.StreamReader fileJson = new System.IO.StreamReader(openFileDialog.FileName);
                        
                        string lineJson = "";
                        string tempObj = "";

                        while ((lineJson = fileJson.ReadLine()) != null)
                        {
                            string[] objects = lineJson.Split('}');
                            
                            foreach (string obj in objects)
                            {
                                if (obj == String.Empty)
                                    break;

                                // JObject json1 = JObject.Parse(obj);
                                // dynamic json = JsonConvert.DeserializeObject(obj);
                                // string id = (string)json["id"][0];
                                
                                string id_body = obj.Substring(obj.IndexOf("id")); // id and body
                                string id = id_body.Split(',')[0]; // id
                                tempObj = obj + "}"; // THE JSON STRING

                                Message message = new Message();

                                if (Regex.IsMatch(id, "S"))
                                {
                                    // var sms1 = JsonConvert.DeserializeObject<List<Sms>>(tempObj);
                                    var des = (Sms)Newtonsoft.Json.JsonConvert.DeserializeObject(tempObj, typeof(Sms));

                                    Sms sms = des;

                                    message.id = sms.id;
                                    message.body = sms.body;

                                    JsonLines.Items.Add(sms.id);
                                    data_in.Add(message);
                                }
                                else if (Regex.IsMatch(id, "E"))
                                {
                                    Email email = JsonConvert.DeserializeObject<Email>(tempObj);

                                    message.id = email.id;
                                    message.body = email.body;

                                    JsonLines.Items.Add(email.id);
                                    data_in.Add(message);
                                }
                                else if (Regex.IsMatch(id, "T"))
                                {
                                    Tweet tweet = JsonConvert.DeserializeObject<Tweet>(tempObj);

                                    message.id = tweet.id;
                                    message.body = tweet.body;

                                    JsonLines.Items.Add(tweet.id);
                                    data_in.Add(message);
                                }
                                else
                                {
                                    MessageBox.Show("ID is in wrong format.");
                                }
                            }
                        }

                    }
                    else if (ext.Equals(".txt"))
                    {
                        string json = File.ReadAllText(openFileDialog.FileName);
                       
                        foreach (string line in File.ReadAllLines(openFileDialog.FileName))
                        {
                            lines.Items.Add(line);
                        }
                    }
                }
                else
                {
                    label.Text = "Your file is empty.";
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            } 
        }

        // READ LINE OF FILE
        private void lines_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Message message = new Message();
                
                string line = Convert.ToString(lines.SelectedItem);

                message.id = line.Split(' ')[0].ToUpper(); // MESSAGE ID
                    
                // if the message header has first a letter and then 9 numbers:
                int i = line.IndexOf(" ") + 1;
                string str = line.Substring(i); // delete the first word
                message.body = str;

                if ((message.id)[0].Equals('S')) { sms_process(message); }
                else if ((message.id)[0].Equals('E')) { email_process(message); }
                else if ((message.id)[0].Equals('T')) { tweet_process(message); }
                else { MessageBox.Show("Insert a valid ID please starting with either S, E or T."); }
                    
            }
            catch (ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
        }

        // ADD JSON IDs TO PROCESS
        private void JsonLines_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // read id
                string line = Convert.ToString(JsonLines.SelectedItem);

                // search for object with that id in data in
                foreach (Message message in data_in)
                {
                    if (line.Equals(message.id))
                    {
                        // depending on type, select process
                        if ((message.id)[0].Equals('S')) { sms_process(message); }
                        else if ((message.id)[0].Equals('E')) { email_process(message); }
                        else if ((message.id)[0].Equals('T')) { tweet_process(message); }
                    }
                }
            }
            catch (Exception b)
            {
                MessageBox.Show(b.Message);
            }

            
        }

        // CLICK BUTTON TO PROCESS MESSAGE
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Message message = new Message();

            // IF TEXTBOXES ARE NOT EMPTY
            if (!(txtBoxbody.Text).Equals("") && !(txtBoxheader.Text).Equals(""))
            {
                string header = txtBoxheader.Text.ToUpper();
                string body = txtBoxbody.Text;
                
                try
                {
                    message.id = header;
                    message.body = body;

                    // HEADER OPTIONS - S, E or T
                    if (header[0].Equals('S')) { sms_process(message); }
                    else if (header[0].Equals('E')) { email_process(message); }
                    else if (header[0].Equals('T')) { tweet_process(message); }
                }
                catch (ArgumentException ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
            else
            {
                MessageBox.Show("Fill in both boxes pls");
            }
        }
        
        private void sms_process(Message message)
        {
            Sms sms = new Sms();

            // SENDER - int
            try
            {
                sms.id = message.id;
                sms.body = message.body;
                sms.Sender = (sms.body).Substring(0, (sms.body).IndexOf(" ")); // first word (number)
                int i = sms.body.IndexOf(" ") + 1;
                string str = sms.body.Substring(i); // delete the first word
                sms.Text = str;

                // ABBREVIATIONS
                string newM = abbreviations(sms.Text);
                sms.Text = newM;

                // OUTPUT TO FILE
                data_out.Add(sms);
                JsonSave save = new JsonSave();
                save.outputFile(data_out);

                // SHOW RESULTS
                label.Text = "ID: " + sms.id;
                label2.Text = "Sender: " + sms.Sender;
                label3.Text = "Text: " + sms.Text;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private void email_process(Message message)
        {
            Email email = new Email();

            List<string> incidentList = new List<string>();
            incidentList.Add("theft");
            incidentList.Add("staffattack");
            incidentList.Add("atmtheft");
            incidentList.Add("raid");
            incidentList.Add("customerattack");
            incidentList.Add("staffabuse");
            incidentList.Add("bombthreat");
            incidentList.Add("terrorism");
            incidentList.Add("suspiciousincident");
            incidentList.Add("intelligence");
            incidentList.Add("cashloss");

            //string sentence = "maria@gmail.com ,SIRhello, 99-99-99 ,Theft, Hi";
            string sentence = message.body;
            
            try
            {
                email.id = message.id; // ID
                email.body = message.body; // BODY 
                email.Sender = sentence.Split(',')[0]; //SENDER
                email.Subject = sentence.Split(',')[1]; // SUBJECT

                // SIGNIFICANT INCIDENT REPORT
                if ((email.Subject).Contains("SIR")) 
                {
                    email.Text =
                        sentence.Split(',')[2] + ", " +
                        sentence.Split(',')[3] + ", " +
                        sentence.Split(',')[4];

                    Boolean found = false;

                    // check nature of incident
                    foreach (string incident in incidentList)
                    {
                        string word = (((email.Text).Split(',')[1]).ToLower());
                        word = Regex.Replace(word, @"\s+", "");

                        if (word.Equals(incident))
                        {
                            SIR.Add((email.Text).Split(',')[0], (email.Text).Split(',')[1]);
                            sirList.Items.Add((email.Text).Split(',')[0] + ", " + (email.Text).Split(',')[1]);
                            found = true;
                        }
                    }

                    if (!found)
                        MessageBox.Show("Nature of incident not found.");
                    
                }
                // STANDARD EMAIL MESSAGE
                else
                {
                    //string sentence = "maria@gmail.com 12345678901234567890 hello this is the text";

                    // TEXT - max of 1028 chars
                    email.Text = (email.body).Split(',')[2];
                }

                // URLs 
                email.Text = url_search(email.Text);
                
                // SHOW INFO
                label.Text = "Sender: " + email.Sender;
                label2.Text = "Subject: " + email.Subject;
                label3.Text = "Text: " + email.Text;
                
                // SAVE IN JSON FILE
                data_out.Add(email);
                JsonSave save = new JsonSave();
                save.outputFile(data_out);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private void tweet_process(Message message)
        {
            try
            {
                Tweet tweet = new Tweet();
                tweet.id = message.id;
                tweet.body = message.body;

                // SENDER - max 15 chars, starts with @
                tweet.Sender = message.body.Substring(0, (message.body).IndexOf(" "));

                // TEXT - max 140 chars
                int i = message.body.IndexOf(" ") + 1;
                string str = message.body.Substring(i);
                tweet.Text = str;

                // SEARCH THROUGH WORDS 
                string sentence = tweet.Text;

                foreach (string word in (sentence).Split(' '))
                {
                    // If there is a mention
                    if (word.StartsWith("@"))
                    {
                        if (!mentions.Contains(word))
                        {
                            mentions.Add(word);
                            mentionList.Items.Add(word);
                        }
                    }

                    // If there is a hashtag
                    if (word.StartsWith("#"))
                    {
                        if (hashtags.ContainsKey(word))
                        {
                            hashtags[word] += 1;
                        }
                        else
                        {
                            hashtags.Add(word, 1);
                        }
                    }
                }

                // ABBREVIATIONS
                string newM = abbreviations(tweet.Text);
                tweet.Text = newM;

                //Output file
                data_out.Add(tweet);
                JsonSave save = new JsonSave();
                save.outputFile(data_out);

                // SHOW RESULTS
                label.Text = "ID: " + tweet.id;
                label2.Text = "Sender: " + tweet.Sender;
                label3.Text = "Text: " + tweet.Text;

                // TRENDING LIST
                trendList.Items.Clear();
                var sortedDict = hashtags.OrderBy(x => x.Value);
                foreach (var item in sortedDict.OrderByDescending(key => key.Value))
                {
                    trendList.Items.Add(item);
                }
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private string url_search(string sentence)
        {
            try
            {
                foreach (string word in sentence.Split(' '))
                {
                    if (word.StartsWith("http:") || word.StartsWith("https:"))
                    {
                        string newM = sentence.Replace(word, "<URL Quarantined>");
                        sentence = newM;
                        quarantineList.Add(word);
                        qList.Items.Add(word);
                    }
                }
                return sentence;
            }
            catch
            {
                return sentence;
            }
        }
        
        private string abbreviations(string sentence)
        {
            try
            {
                foreach (string word in (sentence).Split(' '))
                {
                    foreach (string abr in abb)
                    {
                        if (word.Equals(abr))
                        {
                            // Find the definition
                            int index = abb.IndexOf(abr);
                            string all = def[index];

                            // Replace word for actual words
                            string words = word + " <" + all + ">";

                            int index2 = sentence.IndexOf(word);

                            char wordAfter;
                            string wordAfter2;

                            try // if it's not the last word
                            {
                                wordAfter = sentence[index2 + 1 + word.Length];

                                wordAfter2 = wordAfter + "";

                                if (wordAfter2.Contains("<"))
                                {
                                    break;
                                }
                                else
                                {
                                    string newM = (sentence).Replace(word, words);
                                    sentence = newM;
                                }
                            }
                            catch // if it is the last word
                            {
                                string newM = (sentence).Replace(word, words);
                                sentence = newM;
                            }
                        }
                    }
                }
                return (sentence);
            }
            catch
            {
                return sentence;
            }
            
        }
        
    }
}
