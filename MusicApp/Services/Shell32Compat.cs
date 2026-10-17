namespace MusicApp.Services
{
    // Lightweight COM wrapper around Shell.Application to avoid a COMReference in SDK-style projects.
    internal class Shell
    {
        private dynamic _comShell;
        public Shell()
        {
            Type t = Type.GetTypeFromProgID("Shell.Application");
            if (t == null) throw new InvalidOperationException("Shell.Application not available");
            _comShell = Activator.CreateInstance(t);
        }

        public Folder NameSpace(string path)
        {
            try
            {
                var ns = _comShell.NameSpace(path);
                return ns == null ? null : new Folder(ns);
            }
            catch
            {
                return null;
            }
        }
    }

    internal class Folder
    {
        private dynamic _folder;
        public Folder(dynamic folder)
        {
            _folder = folder;
        }

        public FolderItem ParseName(string name)
        {
            try
            {
                var item = _folder.ParseName(name);
                return item == null ? null : new FolderItem(item);
            }
            catch
            {
                return null;
            }
        }

        public string GetDetailsOf(FolderItem item, int index)
        {
            try
            {
                if (item == null) return string.Empty;
                var inner = item.Inner;
                var result = _folder.GetDetailsOf(inner, index);
                return result == null ? string.Empty : result.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    internal class FolderItem
    {
        internal dynamic Inner { get; }
        public FolderItem(dynamic inner)
        {
            Inner = inner;
        }
    }
}