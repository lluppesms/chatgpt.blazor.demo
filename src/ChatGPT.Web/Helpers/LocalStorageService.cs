namespace ChatGPT.Web.Helpers;

/// <summary>
/// Session Storage Service
/// </summary>
public class SessionStorageService
{
    readonly ILocalStorageService localStorageSvc;

    /// <summary>
    /// Get State
    /// </summary>
    public async Task<SessionState> GetState(string objectName)
    {
        SessionState state = new();
        var json = await localStorageSvc.GetItemAsync<string>(objectName);
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
    public async Task StoreState(string objectName, SessionState state)
    {
        var json = JsonConvert.SerializeObject(state);
        json = json.Replace("\"", "~");
        await localStorageSvc.SetItemAsync(objectName, json);
    }

    /// <summary>
    /// Remove State
    /// </summary>
    public async Task RemoveState(string objectName)
    {
        await localStorageSvc.RemoveItemAsync(objectName);
    }

    /// <summary>
    /// Initialization
    /// </summary>
    public SessionStorageService(ILocalStorageService svc)
    {
        localStorageSvc = svc;
    }
}
