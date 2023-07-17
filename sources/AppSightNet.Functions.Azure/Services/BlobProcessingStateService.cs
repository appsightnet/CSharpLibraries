using Azure.Storage.Blobs;
using Azure;
using System.Text.Json;
using AppSightNet.Functions.Azure.Options;
using AppSightNet.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Text;
using AppSightNet.Functions.Azure.Schemas;

namespace AppSightNet.Functions.Azure.Services;

public class BlobProcessingStateService<TState> : IProcessingStateService<TState>
    where TState : IProcessingState
{
    private BlobServiceClient _blobServiceClient { get; }
    private BlobProcessingStateServiceOptions _options { get; }

    public BlobProcessingStateService(
        BlobServiceClient blobServiceClient,
        BlobProcessingStateServiceOptions options
    )
    {
        this._blobServiceClient = blobServiceClient;
        this._options = options;
    }

    public async Task<TState?> GetOrDefaultAsync(
        string key,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await this.GetAsync(key, cancellationToken).ConfigureAwait(false);
        }
        catch (RequestFailedException ex) when (ex.Status == StatusCodes.Status404NotFound)
        {
            return default;
        }
    }

    public async Task<TState> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        var containerClient = this._blobServiceClient.GetBlobContainerClient(
            this._options.BlobContainerName
        );
        var blobClient = containerClient.GetBlobClient(key);

        var downloadResult = await blobClient
            .DownloadContentAsync(cancellationToken)
            .ConfigureAwait(false);
        var state = JsonSerializer.Deserialize<TState>(
            downloadResult.Value.Content.ToString(),
            JsonSerializerOptionsProvider.DefaultOptions
        );
        return state ?? throw new JsonException("Failed to deserialize.");
    }

    public async Task PutAsync(
        string key,
        TState state,
        CancellationToken cancellationToken = default
    )
    {
        var containerClient = this._blobServiceClient.GetBlobContainerClient(
            this._options.BlobContainerName
        );
        var blobClient = containerClient.GetBlobClient(key);

        var content = JsonSerializer.Serialize(state, JsonSerializerOptionsProvider.DefaultOptions);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        await blobClient
            .UploadAsync(content: stream, overwrite: true, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
