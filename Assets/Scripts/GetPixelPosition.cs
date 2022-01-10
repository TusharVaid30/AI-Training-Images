using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GetPixelPosition : MonoBehaviour
{
    public int framesDone;
    
    [SerializeField] private Animator cameraPosition;
    [SerializeField] private new GameObject camera;
    [SerializeField] private Data data;
    [SerializeField] private Text debugText;
    [SerializeField] private Transform pointsHolder;
    
    private CoordsPerFrame[] coordsPerFrame;

    private const float TIME_TAKEN_PER_FRAME = 1f;
    private int numberOfFrames;
    private readonly int stateNameHash = Animator.StringToHash("Camera Movement");

    private void Start()
    {
        numberOfFrames = data.numberOfFrames;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        cameraPosition.Play(stateNameHash, -1, 0f);
        SetCamPosition();
        InvokeRepeating(nameof(Run), TIME_TAKEN_PER_FRAME, TIME_TAKEN_PER_FRAME);
        GetComponent<Animator>().Play("ChangeData", -1, 0f);
    }

    private void SetCamPosition()
    {
        var camTransform = cameraPosition.transform;
        camera.transform.position = camTransform.position;
        camera.transform.rotation = camTransform.rotation;
    }

    private void DebugStatus(string info)
    {
        Debug.Log(info);
        debugText.text = info;
    }
    
    private void Run()
    {
        if (framesDone > numberOfFrames)
        {
            DebugStatus("Writing Data to File");
            return;
        }

        framesDone++;
        
        DebugStatus("Capturing Frame " + framesDone);
        SetCamPosition();

        if (Camera.main is null) return;
        foreach (var coords in pointsHolder.GetComponentsInChildren<CoordsPerFrame>())
        {
            var screenPosition = Camera.main.WorldToScreenPoint(coords.transform.position);

            coords.coordsX.Add((screenPosition.x).ToString(CultureInfo.InvariantCulture));
            coords.coordsY.Add((1080 - screenPosition.y).ToString(CultureInfo.InvariantCulture));
        }
    }
}
