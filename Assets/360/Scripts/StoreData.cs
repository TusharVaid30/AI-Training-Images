using System.Globalization;
using _360.Misc_;
using UnityEngine;

namespace _360.Scripts
{
    public class StoreData : MonoBehaviour
    {
        [SerializeField] private CollectData data;

        private int carIndex = 1;
        private int index;
        
        private void OnEnable()
        {
            CreateData.OnStart += WriteData;
        }

        private void OnDisable()
        {
            CreateData.OnStart -= WriteData;
        }

        private void WriteData()
        {
            if (index > 719) return;
            Output.WriteStringLine("{");

            Output.WriteStringLine("     \"Car " + carIndex + "\": [");
            Output.WriteStringLine("     [");

            for (var i = 0; i <= 239; i++)
            {
                Output.WriteString("     [");
                Output.WriteStringLine("     {\"img_name\"    :    \"" + (index + 1) + ".png\", " +
                                       "\"properties(x, y, z, rot)\":");
                Output.WriteString("               [");
                var tempVec = data.positionVector[index];
                Output.WriteString(tempVec.x.ToString(CultureInfo.InvariantCulture) + ", " +
                                   tempVec.y.ToString(CultureInfo.InvariantCulture)
                                   + ", " + tempVec.z.ToString(CultureInfo.InvariantCulture) + ", " +
                                   data.cameraRotation[index]);
                Output.WriteStringLine("]");
                index++;
                Output.WriteStringLine("     }");
                Output.WriteStringLine(i == 239 ? "     ]" : "     ],");
                if (i != 239) continue;
                carIndex++;
                if (carIndex < 3)
                    WriteData();
            }
        }
    }
}
