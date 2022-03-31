using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Misc
{
    public class AlignPoints : MonoBehaviour
    {
        [DefaultValue(true)]
        public bool align = true;
        
        
        private readonly List<Transform> points = new List<Transform>();

        private bool start;
        private Transform current;
        
        private static Transform GetClosestObject(Transform currentObj, IEnumerable<Transform> enemies)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            if (Camera.main == null) return null;
            Vector3 currentPos = Camera.main.WorldToScreenPoint(currentObj.position);
            foreach (Transform t in enemies)
            {
                float dist = Vector3.Distance( Camera.main.WorldToScreenPoint(t.position), currentPos);
                if (!(dist < minDist)) continue;
                tMin = t;
                minDist = dist;
            }

            return tMin;
        }

        public void Align()
        {
            if (!align) return;
            for (int i = 0; i < transform.childCount; i++)
                points.Add(transform.GetChild(i));
            for (int i = 0; i < transform.childCount; i++)
            {
                points.Remove(transform.GetChild(i));
                Transform next = GetClosestObject(transform.GetChild(i), points);
                if (i < transform.childCount - 1)
                {
                    next.SetSiblingIndex(i + 1);
                    next.name = i.ToString();
                }
            }
            
            if (transform.childCount < 50) return;
            for (int i = transform.childCount - 1; i > transform.childCount - 20; i--)
                Destroy(transform.GetChild(i).gameObject);
        }
    }
}