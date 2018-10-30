using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
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
        List<Sms> smsList = new List<Sms>(); 

        public MainWindow()
        {
            InitializeComponent();

            // READ JSON FILES
            





            // ABREVIATIONS - read file
            using (var reader = new StreamReader(@"textwords.csv"))
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Message message = new Message();

            if (!(txtBoxbody.Text).Equals("") && !(txtBoxheader.Text).Equals(""))
            {
                string header = txtBoxheader.Text.ToUpper();
                string body = txtBoxbody.Text;

                message.id = header;
                message.body = body;
                

                if (header[0].Equals('S'))
                {
                    sms_process(message);
                }
                else if (header[0].Equals('E'))
                {
                    email_process(message);
                }
                else if (header[0].Equals('T'))
                {
                    tweet_process(message);
                }
                else
                {
                    MessageBox.Show("Insert a valid ID please starting with either S, E or T.");
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

            sms.id = message.id;
            sms.body = message.body;

            // SENDER - int
            sms.Sender = Convert.ToInt32((message.body).Substring(0, (message.body).IndexOf(" ")));
            label.Content = sms.Sender;

            // TEXT - max 140 characters
            if (message.body.Length > 0)
            {
                int i = message.body.IndexOf(" ") + 1;
                string str = message.body.Substring(i);
                sms.Text = str;
                label2.Content = sms.Text;
            }
            else
            {
                MessageBox.Show("Text in sms is empty.");
            }

            // ABBREVIATIONS
            string newM = abbreviations(sms.Text);

            MessageBox.Show(newM);
            sms.Text = newM;

            outputFile(sms);
        }

        private void email_process(Message message)
        {
            Email email = new Email();

            // SENDER - email
            email.Subject = message.body.Substring(0, (message.body).IndexOf(" ")); // get first word
            label.Content = email.Subject;

            // SUBJECT - 20 chars
            int i = message.body.IndexOf(" ") + 1;
            string str = message.body.Substring(i); // deletes first word (email sender)
            email.Subject = str.Substring(0, 20); // gets the subject
            label2.Content = email.Subject; // shows subject

            // TEXT - max of 1028 chars
            email.Text = str.Remove(0, 20); //deletes 20 characters which are the subject
            label3.Content = email.Text; // shows text

            email.id = message.id;
            email.body = message.body;

            outputFile(email);

        }

        private void tweet_process(Message message)
        {
            Tweet tweet = new Tweet();

            // SENDER - max 15 chars, starts with @
            tweet.Sender = message.body.Substring(0, (message.body).IndexOf(" "));
            label.Content = tweet.Sender;

            // TEXT - max 140 chars
            int i = message.body.IndexOf(" ") + 1;
            string str = message.body.Substring(i);
            tweet.Text = str;
            label2.Content = tweet.Text;

        }

        private string abbreviations(string message)
        {
            MessageBox.Show(message);
            foreach (string word in message.Split(' '))
            {
                foreach (string abr in abb)
                {
                    if (word.Equals(abr))
                    {
                        // Find the actual words
                        int index = abb.IndexOf(abr);
                        string all = def[index];

                        // Replace word for actual words
                        string words = "<" + all + ">";
                        string newM = (message).Replace(word, words);
                        return newM; 

                        //sms.Text = newM;
                        //message.body = newM;
                        //label.Content = word;
                        //label3.Content = message.body;
                    }
                    else
                    {
                        return message;
                    }
                }
            }
            return message;
        }

        private void outputFile(object message)
        {
            using (StreamWriter file = File.CreateText(@"../../../output.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, message);
            }
        }

        private void LoadJson()
        {
            using (StreamReader r = new StreamReader(@"../../../output.json"))
            {
                string json = r.ReadToEnd();
                List<Email> emails = JsonConvert.DeserializeObject<List<Email>>(json); 
            }
        }

    }
}
