using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

namespace Mesh_Manipulation
{
    [RequireComponent(typeof(AlignPoints), typeof(StoreData), typeof(FramesAndCoords))]
    public class MeshManipulation : UpdateMesh
    {
        public bool updateBorders = true;

        [SerializeField] private LayerMask ignore;
    
        [SerializeField] private GameObject sphere;
        [SerializeField] private int factorX;
        [SerializeField] private int factorY;
        [SerializeField] private int resX;
        [SerializeField] private int resY;
        [SerializeField] private List<Transform> colliders;
    
        private AlignPoints alignPoint;
        private List<int> hitPoints1 = new List<int>();
        private List<int> hitPoints2 = new List<int>();
        private List<int> hitPoints3 = new List<int>();
        private List<int> hitPoints4 = new List<int>();

        private Vector3 topRight;
        private Vector3 topLeft;
        private Vector3 bottomRight;
        private Vector3 bottomLeft;

        private void Start()
        {
            alignPoint = GetComponent<AlignPoints>();
        }

        private void FocusOnBounds(Bounds bounds) { 
            Vector3 c = bounds.center;
            Vector3 e = bounds.extents;
 
            Vector3[] worldCorners = new [] {
                new Vector3( c.x + e.x, c.y + e.y, c.z + e.z ),
                new Vector3( c.x + e.x, c.y + e.y, c.z - e.z ),
                new Vector3( c.x + e.x, c.y - e.y, c.z + e.z ),
                new Vector3( c.x + e.x, c.y - e.y, c.z - e.z ),
                new Vector3( c.x - e.x, c.y + e.y, c.z + e.z ),
                new Vector3( c.x - e.x, c.y + e.y, c.z - e.z ),
                new Vector3( c.x - e.x, c.y - e.y, c.z + e.z ),
                new Vector3( c.x - e.x, c.y - e.y, c.z - e.z )
            };
 
            var screenCorners = worldCorners.Select(corner => Camera.main.WorldToScreenPoint(corner));
            var enumerable = screenCorners as Vector3[] ?? screenCorners.ToArray();
            var maxX = enumerable.Max(corner => corner.x);
            var minX = enumerable.Min(corner => corner.x);
            var maxY = enumerable.Max(corner => corner.y);
            var minY = enumerable.Min(corner => corner.y);

            maxX = Mathf.Clamp(maxX, 0, resX);
            minX = Mathf.Clamp(minX, 0, resX);
            maxY = Mathf.Clamp(maxY, 0, resY);
            minY = Mathf.Clamp(minY, 0, resY);
        
            topRight = new Vector3(maxX, maxY, 1f);
            topLeft = new Vector3(minX, maxY, 1f);
            bottomRight = new Vector3(maxX, minY, 1f);
            bottomLeft = new Vector3(minX, minY, 1f);
        }
    
        public override void UpdateBorders()
        {
            if (Camera.main == null) return;

            FocusOnBounds(GetComponent<MeshRenderer>().bounds);
        
            if (transform.name == "left_fog_lamp")
                print(bottomLeft.x);

            for (var x = (int) bottomLeft.x; x < topRight.x; x += factorX)
            {
                for (var y = (int) bottomLeft.y; y < topRight.y; y += factorY)
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignore))
                    {
                        if (hit.transform == transform)
                        {
                            if (!hitPoints1.Contains(y))
                            {
                                Spawn(hit.point);
                                hitPoints1.Add(y);
                            }
                        }
                        else
                        {
                            if (hitPoints1.Contains(y))
                                hitPoints1.Remove(y);
                        }
                    }
                    else
                    {
                        if (hitPoints1.Contains(y))
                            hitPoints1.Remove(y);
                    }
        
                }
            }
        
            for (var y = (int) bottomLeft.y; y < topRight.y; y += factorY)
            {
                for (var x = (int) bottomLeft.x; x < topRight.x; x += factorX)
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignore))
                    {
                        if (hit.transform == transform)
                        {
                            if (!hitPoints2.Contains(x))
                            {
                                Spawn(hit.point);
                                hitPoints2.Add(x);
                            }
                        }
                        else
                        {
                            if (hitPoints2.Contains(x))
                                hitPoints2.Remove(x);
                        }
                    }
                    else
                    {
                        if (hitPoints2.Contains(x))
                            hitPoints2.Remove(x);
                    }
                }
            }
        
            for (var x = (int) topRight.x; x > bottomLeft.x; x -= factorX)
            {
                for (var y = (int) topRight.y; y > bottomLeft.y; y -= factorY)
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignore))
                    {
                        if (hit.transform == transform)
                        {
                            if (!hitPoints3.Contains(y))
                            {
                                Spawn(hit.point);
                                hitPoints3.Add(y);
                            }
                        }
                        else
                        {
                            if (hitPoints3.Contains(y))
                                hitPoints3.Remove(y);
                        }
                    }
                    else
                    {
                        if (hitPoints3.Contains(y))
                            hitPoints3.Remove(y);
                    }
                }
            }
        
            for (var y = (int) topRight.y; y > bottomLeft.y; y -= factorY)
            {
                for (var x = (int) topRight.x; x > bottomLeft.x; x -= factorX)
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignore))
                    {
                        if (hit.transform == transform)
                        {
                            if (!hitPoints4.Contains(x))
                            {
                                Spawn(hit.point);
                                hitPoints4.Add(x);
                            }
                        }
                        else
                        {
                            if (hitPoints4.Contains(x))
                                hitPoints4.Remove(x);
                        }
                    }
                    else
                    {
                        if (hitPoints3.Contains(x))
                            hitPoints3.Remove(x);
                    }
                }
            }
            alignPoint.Align();
            hitPoints1.Clear();
            hitPoints2.Clear();
            hitPoints3.Clear();
            hitPoints4.Clear();
        }

        private void Spawn(Vector3 position)
        {
            Instantiate(sphere, position, Quaternion.identity, transform);
        }
        private void Spawn2(Vector3 position)
        {
            Instantiate(sphere, Camera.main.ScreenToWorldPoint(position), Quaternion.identity);
        }
    }
}