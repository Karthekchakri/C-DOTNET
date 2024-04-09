//using System;
using Azure.Storage.Blobs;
using NemsisImport.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemsisImport.Interface
{
    public interface IBlobService
    {

        //Task<byte[]?> GetFileBytesAsync(string path, CancellationToken cancellationToken);
        //byte[]? GetFileBytes(string path, CancellationToken cancellationToken = default);
        //Task<IEnumerable<BlobFileInfoModel>> GetFilesAsync([NotNull] TagCriteriaModel criteria, CancellationToken cancellationToken);
        //IEnumerable<BlobFileInfoModel> GetFiles([NotNull] TagCriteriaModel criteria, CancellationToken cancellationToken = default);
        //Task SaveFileAsync(string path, byte[] contents, CancellationToken cancellationToken);
        //Task SaveFileAsync(string path, byte[] contents, TagModel? tagData, CancellationToken cancellationToken);
        //void SaveFile(string path, byte[] contents, CancellationToken cancellationToken = default);
        //void SaveFile(string path, byte[] contents, TagModel? tagData, CancellationToken cancellationToken = default);
        //Task DeleteFileAsync([NotNull] string path, CancellationToken cancellationToken);
        //void DeleteFile([NotNull] string path, CancellationToken cancellationToken = default);
        //Task UpdateFileContentAsync(string path, byte[] contents, CancellationToken cancellationToken);
        //Task<bool> MoveFileAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken);


        //IAsyncEnumerable<string> GetListAsync(string container, CancellationToken cancellationToken);

        //Task<string?> GetAsync(GetBlobRequest request);

        //IAsyncEnumerable<string?> GetAllAsync(string container, CancellationToken cancellationToken);

        //Task<string> SaveAsync(SaveBlobRequest request, CancellationToken cancellationToken);

        //Task<bool> DeleteAsync(DeleteBlobRequest request, CancellationToken cancellationToken);

        Task ListContainers();
        Task UploadBlobAsync();
        Task ListBlobsWithMetadataAsync();
        Task GetAllBlobsinContainer();
        Task<bool> MoveFileAsync();

        
    }
}




