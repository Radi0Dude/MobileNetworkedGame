using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.CloudSave;
using UnityEngine;

public class PlayerCreateName : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputField;

    bool clickWhileDisplayEmpty;
    [SerializeField]
    private string noNameText = "no name entered, please eneter a name";

    private string user;
    [SerializeField]
    private string playerNameFinder = "PlayerName";

    private void Start()
    {
        if(inputField == null)
            inputField = FindAnyObjectByType<TMP_InputField>();
        playerNameFinder = UnityCloudCodeManager.Instance.playerNameFinder;
    }

    public async void SetPlayerName()
    {
        var name = inputField.text;
        if(clickWhileDisplayEmpty) return;
        if (string.IsNullOrEmpty(name))
        {
            StartCoroutine(SetInputFieldToDisplayNoName());
            return;
        }
        await UnityCloudCodeManager.Instance.SavePlayerName(name);
    }

    public void OnInputFieldChange()
    { 
        if (!clickWhileDisplayEmpty)
            return;
        clickWhileDisplayEmpty = false;
        inputField.text = inputField.text.Replace(noNameText, "");
    }

    IEnumerator SetInputFieldToDisplayNoName()
    {
        inputField.SetTextWithoutNotify(noNameText);
        clickWhileDisplayEmpty = true;
        yield return new WaitForSeconds(2f);
        if(!clickWhileDisplayEmpty)
            yield break;
        inputField.text = "";
        clickWhileDisplayEmpty = false;
    }
}
