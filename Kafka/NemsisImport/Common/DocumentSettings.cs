using NemsisImport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemsisImport.Common
{
    public class DocumentSettings : Settings
    {
        private readonly Lazy<HashSet<string>> _imageMimeTypes;

        public DocumentSettings()
        {
            _imageMimeTypes = new Lazy<HashSet<string>>(() => GetImageMimeTypes());
        }

        public string? ValidFileTypes { get; set; }

        public int PdfRenderingTimeout { get; set; } = 60;

        public string FilePathTemplate { get; set; } = string.Empty;

        public string IronPdfLicense { get; set; } = string.Empty;

        public Dictionary<string, string> MimeTypes { get; set; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public string? GetMimeType(string? type)
        {
            string key = type ?? string.Empty;
            if (!MimeTypes.ContainsKey(key))
            {
                return default;
            }

            return MimeTypes[key];
        }

        private HashSet<string> GetImageMimeTypes()
        {
            var imageMimeTypes =
            Enum.GetValues(typeof(ImageType))!
                .Cast<ImageType>()!
                .SelectMany(m => MimeTypes[m.ToString()].Split(','))
                .Select(m => m.Trim())
                .ToHashSet();
            return imageMimeTypes;
        }

        public bool IsImageMimeType(string? mimeType)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return _imageMimeTypes.Value.Contains(mimeType);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        //public HostMapping? HostMapping { get; set; }

        public int MaximumSigners { get; set; }

    }

   
}
