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
using System.Diagnostics;

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

            contentObjectVideosListBox.SelectedIndex = -1;
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

                for (int i = 0; i < mContent.mActors.Count; i++)
                {
                    contentObjectActorsCheckedListBox.SetItemCheckState(contentObjectActorsCheckedListBox.FindString(mContent.mActors.ElementAt(i)),
                        CheckState.Checked);
                }

                contentObjectTextTextBox.Enabled = true;
                contentObjectTextTextBox.Text = mContent.mText;

                contentObjectVideosListBox.Enabled = true;
                contentObjectVideosListBox.Items.Clear();
                foreach (var video in mContent.mVideos)
                    contentObjectVideosListBox.Items.Add(video.mName);
            }
            else
            {
                mContent = new Content();
                contentObjectNameTextBox.Enabled = false;
                contentObjectNameTextBox.Text = "";

                contentObjectActorsCheckedListBox.Enabled = false;
                for (int i = 0; i < contentObjectActorsCheckedListBox.Items.Count; i++)
                    contentObjectActorsCheckedListBox.SetItemCheckState(i, CheckState.Unchecked);

                contentObjectTextTextBox.Enabled = false;
                contentObjectTextTextBox.Text = "";

                contentObjectVideosListBox.Enabled = false;
                contentObjectVideosListBox.Items.Clear();
            }
        }

        private void contentsListBoxNewButton_Click(object sender, EventArgs e)
        {
            mContent = new Content();
            mContent.mName = "My Content";
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
            if (contentsListBox.SelectedIndex != -1)
            {
                mContent.mName = contentObjectNameTextBox.Text;
                contentsListBox.SelectedIndexChanged -= contentsListBox_SelectedIndexChanged;
                contentsListBox.Items[contentsListBox.SelectedIndex] = contentObjectNameTextBox.Text;
                contentsListBox.SelectedIndexChanged += contentsListBox_SelectedIndexChanged;
                contentObjectNameTextBox.Focus();
                contentObjectNameTextBox.Select(contentObjectNameTextBox.Text.Length, 0);
            }
        }

        private void contentObjectTextTextBox_TextChanged(object sender, EventArgs e)
        {
            mContent.mText = contentObjectTextTextBox.Text;
        }

        private void contentObjectVideosNewButton_Click(object sender, EventArgs e)
        {
            mVideo = new Video();
            mVideo.mName = "My Video";
            mContent.mVideos.Add(mVideo);
            contentObjectVideosListBox.Items.Add(mVideo.mName);
            contentObjectVideosListBox.SelectedIndex = contentObjectVideosListBox.Items.Count - 1;
        }

        private void contentObjectVideosListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (contentObjectVideosListBox.SelectedIndex == -1)
            {
                videoNameTextBox.Enabled = false;
                videoIconPictureBox.Enabled = false;
                videoIconLoadButton.Enabled = false;
                videoVideoLoadButton.Enabled = false;
                contentObjectVideosUpButton.Enabled = false;
                contentObjectVideosDeleteButton.Enabled = false;
                contentObjectVideosDownButton.Enabled = false;

                mVideo = new Video();

                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer1.currentPlaylist.clear();
            }
            else
            {
                videoNameTextBox.Enabled = true;
                videoIconPictureBox.Enabled = true;
                videoIconLoadButton.Enabled = true;
                videoVideoLoadButton.Enabled = true;
                contentObjectVideosDeleteButton.Enabled = true;

                if (contentObjectVideosListBox.SelectedIndex != 0)
                    contentObjectVideosUpButton.Enabled = true;
                else
                    contentObjectVideosUpButton.Enabled = false;

                if (contentObjectVideosListBox.SelectedIndex != contentObjectVideosListBox.Items.Count - 1)
                    contentObjectVideosDownButton.Enabled = true;
                else
                    contentObjectVideosDownButton.Enabled = false;

                mVideo = mContent.mVideos.ElementAt(contentObjectVideosListBox.SelectedIndex);
                playVideo();
            }

            videoNameTextBox.Text = mVideo.mName;

            MemoryStream ms = new MemoryStream(mVideo.mIcon, 0, mVideo.mIcon.Length);
            ms.Write(mVideo.mIcon, 0, mVideo.mIcon.Length);
            videoIconPictureBox.Image = Image.FromStream(ms);
        }

        private void videoNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (contentObjectVideosListBox.SelectedIndex != -1)
            {
                mVideo.mName = videoNameTextBox.Text;
                contentObjectVideosListBox.SelectedIndexChanged -= contentObjectVideosListBox_SelectedIndexChanged;
                contentObjectVideosListBox.Items[contentObjectVideosListBox.SelectedIndex] = videoNameTextBox.Text;
                contentObjectVideosListBox.SelectedIndexChanged += contentObjectVideosListBox_SelectedIndexChanged;
                videoNameTextBox.Focus();
                videoNameTextBox.Select(videoNameTextBox.Text.Length, 0);
            }
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

        private void videoVideoLoadButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openVideoFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                mVideo.mData = File.ReadAllBytes(openVideoFileDialog.FileName);
                playVideo();
            }
        }

        private void playVideo()
        {
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += new DoWorkEventHandler(
                    delegate(object o, DoWorkEventArgs args)
                    {
                        BackgroundWorker b = o as BackgroundWorker;

                        axWindowsMediaPlayer1.Ctlcontrols.stop();
                        axWindowsMediaPlayer1.currentPlaylist.clear();
                        using (FileStream fs = File.Create("video.mp4"))
                        {
                            fs.Write(mVideo.mData, 0, mVideo.mData.Length);
                        }
                        axWindowsMediaPlayer1.URL = "video.mp4";
                        axWindowsMediaPlayer1.settings.playCount = 999999999;

                    });
            bw.RunWorkerAsync();
        }

        private void contentObjectActorsCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = contentObjectActorsCheckedListBox.SelectedIndex;
            string actor = contentObjectActorsCheckedListBox.Items[index].ToString();
            if (index != -1)
            {
                if (contentObjectActorsCheckedListBox.GetItemCheckState(index) == CheckState.Checked)
                    mContent.mActors.Add(actor);
                else if (contentObjectActorsCheckedListBox.GetItemCheckState(index) == CheckState.Unchecked)
                    mContent.mActors.Remove(actor);
            }
        }

        private void contentsListBoxUpButton_Click(object sender, EventArgs e)
        {
            contentsListBox.SelectedIndexChanged -= contentsListBox_SelectedIndexChanged;
            int index = contentsListBox.SelectedIndex;
            string tempString = contentsListBox.Items[index - 1].ToString();
            contentsListBox.Items[index - 1] = contentsListBox.Items[index];
            contentsListBox.Items[index] = tempString;

            Content tempContent = mApp.mContents[index - 1];
            mApp.mContents[index - 1] = mApp.mContents[index];
            mApp.mContents[index] = tempContent;

            index--;
            if (index == contentsListBox.Items.Count - 1)
                contentsListBoxDownButton.Enabled = false;
            else
                contentsListBoxDownButton.Enabled = true;

            if (index == 0)
                contentsListBoxUpButton.Enabled = false;
            else
                contentsListBoxUpButton.Enabled = true;

            contentsListBox.SelectedIndex = index;
            contentsListBox.SelectedIndexChanged += contentsListBox_SelectedIndexChanged;
        }

        private void contentsListBoxDownButton_Click(object sender, EventArgs e)
        {
            contentsListBox.SelectedIndexChanged -= contentsListBox_SelectedIndexChanged;
            int index = contentsListBox.SelectedIndex;
            string tempString = contentsListBox.Items[index + 1].ToString();
            contentsListBox.Items[index + 1] = contentsListBox.Items[index];
            contentsListBox.Items[index] = tempString;

            Content tempContent = mApp.mContents[index + 1];
            mApp.mContents[index + 1] = mApp.mContents[index];
            mApp.mContents[index] = tempContent;

            index++;
            if (index == contentsListBox.Items.Count - 1)
                contentsListBoxDownButton.Enabled = false;
            else
                contentsListBoxDownButton.Enabled = true;

            if (index == 0)
                contentsListBoxUpButton.Enabled = false;
            else
                contentsListBoxUpButton.Enabled = true;

            contentsListBox.SelectedIndex = index;
            contentsListBox.SelectedIndexChanged += contentsListBox_SelectedIndexChanged;
        }

        private void contentObjectVideosDeleteButton_Click(object sender, EventArgs e)
        {
            mContent.mVideos.RemoveAt(contentObjectVideosListBox.SelectedIndex);
            contentObjectVideosListBox.Items.RemoveAt(contentObjectVideosListBox.SelectedIndex);
        }

        private void contentObjectVideosUpButton_Click(object sender, EventArgs e)
        {
            contentObjectVideosListBox.SelectedIndexChanged -= contentObjectVideosListBox_SelectedIndexChanged;
            int index = contentObjectVideosListBox.SelectedIndex;
            string tempString = contentObjectVideosListBox.Items[index - 1].ToString();
            contentObjectVideosListBox.Items[index - 1] = contentObjectVideosListBox.Items[index];
            contentObjectVideosListBox.Items[index] = tempString;

            Video tempVideo = mContent.mVideos[index - 1];
            mContent.mVideos[index - 1] = mContent.mVideos[index];
            mContent.mVideos[index] = tempVideo;

            index--;
            if (index == contentObjectVideosListBox.Items.Count - 1)
                contentObjectVideosDownButton.Enabled = false;
            else
                contentObjectVideosDownButton.Enabled = true;

            if (index == 0)
                contentObjectVideosUpButton.Enabled = false;
            else
                contentObjectVideosUpButton.Enabled = true;

            contentObjectVideosListBox.SelectedIndex = index;
            contentObjectVideosListBox.SelectedIndexChanged += contentObjectVideosListBox_SelectedIndexChanged;
        }

        private void contentObjectVideosDownButton_Click(object sender, EventArgs e)
        {
            contentObjectVideosListBox.SelectedIndexChanged -= contentObjectVideosListBox_SelectedIndexChanged;
            int index = contentObjectVideosListBox.SelectedIndex;
            string tempString = contentObjectVideosListBox.Items[index + 1].ToString();
            contentObjectVideosListBox.Items[index + 1] = contentObjectVideosListBox.Items[index];
            contentObjectVideosListBox.Items[index] = tempString;

            Video tempVideo = mContent.mVideos[index + 1];
            mContent.mVideos[index + 1] = mContent.mVideos[index];
            mContent.mVideos[index] = tempVideo;

            index++;
            if (index == contentObjectVideosListBox.Items.Count - 1)
                contentObjectVideosDownButton.Enabled = false;
            else
                contentObjectVideosDownButton.Enabled = true;

            if (index == 0)
                contentObjectVideosUpButton.Enabled = false;
            else
                contentObjectVideosUpButton.Enabled = true;

            contentObjectVideosListBox.SelectedIndex = index;
            contentObjectVideosListBox.SelectedIndexChanged += contentObjectVideosListBox_SelectedIndexChanged;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveXMLFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                mApp.save(saveXMLFileDialog.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openXMLFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                mApp = App.load(openXMLFileDialog.FileName);
                mContent = new Content();
                mVideo = new Video();

                applicationNameTextBox.Text = mApp.mName;

                MemoryStream ms = new MemoryStream(mApp.mIcon, 0, mApp.mIcon.Length);
                ms.Write(mApp.mIcon, 0, mApp.mIcon.Length);
                applicationIconPictureBox.Image = Image.FromStream(ms);

                actorsListBox.Items.Clear();
                contentObjectActorsCheckedListBox.Items.Clear();
                foreach (string actor in mApp.mActors)
                {
                    actorsListBox.Items.Add(actor);
                    contentObjectActorsCheckedListBox.Items.Add(actor);
                }

                contentsListBox.Items.Clear();
                foreach (Content content in mApp.mContents)
                    contentsListBox.Items.Add(content.mName);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you wish to discard any unsaved progress?", "New App", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                mApp = new App();
                mContent = new Content();
                mVideo = new Video();

                applicationNameTextBox.Text = mApp.mName;

                MemoryStream ms = new MemoryStream(mApp.mIcon, 0, mApp.mIcon.Length);
                ms.Write(mApp.mIcon, 0, mApp.mIcon.Length);
                applicationIconPictureBox.Image = Image.FromStream(ms);

                actorsListBox.Items.Clear();
                contentObjectActorsCheckedListBox.Items.Clear();

                contentsListBox.Items.Clear();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you wish to lose any unsaved progress?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Close();
            }
        }

        private void contactDeveloperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string command = "mailto:ianpedwards87@gmail.com?subject=XML Tool";
            Process.Start(command);
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/TVOHM/MOD-XMLTool/issues");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version 2.0\nTeam7 ISYBATH\nIan Edwards 2013\nianpedwards87@gmail.com", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
