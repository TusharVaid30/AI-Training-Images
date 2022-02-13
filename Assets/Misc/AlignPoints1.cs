using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Misc_
{
    [ExecuteInEditMode]
    public class AlignPoints1 : MonoBehaviour
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
            var currentPos = currentObj.position;
            foreach (var t in enemies)
            {
                var dist = Vector3.Distance( t.position, currentPos);
                if (!(dist < minDist)) continue;
                tMin = t;
                minDist = dist;
            }
            return tMin;
        }

        public void Start()
        {
            if (!align) return;
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
