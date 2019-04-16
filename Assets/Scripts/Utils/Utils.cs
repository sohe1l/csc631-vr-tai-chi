using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public static class Utils
{

    public static void DrawLine(Vector3 start, Vector3 end, Material material, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = material; // new Material(Shader.Find("Green"));

        //lr.startColor = color;

        lr.startWidth = 0.1f;
        lr.startWidth = 0.01f;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //GameObject.Destroy(myLine, duration);
    }

    public static IEnumerator SetVRDevice(string device, bool vrEnabled)
    {
        XRSettings.LoadDeviceByName(device);
        yield return null;
        XRSettings.enabled = vrEnabled;
    }

}
