using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Misc_
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
                    next.SetSiblingIndex(i + 1);
            }
            if (transform.childCount < 5) return;
            for (var i = transform.childCount - 1; i > transform.childCount - 5; i--)
            {
                try
                { 
                    Destroy(transform.GetChild(i).gameObject);
                }
                catch (UnityException e)
                {
                    print(e + " in object " + transform.name);
                    throw;
                }
            }
            if (transform.name != "Front Bumper") return;
            for (var i = transform.childCount - 1; i > transform.childCount - 10; i--)
            {
                try
                {
                   // Destroy(transform.GetChild(i).gameObject);
                }
                catch (UnityException e)
                {
                    print(e + " in object " + transform.name);
                    throw;
                }
            }
        }
    }
}
