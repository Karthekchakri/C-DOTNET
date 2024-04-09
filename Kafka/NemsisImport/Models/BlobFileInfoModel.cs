using Azure.Storage.Blobs.Models;
using System.Diagnostics.CodeAnalysis;

namespace NemsisImport.Models;

public class BlobFileInfoModel
{
    public BlobFileInfoModel([NotNull] TaggedBlobItem blobItem)
    {
        this.BlobPath = blobItem.BlobName;
        string? blobCreated;
        DateTime blobCreatedDateTime;
        blobItem.Tags.TryGetValue(TagConstants.Tag_Key_Created, out blobCreated);
        if (DateTime.TryParse(blobCreated, out blobCreatedDateTime))
        {
            this.BlobCreateDate = blobCreatedDateTime;
        }

    }

    public string? BlobPath { get; set; }
    public DateTime? BlobCreateDate { get; set; }

}
