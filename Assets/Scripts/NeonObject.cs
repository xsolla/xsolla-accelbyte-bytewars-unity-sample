using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonObject
{
    public List<Vector3> vertexList = new List<Vector3>();
    public List<Vector2> uvList = new List<Vector2>();
    public List<int> indexList = new List<int>();

    const float UNREAL_TO_UNITY_SCALE = 0.01f;

    public NeonObject(List<Vector3> outerVerts, List<Vector3> innerVerts)
    {
        Debug.AssertFormat(innerVerts.Count == outerVerts.Count, "innerVerts and outerVerts must be of equal length");


        // combine inner and outer into one list inner-outer-inner-outer for clockwise rotation
        for (int i = 0; i < innerVerts.Count; ++i)
        {
            vertexList.Add(innerVerts[i] * UNREAL_TO_UNITY_SCALE);
            vertexList.Add(outerVerts[i] * UNREAL_TO_UNITY_SCALE);

            uvList.Add(new Vector2(0, 0)); // distribute u across inner->outer
            uvList.Add(new Vector2(1, 0)); //
        }

        // mirror through Y-axis. 
        // start at count-2 as already added the last pair of coords in the previous pass otherwise we'll have pointless degenerate triangles in the middle.
        for (int i = innerVerts.Count - 2; i >= 0; --i)
        {
            Vector3 inner = innerVerts[i] * UNREAL_TO_UNITY_SCALE;
            Vector3 outer = outerVerts[i] * UNREAL_TO_UNITY_SCALE;

            vertexList.Add(new Vector3(-inner.x, inner.y, inner.z));
            vertexList.Add(new Vector3(-outer.x, outer.y, outer.z));

            uvList.Add(new Vector2(0, 0));
            uvList.Add(new Vector2(1, 0));
        }


        // set index pattern
        int[] indexPattern = new int[] { 0, 1, 2, 1, 3, 2 };

        // loop through +2 (skip over each outer vert)
        for (int i = 0; i < vertexList.Count - 2; i += 2)
        {
            for (int j = 0; j < indexPattern.Length; ++j)
            {
                indexList.Add(indexPattern[j] + i);
            }
        }
    }
}
