using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
//using NAudio.Wave;
using System.Reflection;

namespace PathOfTheHero
{
    public partial class EditForm : Form
    {
        public Game Game { get; set; }
       // private DirectSoundOut output;
      //  private BlockAlignReductionStream stream = null;

        public EditForm(Game game)
        {
            Game = game;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Game.gameName = textBox1.Text;
                Game.creatorName = textBox2.Text;
                Game.releaseDate = dateTimePicker1.Value;
                Game.startDate = dateTimePicker2.Value.ToString("dd MMMMMMMM yyyy");
                Game.finishDate = dateTimePicker3.Value.ToString("dd MMMMMMMM yyyy");
                Game.herocomment = richTextBox1.Text;

                var image = pictureBox1.Image;
                if (image != null)
                {
                    var ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Png);
                    Game.Photo = ms.ToArray();
                }

                if (checkBox1.Checked)
                {
                    Game.inprogress = false;
                }
                else
                {
                    Game.inprogress = true;
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) //загрузить картинку
        {
            try
            {
                var fileDialog = new OpenFileDialog
                { Filter = "Изображения (*.jpg, *.jpeg, *.png, *.ico, *.bmp) | *.jpg; *.jpeg; *.png, *.ico, *.bmp" };

                var result = fileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    var fileName = fileDialog.FileName;
                    pictureBox1.Load(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime x;

                textBox1.Text = Game.gameName;
                textBox2.Text = Game.creatorName;
                dateTimePicker1.Value = Game.releaseDate;
                dateTimePicker2.Value = Convert.ToDateTime(Game.startDate);
                richTextBox1.Text = Game.herocomment;

                if (DateTime.TryParse(Game.finishDate, out x))
                {
                    dateTimePicker3.Value = Convert.ToDateTime(Game.finishDate);
                }
                else
                {
                    dateTimePicker3.Value = DateTime.Now;
                }

                if (Game.Photo != null)
                {
                    var image = Image.FromStream(new MemoryStream(Game.Photo));
                    pictureBox1.Image = image;
                }

                if (!Game.inprogress)
                {
                    checkBox1.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
