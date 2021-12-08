using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GetPixelPosition : MonoBehaviour
{
    public int time;
    public int numberOfFrames;
    
    [SerializeField] private int screenWidth;
    [SerializeField] private int screenHeight;
    [SerializeField] private Animator camera;

    private float timeTakenPerFrame = 1 / 60f;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            camera.Play("Camera Movement", -1, 0f);
            InvokeRepeating(nameof(Run), timeTakenPerFrame, timeTakenPerFrame);
        }
    }

    private void Run()
    {
        time++;

        if (time < numberOfFrames)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            
            GetComponent<CoordsPerFrame>().coords.Add((screenWidth - screenPosition.x) + ", " + (screenHeight - (int) screenPosition.y));
        }
    }
}
