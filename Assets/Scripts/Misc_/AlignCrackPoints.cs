using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc_
{
    public class AlignCrackPoints : MonoBehaviour
    {
        private List<Transform> points = new List<Transform>();

        private Transform current;
        
        private Transform GetClosestObject(Transform currentObj, IEnumerable<Transform> enemies)
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
            foreach (var point in transform.parent.GetComponentsInChildren<Transform>())
                points.Add(point);
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
