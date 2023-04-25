using UnityEngine;
//using WebSocketSharp;
using NativeWebSocket;
public class BlockchainVisualizer : MonoBehaviour
{
    private WebSocket ws;

    async void Start()
    {
        // Connect to the Blockchain.com WebSocket API
        ws = new WebSocket("wss://ws.blockchain.info/inv");

        ws.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        ws.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        ws.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        ws.OnMessage += (bytes) =>
        {
            //Debug.Log("OnMessage!");
            //Debug.Log(bytes);

            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage: " + message);
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await ws.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
        if (ws.State == WebSocketState.Open)
        {
            // Sending plain text
            await ws.SendText("{\"op\":\"unconfirmed_sub\"}");
            await ws.SendText("{\"op\":\"blocks_sub\"}");
        }
    }

    private async void OnApplicationQuit()
    {
        await ws.Close();
    }
}