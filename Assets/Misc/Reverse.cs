using System;
using UnityEditor;
using UnityEngine;

namespace Misc_
{
    [ExecuteInEditMode]
    public class Reverse : MonoBehaviour
    {
        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(0).SetSiblingIndex(transform.childCount - i);
            }
        }
    }
}
