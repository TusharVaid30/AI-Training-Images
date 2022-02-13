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
    [SerializeField] private Transform[] points;
    [SerializeField] private Transform frontBumper;
    [SerializeField] private Transform rearBumper;
    [SerializeField] private Transform rightORVM;
    [SerializeField] private Transform leftORVM;
    [SerializeField] private Transform trunk;
    [SerializeField] private Transform leftTaillight;
    [SerializeField] private Transform rightTaillight;
    [SerializeField] private Transform rightHeadlight;
    [SerializeField] private Transform leftHeadlight;
    [SerializeField] private Transform hood;
    [SerializeField] private Transform[] sidePanels;
    [SerializeField] private Transform[] frontPanels;
    [SerializeField] private Transform[] backPanels;
    
    private CoordsPerFrame[] coordsPerFrame;

    private const float TIME_TAKEN_PER_FRAME = 1f;
    private int numberOfFrames;
    private readonly int stateNameHash = Animator.StringToHash("Camera Movement");
    private MeshManipulation[] updateMesh;
    private StoreData[] storeDatas;
    private int pointIndex;
    
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

        if (pointIndex == points.Length)
            pointIndex = 0;

        if (framesDone < 500)
        {
            for (var i = 0; i < frontBumper.childCount; i++)
                frontBumper.GetChild(i).parent = points[pointIndex - 1];
            while (points[pointIndex].childCount > 0)
                points[pointIndex].GetChild(points[pointIndex].childCount - 1).SetParent(frontBumper);
        }
        else if (framesDone is >= 500 and < 1000)
        {
            frontBumper.GetComponent<MeshManipulation>().updateBorders = true;
            frontBumper.GetComponent<AlignPoints>().align = true;
            rightHeadlight.GetComponent<MeshManipulation>().updateBorders = false;

            foreach (var bumperSidePanel in sidePanels)
                bumperSidePanel.GetComponent<StoreData>().dontStore = false;
            foreach (var frontPanel in frontPanels)
                frontPanel.GetComponent<StoreData>().dontStore = true;
        }
        else if (framesDone is >= 1000 and < 1500)
        {
            hood.GetComponent<MeshManipulation>().updateBorders = false;
            rearBumper.GetComponent<MeshManipulation>().updateBorders = true;
            rearBumper.GetComponent<AlignPoints>().align = true;
            rightORVM.GetComponent<MeshManipulation>().updateBorders = false;
        }
        else if (framesDone is >= 1500 and < 2000)
        {
            trunk.GetComponent<MeshManipulation>().updateBorders = true;
            trunk.GetComponent<AlignPoints>().align = true;
            leftTaillight.GetComponent<MeshManipulation>().updateBorders = true;
            leftTaillight.GetComponent<AlignPoints>().align = true;
            
            frontBumper.GetComponent<MeshManipulation>().updateBorders = false;
            frontBumper.GetComponent<AlignPoints>().align = false;
            leftHeadlight.GetComponent<MeshManipulation>().updateBorders = false;
            leftORVM.GetComponent<MeshManipulation>().updateBorders = false;
        }
        else if (framesDone >= 2000)
        {
            rightTaillight.GetComponent<MeshManipulation>().updateBorders = true;
            rightTaillight.GetComponent<AlignPoints>().align = true;
            foreach (var bumperSidePanel in sidePanels)
                bumperSidePanel.GetComponent<StoreData>().dontStore = true;

            foreach (var backPanel in backPanels)
                backPanel.GetComponent<StoreData>().dontStore = false;
        }
        

        framesDone++;
        pointIndex++;
        
        DebugStatus("Capturing Frame " + framesDone);
        SetCamPosition();
        
        foreach (var mesh in updateMesh)
            mesh.UpdateBorders();

        foreach (var storeData in storeDatas)
            storeData.Store(framesDone - 1);
        
        if (framesDone < 60)
        {
            for (var i = 0; i < points[pointIndex - 1].childCount; i++)
                points[pointIndex - 2].GetChild(i).parent = frontBumper;
            while (frontBumper.childCount > 0)
                frontBumper.GetChild(frontBumper.childCount - 1).SetParent(points[pointIndex - 1]);
        }
    }
}
