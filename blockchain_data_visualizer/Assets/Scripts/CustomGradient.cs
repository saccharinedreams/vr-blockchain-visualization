using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomGradient { 

    public Color Evaluate(float volume)
    {
        Color color = Color.red;
        if (volume > 0.5f) color = Color.green;
        else if (volume > 0.2f) color = Color.Lerp(Color.red, Color.green, 0.75f);
        else if (volume > 0.05f) color = Color.Lerp(Color.red, Color.green, 0.5f);
        return color;
    }

    // This function updates the texture based on the current transaction volumes
    public void UpdateHeatMapTexture(Texture2D heatMapTexture, List<float> transactionVolumes)
    {
        // Normalize the transaction volumes to a range of 0 to 1
        float maxVolume = Mathf.Max(transactionVolumes.ToArray());
        for (int i = 0; i < transactionVolumes.Count; i++)
        {
            transactionVolumes[i] /= maxVolume;
        }

        float volume = transactionVolumes.Last();
        Color color = Evaluate(volume);
      
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
