using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
public class GenerateFriendCode : MonoBehaviour
{
    string playerId;
    [SerializeField]
    private RawImage qrCodeImage;
    private void Start()
    {
        OnDisplayFriendCode();
    }
    public async void OnDisplayFriendCode()
    {
        playerId = await UnityAuthManager.Instance.GetPlayerId();
        qrCodeImage.texture = GenerateQRCode(playerId);
        Debug.Log($"Player ID: {playerId}");
    }
    public Texture2D GenerateQRCode(string text)
    {
        var encode = new Texture2D(256, 256);
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new ZXing.Common.EncodingOptions
            {
                Height = encode.height,
                Width = encode.width,
                Margin = 1,

            }
        };
        var pixelData = writer.Write(text);
        encode = new Texture2D(pixelData.Width, pixelData.Height, TextureFormat.RGBA32, false);

        encode.LoadRawTextureData(pixelData.Pixels);
        encode.Apply();
        return encode;
    }
}
