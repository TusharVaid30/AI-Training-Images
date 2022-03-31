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
            float minDist = Mathf.Infinity;
            Vector3 currentPos = currentObj.position;
            foreach (Transform t in enemies)
            {
                float dist = Vector3.Distance( t.position, currentPos);
                if (!(dist < minDist)) continue;
                tMin = t;
                minDist = dist;
            }
            return tMin;
        }

        public void Start()
        {
            if (!align) return;
            for (int i = 0; i < transform.childCount; i++)
                points.Add(transform.GetChild(i));
            for (int i = 0; i < transform.childCount; i++)
            {
                points.Remove(transform.GetChild(i));
                Transform next = GetClosestObject(transform.GetChild(i), points);
                if (i < transform.childCount - 1)
                    next.SetSiblingIndex(i + 1);
            }
        }
    }
}
