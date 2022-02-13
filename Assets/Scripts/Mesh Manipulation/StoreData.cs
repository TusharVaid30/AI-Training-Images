using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreData : MonoBehaviour
{
    public bool dontStore;
    
    private FramesAndCoords framesAndCoords;
    
    private List<Vector2> temp = new List<Vector2>();

    private void Start()
    {
        framesAndCoords = GetComponent<FramesAndCoords>();
        temp.Add(new Vector2(1f, 0f));
    }

    public void Store(int frame)
    {
        if (dontStore) return;
        if (transform.childCount == 0) return;
        var positions = new Vector2[transform.childCount];
        if (Camera.main == null) return;
        for (var i = 0; i < transform.childCount; i++)
        {
            var position = Camera.main.WorldToScreenPoint(transform.GetChild(i).position);
            var screenPos = new Vector2(position.x, 1080 - position.y);
            positions[i] = screenPos;
        }

        framesAndCoords.data.Add(frame, positions);
        
        // foreach (var position in positions)
        // {
        //     if (transform.name == "Grill" && position.x > 1500f)
        //         print(position);
        // } = positions;
        StartCoroutine(Delay());        
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(.2f);
        if (transform.CompareTag("AUTOMESH"))
            for (var i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);  
    }
}