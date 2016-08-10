namespace L2dotNET.Models.GameModels
{
    public class L2Html
    {
        public string Filename { get; set; }
        public string Content { get; set; }
        public string Filepath { get; set; }

        public L2Html(string filename, string content, string filepath)
        {
            Filename = filename;
            Content = content;
            Filepath = filepath.Replace("\\", "/");
        }
    }
}