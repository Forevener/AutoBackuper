using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Autobackuper
{
    public class Config
    {
        private bool minimizeToTray = false;
        private bool closeToTray = false;
        private bool confirmExit = false;
        private bool startMinimized = false;

        private bool unsaved = false;

        public bool MinimizeToTray 
        { 
            get => minimizeToTray;
            set
            {
                if (minimizeToTray != value)
                {
                    unsaved = true;
                    minimizeToTray = value;
                }
            }
        }
        public bool CloseToTray 
        { 
            get => closeToTray;
            set
            {
                if (closeToTray != value)
                {
                    unsaved = true;
                    closeToTray = value;
                }
            }
        }
        public bool ConfirmExit 
        { 
            get => confirmExit;
            set
            {
                if (confirmExit != value)
                {
                    unsaved = true;
                    confirmExit = value;
                }
            }
        }
        public bool StartMinimized 
        { 
            get => startMinimized;
            set
            {
                if (startMinimized != value)
                {
                    unsaved = true;
                    startMinimized = value;
                }
            }
        }

        [XmlArray]
        public List<WatchedFolder> WatchedFolders = new List<WatchedFolder>();

        public Config()
        {
            
        }

        public void Save()
        {
            if (unsaved)
            {
                try
                {
                    using (TextWriter writer = new StreamWriter("config.xml"))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Config));
                        serializer.Serialize(writer, this);
                        unsaved = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static Config Load()
        {
            if (File.Exists("config.xml"))
            {
                try
                {
                    using (TextReader reader = new StreamReader("config.xml"))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Config));
                        return serializer.Deserialize(reader) as Config;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return new Config();
                }
            }
            else
            {
                return new Config();
            }
        }

        public void AddFolder(WatchedFolder folder)
        {
            unsaved = true;
            WatchedFolders.Add(folder);
        }

        public void UpdateFolder(int index, WatchedFolder folder)
        {
            unsaved = true;
            WatchedFolders[index] = folder;
        }

        public void RemoveFolder(int index)
        {
            unsaved = true;
            WatchedFolders.RemoveAt(index);
        }
    }
}
