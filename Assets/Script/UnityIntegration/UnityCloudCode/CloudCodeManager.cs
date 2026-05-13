using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using UnityEngine;

public class UnityCloudCodeManager
{
    private static UnityCloudCodeManager _instance;
    public static UnityCloudCodeManager Instance => _instance ??= new UnityCloudCodeManager();
    public async Task<string> GetRandomShow()
    {
        var showId = await CloudCodeService.Instance.CallEndpointAsync<string>("GetRandomShow", new Dictionary<string, object>());
        return showId.ToString();
    }

    public async Task GetNewShowRemoveOld(string showId)
    {
        showId = await CloudCodeService.Instance.CallEndpointAsync<string>(
        "RemoveAndGetNewShow",
        new Dictionary<string, object> { { "showId", showId } });
    }
}
