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
    private Vector3 SCALE = new Vector3(10f, 10f, 10f);
    private Vector3 POS = new Vector3(0f, 0f, -1f);
    public GameObject cover;
    private bool chartsActive = false;

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
        if (UnityEngine.Input.GetKeyDown("1") || UnityEngine.Input.GetKeyDown("2") || UnityEngine.Input.GetKeyDown("3") || UnityEngine.Input.GetKeyDown("4"))
        {   
            int inputChar = int.Parse(UnityEngine.Input.inputString)-1;
            candlestickCharts[inputChar].SetActive(!candlestickCharts[inputChar].activeSelf);
            
        }
        else if(UnityEngine.Input.GetKeyDown("space")){
            historicalCharts.SetActive(!historicalCharts.activeSelf);
        }
        chartsActive = false;
        foreach(GameObject obj in candlestickCharts){
            if(obj.activeSelf) {
                chartsActive = true;
                break;
            }
        }
        if(historicalCharts.activeSelf) chartsActive = true;
        cover.SetActive(chartsActive);
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
