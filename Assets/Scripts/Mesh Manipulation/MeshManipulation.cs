using System;
using System.Collections.Generic;
using Misc_;
using UnityEngine;

public class MeshManipulation : UpdateMesh
{
    public bool updateBorders = true;
    
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

    private void Start()
    {
        alignPoint = GetComponent<AlignPoints>();
    }

    public override void UpdateBorders()
    {
        if (Camera.main == null || !updateBorders) return;

        for (var x = 0; x < resX; x += factorX)
        {
            for (var y = 0; y < resY; y += factorY)
            {
                if (!hitPoints1.Contains(y))
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (colliders.Count > 0)
                            foreach (var collider1 in colliders)
                                if (hit.transform == collider1)
                                {
                                    Spawn(hit.point);
                                    hitPoints1.Add(y);
                                }
                        if (hit.transform == transform)
                        {
                            Spawn(hit.point);
                            hitPoints1.Add(y);
                        }
        
                    }
                }
            }
        }
        
        for (var y = 0; y < resY; y += factorY)
        {
            for (var x = 0; x < resX; x += factorX)
            {
                if (!hitPoints2.Contains(x))
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (colliders.Count > 0)
                            foreach (var collider1 in colliders)
                                if (hit.transform == collider1)
                                {
                                    Spawn(hit.point);
                                    hitPoints2.Add(x);
                                }
                        if (hit.transform == transform)
                        {
                            Spawn(hit.point);
                            hitPoints2.Add(x);
                        }
                    }
                }
            }
        }
        
        for (var x = resX; x > 0; x -= factorX)
        {
            for (var y = resY; y > 0; y -= factorY)
            {
                if (!hitPoints3.Contains(y))
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (colliders.Count > 0)
                            foreach (var collider1 in colliders)
                                if (hit.transform == collider1)
                                {
                                    Spawn(hit.point);
                                    hitPoints3.Add(y);
                                }
                        if (hit.transform == transform)
                        {
                            Spawn(hit.point);
                            hitPoints3.Add(y);
                        }
                    }
                }
            }
        }
        
        for (var y = resY; y > 0; y -= factorY)
        {
            for (var x = resX; x > 0; x -= factorX)
            {
                if (!hitPoints4.Contains(x))
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (colliders.Count > 0)
                            foreach (var collider1 in colliders)
                                if (hit.transform == collider1)
                                {
                                    Spawn(hit.point);
                                    hitPoints4.Add(x);
                                }
                        if (hit.transform == transform)
                        {
                            Spawn(hit.point);
                            hitPoints4.Add(x);
                        }
                    }
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
}
