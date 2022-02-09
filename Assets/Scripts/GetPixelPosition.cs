using System.Globalization;
using Misc_;
using UnityEngine;
using UnityEngine.UI;

public class GetPixelPosition : MonoBehaviour
{
    public int framesDone;
    
    [SerializeField] private Animator cameraPosition;
    [SerializeField] private new GameObject camera;
    [SerializeField] private Data data;
    [SerializeField] private Text debugText;
    [SerializeField] private Animator car;
    [SerializeField] private Transform[] points;
    [SerializeField] private Transform frontBumper;
    
    
    private CoordsPerFrame[] coordsPerFrame;

    private const float TIME_TAKEN_PER_FRAME = 1f;
    private int numberOfFrames;
    private readonly int stateNameHash = Animator.StringToHash("Camera Movement");
    private MeshManipulation[] updateMesh;
    private StoreData[] storeDatas;
    
    private void Start()
    {
        numberOfFrames = data.numberOfFrames;
        updateMesh = FindObjectsOfType<MeshManipulation>();
        storeDatas = FindObjectsOfType<StoreData>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //car.Play("change", -1, 0f);
            cameraPosition.Play(stateNameHash, -1, 0f);
            SetCamPosition();

            InvokeRepeating(nameof(Run), TIME_TAKEN_PER_FRAME, TIME_TAKEN_PER_FRAME);
        }
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
       
        for (var i = 0; i < frontBumper.childCount; i++)
            frontBumper.GetChild(i).parent = points[framesDone - 1];
        while (points[framesDone].childCount > 0)
            points[framesDone].GetChild(points[framesDone].childCount - 1).SetParent(frontBumper);

        framesDone++;
        
        DebugStatus("Capturing Frame " + framesDone);
        SetCamPosition();
        
        foreach (var mesh in updateMesh)
            mesh.UpdateBorders();

        foreach (var storeData in storeDatas)
            storeData.Store(framesDone - 1);
    }
}
