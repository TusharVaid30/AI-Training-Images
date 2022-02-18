using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WriteImageData : MonoBehaviour
{
    [SerializeField] private Data data;
    [SerializeField] private GetPixelPosition pixelPosition;
    [SerializeField] private Text debugText;

    [SerializeField] private Transform[] panels;
    
    private string path;
    private bool stopWriting;
    private StreamWriter writer;

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
        WriteStringLine("     \"data\":" + " [");
        for (var i = 0; i <= data.numberOfFrames - 1; i++)
        {
            WriteStringLine("     [");
            WriteStringLine("     {" + "\"muv3_"+ (i + 1) + ".png\":");

            WriteStringLine("          [{");
            for (var x = 0; x < panels.Length; x++)
            {
                if (panels[x].GetComponent<FramesAndCoords>().data.ContainsKey(i))
                {
                    WriteStringLine("               \"" + panels[x].name + "\": ");
                    WriteStringLine("                [");
                    for (var j = 0; j < panels[x].GetComponent<FramesAndCoords>().data[i].Length; j++)
                    {
                        WriteString("                       [" + panels[x].GetComponent<FramesAndCoords>().data[i][j].x + ", " + 
                                    panels[x].GetComponent<FramesAndCoords>().data[i][j].y);
                        WriteStringLine(j == panels[x].GetComponent<FramesAndCoords>().data[i].Length - 1 ? "]" : "],");
                    }

                    WriteStringLine(x == panels.Length - 1 ? "              ]" : "            ],");
                }
            }

            WriteStringLine("          }]");

            WriteStringLine("      }");
            WriteStringLine(i == data.numberOfFrames - 1 ? "     ]" : "     ],");
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
