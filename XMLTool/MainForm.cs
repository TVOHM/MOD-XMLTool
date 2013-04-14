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
    /// <summary>
    /// The main for that is used for UI, contains all editable elements of the application
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The application object used to store the app data
        /// </summary>
        public App mApp = new App();

        /// <summary>
        /// The currently selected content object in the app
        /// </summary>
        public Content mContent = new Content();

        /// <summary>
        /// The currently selected video object inside the currently selected content object
        /// </summary>
        public Video mVideo = new Video();

        /// <summary>
        /// Constructor initialises the form
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the selected index in the actors list box is changed to update the state of the actor edit and delete buttons on the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void actorsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If no actor is selected, the delete and edit buttons are disabled
            if (actorsListBox.SelectedIndex == -1)
            {
                actorsListViewDeleteButton.Enabled = false;
                actorsListViewEditButton.Enabled = false;
            }
            // If an actor is selected the edit and delete buttons are enabled.
            else
            {
                actorsListViewDeleteButton.Enabled = true;
                actorsListViewEditButton.Enabled = true;
            }
        }

        /// <summary>
        /// Called when the new actor button is clicked, appends an actor to the actors list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void actorsListViewNewButton_Click(object sender, EventArgs e)
        {
            // Open the text input form
            TextInputForm dialog = new TextInputForm("Add Actor");
            DialogResult result = dialog.ShowDialog();
            // If the user clicks OK and the result is not empty
            if (result != DialogResult.Cancel && dialog.mInput.Length != 0)
            {
                // Ensure that this actor does not already exist
                if (mApp.mActors.Contains(dialog.mInput))
                {
                    // Report an error if duplicate
                    MessageBox.Show("Duplicate actor name \"" + dialog.mInput + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Add the unique actor
                    actorsListBox.Items.Add(dialog.mInput);
                    // Copy the contents of actorsListBox to update the store in mApp
                    mApp.mActors = actorsListBox.Items.OfType<string>().ToList();
                    // Also update the available options in contentObjectActorsCheckedListBox
                    contentObjectActorsCheckedListBox.Items.Add(dialog.mInput);
                }
            }
        }

        /// <summary>
        /// Called when the edit actor button is clicked. Edits an existing actor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void actorsListViewEditButton_Click(object sender, EventArgs e)
        {
            // Open the text input form
            TextInputForm dialog = new TextInputForm("Edit Actor", actorsListBox.Items[actorsListBox.SelectedIndex].ToString());
            DialogResult result = dialog.ShowDialog();
            // Only progress if the user clicks OK, the input is not empty and the input has been changed
            if (result != DialogResult.Cancel && dialog.mInput.Length != 0 && dialog.mInput != actorsListBox.Items[actorsListBox.SelectedIndex].ToString())
            {
                // Report the error if the edited name duplicates another actor
                if (mApp.mActors.Contains(dialog.mInput))
                {
                    MessageBox.Show("Duplicate actor name \"" + dialog.mInput + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // If everything is ok, update the list box
                    actorsListBox.Items[actorsListBox.SelectedIndex] = dialog.mInput;
                    // Update the contentObjectActorsCheckedListBox too
                    contentObjectActorsCheckedListBox.Items[actorsListBox.SelectedIndex] = dialog.mInput;
                    // Update the app store for actors
                    mApp.mActors = actorsListBox.Items.OfType<string>().ToList();
                }
            }
        }

        /// <summary>
        /// Called when the delete actor button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void actorsListViewDeleteButton_Click(object sender, EventArgs e)
        {
            // Removes the actor from the listBox, the checkedListBox and mApp.Actors
            contentObjectActorsCheckedListBox.Items.RemoveAt(actorsListBox.SelectedIndex);
            mApp.mActors.RemoveAt(actorsListBox.SelectedIndex);
            actorsListBox.Items.RemoveAt(actorsListBox.SelectedIndex);
        }

        /// <summary>
        /// Called when the load icon button is pressed to load a new application Icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            mApp.mName = applicationNameTextBox.Text;
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
                videoVideoPlayButton.Enabled = false;
                videoVideoLoadButton.Enabled = false;
                contentObjectVideosUpButton.Enabled = false;
                contentObjectVideosDeleteButton.Enabled = false;
                contentObjectVideosDownButton.Enabled = false;

                mVideo = new Video();
            }
            else
            {
                videoNameTextBox.Enabled = true;
                videoIconPictureBox.Enabled = true;
                videoVideoPlayButton.Enabled = true;
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
            }
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

        private void videoVideoPlayButton_Click(object sender, EventArgs e)
        {
            string temp = Path.GetTempPath() + "video.mp4";
            using (FileStream fs = File.Create(temp))
            {
                fs.Write(mVideo.mData, 0, mVideo.mData.Length);
                fs.Close();
                Process.Start(temp);
            }
        }

        private void exportToAndroidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
