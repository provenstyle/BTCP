namespace BibleTraining.Web.UI.Features
{
    using System.Diagnostics;
    using System.Reflection;

    public static class VersionHelper
    {
        private static string _fileVersion = string.Empty;

        public static string VersionNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(_fileVersion)) return _fileVersion;

                var assembly = Assembly.GetExecutingAssembly();
                var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                _fileVersion = versionInfo.FileVersion;

                return _fileVersion;
            }
        }
    }
}