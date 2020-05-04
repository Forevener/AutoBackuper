using Autobackuper.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Autobackuper
{
    public partial class MainForm : Form
    {
        readonly string appPath = "\"" + Application.ExecutablePath + "\"";
        readonly RegistryKey keyAutoStart = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private FormWindowState previousWindowState = FormWindowState.Normal;
        private readonly Config config = Config.Load();
        readonly Dispatcher mainDispatcher = Dispatcher.CurrentDispatcher;
        readonly List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();

        public MainForm()
        {
            InitializeComponent();
            linkLabel1.Links.Add(13, 12, "https://www.flaticon.com/authors/dinosoftlabs");
            linkLabel1.Links.Add(31, 16, "https://www.flaticon.com");

            if (config.StartMinimized)
            {
                WindowState = FormWindowState.Minimized;
            }

            FillControls();
            if (config.WatchedFolders.Count > 0)
            {
                UpdateDataGrid();
            }
            
            object keyValue = keyAutoStart.GetValue("AutoBackuper");
            if (keyValue == null)
            {
                checkBoxAutostart.Checked = false;
            }
            else
            {
                checkBoxAutostart.Checked = true;
                if (!keyValue.ToString().StartsWith(appPath))
                {
                    keyAutoStart.SetValue("AutoBackuper", appPath);
                    Log("Application directory have changed, updating registry key");
                }
            }
            
            Resize += MainForm_Resize;
            Load += MainForm_Load;
            notifyIconTray.MouseClick += NotifyIconTray_MouseClick;
            toolStripMenuItemShow.Click += ToolStripMenuItemShow_Click;
            toolStripMenuItemExit.Click += ToolStripMenuItemExit_Click;
            tabControlMain.SelectedIndexChanged += TabControlMain_SelectedIndexChanged;
            checkBoxAutostart.CheckedChanged += CheckBoxAutostart_CheckedChanged;
            checkBoxMinimize.CheckedChanged += CheckBoxMinimize_CheckedChanged;
            checkBoxClose.CheckedChanged += CheckBoxClose_CheckedChanged;
            checkBoxConfirm.CheckedChanged += CheckBoxConfirm_CheckedChanged;
            checkBoxStartMin.CheckedChanged += CheckBoxStartMin_CheckedChanged;
            linkLabel1.LinkClicked += LinkLabel1_LinkClicked;
            textBoxLog.VisibleChanged += TextBoxLog_VisibleChanged;
            dataGridViewMain.CellMouseDoubleClick += DataGridViewMain_CellMouseDoubleClick;
            FormClosing += MainForm_FormClosing;
        }

        private void DataGridViewMain_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                Process.Start(config.WatchedFolders[e.RowIndex].folder);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (config.MinimizeToTray && config.StartMinimized)
            {
                Conceal();
            }
            foreach (WatchedFolder f in config.WatchedFolders)
            {
                AddWatcher(f.folder);
            }
        }

        private void AddWatcher(string folder)
        {
            watchers.Add(new FileSystemWatcher(folder) { EnableRaisingEvents = true });
            watchers.Last().Created += Watcher_Alarm;
            watchers.Last().Changed += Watcher_Alarm;
            watchers.Last().Renamed += Watcher_Alarm;
            watchers.Last().Deleted += Watcher_Alarm;
        }

        private void Watcher_Alarm(object sender, FileSystemEventArgs e)
        {
            if (!e.Name.Contains("-AS_B"))
            {
                Log(e.FullPath + " - " + e.ChangeType);
                if (e.ChangeType != WatcherChangeTypes.Deleted)
                {
                    try
                    {
                        WatchedFolder watchedFolder = config.WatchedFolders[watchers.IndexOf(sender as FileSystemWatcher)];
                        bool isDirectory = !File.Exists(e.FullPath);
                        DirectoryInfo backupDir = new DirectoryInfo(watchedFolder.backupPath);
                        IOrderedEnumerable<FileInfo> backups = backupDir.GetFiles(e.Name + "-AS_B*.zip").OrderBy(fi => fi.LastWriteTime);
                        
                        if (backups.Count() == 0 || (DateTime.Now - backups.Last().LastWriteTime).TotalSeconds >= watchedFolder.interval)
                        {
                            string zipPath = backups.Count() < watchedFolder.backupSlots
                                ? Path.Combine(backupDir.FullName, e.Name + GetVacantSuffix(backups, watchedFolder.backupSlots))
                                : backups.First().FullName;
                            
                            if (isDirectory)
                            {
                                ZipFile.CreateFromDirectory(e.FullPath, zipPath, CompressionLevel.Optimal, true);
                            }
                            else
                            {
                                using (FileStream stream = new FileStream(zipPath, FileMode.Create))
                                {
                                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create))
                                    {
                                        archive.CreateEntryFromFile(e.FullPath, e.Name, CompressionLevel.Optimal);
                                    }
                                }
                            }
                            Log("Successfully saved to " + zipPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("ERROR: " + ex.Message);
                    }
                }
            }
        }

        private string GetVacantSuffix(IOrderedEnumerable<FileInfo> backups, int total)
        {
            int[] used = new int[backups.Count()];
            for (int i = 0; i < used.Length; i++)
            {
                used[i] = int.Parse(backups.ElementAt(i).Name.Split(new string[] { "-AS_B" }, StringSplitOptions.None).Last().Split(new char[] { '.' }).First());
            }
            return "-AS_B" + Enumerable.Range(1, total).Except(used).Min().ToString() + ".zip";
        }

        private void CheckBoxStartMin_CheckedChanged(object sender, EventArgs e)
        {
            config.StartMinimized = checkBoxStartMin.Checked;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            config.Save();
            if (e.CloseReason == CloseReason.UserClosing && config.CloseToTray)
            {
                WindowState = FormWindowState.Minimized;
                Conceal();
                e.Cancel = true;
            }
            else if (config.ConfirmExit && MessageBox.Show("Do you really want to exit the AutoBackuper?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void CheckBoxConfirm_CheckedChanged(object sender, EventArgs e)
        {
            config.ConfirmExit = checkBoxConfirm.Checked;
        }

        private void CheckBoxClose_CheckedChanged(object sender, EventArgs e)
        {
            config.CloseToTray = checkBoxClose.Checked;
        }

        private void FillControls()
        {
            checkBoxMinimize.Checked = config.MinimizeToTray;
            checkBoxClose.Checked = config.CloseToTray;
            checkBoxConfirm.Checked = config.ConfirmExit;
            checkBoxStartMin.Checked = config.StartMinimized;
        }

        private void TextBoxLog_VisibleChanged(object sender, EventArgs e)
        {
            if (textBoxLog.Visible)
            {
                textBoxLog.SelectionStart = textBoxLog.TextLength;
                textBoxLog.ScrollToCaret();
            }
        }

        private void ToolStripMenuItemShow_Click(object sender, EventArgs e)
        {
            Reveal();
        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NotifyIconTray_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Reveal();
            }
        }

        private void Reveal()
        {
            notifyIconTray.Visible = false;
            
            Show();
            WindowState = previousWindowState;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (checkBoxMinimize.Checked)
                {
                    Conceal();
                }
            }
            else
            {
                previousWindowState = WindowState;
            }
        }

        private void Conceal()
        {
            Hide();
            notifyIconTray.Visible = true;
        }

        private void CheckBoxMinimize_CheckedChanged(object sender, EventArgs e)
        {
            config.MinimizeToTray = checkBoxMinimize.Checked;
        }

        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedIndex == 2)
            {
                tabPage3.Text = "Log";
            }
            if (tabControlMain.SelectedIndex != 1)
            {
                config.Save();
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
                linkLabel1.Links[linkLabel1.Links.IndexOf(e.Link)].Visited = true;
            }
            catch (Exception)
            {
                Log("Unable to open the link");
            }
        }

        private void CheckBoxAutostart_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutostart.Checked)
            {
                keyAutoStart.SetValue("AutoBackuper", appPath);
                Log("Registry key is set");
            }
            else
            {
                keyAutoStart.DeleteValue("AutoBackuper", false);
                Log("Registry key is removed");
            }
        }

        private void Log(string text)
        {
            mainDispatcher.Invoke(new Action(() => 
            {
                textBoxLog.AppendText(DateTime.Now.ToLocalTime() + " " + text + Environment.NewLine);
                if (tabControlMain.SelectedIndex != 2)
                {
                    tabPage3.Text = "*Log";
                }
            }));
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count > 0)
            {
                int index = dataGridViewMain.SelectedRows[0].Index;
                using (WatchedFolderDialog dialog = new WatchedFolderDialog(config.WatchedFolders[index]))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        config.UpdateFolder(index, dialog.WatchedFolder);
                        watchers[index].Path = dialog.WatchedFolder.folder;
                        UpdateDataGrid();
                    }
                }
            }

        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count > 0)
            {
                int index = dataGridViewMain.SelectedRows[0].Index;
                if (index < dataGridViewMain.Rows.Count)
                {
                    dataGridViewMain.Rows.RemoveAt(index);
                    config.RemoveFolder(index);
                    watchers[index].Dispose();
                    watchers.RemoveAt(index);
                }
            }
        }

        private void UpdateDataGrid()
        {
            int index = dataGridViewMain.SelectedRows.Count > 0 ? dataGridViewMain.SelectedRows[0].Index : 0;
            dataGridViewMain.Rows.Clear();
            foreach (WatchedFolder wf in config.WatchedFolders)
            {
                dataGridViewMain.Rows.Add(new object[] { wf.folder, wf.interval });
            }
            dataGridViewMain.Rows[index].Selected = true;
            dataGridViewMain.CurrentCell = dataGridViewMain.Rows[index].Cells[0];
        }

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            using (WatchedFolderDialog dialog = new WatchedFolderDialog(null))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    config.AddFolder(dialog.WatchedFolder);
                    AddWatcher(dialog.WatchedFolder.folder);
                    UpdateDataGrid();
                }
            }
        }
    }
}
