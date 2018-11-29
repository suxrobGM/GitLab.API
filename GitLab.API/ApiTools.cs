using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLab.API
{
    static class ApiTools
    {
        public static ProjecVisibility GetProjecVisibility(string projecVisibility)
        {         
            switch (projecVisibility)
            {
                case "private":     return ProjecVisibility.Private;
                case "internal":    return ProjecVisibility.Internal;
                default:            return ProjecVisibility.Public;
            }          
        }

        public static ArchiveFormat GetArchiveFormat(string archiveFormat)
        {
            switch (archiveFormat)
            {
                case "tar.bz2":     return ArchiveFormat.TarBz2;
                case "tbz":         return ArchiveFormat.Tbz;
                case "tbz2":        return ArchiveFormat.Tbz2;
                case "tb2":         return ArchiveFormat.Tb2;
                case "bz2":         return ArchiveFormat.Bz2;
                case "tar":         return ArchiveFormat.Tar;
                case "zip":         return ArchiveFormat.Zip;
                default:            return ArchiveFormat.TarGz;
            }
        }

        public static string  GetArchiveFormat(ArchiveFormat archiveFormat)
        {
            switch (archiveFormat)
            {
                case ArchiveFormat.TarBz2:  return "tar.bz2";
                case ArchiveFormat.Tbz:     return "tbz";
                case ArchiveFormat.Tbz2:    return "tbz2";
                case ArchiveFormat.Tb2:     return "tb2";
                case ArchiveFormat.Bz2:     return "bz2";
                case ArchiveFormat.Tar:     return "tar";
                case ArchiveFormat.Zip:     return "zip";
                default: return "tar.gz";
            }
        }
    }
}
