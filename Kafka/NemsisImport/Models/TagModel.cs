using Azure.Storage.Blobs.Models;
using System.Reflection;

namespace NemsisImport.Models;

public class TagModel
{
    public string? TagBlobName { get; set; }

    public string? TagBlobType { get; set; }

    public DateTime? TagCreated { get; set; }

    public BlobUploadOptions ToBlobUploadOptions()
    {
        var uploadOptions = new BlobUploadOptions();
        var tags = this.ToDictionary();
        uploadOptions.Tags = tags;
        return uploadOptions;
    }

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> tags = new Dictionary<string, string>();

        // convert TagModel instance to tags
        var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            var propValue = prop.GetValue(this, null);
            if (propValue != null)
            {
                var dateValue = propValue as DateTime?;
                if (dateValue != null)
                {
                    tags.Add(prop.Name, dateValue.Value.ToString(TagConstants.Tag_Date_Format));
                    continue;
                }
                tags.Add(prop.Name, $"{propValue}");
            }
        }

        return tags;

    }
}
