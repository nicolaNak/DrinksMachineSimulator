using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //attached to empty game object in every scene
    public int levelIndex;
    public GameObject can;
    public GameObject failed;
    public AudioSource music;
    bool canCollided;

	void Start () 
    {
        canCollided = false;
	}

    public void ResetLevel()
    {
        Application.LoadLevel(levelIndex);
    }

    public void StartMenu()
    {
        Application.LoadLevel(0);
    }

    public void NextLevel()
    {
        if (levelIndex < 4)
        {
            Application.LoadLevel(levelIndex + 1); //start the next level
        }
        if (levelIndex >= 4)
        {
            //completed the game, now need to go take a piss
            Application.LoadLevel(0);
        }
    }

    void Failure()
    {
        failed.SetActive(true);
        music.Stop();
        //play crash music
        //failed.GetComponent<AudioSource>().Play();
    }

	void Update () 
    {
        canCollided = can.GetComponent<CanControls>().hasCollided;
        if(canCollided)
        {
            Failure();
        }
	}
}
