using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPA.Client
{
    public partial class Form1 : Form
    {
        public Parser.Parser Parser;

        private const string RUN_START = "Uruchamianie...";
        private const string RUN_ERROR = "Błąd uruchamiania: ";
        private const string COMPILE_START = "Rozpoczęcie kompilacji...";
        private const string COMPILE_END = "Kompilacja przebiegła pomyślnie";
        private const string COMPILE_ERROR = "Błąd kompilacji: ";
        private Dictionary<int, int> errorRangeText = new Dictionary<int, int>();



        public Form1()
        {
            InitializeComponent();
            Parser = new Parser.Parser();
            ControlWriter controlWriter = new ControlWriter(richTextBox2);
            Console.SetOut(controlWriter);
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void openCodeFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
            }
        }

        private void clearCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void RunCode()
        {
            try
            {
                //Console.WriteLine(RUN_START);
                QueryProcessor.QueryProcessor.ProcessQuery(richTextBox3.Text);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(RUN_ERROR);
                Console.WriteLine(ex.Message);
                errorRangeText.Add(richTextBox2.Text.Trim().Length - ex.Message.Trim().Length - RUN_ERROR.Trim().Length - 3, richTextBox2.Text.Length);
                Console.WriteLine(ex.StackTrace);


            }
        }
        private void CompileCode()
        {
            try
            {
                //Console.WriteLine(COMPILE_START);
                Parser.CleanData();
                Parser.StartParse(richTextBox1.Text);
                //Console.WriteLine(COMPILE_END);
            }
            catch(Exception ex)
            {
                //Console.WriteLine(COMPILE_ERROR);
                Console.WriteLine(ex.Message);
                errorRangeText.Add(richTextBox2.Text.Trim().Length - ex.Message.Trim().Length - COMPILE_ERROR.Trim().Length - 3, richTextBox2.Text.Length);
                Console.WriteLine(ex.StackTrace);


            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            
            CompileCode();
           // ChangeErrorColor();

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            RunCode();
           // ChangeErrorColor();
        }

        private void runWithCompileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            CompileCode();
            RunCode();
           // ChangeErrorColor();
        }

        private void ChangeErrorColor()
        {
            foreach(var range in errorRangeText)
            {
                richTextBox2.Select(range.Key, range.Value - range.Key);
                richTextBox2.SelectionColor = Color.Red;
            }
            errorRangeText.Clear();
        }
    }
}
