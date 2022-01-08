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
    
    
    private string path;
    private bool stopWriting;

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
        WriteStringLine("     \"data\":" + " [");
        for (var i = 0; i <= data.numberOfFrames - 1; i++)
        {
            WriteStringLine("     [");
            WriteStringLine("     {\"img_name\"   :    " + "\""+ (i + 1) + ".png\", \"damage_type\"  :   \"crack\", \"points\":   ");

            WriteStringLine("          [");
            for (var j = 0; j < coordsPerFrame.Length; j++)
            {
                WriteString("             [" + coordsPerFrame[j].coordsX[i] + ", " + coordsPerFrame[j].coordsY[i]);
                WriteStringLine(j == coordsPerFrame.Length - 1 ? "]" : "],");
            }
            WriteStringLine("          ]");

            WriteStringLine("      }");
            WriteStringLine(i == data.numberOfFrames - 1 ? "     ]" : "     ],");
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
