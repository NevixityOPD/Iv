using System;
using System.Text;

namespace Iv.Components
{
    public class Page
    {
        public Page()
        {
            _lines = new List<StringBuilder>()
            {
                new StringBuilder()
            };
            _cursor = new Cursor();
            _title = "NewFile.txt";
            _file_status = FileStatus.Unsaved;
            _directory = "";
        }

        public List<StringBuilder> _lines;
        public Cursor _cursor;
        public FileStatus _file_status;
        public string _title;
        public string _directory;
    }
}