using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mesh_Manipulation
{
    public class StoreData : MonoBehaviour
    {
        [SerializeField] private List<Vector2> temp = new List<Vector2>();
        
        private FramesAndCoords framesAndCoords;

        private void Start()
        {
            framesAndCoords = GetComponent<FramesAndCoords>();
            temp.Add(new Vector2(1f, 0f));
        }

        public void Store(int frame)
        {
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
        
            StartCoroutine(Delay());        
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(.2f);
            if (!transform.CompareTag("AUTOMESH")) yield break;
            for (var i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
        }
    }
}
