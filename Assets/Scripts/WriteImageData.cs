using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteImageData : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] private CoordsPerFrame[] coordsPerFrame;

    private GetPixelPosition pixelPosition;
    private bool stopWriting = false;
    
    private void Start()
    {
        pixelPosition = coordsPerFrame[0].GetComponent<GetPixelPosition>();
    }

    private void Update()
    {
        if (pixelPosition.time > pixelPosition.numberOfFrames && !stopWriting)
        {
            SetupData();
            stopWriting = true;
        }
    }

    private void SetupData()
    {
        for (int i = 0; i < coordsPerFrame.Length; i++)
        {
            WriteString("Point " + (i + 1) + ":");
            
            for (int j = 0; j < coordsPerFrame[i].coords.Count; j++)
                WriteString("Frame " + j + ": " + coordsPerFrame[i].coords[j]);
            
            WriteString("");
            WriteString("");
        }
    }
    
    private void WriteString(string text)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }
}
