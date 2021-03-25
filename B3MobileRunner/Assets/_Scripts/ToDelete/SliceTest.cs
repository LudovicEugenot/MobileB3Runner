using System.Collections;
using UnityEngine;
using EzySlice;

public class SliceTest : MonoBehaviour
{
    public UnityEngine.Plane planeSlicing;

    public GameObject[] allObjectsToSlice;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("wéwé");
            foreach(GameObject go in allObjectsToSlice)
            {
                //SlicedHull slicedHull = go.Slice();

            }
        }
    }
}
