using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.IO;

namespace PathOfTheHero
{   
    static class Program
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Directory.CreateDirectory(Form1.savesfilepath);
        //    Directory.CreateDirectory(Form1.playlistpath.TrimEnd('\\'));
        //    Application.Run(new Form1());

        //}
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Directory.CreateDirectory(Form1.savesfilepath);
            Directory.CreateDirectory(Form1.playlistpath.TrimEnd('\\'));
           
            Application.Run(new Form1());

           
        }
    }

    ///////////// закомментить, если не сканает с базой данных
    public class DataModel
    {        
        //public string BaseName { get; set; }
        public List<Game> Games { get; set; }
    }

    public class Game
    {       
        
        public string number { get; set; }
        public string gameName { get; set; }
        public string startDate { get; set; }
        public string finishDate { get; set; }
        public string creatorName { get; set; }
        //public List<string> genres { get; set; }
        public DateTime releaseDate { get; set; }
        public byte[] Photo { get; set; }
        public byte[] Music { get; set; }
        public string herocomment { get; set; }
        public bool inprogress { get; set; }

        public int GameID { get; set; } // - не используется в проге, нужен, чтобы работала SQL БД

        public override string ToString()
        {
            return
                $" {gameName} { creatorName } ";
        }                
    }

    //public class Category
    //{
    //    public string CategoryId { get; set; }
    //   // public string Name { get; set; }

    //    public virtual ICollection<Product> Products { get; set; }
    //}

    //public class Product
    //{
    //    public int ProductId { get; set; }
        
    //    public string Name { get; set; }
    //    //public string CategoryId { get; set; }

    //   // public virtual Category Category { get; set; }
    //}

    public class GameContext : DbContext
    {
        public GameContext()
       : base("PathOfTheHeroDB("+Form1.currentfilename.Remove(0, Form1.savesfilepath.Length + 1)+")")
        { }
        
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Product> Products { get; set; } //-канает
        public DbSet<Game> dbsetGame { get; set; }
                
    }
}
