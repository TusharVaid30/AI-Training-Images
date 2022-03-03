using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WriteImageData : MonoBehaviour
{
    [SerializeField] private string carName;
    
    
    [SerializeField] private Data data;
    [SerializeField] private GetPixelPosition pixelPosition;
    [SerializeField] private Text debugText;

    [SerializeField] private Transform[] panels;
    
    private string path;
    private bool stopWriting;
    private StreamWriter writer;

    private int annID;

    private void Start()
    {
        path = data.path;
        OpenFile();
    }

    private void Update()
    {
        if (pixelPosition.framesDone < data.numberOfFrames || stopWriting) return;
        SetupData();
        stopWriting = true;
    }

    private void DebugInfo(string info)
    {
        Debug.Log(info);
        debugText.text = info;
    }

    private void SetupData()
    {
        WriteStringLine("{");
        WriteStringLine("     \"categories\":" + " [");
        for (var i = 0; i < panels.Length; i++)
        {
            WriteStringLine("       {");
            WriteStringLine("         \"supercategory\": \"" + panels[i].name + "\",");
            WriteStringLine("         \"id\":" + i + ",");
            WriteStringLine("         \"name\": \"" + panels[i].name + "\"");
            WriteStringLine(i == panels.Length - 1 ? "     }" : "      },");
        }
        WriteStringLine("      ],");
        WriteStringLine("     \"images\":" + " [");
        for (var i = 0; i <= data.numberOfFrames - 1; i++)
        {
            WriteStringLine("       {");
            WriteStringLine("         \"height\":  1080,");
            WriteStringLine("         \"width\":  1920,");
            WriteStringLine("         \"id\": " + i + ",");
            WriteStringLine("         \"name\": \" " + carName + "_" + (i + 1) + ".png\"");
            WriteStringLine(i == data.numberOfFrames - 1 ? "     }" : "      },");
        }
        WriteStringLine("     ],");
        WriteStringLine("     \"annotations\":" + " [");
        for (var i = 0; i <= data.numberOfFrames - 1; i++)
        {
            
            for (var x = 0; x < panels.Length; x++)
            {
                if (panels[x].GetComponent<FramesAndCoords>().data.ContainsKey(i))
                {
                    WriteStringLine("        {");
                    WriteStringLine("           \"iscrowd\": 0,");
                    WriteStringLine("           \"image_id\":" + i +",");
            
                    WriteStringLine("          \"segmentation\" : [");
                    WriteStringLine("           [");
                    for (var j = 0; j < panels[x].GetComponent<FramesAndCoords>().data[i].Length; j++)
                    {
                        WriteString("                       " + panels[x].GetComponent<FramesAndCoords>().data[i][j].x + ", " + 
                                    panels[x].GetComponent<FramesAndCoords>().data[i][j].y);

                        WriteStringLine(j == panels[x].GetComponent<FramesAndCoords>().data[i].Length - 1 ? "" : ",");
                    }
                    WriteStringLine("            ]");
                    WriteStringLine("        ],");
                    WriteStringLine("           \"category_id\":" + x + ",");
                    WriteStringLine("           \"id\":" + annID + ",");
                    WriteStringLine("           \"area\": 2073600");
                    WriteStringLine("     },");
                    
                    annID++;
                }
            }
        }

        WriteStringLine("]}");

        DebugInfo("Data Written");


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OpenFile()
    {
        writer = new StreamWriter(path, true);
    }

    private void WriteStringLine(string text)
    {
        writer.WriteLine(text);
    }

    private void WriteString(string text)
    {
        writer.Write(text);
    }

    private void OnDestroy()
    {
        writer.Close();
    }
}
