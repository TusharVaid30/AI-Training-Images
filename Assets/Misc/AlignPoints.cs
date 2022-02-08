using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc_
{
    public class AlignPoints : MonoBehaviour
    {
        private readonly List<Transform> points = new List<Transform>();

        bool start;
        private Transform current;
        
        private static Transform GetClosestObject(Transform currentObj, IEnumerable<Transform> enemies)
        {
            Transform tMin = null;
            var minDist = Mathf.Infinity;
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
            for (var i = 0; i < transform.childCount; i++)
                points.Add(transform.GetChild(i));
            for (var i = 0; i < transform.childCount; i++)
            {
                points.Remove(transform.GetChild(i));
                var next = GetClosestObject(transform.GetChild(i), points);
                if (i < transform.childCount - 1)
                    next.SetSiblingIndex(i + 1);
            }
        }
    }
}
