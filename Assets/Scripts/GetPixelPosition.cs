using JetBrains.Annotations;
using Mesh_Manipulation;
using UnityEngine;
using UnityEngine.UI;

public class GetPixelPosition : MonoBehaviour
{
    public int framesDone;
    
    [SerializeField] private Animator cameraPosition;
    [SerializeField] private new GameObject camera;
    [SerializeField] private Data data;
    [SerializeField] private Text debugText;
    
    private CoordsPerFrame[] coordsPerFrame;
    
    private int numberOfFrames;
    private readonly int stateNameHash = Animator.StringToHash("Camera Movement");
    private MeshManipulation[] updateMesh;
    private StoreData[] dataStorage;
    
    private void Start()
    {
        numberOfFrames = data.numberOfFrames;
        updateMesh = FindObjectsOfType<MeshManipulation>();
        dataStorage = FindObjectsOfType<StoreData>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //car.Play("change", -1, 0f);
            cameraPosition.Play(stateNameHash, -1, 0f);
            SetCamPosition();
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
    
    [UsedImplicitly]
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
        
        foreach (var mesh in updateMesh)
            mesh.UpdateBorders();

        foreach (var storeData in dataStorage)
            storeData.Store(framesDone - 1);
        
    }
}