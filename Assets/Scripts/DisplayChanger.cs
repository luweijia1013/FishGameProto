using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayChanger : MonoBehaviour {

    public GameObject[] cameras;
    private int index;
	// Use this for initialization
	void Start () {
        index = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        cameras[index % cameras.Length].SetActive(false);
        cameras[(index + 1) % cameras.Length].SetActive(true);

        index++;
    }
}
