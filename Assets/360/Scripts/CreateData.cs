using UnityEngine;
using UnityEngine.InputSystem;

namespace _360.Scripts
{
    public class CreateData : MonoBehaviour
    {
        public delegate void StartStoring();
        public static event StartStoring OnStart;
        public int maxFrames;

        [SerializeField] private Transform camHolder;
        [SerializeField] private CollectData data;

        private MouseInput mouseInput;
        private int currentFrame;

        private void Awake()
        {
            mouseInput = new MouseInput();
        }
        
        private void OnEnable()
        {
            mouseInput.Enable();
        }
    
        private void OnDisable()
        {
            mouseInput.Disable();
        }

        private void Start()
        {
            mouseInput.Mouse.Click.performed += StartCreating;
        }

        private void StartCreating(InputAction.CallbackContext obj)
        {
            camHolder.GetComponent<Animator>().Play("CamMov", -1, 0f);
            InvokeRepeating(nameof(SetCamPosition), 1f, 1f);
        }

        private void SetCamPosition()
        {
            var mainCam = transform;
            mainCam.position = camHolder.position;
            mainCam.rotation = camHolder.rotation;
            
            SendData();
        }

        private void SendData()
        {
            if (currentFrame >= maxFrames)
            {
                OnStart?.Invoke();
                return;
            }
            data.positionVector.Add(transform.position);
            data.cameraRotation.Add(currentFrame / 2f);
            
            currentFrame++;
        }
    }
}
