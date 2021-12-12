using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Erin_2._0
{
    public partial class Form1 : Form
    {
        int time = 1;
        bool listening = false;
        SpeechRecognitionEngine KeyWord = new SpeechRecognitionEngine();
        SpeechRecognitionEngine listener = new SpeechRecognitionEngine();
        SpeechRecognitionEngine musicchoice = new SpeechRecognitionEngine();
        SpeechSynthesizer speak = new SpeechSynthesizer();
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            listener.SetInputToDefaultAudioDevice(); 

            //System Word - Start
            GrammarBuilder gb = new GrammarBuilder();
            Choices cmds = new Choices();
            cmds.Add("Hey key");
            cmds.Add("Hey");
            cmds.Add("key");
            gb.Append(cmds);

            Grammar g = new Grammar(gb);
            KeyWord.LoadGrammar(g);

            KeyWord.SetInputToDefaultAudioDevice();
            KeyWord.RecognizeAsync(RecognizeMode.Multiple);
            KeyWord.RecognizeAsyncStop();
            KeyWord.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Speech_Recognized_System);
            //System Word - End

            //Listener - Start
            GrammarBuilder gbL = new GrammarBuilder();
            Choices cmdsL = new Choices();
            cmdsL.Add("Shut down System");
            // works
            cmdsL.Add("Shut down");
            cmdsL.Add("down System");
            cmdsL.Add("Shut");
            cmdsL.Add("down");
            cmdsL.Add("System");
            cmdsL.Add("Hello Key");
            // works
            cmdsL.Add("Key");
            cmdsL.Add("Run");
            cmdsL.Add("Spotify");
            cmdsL.Add("System Run Spotify");
            // works
            cmdsL.Add("System Shut down Spotify");
            // works
            cmdsL.Add("What");
            cmdsL.Add("time");
            cmdsL.Add("date");
            cmdsL.Add("is");
            cmdsL.Add("it");
            cmdsL.Add("What date is it");
            // works
            cmdsL.Add("System run us");
            cmdsL.Add("System shut down us");
            // works
            cmdsL.Add("System shut down Discord");
            // 



            gbL.Append(cmdsL);

            Grammar gL = new Grammar(gbL);
            listener.LoadGrammar(gL);

            listener.SetInputToDefaultAudioDevice();
            listener.RecognizeAsync(RecognizeMode.Multiple);
            listener.RecognizeAsyncStop();
            listener.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Speech_Recognized_listener);
            //Listener - End



            string[] commands = (File.ReadAllLines(@"Grammar.txt"));
            foreach (string command in commands)
            {
                listBox1.Items.Add(command);
            }


        }

        private void Speech_Recognized_listener(object sender, SpeechRecognizedEventArgs e)
        {
            string Speech = e.Result.Text;
            richTextBox1.Text = Speech;
            //Spotify - Start
            if (Speech == "System Run Spotify")
            {

                try
                { 
                    Process.Start("Spotify.exe");
                    speak.Speak("Spotify");

                }
                catch (Exception)
                {
                    speak.SpeakAsync("You dont have spotify!");
                }
            }
            if (Speech == "System Shut down Spotify")
            {
                Process[] runingProcess = Process.GetProcesses();
                for (int i = 0; i < runingProcess.Length; i++)
                {
                    // compare equivalent process by their name

                    if (runingProcess[i].ProcessName == "Spotify")
                    {
                        // shut down running process
                        runingProcess[i].Kill();
                    }

                }
                speak.Speak("Spotify shut down initiated");
            }
            //Spotify - End

            //osu! - Start
            if(Speech=="System run us")
            {
                string NoName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string[] name = NoName.Split('\\');
                Process.Start(@"C:\\Users\\" + name[1]+ "\\AppData\\Local\\osu!\\osu!.exe");
                speak.Speak("us");
            }
            if (Speech == "System shut down us")
            {
                Process[] runingProcess = Process.GetProcesses();
                for (int i = 0; i < runingProcess.Length; i++)
                {
                    // compare equivalent process by their name
                    if (runingProcess[i].ProcessName == "osu!")
                    {
                        // shut down  running process
                        runingProcess[i].Kill();
                    }

                }
                speak.Speak("Us shut down initiated");
            }
            //osu! - End

            //Hello - Start
            if (Speech == "Hello Key")
            {
                speak.SpeakAsync("Hello");
            }
            //Hello - End

            //Shut Down System - Start
            if (Speech == "Shut down System")
            {
                speak.SpeakAsync("System is being shut down");
                Thread.Sleep(2000);
                this.Close();
            }
            //Shut Down System - End

            //What time is it - Start
            if (Speech == "What time is it")
            {
                var hodiny = DateTime.Now.ToString("HH");
                var minuty = DateTime.Now.ToString("mm");
                richTextBox1.Clear();
                speak.Speak("Its " + hodiny + "hours and " + minuty + "minutes");
            }
            //What time is it - End

            //What date is it - Start
            if (Speech == "What date is it")
            {
                var měsíc = DateTime.Today.ToString("d");
                richTextBox1.Clear();
                speak.Speak("Its " + měsíc);
            }
            //What date is it - End

            //Discord - Start

            if (Speech == "System shut down Discord")
            {
                Process[] runingProcess = Process.GetProcesses();
                for (int i = 0; i < runingProcess.Length; i++)
                {
                    // compare equivalent process by their name
                    if (runingProcess[i].ProcessName == "Discord")
                    {
                        // shut down  running process
                        runingProcess[i].Kill();
                    }

                }
                speak.Speak("Discord shut down initiated");
            }
            //Discord - End
        }

        private void Speech_Recognized_System(object sender, SpeechRecognizedEventArgs e)
        {
            string Speech = e.Result.Text;
            richTextBox1.Text = Speech;
      
            if(Speech=="Hey key")
            {
                KeyWord.RecognizeAsyncCancel();
                listener.RecognizeAsync(RecognizeMode.Multiple);
                speak.SpeakAsync("Listening");
                listening = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KeyWord.RecognizeAsync(RecognizeMode.Multiple);
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KeyWord.RecognizeAsyncCancel();
            button1.Enabled = true;
            button2.Enabled = false;
        }





        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listening == true)
            {
                time++;
                if (time > 10)
                {
                    listener.RecognizeAsyncCancel();
                    KeyWord.RecognizeAsync(RecognizeMode.Multiple);
                    listening = false;
                    time = 1;
                    speak.SpeakAsync("Not listening");
                }
            }
        }
    }
}