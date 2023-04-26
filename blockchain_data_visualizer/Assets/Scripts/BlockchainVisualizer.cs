using UnityEngine;
//using WebSocketSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using NativeWebSocket;
using TMPro;

[Serializable]
public class TransactionData
{
    public string op;
    public Transaction x;
}

[Serializable]
public class Transaction
{
    public string hash;
    public Input[] inputs;
    public Output[] @out;
}

[Serializable]
public class PrevOut
{
    public string addr;
}

[Serializable]
public class Input
{
    public PrevOut prev_out;
}

[Serializable]
public class Output
{
    public string addr;
    public long value;
}

public class BlockchainVisualizer : MonoBehaviour
{
    private WebSocket ws;
    public TMP_Text dataText;

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
            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage: " + message);
            TransactionData data = JsonUtility.FromJson<TransactionData>(message);
            if (data.op == "utx")
            {
                var transactionData = $"Transaction Hash: {data.x.hash}\n" +
                                      $"From: {data.x.inputs[0].prev_out.addr}\n" +
                                      $"To: {data.x.@out[0].addr}\n" +
                                      $"Value: {data.x.@out[0].value / 100000000.0:F8} BTC";

                dataText.text = transactionData;
            }
            
        };

        //SendWebSocketMessage();
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