using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using NativeWebSocket;
using TMPro;
using System.Linq;
using System.Diagnostics;
using System.IO;

public class BitcoinVisualizer : MonoBehaviour
{
    private WebSocket ws;
    private List<float> transactionVolumes = new List<float>();
    public TMP_Text dataText;
    public Texture2D heatMapTexture;
    private CustomGradient gradient;
    private TransactionSphere btc_spheres;
    private float conversion_to_usd;
    private float MAX_SPHERE_SIZE = 10f;
    private float MIN_SPHERE_SIZE = 0.5f;
    private float SPHERE_SCALE = 50f;
    private float curr_size;
    private Vector3 btc_pos;
    StreamReader reader;

    async void Start()
    {
        // Connect to the Blockchain.com WebSocket API
        ws = new WebSocket("wss://ws.blockchain.info/inv");
        reader = new StreamReader("Assets/Analysis/conversion_rates.txt");
        conversion_to_usd = getConversionRate(reader);
        gradient = new CustomGradient();
        btc_spheres = new TransactionSphere();
        btc_pos = new Vector3(-70f, -10f, 10f);
        ws.OnOpen += () =>
        {
            UnityEngine.Debug.Log("Connection open!");
        };

        ws.OnError += (e) =>
        {
            UnityEngine.Debug.Log("Error! " + e);
        };

        ws.OnClose += (e) =>
        {
            UnityEngine.Debug.Log("Connection closed!");
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
                gradient.UpdateHeatMapTexture(heatMapTexture, transactionVolumes.Last()*conversion_to_usd);
                curr_size = transactionVolumes.Last() * conversion_to_usd / SPHERE_SCALE;
                curr_size = curr_size < MIN_SPHERE_SIZE ? MIN_SPHERE_SIZE : curr_size;
                curr_size = curr_size > MAX_SPHERE_SIZE ? MAX_SPHERE_SIZE : curr_size;
                btc_spheres.createSphere(Color.yellow, curr_size, btc_pos);
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

    private float getConversionRate(StreamReader reader)
    {
        // Read each line of the file
        string line;
        using (reader)
        {
            // Read the first line.
            line = reader.ReadLine();
        }

        // Close the file
        reader.Close();
        UnityEngine.Debug.Log("Conversion rate: " + line);
        return float.Parse(line);
    }

    private void debugBlockchainData(TransactionData data)
    {
        UnityEngine.Debug.Log("Hash, previous address, output address, value:");
        UnityEngine.Debug.Log(data.x.hash);
        UnityEngine.Debug.Log(data.x.inputs[0].prev_out.addr);
        UnityEngine.Debug.Log(data.x.@out[0].addr);
        UnityEngine.Debug.Log(data.x.@out[0].value);
    }
}