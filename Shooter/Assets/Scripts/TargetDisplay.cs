using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDisplay : MonoBehaviour {

    [SerializeField]
    Target[] targets;
    // Use this for initialization
    void Start () {
        targets[0].gameObject.SetActive(false);
        targets[1].gameObject.SetActive(false);
        targets[2].gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
