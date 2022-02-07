using System.Collections.Generic;
using UnityEngine;

public class MeshManipulation : UpdateMesh
{
    [SerializeField] private GameObject sphere;
    [SerializeField] private int factorX;
    [SerializeField] private int factorY;
    [SerializeField] private int resX;
    [SerializeField] private int resY;
    [SerializeField] private bool updateBorders = true;
    
    private List<int> hitPoints1 = new List<int>();
    private List<int> hitPoints2 = new List<int>();

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
                if (!hitPoints1.Contains(x))
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == transform)
                        {
                            Spawn(hit.point);
                            hitPoints1.Add(x);
                        }
                    }
                }
            }
        }
        
        for (var x = resX; x > 0; x -= factorX)
        {
            for (var y = resY; y > 0; y -= factorY)
            {
                if (!hitPoints2.Contains(y))
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == transform)
                        {
                            Spawn(hit.point);
                            hitPoints2.Add(y);
                        }
                    }
                }
            }
        }
        
        for (var y = resY; y > 0; y -= factorY)
        {
            for (var x = resX; x > 0; x -= factorX)
            {
                if (!hitPoints2.Contains(x))
                {
                    var ray = Camera.main.ScreenPointToRay(new Vector2(x, y));
        
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == transform)
                        {
                            Spawn(hit.point);
                            hitPoints2.Add(x);
                        }
                    }
                }
            }
        }
    }

    private void Spawn(Vector3 position)
    {
        Instantiate(sphere, position, Quaternion.identity, transform);
    }
}
