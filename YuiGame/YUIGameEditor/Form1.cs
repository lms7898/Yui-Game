using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace YUIGameEditor
{
    public partial class Form1 : Form
    {
        private StreamWriter output;
        public StreamReader input;
        private string fileName = "..\\..\\..\\..\\YuiGame\\YuiGame\\bin\\windowsGL\\Debug\\settings.txt";
        private string hp, mana, maxhp, maxmana, sp;

        public Form1()
        {
            InitializeComponent();
            MaxHealth.Clear();
            MaxMana.Clear();
            Health.Clear();
            Mana.Clear();
            SkillPoints.Clear();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            WriteSettings();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            MaxHealth.Clear();
            MaxMana.Clear();
            Health.Clear();
            Mana.Clear();
            SkillPoints.Clear();
        }

        //method to write data to a file
        public void WriteSettings()
        {
            output = new StreamWriter(fileName);
            //storing everything
            try
            {
                output.Write(hp);
                output.Write(mana);
                output.Write(maxhp);
                output.Write(maxmana);
                output.Write(sp);
            }

            catch (IOException ioe)
            {
                Console.WriteLine("Settings output Message: " + ioe.Message);
                Console.WriteLine("Settings output Stack Trace: " + ioe.StackTrace);
            }
            finally
            {
                output.Close();
            }

        }

        private void Health_TextChanged(object sender, EventArgs e)
        {
            if (Health.Text == null)
            {
                hp = "100";
            }
            else hp = Health.Text;
        }

        private void Mana_TextChanged(object sender, EventArgs e)
        {
            if (Mana.Text == null)
            {
                mana = "100";
            }
            else mana = Mana.Text;
        }

        private void MaxHealth_TextChanged(object sender, EventArgs e)
        {
            if (MaxHealth.Text == null)
            {
                maxhp = "100";
            }
            else maxhp = MaxHealth.Text;
        }

        private void MaxMana_TextChanged(object sender, EventArgs e)
        {
            if (MaxMana.Text == null)
            {
                maxmana = "100";
            }
            else maxmana = MaxMana.Text;
        }

        private void SkillPoints_TextChanged(object sender, EventArgs e)
        {
            if (SkillPoints.Text == null)
            {
                sp = "1";
            }
            else sp = SkillPoints.Text;
            
        }
    }
}
