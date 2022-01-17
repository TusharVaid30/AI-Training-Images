using System.IO;
using UnityEngine;

namespace _360.Misc_
{
    public class Output : MonoBehaviour
    {
        [SerializeField] private string path;

        private static string _path;

        private void Start()
        {
            _path = path;
        }

        public static void WriteStringLine(string text)
        {
            var writer = new StreamWriter(_path, true);
            writer.WriteLine(text);
            writer.Close();
        }

        public static void WriteString(string text)
        {
            var writer = new StreamWriter(_path, true);
            writer.Write(text);
            writer.Close();
        }
    }
}
