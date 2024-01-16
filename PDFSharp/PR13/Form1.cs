using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Diagnostics.Eventing.Reader;

namespace PR13
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            textBox1.Font = fontDialog1.Font;
        }

        private void цветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            textBox1.ForeColor = colorDialog1.Color;
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы хотите выйти", "Предупреждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
            else { return; }
        }
        string[] strings;
        int ArrayCounter = 0;
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fn = openFileDialog1.FileName; this.Text = "Файл открыт" + fn;
                try
                {
                    StreamReader sr = new StreamReader(fn);
                    textBox1.Text = sr.ReadToEnd();
                    sr.Close();
                    strings = textBox1.Text.Split('\n');
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения.\n" + ex.ToString());
                }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fn = saveFileDialog1.FileName;
                this.Text = "Файл сохранен" + fn;
                if (fn != string.Empty)
                {
                    FileInfo fi = new FileInfo(fn);
                    try
                    {
                        StreamWriter sw = fi.CreateText();
                        sw.Write(textBox1.Text);
                        sw.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка записи \n" + ex.ToString());
                    }
                }
            }
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //m_PrintPageNumber = 1;
            //string strText = this.textBox1.Text;
            //m_myReader = new StringReader(strText);
            //Margins marguns = new Margins(100, 50, 50, 50);
            //printDocument1.DefaultPageSettings.Margins = margins;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                this.printDocument1.Print();
            }
            //m_myReader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
            gfx.DrawString("My First PDF Document", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            string filename = "FirstPDFDocument.pdf";
            document.Save(filename);
            Process.Start(filename);
        }

        private void предварительныйПросмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintPreview aForm = new PrintPreview();
            DialogResult aResult;
            aForm.printPreviewControl1.Document = printDocument1;
            aResult = aForm.ShowDialog();
            if (aResult == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void параметрыСтраницыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            float LeftMargin = e.MarginBounds.Left;
            float TopMargin = e.MarginBounds.Top;
            float MyLines;
            float YPosition;
            int Counter = 0;
            string CurrentLine;
            MyLines = e.MarginBounds.Height /
            this.Font.GetHeight(e.Graphics);
            while (Counter < MyLines && ArrayCounter <= strings.Length - 1)
            {
                CurrentLine = strings[ArrayCounter];
                YPosition = TopMargin + Counter * this.Font.GetHeight(e.Graphics);
                e.Graphics.DrawString(CurrentLine, this.Font, Brushes.Black, LeftMargin, YPosition, new StringFormat());
                Counter++;
                ArrayCounter++;
            }
            if (!(ArrayCounter >= strings.GetLength(0) - 1))
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }
    }
}
