using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreData : MonoBehaviour
{
    private FramesAndCoords framesAndCoords;

    private List<Vector2> positions = new List<Vector2>();

    private void Start()
    {
        framesAndCoords = GetComponent<FramesAndCoords>();
    }

    public void Store(int frame)
    {
        positions.Clear();
        
        if (Camera.main == null) return;
        for (var i = 0; i < transform.childCount; i++)
        {
            var position = Camera.main.WorldToScreenPoint(transform.GetChild(i).position);
            var screenPos = new Vector2(position.x, 1080 - position.y);
            positions.Add(screenPos);
        }
        framesAndCoords.data.Add(frame, positions);
        if (transform.name == "Grill")
            print(framesAndCoords.data[0][0]);
    }

    private void OnGUI()
    {
        if (transform.name == "Grill")
        GUI.Label(new Rect(Camera.main.WorldToScreenPoint(transform.GetChild(0).position).x, Camera.main.WorldToScreenPoint(transform.GetChild(0).position).y, 50, 20),"Health..."); 
    }
}
