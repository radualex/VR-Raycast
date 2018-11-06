using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class ViveInput : MonoBehaviour
{

    public SteamVR_Controller.Device Device;

    private SteamVR_TrackedObject m_trackedObject = null;
    private GunController m_gun = null;

    void OnApplicationQuit()
    {
        GunController.MyList.Add(Environment.NewLine);
        using (var fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Shit.txt"), FileMode.Append, FileAccess.Write))
        using (var stream = new StreamWriter(fs))
        {
            foreach (var s in GunController.MyList)
            {
                stream.WriteLine(s);
            }
        }
    }

    private void Awake()
    {
        GunController.MyList.Add(SceneManager.GetActiveScene().name);
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

            GunController.MyList.Add("StopWatch started");
            //File.WriteAllLines(SaveToThisShit, new [] {"StopWatch started."});
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
