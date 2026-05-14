using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using UnityEngine;

public class UnityCloudCodeManager
{
    private static UnityCloudCodeManager _instance;
    public static UnityCloudCodeManager Instance => _instance ??= new UnityCloudCodeManager();

    public async Task RemoveShow(string showId) 
    {
        var args = new Dictionary<string, object>
        {
            { "showId", showId }
        };

        await CloudCodeService.Instance.CallEndpointAsync<object>("RemoveShow", args);
    }
   
}
