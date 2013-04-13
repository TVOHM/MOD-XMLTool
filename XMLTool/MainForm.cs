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

namespace XMLTool
{
    public partial class MainForm : Form
    {
        private App mApp = new App();

        private Content mContent = null;

        private Video mVideo = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
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

        private void actorsListViewNewButton_Click(object sender, EventArgs e)
        {
            TextInputForm dialog = new TextInputForm("Add Actor");
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.Cancel && dialog.mInput.Length != 0)
            {
                if (mApp.mActors.Contains(dialog.mInput))
                {
                    MessageBox.Show("Duplicate actor name \"" + dialog.mInput + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    actorsListBox.Items.Add(dialog.mInput);
                    mApp.mActors = actorsListBox.Items.OfType<string>().ToList();
                    contentObjectActorsCheckedListBox.Items.Add(dialog.mInput);
                }
            }
        }

        private void actorsListViewEditButton_Click(object sender, EventArgs e)
        {
            TextInputForm dialog = new TextInputForm("Edit Actor", actorsListBox.Items[actorsListBox.SelectedIndex].ToString());
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.Cancel && dialog.mInput.Length != 0)
            {
                if (mApp.mActors.Contains(dialog.mInput))
                {
                    MessageBox.Show("Duplicate actor name \"" + dialog.mInput + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    actorsListBox.Items[actorsListBox.SelectedIndex] = dialog.mInput;
                    contentObjectActorsCheckedListBox.Items[actorsListBox.SelectedIndex] = dialog.mInput;
                    mApp.mActors = actorsListBox.Items.OfType<string>().ToList();
                }
            }
        }

        private void actorsListViewDeleteButton_Click(object sender, EventArgs e)
        {
            contentObjectActorsCheckedListBox.Items.RemoveAt(actorsListBox.SelectedIndex);
            mApp.mActors.RemoveAt(actorsListBox.SelectedIndex);
            actorsListBox.Items.RemoveAt(actorsListBox.SelectedIndex);
        }

        private void applicationIconLoadButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openImageFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (isValidImage(openImageFileDialog.FileName))
                {
                    applicationIconPictureBox.Load(openImageFileDialog.FileName);
                    mApp.mIcon = File.ReadAllBytes(openImageFileDialog.FileName);
                }
                else
                    MessageBox.Show(openImageFileDialog.FileName + " is not a valid image file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool isValidImage(string fileName)
        {
            try
           {
               Image newImage = Image.FromFile(fileName);
            }
            catch (OutOfMemoryException ex)
            {
                return false;
            }
            return true;
        }

        private void applicationNameTextBox_TextChanged(object sender, EventArgs e)
        {
            mApp.mName = ((TextBox)sender).Text;
        }

        private void contentsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (contentsListBox.SelectedIndex == -1)
            {
                contentsListBoxDeleteButton.Enabled = false;
                contentsListBoxUpButton.Enabled = false;
                contentsListBoxDownButton.Enabled = false;
            }
            else
            {
                contentsListBoxDeleteButton.Enabled = true;

                if(contentsListBox.SelectedIndex != 0)
                    contentsListBoxUpButton.Enabled = true;
                else
                    contentsListBoxUpButton.Enabled = false;

                if (contentsListBox.SelectedIndex != contentsListBox.Items.Count - 1)
                    contentsListBoxDownButton.Enabled = true;
                else
                    contentsListBoxDownButton.Enabled = false;

                contentObjectVideosNewButton.Enabled = true;
            }

            updateContentObjectFields(contentsListBox.SelectedIndex);
        }

        private void updateContentObjectFields(int selectedIndex)
        {
            if(selectedIndex != -1)
            {
                mContent = mApp.mContents.ElementAt(selectedIndex);
                contentObjectNameTextBox.Enabled = true;
                contentObjectNameTextBox.Text = mContent.mName;

                contentObjectActorsCheckedListBox.Enabled = true;
                for(int i = 0; i < contentObjectActorsCheckedListBox.Items.Count; i++)
                    contentObjectActorsCheckedListBox.SetItemCheckState(i, CheckState.Unchecked);

                foreach (var actor in mContent.mActors)
                    contentObjectActorsCheckedListBox.SetItemCheckState(contentObjectActorsCheckedListBox.FindString(actor), CheckState.Checked);

                contentObjectTextTextBox.Enabled = true;
                contentObjectTextTextBox.Text = mContent.mText;

                contentObjectVideosListBox.Enabled = true;
                contentObjectVideosListBox.Items.Clear();
                foreach (var video in mContent.mVideos)
                    contentObjectVideosListBox.Items.Add(video.mName);
            }
            else
            {
            }
        }

        private void contentsListBoxNewButton_Click(object sender, EventArgs e)
        {
            mContent = new Content();
            mApp.mContents.Add(mContent);
            contentsListBox.Items.Add(mContent.mName);
            contentsListBox.SelectedIndex = contentsListBox.Items.Count - 1;
        }

        private void contentsListBoxDeleteButton_Click(object sender, EventArgs e)
        {
            mApp.mContents.RemoveAt(contentsListBox.SelectedIndex);
            contentsListBox.Items.RemoveAt(contentsListBox.SelectedIndex);
        }

        private void contentObjectNameTextBox_TextChanged(object sender, EventArgs e)
        {
            mContent.mName = contentObjectNameTextBox.Text;
        }

        private void contentObjectTextTextBox_TextChanged(object sender, EventArgs e)
        {
            mContent.mText = contentObjectTextTextBox.Text;
        }

        private void contentObjectActorsCheckedListBox_ItemCheck(object sender, EventArgs e)
        {
            mContent.mActors.Remove(contentObjectActorsCheckedListBox.GetItemText(contentObjectActorsCheckedListBox.SelectedIndex));
        }

        private void contentObjectVideosNewButton_Click(object sender, EventArgs e)
        {
            mVideo = new Video();
            mContent.mVideos.Add(mVideo);
            contentObjectVideosListBox.Items.Add(mVideo.mName);
            contentObjectVideosListBox.SelectedIndex = contentObjectVideosListBox.Items.Count - 1;
        }

        private void contentObjectVideosListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (contentObjectVideosListBox.SelectedIndex == -1)
            {
                videoNameTextBox.Enabled = false;
                videoIconLoadButton.Enabled = false;
                videoVideoLoadButton.Enabled = false;
                contentObjectVideosUpButton.Enabled = false;
                contentObjectVideosDeleteButton.Enabled = false;
                contentObjectVideosDownButton.Enabled = false;
                mVideo = mContent.mVideos.ElementAt(contentObjectVideosListBox.SelectedIndex);
            }
            else
            {
            }

            updateVideoFields();
        }

        private void updateVideoFields()
        {
        }

        private void videoNameTextBox_TextChanged(object sender, EventArgs e)
        {
            mVideo.mName = videoNameTextBox.Text;
        }

        private void videoIconLoadButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openImageFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (isValidImage(openImageFileDialog.FileName))
                {
                    videoIconPictureBox.Load(openImageFileDialog.FileName);
                    mVideo.mIcon = File.ReadAllBytes(openImageFileDialog.FileName);
                }
                else
                    MessageBox.Show(openImageFileDialog.FileName + " is not a valid image file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
