using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAudio;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private AudioPlayer Player;

        private bool SelectedAudio = false; 


        public Form1()
        {
            InitializeComponent();

            Player = new AudioPlayer();

            Player.PlayingStatusChanged += (s, e) => Play.Text = e == Status.Playing ? "Pause" : "Play";

            
            Player.AudioSelected += (s, e) =>
            {
                trackBar1.Maximum = (int)e.Duration;
                this.Text = e.Name;
                label1.Text= e.Name;
                label2.Text = e.DurationTime.ToString(@"mm\:ss");
                listBox1.SelectedItem = e.Name;


            };



            Player.ProgressChanged += (s, e) =>
            {
                 trackBar1.Value= (int)e;
                 label2.Text = ((AudioPlayer)s).PositionTime.ToString(@"mm\:ss");

            };

        }



        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog() {  Filter = "AudioFiles|*.wav;*.acc;*.wma;*.wmv;*.avi;*.mp2;*.mp3;*.mp4;*.mpa;", Multiselect = true })
            {

                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    Player.LoadAudio(dialog.FileName);
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(Player.Playlist);

                    if (!SelectedAudio)
                    {
                        listBox1.SelectedIndex = 0;

                        SelectedAudio = true;
                    }
                    

                }
            }
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListBox)sender).SelectedItem==null) 
            return;


              Player.SelectAudio(((ListBox)sender).SelectedIndex);


        }



        private void Play_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Play")
            {
                Player.Play();
            }
            else if (((Button)sender).Text == "Pause")
            {
                Player.Pause();
            }

        }


        // Time
        private void trackBar1_Scroll(object sender, EventArgs e) => Player.Position = ((TrackBar)sender).Value;

        // Volume
        private void trackBar2_Scroll(object sender, EventArgs e) => Player.Volume = ((TrackBar)sender).Value;


    }
}
