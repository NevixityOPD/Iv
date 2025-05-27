using System;

namespace Iv.Components
{
    public class Page
    {
        public Page()
        {
            _lines = new List<string>();
            _cursor = new Cursor();
            _title = "NewFile";
            _file_status = FileStatus.Unsaved;
        }

        public List<string> _lines;
        public Cursor _cursor;
        public FileStatus _file_status;
        public string _title;
    }
}