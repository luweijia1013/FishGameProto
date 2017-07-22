using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int score = 0;
        score = (int)(Time.time * 40);
        this.GetComponent<Text>().text = "Score: " + score;
	}
}
