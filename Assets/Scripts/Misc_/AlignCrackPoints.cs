using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc_
{
    [ExecuteInEditMode]
    public class AlignCrackPoints : MonoBehaviour
    {
        private readonly List<Transform> points = new List<Transform>();

        private Transform current;
        
        private static Transform GetClosestObject(Transform currentObj, IEnumerable<Transform> enemies)
        {
            Transform tMin = null;
            var minDist = Mathf.Infinity;
            var currentPos = currentObj.position;
            foreach (var t in enemies)
            {
                var dist = Vector3.Distance(t.position, currentPos);
                if (!(dist < minDist)) continue;
                tMin = t;
                minDist = dist;
            }
            return tMin;
        }

        private void Start()
        {
            current = transform;
            for (var i = 0; i < transform.parent.childCount; i++)
            {
                points.Add(transform.parent.GetChild(i));
            }
            points.Remove(current);
        }

        private void Update()
        {
            var temp = GetClosestObject(current, points);
            temp.SetSiblingIndex(current.GetSiblingIndex() + 1);

            current = temp;
            points.Remove(current);
        }
    }
}
