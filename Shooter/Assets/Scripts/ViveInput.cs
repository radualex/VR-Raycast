using System.Collections;
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

    }

    private void Update()
    {
        m_trackedObject = GetComponent<SteamVR_TrackedObject>();
        m_gun = GetComponentInChildren<GunController>();

        if (m_trackedObject == null || m_gun == null)
        {
            return;
        }

        Device = SteamVR_Controller.Input((int) m_trackedObject.index);

        if (Device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            // m_gun.ShowRay = true;
            m_gun.StartStopWatch();
            Debug.Log("StopWatch started.");
        }

        if (Device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            //m_gun.ShowRay = false;
        }

        if (Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            m_gun.Fire();
        }

        var triggerValue = Device.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
        m_gun.SetTriggerRotation(triggerValue.x);
    }
}
