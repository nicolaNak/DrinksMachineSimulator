using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

    public GameObject can;
    Vector3 canPos;

	void Start () 
    {
        
	}
	
    void Position()
    {
        gameObject.transform.position = new Vector3((canPos.x + 64.1f), gameObject.transform.position.y, gameObject.transform.position.z); //only move on x axis
    }

	void Update () 
    {
        canPos = can.transform.position;
	}

    void LateUpdate()
    {
        Position();
    }
}
