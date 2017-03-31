using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathOfTheHero
{
    public partial class LicenseAgreementForm : Form
    {
        public LicenseAgreementForm()
        {
            InitializeComponent();
        }

        public static string licensefilepath = Form1.resorcesfilepath + "PathOfTheHeroEULA.txt";

        private void LicenseAgreementForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(licensefilepath))
            {
                richTextBox1.Lines = File.ReadAllLines(licensefilepath, Encoding.Default);
            }

        }
    }
}
