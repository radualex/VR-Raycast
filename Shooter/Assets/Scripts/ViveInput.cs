﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour
{

    public SteamVR_Controller.Device Device;

    private SteamVR_TrackedObject m_trackedObject = null;
    private GunController m_gun = null;

    private void Awake()
    {
        m_trackedObject = GetComponent<SteamVR_TrackedObject>();
        m_gun = GetComponentInChildren<GunController>();
    }
	
	private void Update ()
	{
	    Device = SteamVR_Controller.Input((int) m_trackedObject.index);

	    if (Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
	    {
	        m_gun.Fire();
	    }

	    if (Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
	    {
	        
	    }

	    var triggerValue = Device.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
	    m_gun.SetTriggerRotation(triggerValue.x);
	}
}