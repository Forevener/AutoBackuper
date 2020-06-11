using Autobackuper.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
        readonly string regAutoStart = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private FormWindowState previousWindowState = FormWindowState.Normal;
        private readonly Config config = Config.Load();
        readonly Dispatcher mainDispatcher = Dispatcher.CurrentDispatcher;
        readonly List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
        private readonly string tempDir = Path.Combine(Path.GetTempPath(), "AutoBackuper");
        
        const string SUFFIX = "-AB_B";
        const int MAX_RETRIES = 1000;
        const int RETRY_DELAY = 1;

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

            using (RegistryKey keyAutoStart = Registry.CurrentUser.OpenSubKey(regAutoStart, true))
            {
                object keyValue = keyAutoStart.GetValue("AutoBackuper");
                if (keyValue == null)
                {
                    checkBoxAutostart.Checked = false;
                }
                else
                {
                    checkBoxAutostart.Checked = true;
                    if (!keyValue.ToString().StartsWith(appPath, StringComparison.Ordinal))
                    {
                        keyAutoStart.SetValue("AutoBackuper", appPath);
                        Log("Application directory have changed, updating registry key");
                    }
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
                Process.Start(config.WatchedFolders[e.RowIndex].Folder);
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
                AddWatcher(f.Folder);
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
            if (!e.Name.Contains(SUFFIX))
            {
                Log(e.FullPath + " - " + e.ChangeType);
                if (e.ChangeType != WatcherChangeTypes.Deleted)
                {
                    if (!Directory.Exists(tempDir))
                    {
                        Directory.CreateDirectory(tempDir);
                    }

                    string tempPath = Path.Combine(tempDir, e.Name + "_temp" + DateTime.Now.Ticks.ToString(CultureInfo.CurrentCulture));

                    try
                    {
                        WatchedFolder watchedFolder = config.WatchedFolders[watchers.IndexOf(sender as FileSystemWatcher)];
                        DirectoryInfo backupDir = new DirectoryInfo(watchedFolder.BackupPath);
                        IOrderedEnumerable<FileInfo> backups = backupDir.GetFiles(e.Name + SUFFIX + "*.zip").OrderBy(fi => fi.LastWriteTime);
                        
                        if (!backups.Any() || (DateTime.Now - backups.Last().LastWriteTime).TotalSeconds >= watchedFolder.Interval)
                        {
                            string zipPath = backups.Count() < watchedFolder.BackupSlots
                                ? Path.Combine(backupDir.FullName, e.Name + GetVacantSuffix(backups, watchedFolder.BackupSlots))
                                : backups.First().FullName;
                            
                            // is it a file?
                            if (File.Exists(e.FullPath))
                            {
                                using (FileStream stream = new FileStream(tempPath, FileMode.Create))
                                {
                                    using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create))
                                    {
                                        RetryIO(delegate () { archive.CreateEntryFromFile(e.FullPath, e.Name, CompressionLevel.Optimal); }, e.FullPath);
                                    }
                                }
                            }
                            else
                            {
                                RetryIO(delegate () { ZipFile.CreateFromDirectory(e.FullPath, tempPath, CompressionLevel.Optimal, true); }, e.FullPath);
                            }

                            if (File.Exists(zipPath))
                            {
                                File.Delete(zipPath);
                            }

                            File.Move(tempPath, zipPath);
                            Log("Successfully saved to " + zipPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("ERROR: " + ex.Message);
                    }

                    try
                    {
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                    }
                    catch
                    {
                        // do nothing really
                    }
                }
            }
        }

        private string GetVacantSuffix(IOrderedEnumerable<FileInfo> backups, int total)
        {
            int[] used = new int[backups.Count()];
            for (int i = 0; i < used.Length; i++)
            {
                used[i] = int.Parse(backups.ElementAt(i).Name.Split(new string[] { SUFFIX }, StringSplitOptions.None).Last().Split(new char[] { '.' }).First(), CultureInfo.CurrentCulture);
            }
            return SUFFIX + Enumerable.Range(1, total).Except(used).Min().ToString(CultureInfo.CurrentCulture) + ".zip";
        }

        private void RetryIO(Action action, string path)
        {
            for (int i = 0; i < MAX_RETRIES; i++)
            {
                try
                {
                    action();
                    break;
                }
                catch (IOException)
                {
                    Log(path + " is locked, retrying");
                    Thread.Sleep(RETRY_DELAY);
                }
            }
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
            using (RegistryKey keyAutoStart = Registry.CurrentUser.OpenSubKey(regAutoStart, true))
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
                        watchers[index].Path = dialog.WatchedFolder.Folder;
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
                dataGridViewMain.Rows.Add(new object[] { wf.Folder, wf.Interval });
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
                    AddWatcher(dialog.WatchedFolder.Folder);
                    UpdateDataGrid();
                }
            }
        }
    }
}
