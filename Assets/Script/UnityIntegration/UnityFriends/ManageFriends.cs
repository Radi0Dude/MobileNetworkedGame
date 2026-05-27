using TMPro;
using UnityEngine;

public class ManageFriends : MonoBehaviour
{

    TMP_InputField inputField;
    private void Awake()
    {
        inputField = FindFirstObjectByType<TMP_InputField>();
    }

    public async void SendFriendRequest()
    {
        try
        {
            await UnityFriendsManager.Instance.SendFriendRequest(inputField.text);

        }
        catch
        {
            throw new System.Exception("Failed to send friend request. Please check the player ID and try again.");
        }
    }

    public async void MessagesFromFriends()
    {

    }
}
