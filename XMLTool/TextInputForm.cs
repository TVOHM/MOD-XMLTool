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
    public partial class TextInputForm : Form
    {
        public string mTitle { get; private set; }
        public string mInput { get; set; }

        public TextInputForm(string title, string text="")
        {
            mTitle = title;
            mInput = text;
            InitializeComponent();
        }

        private void TextInputForm_Load(object sender, EventArgs e)
        {
            Text = mTitle;
            inputTextBox.DataBindings.Add("Text", this, "mInput");
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
