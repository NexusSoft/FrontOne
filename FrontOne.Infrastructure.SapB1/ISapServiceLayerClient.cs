using FrontOne.Infrastructure.SapB1.Models;

namespace FrontOne.Infrastructure.SapB1;

public interface ISapServiceLayerClient
{
    Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    Task<T?> PostAsync<T>(string endpoint, object body, CancellationToken cancellationToken = default);
    Task PatchAsync(string endpoint, object body, CancellationToken cancellationToken = default);
    Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SapBatchResult>> BatchAsync(IEnumerable<SapBatchRequest> requests, CancellationToken cancellationToken = default);
}
