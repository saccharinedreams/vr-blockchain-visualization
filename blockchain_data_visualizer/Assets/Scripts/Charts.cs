using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Charts : MonoBehaviour
{
    private string filePath = "Assets/Analysis/";
    private string[] cryptos;
    private GameObject[] candlestickCharts;
    private GameObject historicalCharts;
    SpriteRenderer spriteRenderer;
    Texture2D texture;
    private Vector3 SCALE = new Vector3(9f, 9f, 9f);
    private Vector3 POS = new Vector3(0f, 0f, 0f);

    private void Start(){
        cryptos = new string[] {"BTC", "ETH", "MATIC", "SOL"};
        candlestickCharts = new GameObject[]{new GameObject("BTC_CandleStick"), new GameObject("ETH_CandleStick"),
                           new GameObject("MATIC_CandleStick"), new GameObject("SOL_CandleStick")};
        historicalCharts = new GameObject("historical_charts");
        // Create the candlestick graph objects
        for (int i = 0; i < cryptos.Length; i++)
        {
            setupChart(candlestickCharts[i], filePath+cryptos[i]+"_candlestick.png");
        }
        setupChart(historicalCharts, filePath+"historical_charts.png");
    }

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown("b") )
        {   
            if(candlestickCharts[0].activeSelf) candlestickCharts[0].SetActive(false);
            else candlestickCharts[0].SetActive(true);
        }
        else if (UnityEngine.Input.GetKeyDown("e") )
        {   
            if(candlestickCharts[1].activeSelf) candlestickCharts[1].SetActive(false);
            else candlestickCharts[1].SetActive(true);
        }
        else if (UnityEngine.Input.GetKeyDown("m") )
        {   
            if(candlestickCharts[2].activeSelf) candlestickCharts[2].SetActive(false);
            else candlestickCharts[2].SetActive(true);
        }
        else if (UnityEngine.Input.GetKeyDown("s") )
        {   
            if(candlestickCharts[3].activeSelf) candlestickCharts[3].SetActive(false);
            else candlestickCharts[3].SetActive(true);
        }
        else if(UnityEngine.Input.GetKeyDown("space")){
            if(historicalCharts.activeSelf) historicalCharts.SetActive(false);
            else historicalCharts.SetActive(true);
        }
    }

    private void setupChart(GameObject chart, string fileName){
        spriteRenderer = chart.AddComponent<SpriteRenderer>();
        texture = new Texture2D(0, 0);
        texture.LoadImage(System.IO.File.ReadAllBytes(fileName));
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        chart.transform.localScale = SCALE;
        chart.transform.position = POS;
        chart.SetActive(false);
    }
    // private void onEnable(){
    //     playerControls.Enable();
    // }

    // private void onDisable(){
    //     playerControls.Disable();
    // }

    /*private void runPythonScript(string scriptName)
    {
        var process = new Process
        {
            StartInfo =
            {
                FileName = "python",
                Arguments = scriptName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        process.WaitForExit();
    }*/
}
