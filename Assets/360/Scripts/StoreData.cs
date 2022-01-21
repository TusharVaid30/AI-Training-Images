using System.Globalization;
using _360.Misc_;
using UnityEngine;

namespace _360.Scripts
{
    public class StoreData : MonoBehaviour
    {
        [SerializeField] private CollectData data;

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
            Output.WriteStringLine("{");

            Output.WriteStringLine("     \"Car " + 10 + "\": [");

            for (var i = 0; i <= 719; i++)
            {
                Output.WriteString("     [");
                Output.WriteStringLine("     {\"img_name\"    :    \"" + (index + 1) + ".png\", " +
                                       "\"properties(x, y, z, x-angle, y-angle, z-angle)\":");
                Output.WriteString("               [");
                var tempVec = data.positionVector[index];
                Output.WriteString(tempVec.x.ToString(CultureInfo.InvariantCulture) + ", " +
                                   tempVec.y.ToString(CultureInfo.InvariantCulture)
                                   + ", " + tempVec.z.ToString(CultureInfo.InvariantCulture) + ", " + 
                                   data.xzRotation[index].x + ", " + data.cameraRotation[index] + ", " + data.xzRotation[index].y);
                Output.WriteStringLine("]");
                index++;
                Output.WriteStringLine("     }");
                Output.WriteStringLine(i == 719 ? "     ]" : "     ],");
            }
        }
    }
}
