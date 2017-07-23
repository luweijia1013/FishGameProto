using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour {

    private Vector3 origin_bottle_eulerangles;
	// Use this for initialization
	void Start () {
        origin_bottle_eulerangles = this.transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        SelfRotation();
	}

    void SelfRotation()
    {
        this.transform.eulerAngles = new Vector3(0, Time.time * 60, 0) + origin_bottle_eulerangles;
    }

}
