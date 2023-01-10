using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Triangulation
{
    public partial class ViewForm : Form
    {
        private string fileName;

        public ViewForm()
        {
            InitializeComponent();
            fileName = Path.ChangeExtension(Application.ExecutablePath, ".vtx");
            viewGraph.ContentChanged += (o, e) => 
            {
                tsmiSave.Enabled = viewGraph.ContentUnsaved;
                tsbSave.Enabled = viewGraph.ContentUnsaved;
            };
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            if (Location.IsEmpty)
                CenterToScreen();
            if (File.Exists(fileName))
                viewGraph.LoadVertices(fileName);
            else
                viewGraph.Clear();
            viewGraph.Invalidate();
            tsmiSave.Enabled = viewGraph.ContentUnsaved;
            tsbSave.Enabled = viewGraph.ContentUnsaved;
        }

        private void ViewForm_SizeChanged(object sender, EventArgs e)
        {
            viewGraph.Invalidate();
        }

        private void tsmiCreate_Click(object sender, EventArgs e)
        {
            viewGraph.Clear();
            viewGraph.Invalidate();
            tsmiSave.Enabled = viewGraph.ContentUnsaved;
            tsbSave.Enabled = viewGraph.ContentUnsaved;
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                viewGraph.LoadVertices(fileName);
                viewGraph.Invalidate();
                tsmiSave.Enabled = viewGraph.ContentUnsaved;
                tsbSave.Enabled = viewGraph.ContentUnsaved;
            }
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            viewGraph.SaveVertices(fileName);
            tsmiSave.Enabled = viewGraph.ContentUnsaved;
            tsbSave.Enabled = viewGraph.ContentUnsaved;
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                viewGraph.SaveVertices(fileName);
                tsmiSave.Enabled = viewGraph.ContentUnsaved;
                tsbSave.Enabled = viewGraph.ContentUnsaved;
            }
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
