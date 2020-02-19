using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MVCSite.Common
{
    public static class FileSystemHelper
    {
        public static DirectoryInfo GetFolderFromParentPaths(string name)
        {
            var executingAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", string.Empty);
            var dir = new DirectoryInfo(executingAssemblyPath);
            DirectoryInfo extractionScriptsDir = null;
            do
            {
                extractionScriptsDir = dir.GetDirectories().Where(x => x.Name.ToUpper() == name.ToUpper()).SingleOrDefault();
            } while ((dir = dir.Parent) != null && extractionScriptsDir == null);
            if (extractionScriptsDir == null)
                throw new Exception(string.Format("Unable to find folder specified. Executing assembly path:'{0}'", executingAssemblyPath));

            return extractionScriptsDir;
        }
        public static DirectoryInfo GetPhysicalPath(string relativeDir)
        {
            var executingAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", string.Empty);
            var dir = new DirectoryInfo(executingAssemblyPath + relativeDir);
            return dir;
        }
        public static string GenerateTourImagePath(string extension)
        {
            var randomId = new Randoms().Next(1, 2000);
            return "Tour/" + randomId.ToString() + "/" + Images.GenerateFileName(string.IsNullOrEmpty(extension) ? "jpg" : extension);
        }
        public static string GenerateAvatarImagePath(string extension)
        {
            var randomId = new Randoms().Next(1, 2000);
            return "Avatar/" + randomId.ToString() + "/" + Images.GenerateFileName(string.IsNullOrEmpty(extension) ? "jpg" : extension);
        }
        public static string GenerateVideoPath(string extension)
        {
            var randomId = new Randoms().Next(1, 2000);
            return "Video/" + randomId.ToString() + "/" + Images.GenerateFileName(string.IsNullOrEmpty(extension) ? "mp4" : extension);
        }
        public static void CheckAndDeleteOldFile(string dir, string path, ILogger _logger)
        {
            if (string.IsNullOrEmpty(path))
                return;
            var oldPath = Path.Combine(dir, path);
            oldPath = oldPath.Replace(@"/", @"\");
            if (!File.Exists(oldPath))
                return;
            //File.Delete(oldImagePath);
            FileHelper.DeleteFileAndParentDirIfEmpty(oldPath);
            _logger.LogInfo(string.Format("Old file - {0} DELETED.", oldPath));
        }
    }
}
