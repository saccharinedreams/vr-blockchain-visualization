using UnityEngine;
//using WebSocketSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using NativeWebSocket;
using TMPro;

public class BlockchainVisualizer : MonoBehaviour
{
    private WebSocket ws;
    private List<float> transactionVolumes = new List<float>();
    public TMP_Text dataText;
    public Texture2D heatMapTexture;
    private CustomGradient gradient;

    async void Start()
    {
        // Connect to the Blockchain.com WebSocket API
        ws = new WebSocket("wss://ws.blockchain.info/inv");
        gradient = new CustomGradient();
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
            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            //Debug.Log("OnMessage: " + message);
            TransactionData data = JsonUtility.FromJson<TransactionData>(message);
            if (data.op == "utx")
            {
                var transactionData = $"Transaction Hash: {data.x.hash}\n" +
                                      $"From: {data.x.inputs[0].prev_out.addr}\n" +
                                      $"To: {data.x.@out[0].addr}\n" +
                                      $"Value: {data.x.@out[0].value / 100000000.0:F8} BTC";
                transactionVolumes.Add(data.x.@out[0].value / 100000000.0f);
                dataText.text = transactionData;
                gradient.UpdateHeatMapTexture(heatMapTexture, transactionVolumes);
                // move transaction
            }
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

    private void debugBlockchainData(TransactionData data)
    {
        Debug.Log("Hash, previous address, output address, value:");
        Debug.Log(data.x.hash);
        Debug.Log(data.x.inputs[0].prev_out.addr);
        Debug.Log(data.x.@out[0].addr);
        Debug.Log(data.x.@out[0].value);
    }

}