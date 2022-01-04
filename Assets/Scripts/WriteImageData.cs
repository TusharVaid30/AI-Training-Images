using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WriteImageData : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] private Data data;
    [SerializeField] private GetPixelPosition pixelPosition;
    [SerializeField] private Text debugText;
    [SerializeField] private CoordsPerFrame[] coordsPerFrame;
    
    private bool stopWriting;

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
        WriteString("{");
        WriteString("     \'data\':" + " [");
        for (var i = 0; i <= data.numberOfFrames - 1; i++)
        {
            WriteString("     [");
            WriteString("     {\'img_name\'   :    " + "\'"+ (i + 1) + ".png\', \'damage_type\'  :   \'crack\', \'points\':   ");

            for (var j = 0; j < coordsPerFrame.Length; j++)
            {
                WriteString("          [");
                WriteString("             [" + coordsPerFrame[j].coordsX[i] + "],");
                WriteString("             [" + coordsPerFrame[j].coordsY[i] + "]");
                WriteString(j == coordsPerFrame.Length - 1 ? "          ]" : "          ],");
            }

            WriteString("      }");
            WriteString(i == data.numberOfFrames - 1 ? "     ]" : "     ],");
        }

        WriteString("]}");

        DebugInfo("Data Written");


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void WriteString(string text)
    {
        var writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }
}
