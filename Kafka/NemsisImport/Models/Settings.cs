using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemsisImport.Models
{
    public class Settings
    {
        public Dictionary<string, string> DBConnectionStrings { get; set; } = new Dictionary<string, string>();

        //public string? GlobalDBConnectionString { get; set; }

        ////public KafkaSettings? KafkaSettings { get; set; }
        ///**
        // * ER-4098 : Temporarily kept this variable
        // */
        //public string? WorkqueueApi { get; set; }

        //public string? SnowflakeConnectionString { get; set; }

        //public string? SnowflakeSchema { get; set; }

        //public Auth0Settings? Auth0Config { get; set; }

        //public FdbSettings? FdbConfig { get; set; }

        //public NccnSettings? NccnConfig { get; set; }

        /// <summary>
        /// Returns the practiceId's that have a valid connection string
        /// </summary>
        //public virtual IEnumerable<long> GetPracticeIds()
        //{
        //    static long? getLong(string sId)
        //    {
        //        if (long.TryParse(sId, out var id))
        //        {
        //            return id;
        //        }
        //        return null;
        //    }

        //    IEnumerable<long?> ids = DBConnectionStrings.Keys
        //        .Select(k => k.Replace("PracticeId_", string.Empty, StringComparison.Ordinal))
        //        .Select(id => getLong(id));

        //    return ids.Where(id => id.HasValue).Select(id => id!.Value);

        //}

        //public Dictionary<string, int> WorkQueueAlertInterval { get; set; } = new Dictionary<string, int>();

        //public string? GetPracticeDbConnection(long? practiceId)
        //{
        //    string dbKey = $"PracticeId_{practiceId?.ToString() ?? string.Empty}";
        //    if (!DBConnectionStrings.ContainsKey(dbKey))
        //    {
        //        return default;
        //    }

        //    return DBConnectionStrings[dbKey];
        //}

        //public long GetPracticeWQAlertInterval(long? practiceId)
        //{
        //    string practiceKey = $"PracticeId_{practiceId?.ToString() ?? string.Empty}";
        //    if (!WorkQueueAlertInterval.ContainsKey(practiceKey))
        //    {
        //        return -1;
        //    }

        //    return WorkQueueAlertInterval[practiceKey];
        //}


        public BlobStorageSettings? BlobStorage { get; set; }

        public Dictionary<string, string> Services { get; set; } = new Dictionary<string, string>();


    }
    public class BlobStorageSettings
    {
        public string? ConnectionString { get; set; } = string.Empty;
        public string? ContainerName { get; set; } = string.Empty;

    }

}
