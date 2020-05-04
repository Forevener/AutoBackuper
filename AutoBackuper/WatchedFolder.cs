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
        public string folder;
        [XmlAttribute]
        public int interval;
        [XmlAttribute]
        public string backupPath;
        [XmlAttribute]
        public int backupSlots;
        [XmlAttribute]
        public StorageType storageType;

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
