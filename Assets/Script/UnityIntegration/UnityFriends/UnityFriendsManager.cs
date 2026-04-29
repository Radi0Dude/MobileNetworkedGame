using System.Threading.Tasks;
using Unity.Services;
using Unity.Services.Friends;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using UnityEngine;

public class UnityFriendsManager : MonoBehaviour
{
    
    public async Task SendFriendRequest(string friendPlayerId)
    {
        await FriendsService.Instance.AddFriendAsync(friendPlayerId);
    }
}

