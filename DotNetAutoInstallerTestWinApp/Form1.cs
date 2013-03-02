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
using Newtonsoft.Json.Linq;

namespace DotNetAutoInstallerTestWinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                var o = JObject.Parse(this.txtJSON.Text);
                System.Windows.Forms.MessageBox.Show("Valid JSON");
                var b = System.IO.File.Exists(Path.Combine(DotNetAutoInstaller.AutoInstaller.ApplicationDataFolder, @"Help\Help.markdown"));
              
                if(!b)
                {
                    throw new ApplicationException("Instllation not complete");
                }
            }
            catch(System.Exception ex)
            {
                 System.Windows.Forms.MessageBox.Show(String.Format("Invalid JSON:{0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);       
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var a = System.Configuration.ConfigurationSettings.AppSettings["Key1"];
            this.Text = String.Format("{0} - {1}", this.Text, a);
        }
    }
}
