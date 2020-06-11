using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Autobackuper
{
    public class WatchedFolder
    {
        [XmlAttribute]
        public string Folder { get; set; }
        [XmlAttribute]
        public int Interval { get; set; }
        [XmlAttribute]
        public string BackupPath { get; set; }
        [XmlAttribute]
        public int BackupSlots { get; set; }
        [XmlAttribute]
        public StorageType StorageType { get; set; }

        public WatchedFolder()
        { 
            
        }
    }

    public enum StorageType
    {
        WatchedFolder = 0,
        ProgramFolder = 1,
        SeparateFolder = 2
    }
}
