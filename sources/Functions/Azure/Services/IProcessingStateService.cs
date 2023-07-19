using AppSightNet.Functions.Azure.Schemas;

namespace AppSightNet.Functions.Azure.Services;

public interface IProcessingStateService<TState> where TState : IProcessingState
{
    Task<TState> GetAsync(string key, CancellationToken cancellationToken = default);
    Task<TState?> GetOrDefaultAsync(string key, CancellationToken cancellationToken = default);
    Task PutAsync(string key, TState state, CancellationToken cancellationToken = default);
}
