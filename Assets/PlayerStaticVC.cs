﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerStaticVC : MonoBehaviour
{
    public Transform[] FootTarget;
    public Transform LookTarget;
    public Transform HandTarget;
    public Transform HandPole;
    public Transform Step;
    public Transform Attraction;

    public Transform TargetHandLeft;
    public Transform TargetHandRight;
    public GameObject Master;

    private Transform offset;


    // Start is called before the first frame update
    void Start()
    {
        //offset.Translate(new Vector3(10, 0, 10));

        StartCoroutine(Utils.SetVRDevice("OpenVR", true));
    }

    // Update is called once per frame
    void Update()
    {
        TargetHandLeft.transform.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
        TargetHandRight.transform.position = InputTracking.GetLocalPosition(XRNode.RightHand);

        Vector3 m = InputTracking.GetLocalPosition(XRNode.Head);
        Master.transform.position = new Vector3(m.x, 0, m.z);

        TargetHandLeft.transform.Translate(0, 0, 2, Space.World);
        TargetHandRight.transform.Translate(0, 0, 2, Space.World);
        Master.transform.Translate(0, 0, 2, Space.World);
    }
}
