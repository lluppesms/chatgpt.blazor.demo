namespace chatgpt.blazor.Helpers;

/// <summary>
/// Session Storage Service
/// </summary>
public class SessionStorageService
{
    readonly ILocalStorageService localStorageSvc;

    /// <summary>
    /// Get State
    /// </summary>
    public async Task<SessionState> GetState()
    {
        SessionState state = new();
        var json = await localStorageSvc.GetItemAsync<string>(data.Constants.LocalStorage.SessionObject);
        if (!string.IsNullOrEmpty(json))
        {
            if (json.Contains('~'))
            {
                json = json.Replace("~", "\"");
            }
            state = JsonConvert.DeserializeObject<SessionState>(json);
        }
        return state;
    }

    /// <summary>
    /// Store State
    /// </summary>
    public async Task StoreState(SessionState state)
    {
        var json = JsonConvert.SerializeObject(state);
        json = json.Replace("\"", "~");
        await localStorageSvc.SetItemAsync(data.Constants.LocalStorage.SessionObject, json);
    }

    /// <summary>
    /// Remove State
    /// </summary>
    public async Task RemoveState()
    {
        await localStorageSvc.RemoveItemAsync(data.Constants.LocalStorage.SessionObject);
    }

    /// <summary>
    /// Initialization
    /// </summary>
    public SessionStorageService(ILocalStorageService svc)
    {
        localStorageSvc = svc;
    }
}
