using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using NAudio.Wave;
using System.IO;

namespace PathOfTheHero
{
    public partial class AdvForm : Form
    {
        public int index { get; set; }
        public Game Game { get; set; }
        TimeSpan DaysOfPlaying { get; set; }
        //private BlockAlignReductionStream stream = null;
        //private DirectSoundOut output;

        public AdvForm(Game game, int i)
        {
            InitializeComponent();
            Game = game;
            index = i;
        }

        

        private void AdvForm_Load(object sender, EventArgs e)
        {
            //WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader("defaultmusic.mp3")); //попытки работы со звуком

            //stream = new BlockAlignReductionStream(pcm);
            //output = new DirectSoundOut();
            //output.Init(stream);
            //output.Play();

            //IWaveProvider provider = new RawSourceWaveStream(
            //             new MemoryStream(Game.Music), new WaveFormat());
            //output = new DirectSoundOut();
            //output.Init(provider);
            //output.Play();
            try
            {
                DateTime x;
                if (DateTime.TryParse(Game.finishDate, out x))
                {
                    DaysOfPlaying = Convert.ToDateTime(Game.finishDate) - Convert.ToDateTime(Game.startDate);
                    label14.Text = DaysOfPlaying.Days.ToString();
                }
                else
                {
                    label14.Text = "пока хз";
                }


                label9.Text = Game.gameName;
                label10.Text = Game.creatorName;
                label11.Text = Game.releaseDate.ToString("dd MMMMMMMM yyyy");
                label12.Text = Game.startDate;
                label13.Text = Game.finishDate;

                label15.Text = (index + 1).ToString();
                richTextBox1.Text = Game.herocomment;

                if (Game.Photo != null)
                {
                    var image = Image.FromStream(new MemoryStream(Game.Photo));
                    pictureBox1.Image = image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        
        //private void DisposeWave()
        //{
        //    if (output != null)
        //    {
        //        if (output.PlaybackState == PlaybackState.Playing) output.Stop();
        //        output.Dispose();
        //        output = null;
        //    }
        //    if (stream != null)
        //    {
        //        stream.Dispose();
        //        stream = null;
        //    }
        //}        

        //private void pausemusicBtn_Click_1(object sender, EventArgs e)
        //{
        //    if (output != null)
        //    {
        //        if (output.PlaybackState == PlaybackState.Playing) output.Pause();
        //        else if (output.PlaybackState == PlaybackState.Paused) output.Play();
        //    }
        //}

        private void AdvForm_FormClosing(object sender, FormClosingEventArgs e)
        {
         //   DisposeWave();
        }

        private void button1_Click(object sender, EventArgs e)  // ДОРАБОТАТЬ если вдруг редактить отсюда надо
        {
            try
            {
                var game = Game as Game;
                var editForm = new EditForm(game);
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {                  
                    this.Close();                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
