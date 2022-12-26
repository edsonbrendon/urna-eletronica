using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace urna_eletronica
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Candidate> _dicCandidate;

        public Form1()
        {
            InitializeComponent();

            _dicCandidate = new Dictionary<string, Candidate>();
            _dicCandidate.Add("12", new Candidate() { Id = 12, Name = "Ciro Gomes", PoliticalParty = "PDT", Image = Properties.Resources._12 });
            _dicCandidate.Add("30", new Candidate() { Id = 30, Name = "Felipe D Avila", PoliticalParty = "Novo", Image = Properties.Resources._30 });
            _dicCandidate.Add("22", new Candidate() { Id = 22, Name = "Jair Bolsonaro", PoliticalParty = "PL", Image = Properties.Resources._22 });
            _dicCandidate.Add("13", new Candidate() { Id = 13, Name = "Luiz Inácio Lula da Silva", PoliticalParty = "PT", Image = Properties.Resources._13 });
            _dicCandidate.Add("14", new Candidate() { Id = 14, Name = "Padre Kelmon", PoliticalParty = "PTB", Image = Properties.Resources._14 });
            _dicCandidate.Add("15", new Candidate() { Id = 15, Name = "Simone Tebet", PoliticalParty = "MDB", Image = Properties.Resources._15 });
            _dicCandidate.Add("44", new Candidate() { Id = 44, Name = "Soraya Thronicke", PoliticalParty = "UNIÃO", Image = Properties.Resources._44 });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterDigit("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterDigit("2");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RegisterDigit("3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RegisterDigit("4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RegisterDigit("5");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RegisterDigit("6");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RegisterDigit("7");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            RegisterDigit("8");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            RegisterDigit("9");
        }

        private void button0_Click(object sender, EventArgs e)
        {
            RegisterDigit("0");
        }

        private void RegisterDigit(string digit)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                Clean();
                textBox1.Text = digit;
            }
            else if (string.IsNullOrEmpty(textBox1.Text)) 
            {
                textBox1.Text = digit;
            }
            else
            {
                textBox2.Text = digit;
                ProcessCandidate(textBox1.Text, textBox2.Text);
            }

            PlayClickSound();
        }

        private void buttonWhite_Click(object sender, EventArgs e)
        {
            PlayClickSound();

            if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text))
                lbl_Status.Text = "VOTO BRANCO";
            else
                MessageBox.Show("Para votar em BRANCO o campo de voto deve estar vazio. Aperte CORRIGE para apagar o campo de voto.");
        }

        private void buttonCorrect_Click(object sender, EventArgs e)
        {
            PlayClickSound();
            Clean();
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            PlayClickSound();

            if(!string.IsNullOrEmpty(lbl_Status.Text)) 
                Vote();
            else if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                MessageBox.Show("Favor informar o candidato.");
            else Vote();
        }

        private void ProcessCandidate(string d1, string d2)
        {
            if (_dicCandidate.ContainsKey(d1 + d2))
            {
                lbl_Name.Text = _dicCandidate[d1 + d2].Name;
                lbl_PoliticalParty.Text = _dicCandidate[d1 + d2].PoliticalParty;
                pictureBox.Image = _dicCandidate[d1 + d2].Image;
            }
            else
            {
                lbl_Status.Text = "VOTO NULO";
                MessageBox.Show("Candidato não encontrado!");
            }
        }

        private void StartPanel()
        {
            panel2.Visible = true;
            timer.Tick += new EventHandler(EndPanel);
            timer.Interval = 3000;
            timer.Enabled = true;
            timer.Start();
        }

        private void EndPanel(Object myObject, EventArgs myEventArgs)
        {
            timer.Stop();
            timer.Enabled = false;
            panel2.Visible = false;
        }

        private void PlayClickSound()
        {
            SoundPlayer s = new SoundPlayer(Properties.Resources.click);
            s.Play();
        }

        private void PlayEndSound()
        {
            SoundPlayer s = new SoundPlayer(Properties.Resources.end);
            s.Play();
        }

        private void Clean()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            lbl_Name.Text = "";
            lbl_PoliticalParty.Text = "";
            lbl_Status.Text = "";
            pictureBox.Image = null;
        }

        private void Vote()
        {
            PlayEndSound();
            StartPanel();
            SaveVote();
        }

        private void SaveVote()
        {
            string path = "Votos.txt";

            using (StreamWriter sw = (File.Exists(path)) ? File.AppendText(path) : File.CreateText(path))
            {
                if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text)) 
                    sw.WriteLine(GetHashString("Branco"));
                else if (_dicCandidate.ContainsKey(textBox1.Text + textBox2.Text))
                    sw.WriteLine(GetHashString(textBox1.Text + textBox2.Text)); 
                else 
                    sw.WriteLine(GetHashString("Nulo")); 
            }

            Clean();
        }

        public static string GetHashString(string inputString)
        {
            inputString = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + inputString;

            using (var sha = new SHA256Managed())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(inputString);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                return BitConverter.ToString(hashBytes);
            }
        }
    }
}
