using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Media;
//using WMPLib;
using System.Reflection;
using System.Configuration;

namespace PathOfTheHero
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Directory.CreateDirectory(resorcesfilepath);
            
            InitializeComponent();
        }
                
        List<Game> list;
        public static string resorcesfilepath = @"Resources\";
        public static string playlistpath = resorcesfilepath + @"Playlist\";
        public static string savesfilepath = @"Saves";
        bool isconfigsaved = false;
        //string[] playlistarray = { resorcesfilepath + "lindsey_stirling_assassin_s_creed_iii.mp3",
         //   resorcesfilepath + "Stalker.mp3" };
        string[] playlistarray = Directory.GetFiles(playlistpath, "*.mp3");

        //string streamPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\";
        public bool issaved = true; // чтобы вывести предупреждение, если забыли сохранить
        public static string currentfilename = savesfilepath + @"\"+ /*"\defaultsavefile.poth"*/ ConfigurationManager.AppSettings["currentfilename"]; //имя файла для открытия по умолчанию

        public DataModel GetDataObject()
        {
            return new DataModel
            {
               // BaseName = textBox1.Text,
                Games = list
                //Games = listBox1.Items.OfType<Game>().ToList()

            };
        }

        public void SetDataObject(DataModel dataModel)
        {
            list.Clear();
            foreach (var game in dataModel.Games)
            {
                list.Add(game);
            }
        }             

        public void SerializeAndSave(string filepath) // функция сохранения в файл
        {
            try
            {
                var fileName = filepath;
                var data = GetDataObject();

                var serializer = new XmlSerializer(typeof(DataModel));
                var stream = File.Create(fileName);
                serializer.Serialize(stream, data);
                stream.Dispose();

                issaved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить в файл: " + Environment.NewLine + ex.ToString());
            }
        }

        public void OpenAndDeserialize(string filepath)  // функция чтения из файла
        {
            try
            {
                var fileName = filepath;

                var serializer = new XmlSerializer(typeof(DataModel));
                var stream = File.OpenRead(fileName);
                DataModel o;
                try
                {
                    o = (DataModel)serializer.Deserialize(stream);
                }
                finally
                {
                    stream.Dispose();
                }

                SetDataObject(o);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл: " + Environment.NewLine + ex.ToString());
            }
        }

        public bool IsDefaultFileCorrect() //проверка существования файла по умолчанию
        {
            if (File.Exists(currentfilename)) { return true; }
            else
            {
                list.Add(new Game()
                {
                    gameName = "Новая игра",
                    releaseDate = DateTime.Now,
                    startDate = DateTime.Now.ToString("dd MMMMMMMM yyyy"),
                    finishDate = DateTime.Now.ToString("dd MMMMMMMM yyyy"),
                    inprogress = true
                });
                return false;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //WindowsMediaPlayer wmp = new WindowsMediaPlayer();
            //Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PathOfTheHero.Resources.Stalker.mp3");

            //using (Stream output = new FileStream(streamPath + "Stalker.mp3", FileMode.Create))
            //{
            //    byte[] buffer = new byte[32 * 1024];
            //    int read;

            //    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        output.Write(buffer, 0, read);
            //    }
            //}
            //wmp.URL = streamPath + "Stalker.mp3";
            //wmp.controls.play();

            // axWindowsMediaPlayer1.URL = resorcesfilepath + @"\lindsey_stirling_assassin_s_creed_iii.mp3";
            
            try
            {
                WMPLib.IWMPPlaylist playlist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("myplaylist");
                WMPLib.IWMPMedia media;
                // var ofdSong = new OpenFileDialog { Multiselect = true };
                // if (ofdSong.ShowDialog() == DialogResult.OK)
                {
                    foreach (string song in playlistarray)
                    {
                        media = axWindowsMediaPlayer1.newMedia(song);
                        playlist.appendItem(media);
                    }
                }
                axWindowsMediaPlayer1.currentPlaylist = playlist;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.settings.volume = 15;
                
                                

                ToolTip t = new ToolTip();
                t.SetToolTip(savenexitBtn, "Сохранить и выйти");
                t.SetToolTip(addBtn, "Добавить новый элемент");
                t.SetToolTip(editBtn, "Редактировать выбранный элемент");
                t.SetToolTip(deleteBtn, "Удалить выбранный элемент");
                t.SetToolTip(advBtn, "Подробный просмотр элемента");
                t.SetToolTip(axWindowsMediaPlayer1, @"Если хочешь свою музычку, закидывай mp3 в ПапкаУстановкиПроги\Resources\Playlist");

                list = new List<Game>();

                if (IsDefaultFileCorrect()) { OpenAndDeserialize(currentfilename); }
                refreshDGV();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //public void SortThisFknListbox() //сортировка ЛИСТБОКСА (выпиленного), чтоб сохранить ломающуюся хронологию
        //{
        //    var list = listBox1.Items.Cast<Game>().OrderBy(item => item.startDate).ToList();
        //    listBox1.Items.Clear();
        //    foreach (Game listItem in list)
        //    {
        //        listBox1.Items.Add(listItem);
        //    }
        //}

        public void refreshDGV() //обновить таблицу
        {
            try
            {
                dataGridView1.DataSource = null;
                //dataGridView1.Columns[2].CellTemplate = new DataGridView
                dataGridView1.DataSource = list;

                if (dataGridView1.ColumnCount > 4)
                {

                    for (int i = 1; i < 8; i++) //увеличить i, чтобы удалить еще столбец справа в таблице
                    {
                        dataGridView1.Columns.RemoveAt(4);
                    }
                    dataGridView1.Columns[0].HeaderText = "№";
                    dataGridView1.Columns[1].HeaderText = "Название";
                    dataGridView1.Columns[2].HeaderText = "Дата начала";
                    dataGridView1.Columns[3].HeaderText = "Дата окончания";
                }

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
             
                    if(list[i].inprogress)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = "в процессе";
                    }
                }
                dataGridView1.Columns[0].Width = 30;
                dataGridView1.Columns[1].Width = 250;
                dataGridView1.Columns[2].Width = 150;
                dataGridView1.Columns[3].Width = 150;               

                dataGridView1.ClearSelection();


                текущийФайлToolStripMenuItem.Text = "Активный файл - " + currentfilename.Remove(0, savesfilepath.Length + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) //если вдруг сделаю музыку, ДГВ не накроется
        {
            // Don't throw an exception when we're done.
            e.ThrowException = false;

            // Display an error message.
            //string txt = "Error with " +
            //    dataGridView1.Columns[e.ColumnIndex].HeaderText +
            //    "\n\n" + e.Exception.Message;
            //MessageBox.Show(txt, "Error",
            //    MessageBoxButtons.OK, MessageBoxIcon.Error);

            // If this is true, then the user is trapped in this cell.
            e.Cancel = false;
        }
        
        private void addBtn_Click(object sender, EventArgs e) //добавить элемент
        {
            try
            {
               
                issaved = false;
                //listBox1.Items.Add(new Game() { gameName = "Новая игра", releaseDate = DateTime.Now, startDate = DateTime.Now, finishDate = DateTime.Now });
                //listBox1.SetSelected(listBox1.Items.Count - 1, true);

                list.Add(new Game() { gameName = "Новая игра", releaseDate = DateTime.Now,
                    startDate = DateTime.Now.ToString("dd MMMMMMMM yyyy"),
                    finishDate = DateTime.Now.ToString("dd MMMMMMMM yyyy"), inprogress = true });

                refreshDGV();
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[0];
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshDgvBtn_Click(object sender, EventArgs e) //  КНОПКА обновить таблицу
        {
            refreshDGV();
        }

        private void EditElement() //редактировать текущий элемент
        {

            try
            {
                issaved = false;
                //var game = listBox1.SelectedItem as Game;
                var game = list[dataGridView1.CurrentCell.RowIndex] as Game;
                var editForm = new EditForm(game);
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    // listBox1.Items.Remove(game);
                    //listBox1.Items.Add(game);
                    //SortThisFknListbox();
                    //list.Remove(game);
                    //list.Add(game);
                    refreshDGV();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void editBtn_Click(object sender, EventArgs e) //редактировать текущий элемент КНОПКА
        {
            if (IsSelected()) { EditElement(); }
            else { SelectCellMessage(); }
        }

        private void DeleteElement() //удалить выбранный элемент
        {
            try
            {
                //listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                if (MessageBox.Show("Точно удалить?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    list.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                    refreshDGV();
                    issaved = false;
                }
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message);
            }
        }
        
        private void deleteBtn_Click(object sender, EventArgs e) //удалить выбранный элемент КНОПКА
        {
            try
            {

                if (list.Count < 2)
                {
                    MessageBox.Show("Последнюю то не удаляй!", "Ну харош!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (IsSelected()) { DeleteElement(); }
                else { SelectCellMessage(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadfileBtn_Click(object sender, EventArgs e) //загрузить из файла
        {
            var fileDialog = new OpenFileDialog
            { Filter = "PATH OF THE HERO |*.poth" };
            var result = fileDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                OpenAndDeserialize(fileDialog.FileName);
            }            

            refreshDGV();
        }

        private void savefileBtn_Click(object sender, EventArgs e) //сохранить текущий файл
        {
            SerializeAndSave(currentfilename);
        }              
        
        private bool IsSelected() // проверка, выделен ли элемент
        {            
            if (dataGridView1.SelectedRows.Count > 0)
            {             
                return true;
            }

            return false;
        }
        
        private void SelectCellMessage()
        {
            MessageBox.Show("Необходимо выделить элемент!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) // снятие выделения по умолчанию
        {
            ((DataGridView)sender).CurrentCell = null;
            //dataGridView1.ClearSelection(); // - почему то не канает
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void RunAdvancedView () //подробный просмотр
        {
            try
            {
                var game = list[dataGridView1.CurrentCell.RowIndex] as Game;
                var advForm = new AdvForm(game, dataGridView1.CurrentCell.RowIndex);
                advForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void advBtn_Click(object sender, EventArgs e) //подробнее КНОПКА
        {
            if (IsSelected()) { RunAdvancedView(); }
            else { SelectCellMessage(); }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //подробнее КНОПКА (Еще одна)
        {
            RunAdvancedView();
        }
               
        private void button1_Click(object sender, EventArgs e)//ЗДЕСЬ НИЧЕГО НЕТ! ЮЗАЛАСЬ ДЛЯ ПЛЕЕРА
        {
            //WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
            //player.URL = "Stalker.mp3";
            //player.controls.play();
        }

        private void savenexitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                config.AppSettings.Settings.Remove("currentfilename");
                config.AppSettings.Settings.Add("currentfilename", currentfilename.Remove(0, savesfilepath.Length + 1));
                config.Save(ConfigurationSaveMode.Modified);
                isconfigsaved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SerializeAndSave(currentfilename);
            Environment.Exit(0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!isconfigsaved)
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    config.AppSettings.Settings.Remove("currentfilename");
                    config.AppSettings.Settings.Add("currentfilename", currentfilename.Remove(0, savesfilepath.Length + 1));
                    config.Save(ConfigurationSaveMode.Modified);
                }

                if ((issaved == false) && (e.CloseReason == CloseReason.UserClosing))

                {
                    var dr = MessageBox.Show("Вы не сохранились!\nСохранить перед выходом?", "Подтверждение выхода", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    e.Cancel = dr == DialogResult.Cancel;

                    if (dr == DialogResult.Yes)
                    {
                        SerializeAndSave(currentfilename);                    

                    }
                }

                if (updateSqlDbCheckBox.Checked == true)
                {
                    using (var db = new GameContext())
                    {
                        db.Database.ExecuteSqlCommand("delete from Games");

                        foreach (var g in list)
                        {
                            db.dbsetGame.Add(g);
                        }

                        db.SaveChanges();
                        //MessageBox.Show(db.SaveChanges().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e) //МЕНЮ-сохранить
        {
            SerializeAndSave(currentfilename);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e) //МЕНЮ-сохранить как...
        {
            try
            {
                var fileDialog = new SaveFileDialog
                {
                    Filter = "PATH OF THE HERO | *.poth",
                    InitialDirectory = Environment.CurrentDirectory + "\\" + savesfilepath
                };
                var result = fileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    var fileName = fileDialog.FileName;
                    SerializeAndSave(fileName);  //вызов функции сохранения в файл
                    currentfilename = fileDialog.FileName;
                    currentfilename = savesfilepath + @"\" + new FileInfo(currentfilename).Name;
                    текущийФайлToolStripMenuItem.Text = "Активный файл - " + currentfilename.Remove(0, savesfilepath.Length + 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog
                {
                    Filter = "PATH OF THE HERO |*.poth",
                    InitialDirectory = Environment.CurrentDirectory + "\\" + savesfilepath
                };

                var result = fileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {

                    OpenAndDeserialize(fileDialog.FileName);
                    currentfilename = fileDialog.FileName;
                    currentfilename = savesfilepath + @"\" + new FileInfo(currentfilename).Name;

                }

                refreshDGV();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.ShowDialog();
        }

        private void лицензионноеСоглашениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LicenseAgreementForm laf = new LicenseAgreementForm();
            laf.ShowDialog();
        }

        
    }
}
