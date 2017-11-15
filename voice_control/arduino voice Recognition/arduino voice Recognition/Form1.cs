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
using System.IO.Ports;


namespace arduino_voice_Recognition
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
            button2.Enabled = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Choices command = new Choices();
            command.Add(new string[] { "light on", "light off" });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(command);
            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += RecEngine_SpeechRecognized;
        }

        void RecEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            synthesizer.SpeakAsync("What would you like to do?");
            switch (e.Result.Text)
            {
                case "light on":             
                    SerialPort port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One); //Change COM5 to your com.
                    port.Open();
                    port.Write("1"); //the data that will be sent to the arduino, change if it is needed.
                    port.Close();
                    richTextBox1.Text += "\nlight on";
                    break;
                case "light off":
                    SerialPort port1 = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One); //Change COM5 to your com.
                    port1.Open();
                    port1.Write("0"); //the data that will be sent to the arduino, change if it is needed.
                    port1.Close();
                    richTextBox1.Text += "\nlight off";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
            button2.Enabled = false;
        }
    }
}
