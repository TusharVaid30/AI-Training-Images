using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WriteImageData : MonoBehaviour
{
    [SerializeField] private Data data;
    [SerializeField] private GetPixelPosition pixelPosition;
    [SerializeField] private Text debugText;
    [SerializeField] private CoordsPerFrame[] coordsPerFrame;

    [SerializeField] private Transform[] points;

    private int pointCurrentlyWriting;
    
    private string path;
    private bool stopWriting;

    private int k;

    private void Start()
    {
        path = data.path;
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
        WriteStringLine("     \"Car 1\":" + " [");
        for (var i = 0; i <= data.numberOfFrames - 1; i++)
        {
            WriteStringLine("     [");
            WriteStringLine("     {\"img_name\"   :    " + "\""+ (i + 1) + ".png\", \"damage_type\"  :   \"crack\", \"points\":   ");
            
            if (i is 120 or 220 or 320 or 420 or 520 or 620 or 720 or 820 or 920)
            {
                if (k < points.Length)
                    k++;
                pointCurrentlyWriting = 0;
            }
            
            WriteStringLine("          [");
            for (var j = 0; j < points[k].childCount; j++)
            {
                WriteString("             [" + points[k].GetChild(j).GetComponent<CoordsPerFrame>().coordsX[pointCurrentlyWriting] + ", " + 
                            points[k].GetChild(j).GetComponent<CoordsPerFrame>().coordsY[pointCurrentlyWriting]);
                WriteStringLine(j == points[k].childCount - 1 ? "]" : "],");
            }

            WriteStringLine("          ]");

            WriteStringLine("      }");
            WriteStringLine(i == data.numberOfFrames - 1 ? "     ]" : "     ],");

            pointCurrentlyWriting++;

        }

        WriteStringLine("]}");

        DebugInfo("Data Written");


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void WriteStringLine(string text)
    {
        var writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }

    private void WriteString(string text)
    {
        var writer = new StreamWriter(path, true);
        writer.Write(text);
        writer.Close();
    }
}
