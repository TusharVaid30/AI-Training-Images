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
            var minDist = Mathf.Infinity;
            if (Camera.main == null) return null;
            var currentPos = Camera.main.WorldToScreenPoint(currentObj.position);
            foreach (var t in enemies)
            {
                var dist = Vector3.Distance( Camera.main.WorldToScreenPoint(t.position), currentPos);
                if (!(dist < minDist)) continue;
                tMin = t;
                minDist = dist;
            }

            return tMin;
        }

        public void Align()
        {
            if (!align) return;
            for (var i = 0; i < transform.childCount; i++)
                points.Add(transform.GetChild(i));
            for (var i = 0; i < transform.childCount; i++)
            {
                points.Remove(transform.GetChild(i));
                var next = GetClosestObject(transform.GetChild(i), points);
                if (i < transform.childCount - 1)
                {
                    next.SetSiblingIndex(i + 1);
                    next.name = i.ToString();
                }
            }
            
            if (transform.childCount < 50) return;
            for (var i = transform.childCount - 1; i > transform.childCount - 20; i--)
                Destroy(transform.GetChild(i).gameObject);
        }
    }
}