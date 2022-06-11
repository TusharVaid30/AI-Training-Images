using UnityEngine;

public class CheckVIew : MonoBehaviour
{
    [SerializeField] private Transform bumper;

    public int CheckInView()
    {
        if (Camera.main == null) return 0;
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        var ray = Camera.main.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity)) return 0;
        if (hit.transform == bumper || hit.transform == transform)
            return 1;
        return 0;
    }
}
