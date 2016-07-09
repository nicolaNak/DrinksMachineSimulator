using UnityEngine;
using System.Collections;

public class EndLevelControls : MonoBehaviour {

    public GameObject end;
    public AudioSource music;

    void Start () 
    {
	
	}

    public void EndCanvas()
    {
        end.SetActive(true);
        music.Stop();
        //play cheering
        gameObject.GetComponent<AudioSource>().Play();

    }
	
    void OnTriggerEnter(Collider c)
    {
        if(c.GetComponent<Collider>().tag == "Player")
        {
            EndCanvas();
        }
    }

	void Update () 
    {
	
	}
}
