using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FixBumperEdge : MonoBehaviour
{
    [SerializeField] private int type;
    [SerializeField] private Transform bumper;
    [SerializeField] private float resX;
    [SerializeField] private float resY;
    [SerializeField] private GameObject sphere;
    
    private Vector3 topRight;
    private Vector3 topLeft;
    private Vector3 bottomRight;
    private Vector3 bottomLeft;
    private Vector3 screenPos;
    private Vector3 startPos;
    
    private void Start()
    {
        startPos = transform.position;
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
    
    public void CheckInView()
    {
        if (Camera.main == null) return;
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        var ray = Camera.main.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity)) return;
        if (hit.transform == bumper)
            FixEdge();
    }

    private void FixEdge()
    {
        FocusOnBounds(bumper.GetComponent<MeshRenderer>().bounds);
        if (type == 0)
        {
            for (int x = (int) bottomLeft.x; x < topRight.x; x += 2)
            {
                for (int y = (int) topLeft.y; y > bottomLeft.y; y -= 2)
                {
                    if (Camera.main != null)
                    {
                        var ray = Camera.main.ScreenPointToRay(new Vector2(x, screenPos.y));

                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            if (hit.transform == bumper)
                            {
                                transform.position = hit.point;
                                return;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int x = (int) topRight.x; x > bottomLeft.x; x -= 2)
            {
                for (int y = (int) topLeft.y; y > bottomLeft.y; y -= 2)
                {
                    if (Camera.main != null)
                    {
                        var ray = Camera.main.ScreenPointToRay(new Vector2(x, screenPos.y));

                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            if (hit.transform == bumper)
                            {
                                transform.position = hit.point;
                                return;
                            }
                        }
                    }
                }
            }
        }

        transform.position = startPos;
    }
    
    private void Spawn(Vector3 position)
    {
        Instantiate(sphere, position, Quaternion.identity, bumper);
    }
}
