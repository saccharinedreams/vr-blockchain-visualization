using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomGradient { 

    public Color Evaluate(float volume)
    {
        if (volume > 1000f) return Color.green;
        else if (volume > 100f) return Color.Lerp(Color.red, Color.green, 0.75f);
        else if (volume > 10f) return Color.Lerp(Color.red, Color.green, 0.5f);
        else return Color.red;
    }

    // This function updates the texture based on the current transaction volumes
    public void UpdateHeatMapTexture(Texture2D heatMapTexture, float transactionVolume)
    {
        // Normalize the transaction volumes to a range of 0 to 1
        Color color = Evaluate(transactionVolume);
      
        // Update the color values of the pixels in the texture
        for (int y = 0; y < heatMapTexture.height; y++)
        {
            for (int x = 0; x < heatMapTexture.width; x++)
            {
                heatMapTexture.SetPixel(x, y, color);
            }
        }
        heatMapTexture.Apply();
    }
}
