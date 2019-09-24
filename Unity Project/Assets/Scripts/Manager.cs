using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int dots = 0;
    public int lives = 3;
    public Countdown countdown;
    public LivesCounter lives_counter;
    public AudioClip opening_clip;

	public void DecreaseDots(int amount = 1)
    {
        dots -= amount;

        if (dots == 0)
            WinSequence();
    }

    public void StartSequence()
    {
        // Start countdown
        countdown.StartTimer();

        // Activate ghost and pac-man
        Invoke("Activate", 4.0f);
    }

    private void ResetSequence()
    {
        // Reset positions
        ResetEntities();

        // StartSequence()
        StartSequence();
    }

    public void EndSequence()
    {
        // Deactivate entities
        Deactivate();

        if(--lives > 0)
        {
            lives_counter.DecreaseLives();
            ResetSequence();
        }
        else
        {
            ExplodeGame();
        }

        // Pass score to end screen

        // Load Menu
        Invoke("GoToMenu", 5.0f);
    }

    public void WinSequence()
    {
        Deactivate();
        ExplodeGame();
    }

    public void GoToMenu()
    {
        // Load Menu
    }

    public void ExplodeGame()
    {
        foreach(GameObject o in GameObject.FindGameObjectsWithTag("Wall"))
            ExplodeObject(o);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Ghost"))
            ExplodeObject(o);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Pellet"))
            ExplodeObject(o);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Power Pellet"))
            ExplodeObject(o);

        GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>().enabled = false;
        ExplodeObject(GameObject.FindGameObjectWithTag("Pac-Man"));
    }

    public void ExplodeObject(GameObject o)
    {
        o.AddComponent<Explode>().ExplodeIn(Vector2.Distance(Vector2.zero, o.transform.position) / 10.0f);
    }

    private void Activate()
    {
        GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>().active = true;

        foreach (GameObject ghost in GameObject.FindGameObjectsWithTag("Ghost"))
        {
            ghost.GetComponent<GhostMovement>().active = true;
        }
    }

    private void Deactivate()
    {
        GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>().active = false;

        foreach (GameObject ghost in GameObject.FindGameObjectsWithTag("Ghost"))
        {
            ghost.GetComponent<GhostMovement>().active = false;
        }
    }
    
    private void Start()
    {
        StartSequence();

        AudioSource.PlayClipAtPoint(opening_clip, Vector3.zero);
    }

    public void ResetEntities()
    {
        GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManCornerMovement>().Reset();
        GameObject.FindGameObjectWithTag("Pac-Man").GetComponent<PacManAnimator>().Reset();

        foreach (GameObject ghost in GameObject.FindGameObjectsWithTag("Ghost"))
        {
            ghost.GetComponent<GhostMovement>().Reset();
            ghost.GetComponent<GhostAnimator>().Reset();
        }
    }
}
