using System;
using System.Collections.Generic;
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
        for (var i = 0; i <= data.numberOfFrames - 1; i++)
        {
            path = data.path + "_" + (i + 1) + ".png.json";
            OpenFile();


            WriteStringLine("{");
            WriteStringLine("     \"categories\":" + " [");
            for (var a = 0; a < panels.Length; a++)
            {
                WriteStringLine("       {");
                WriteStringLine("         \"supercategory\": \"" + panels[a].name + "\",");
                WriteStringLine("         \"id\":" + a + ",");
                WriteStringLine("         \"name\": \"" + panels[a].name + "\"");
                WriteStringLine(a == panels.Length - 1 ? "     }" : "      },");
            }

            WriteStringLine("      ],");
            WriteStringLine("     \"images\":" + " [");
            WriteStringLine("       {");
            WriteStringLine("         \"height\":  1080,");
            WriteStringLine("         \"width\":  1920,");
            WriteStringLine("         \"id\": " + i + ",");
            WriteStringLine("         \"file_name\": \"" + carName + "_" + (i + 1) + ".png\"");
            WriteStringLine("     }");


            WriteStringLine("     ],");
            WriteStringLine("     \"annotations\":" + " [");


            for (var x = 0; x < panels.Length; x++)
            {
                if (panels[x].GetComponent<FramesAndCoords>().data.ContainsKey(i))
                {
                    WriteStringLine("        {");
                    WriteStringLine("           \"iscrowd\": 0,");
                    WriteStringLine("           \"image_id\":" + i + ",");

                    WriteStringLine("           \"bbox\": [");

                    var bboxX = new List<float>();
                    var bboxY = new List<float>();

                    for (var z = 0; z < panels[x].GetComponent<FramesAndCoords>().data[i].Length; z++)
                    {
                        bboxX.Add(panels[x].GetComponent<FramesAndCoords>().data[i][z].x);
                        bboxY.Add(panels[x].GetComponent<FramesAndCoords>().data[i][z].y);
                    }

                    WriteStringLine("           " + Mathf.Min(bboxX.ToArray()) + ",");
                    WriteStringLine("           " + Mathf.Min(bboxY.ToArray()) + ",");
                    WriteStringLine("           " + (Mathf.Max(bboxX.ToArray()) - Mathf.Min(bboxX.ToArray())) +
                                    ",");
                    WriteStringLine("           " + (Mathf.Max(bboxY.ToArray()) - Mathf.Min(bboxY.ToArray())));
                    WriteStringLine("           ],");

                    WriteStringLine("          \"segmentation\" : [");
                    WriteStringLine("           [");
                    for (var j = 0; j < panels[x].GetComponent<FramesAndCoords>().data[i].Length; j++)
                    {
                        WriteString("                       " +
                                    panels[x].GetComponent<FramesAndCoords>().data[i][j].x + ", " +
                                    panels[x].GetComponent<FramesAndCoords>().data[i][j].y);

                        WriteStringLine(
                            j == panels[x].GetComponent<FramesAndCoords>().data[i].Length - 1 ? "" : ",");
                    }

                    WriteStringLine("            ]");
                    WriteStringLine("        ],");
                    WriteStringLine("           \"category_name\":" + panels[x].name + ",");
                    WriteStringLine("           \"id\":" + annID + ",");
                    WriteStringLine("           \"area\":" +
                                    (Mathf.Max(bboxX.ToArray()) - Mathf.Min(bboxX.ToArray())) *
                                    (Mathf.Max(bboxY.ToArray()) - Mathf.Min(bboxY.ToArray())));
                    if (x == panels.Length - 1 && i == data.numberOfFrames - 1)
                        WriteStringLine("     }");
                    else
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