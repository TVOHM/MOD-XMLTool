using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLTool
{
    public partial class MainForm : Form
    {
        private App mApp = new App();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void actorsListViewNewButton_Click(object sender, EventArgs e)
        {
            TextInputForm dialog = new TextInputForm("Add Actor");
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.Cancel && dialog.mInput.Length != 0)
                actorsListBox.Items.Add(dialog.mInput);
        }

        private void actorsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actorsListBox.SelectedIndex == -1)
            {
                actorsListViewDeleteButton.Enabled = false;
                actorsListViewEditButton.Enabled = false;
            }
            else
            {
                actorsListViewDeleteButton.Enabled = true;
                actorsListViewEditButton.Enabled = true;
            }
        }

        private void actorsListViewDeleteButton_Click(object sender, EventArgs e)
        {
            actorsListBox.Items.RemoveAt(actorsListBox.SelectedIndex);
        }

        private void actorsListViewEditButton_Click(object sender, EventArgs e)
        {
            TextInputForm dialog = new TextInputForm("Edit Actor", actorsListBox.Items[actorsListBox.SelectedIndex].ToString());
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.Cancel && dialog.mInput.Length != 0)
                actorsListBox.Items[actorsListBox.SelectedIndex] = dialog.mInput;
        }

        private void applicationIconLoadButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openImageFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                applicationIconPictureBox.Load(openImageFileDialog.FileName);
        }
    }
}
