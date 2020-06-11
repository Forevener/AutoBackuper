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

namespace Autobackuper
{
    public partial class WatchedFolderDialog : Form
    {
        private readonly bool[] pathsOK = new bool[] { false, false };
        private readonly List<ToolTip> toolTips = new List<ToolTip>(2);
        public WatchedFolder WatchedFolder { get; set; }

        public WatchedFolderDialog(WatchedFolder folder)
        {
            InitializeComponent();
            for (int i = 0; i < 2; i++)
            {
                toolTips.Add(new ToolTip()
                {
                    InitialDelay = 0,
                    ShowAlways = true,
                    UseAnimation = false,
                    UseFading = false,
                });
            }

            textBoxWatched.TextChanged += TextBoxPath_TextChanged;
            textBoxBackup.TextChanged += TextBoxPath_TextChanged;
            comboBoxType.SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;

            if (folder != null)
            {
                textBoxWatched.Text = folder.Folder;
                comboBoxType.SelectedIndex = (int)folder.StorageType;
                textBoxBackup.Text = folder.BackupPath;
                numericUpDownInterval.Value = folder.Interval;
                numericUpDownSlots.Value = folder.BackupSlots;
            }
            else
            {
                comboBoxType.SelectedIndex = 0;
            }
        }

        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxBackup.Enabled = labelBackup.Enabled = buttonBackup.Enabled = comboBoxType.SelectedIndex == 2;
            CheckOK();
        }

        private void TextBoxPath_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Enabled)
            {
                CheckOK();
            }
        }

        private void CheckOK()
        {
            RefreshBackupFolder();
            foreach (TextBox textBox in tableLayoutPanel1.Controls.OfType<TextBox>().Where(t => t.Tag != null))
            {
                int index = Convert.ToInt32(textBox.Tag);
                pathsOK[index] = Directory.Exists(textBox.Text);

                if (!pathsOK[index])
                {
                    toolTips[index].Show("Path is not valid", textBox, new Point(textBox.Width, textBox.Height * -1));
                }
                else
                {
                    toolTips[index].Hide(textBox);
                }
            }
            buttonOK.Enabled = !pathsOK.Contains(false);
        }

        private void ButtonWatched_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxWatched.Text = dialog.SelectedPath;
                }
            }
        }

        private void ButtonBackup_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxBackup.Text = dialog.SelectedPath;
                }
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            WatchedFolder = new WatchedFolder()
            {
                Folder = textBoxWatched.Text,
                StorageType = (StorageType)comboBoxType.SelectedIndex,
                BackupPath = textBoxBackup.Text,
                Interval = Convert.ToInt32(numericUpDownInterval.Value),
                BackupSlots = Convert.ToInt32(numericUpDownSlots.Value)
            };
        }

        private void RefreshBackupFolder()
        {
            if (comboBoxType.SelectedIndex == 0)
            {
                textBoxBackup.Text = textBoxWatched.Text;
            }
            else if (comboBoxType.SelectedIndex == 1)
            {
                textBoxBackup.Text = Application.StartupPath;
            }
        }
    }
}
