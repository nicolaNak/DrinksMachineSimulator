using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public float timer;
    bool ebolaBegun;
    public GameObject canToFall;
	void Start () 
    {
        timer = 0;
        ebolaBegun = false;
	}
	
	void Update () 
    {
        if (ebolaBegun) 
		{
			timer += Time.deltaTime;
			if (timer >= 3) 
			{
				Application.LoadLevel (1);
				ebolaBegun = false;
				timer = 0;
			}
		}
	}

    void PlayDispensing()
    {
        gameObject.GetComponent<AudioSource>().Play();
        canToFall.GetComponent<Animation>().Play();
    }

    //link to buttons
    public void ColaEbola()
    {
        ebolaBegun = true;
        PlayDispensing();
    }

    public void GoldenShower()
    {
        PlayDispensing();
        Application.LoadLevel(2);
    }

    public void H2No()
    {
        PlayDispensing();
        Application.LoadLevel(3);
    }

    public void SoylentGreen()
    {
        PlayDispensing();
        Application.LoadLevel(4);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
