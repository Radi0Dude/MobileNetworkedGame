using TMPro;
using UnityEngine;

public class GetCorrectInfoOnHover : MonoBehaviour
{
    [SerializeField]
    TMP_Text showNameText;
    [SerializeField]
    TMP_Text ShowURL;

    public void SetInfo(string showName, string showURL)
    {
        showNameText.text = showName;
        ShowURL.text = showURL;
    }
}
