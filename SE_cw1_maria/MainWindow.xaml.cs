
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Text.RegularExpressions;
using System;
using Data;
using Microsoft.Win32;
using Newtonsoft.Json;

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
        List<object> data = new List<object>();

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

        // UPLOAD FILES
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
                string json = File.ReadAllText(openFileDialog.FileName);
                data = JsonConvert.DeserializeObject<List<object>>(json);

                foreach (var item in data)
                {
                    // if ID starts with S go to sms_process()
                }
            }
            else
            {
                label.Content = "You did not choose a file idiot.";
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

                message.id = header;
                message.body = body;
                
                // HEADER OPTIONS - S, E or T
                if (header[0].Equals('S')) { sms_process(message); }
                else if (header[0].Equals('E')) { email_process(message); }
                else if (header[0].Equals('T')) { tweet_process(message); }
                else { MessageBox.Show("Insert a valid ID please starting with either S, E or T."); }
            }
            else
            {
                MessageBox.Show("Fill in both boxes pls");
            }
        }
        
        private void sms_process(Message message)
        {

            Sms sms = new Sms();
            sms.id = message.id;
            sms.body = message.body;

            // SENDER - int
            sms.Sender = (sms.body).Substring(0, (sms.body).IndexOf(" ")); // first word (number)
            
            // TEXT - max 140 characters
            if (sms.body.Length > 0)
            {
                int i = sms.body.IndexOf(" ") + 1;
                string str = sms.body.Substring(i); // delete the first word
                sms.Text = str;
            }
            else
            {
                MessageBox.Show("Text in sms is empty.");
            }

            // ABBREVIATIONS
            string newM = abbreviations(sms.Text);
            sms.Text = newM;

            // OUTPUT TO FILE
            data.Add(sms);
            JsonSave save = new JsonSave();
            save.outputFile(data);

            // SHOW RESULTS
            label.Content = "ID: " + sms.id;
            label2.Content = "Sender: " + sms.Sender;
            label3.Content = "Text: " + sms.Text;
        }

        private void email_process(Message message)
        {
            Email email = new Email();
            
            //string sentence = "maria@gmail.com SIRhello 99-99-99 ,Theft, Hi";
            string sentence = message.body;

            email.id = message.id; // ID
            email.body = message.body; // BODY 
            email.Sender = sentence.Split(' ')[0]; //SENDER
            email.Subject = sentence.Split(' ')[1]; // SUBJECT

             if ((email.Subject).StartsWith("SIR")) // Significant Incident Report
            {
                email.Text =
                    sentence.Split(' ')[2] + ", " +
                    sentence.Split(',')[1] + ", " +
                    sentence.Split(',')[2];  // TEXT
                try
                {
                    SIR.Add((email.Text).Split(',')[0], (email.Text).Split(',')[1]);
                    sirList.Items.Add((email.Text).Split(',')[0] +", "+ (email.Text).Split(',')[1]);
                }
                catch
                {
                    // item with same key has been added
                }
            }
            else // Standard email message
            {
                //string sentence = "maria@gmail.com 12345678901234567890 hello this is the text";

                // SUBJECT - 20 chars
                int i = sentence.IndexOf(" ") + 1;
                string str = sentence.Substring(i); // delete the sender
                email.Subject = str.Substring(0, 20); // gets the subject
                
                // TEXT - max of 1028 chars
                email.Text = (str).Remove(0, 20); //deletes 20 characters which are the subject
            }

            // URLs 
            email.Text = url_search(email.Text);

            // SHOW INFO
            label.Content = "Sender: " + email.Sender;
            label2.Content = "Subject: " + email.Subject;
            label3.Content = "Text: " + email.Text;

            // SAVE IN JSON FILE
            data.Add(email);
            JsonSave save = new JsonSave();
            save.outputFile(data);
        }

        private void tweet_process(Message message)
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
                    mentions.Add(word);
                    mentionList.Items.Add(word);
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
            data.Add(tweet);
            JsonSave save = new JsonSave();
            save.outputFile(data);

            // SHOW RESULTS
            label.Content = "ID: " + tweet.id;
            label2.Content = "Sender: " + tweet.Sender;
            label3.Content = "Text: " + tweet.Text;

            // TRENDING LIST
            trendList.Items.Clear();
            foreach (var item in hashtags)
            {
                trendList.Items.Add(item);
            }
        }

        private string url_search(string sentence)
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
        
        private string abbreviations(string sentence)
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
                       
                        char wordAfter = sentence[index2 + 1 + word.Length];

                        string wordAfter2 = wordAfter + "";
                            
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
                }
            }
            return (sentence);
        }
        
    }
}
