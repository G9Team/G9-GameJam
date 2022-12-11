#define DEBUG_GUI //show debug GUI
using System.Collections;
using System.Collections.Generic;
#if DEBUG_GUI
using UnityEditor;
#endif
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float noiseConsilationDistance = 3f;

    #region DEBUG
#if DEBUG_GUI
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, noiseConsilationDistance);
    }
#endif
    #endregion
}
