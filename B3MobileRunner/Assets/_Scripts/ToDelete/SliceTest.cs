using System.Collections;
using UnityEngine;
using EzySlice;

public class SliceTest : MonoBehaviour
{
    public UnityEngine.Plane planeSlicing;
    public Transform     planeSlicingTr;

    public GameObject[] allObjectsToSlice;

    private void Start()
    {
        //planeSlicing = GetComponent<UnityEngine.Plane>();
        //transform.position = planeSlicing.;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //UnityEngine. EzySlice.Plane
            Debug.Log("wéwé");
            foreach(GameObject go in allObjectsToSlice)
            {
                SlicedHull slicedHull = go.Slice(planeSlicingTr.position, planeSlicingTr.up);
                if (slicedHull != null)
                {
                slicedHull.CreateLowerHull(go); 
                slicedHull.CreateUpperHull(go);
                Destroy(go);
                                    }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            GetPlane(allObjectsToSlice[0].transform.position - allObjectsToSlice[2].transform.position);
            Debug.Log(plane.normal + "   " + plane.dist);
        }
        plane.OnDebugDraw();
    }

    EzySlice.Plane plane = new EzySlice.Plane();
    private EzySlice.Plane GetPlane(Vector2 cutDirection)
    {
        plane = new EzySlice.Plane();
        plane.Compute(cutDirection, Vector3.Cross(cutDirection, Camera.main.transform.forward));
        return plane;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(plane.);
        plane.OnDebugDraw();
    }
}
