using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
namespace FolderWatcher
{
    internal class Program
    {
        static void Main()
        {
            using var watcher = new FileSystemWatcher(@"C:\Projects\Input");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName;
                                 //| NotifyFilters.LastAccess
                                 //| NotifyFilters.LastWrite
                                 //| NotifyFilters.Security
                                 //| NotifyFilters.Size;

           // watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
           // watcher.Deleted += OnDeleted;
            //atcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "*.xml";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            XmlDocument doc = new XmlDocument();
            doc.Load(e.FullPath);
            SavetoDb(doc);
            
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

        private static bool SavetoDb(XmlDocument doc)
        {
            using (SqlConnection con = new SqlConnection("Data Source = (localdb)\\mssqllocaldb; Initial Catalog = Import; Integrated Security = True; Pooling = False"))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Import (PCRID,InputXml,Client,Status,Createdate) VALUES (@PcrId, @InputXml,@Client,@Status,@CreateDate)", con);
                DateTime currentTime = DateTime.UtcNow;
                long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
                cmd.Parameters.AddWithValue("@PcrId", unixTime);
                cmd.Parameters.AddWithValue("@InputXml", new SqlXml(new XmlTextReader(doc.InnerXml, XmlNodeType.Document,null)));
                cmd.Parameters.AddWithValue("@Client", "GMR");
                cmd.Parameters.AddWithValue("@Status", "Open");
                cmd.Parameters.AddWithValue("@CreateDate", System.DateTime.Now);

                cmd.ExecuteNonQuery();
            }

            return true;
        }
    }
}
