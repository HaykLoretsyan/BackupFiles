using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonName : MonoBehaviour {

    // Use this for 
    public string name;

	void Start () {
        transform.FindChild("Text").GetComponent<Text>().text = name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
