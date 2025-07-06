using UnityEngine;
using Random = System.Random;
using UnityEngine.SceneManagement;
using System;

public class BattleCaller : MonoBehaviour
{
    public Random random;

    private Vector2 newPosition;
    private Vector2 oldPosition;
    private GameObject interactedObject;
    private bool isTriggered;
    private bool firstTrigger;
    private void Awake()
    {
        random = new Random();
        isTriggered = false;
        firstTrigger = true;
    }

    void Update()
    {
        if (isTriggered)
        {
            treatInteraction();
        }


    }

    public void treatInteraction()
    {
        newPosition = interactedObject.transform.position;

        if (( Math.Abs(newPosition.x - oldPosition.x) < 0.2) && ((Math.Abs(newPosition.y - oldPosition.y) < 0.2))) //Meio feio pensar em como fazer melhor
        {
            return;
        }


        int val = random.Next(0, 20);
        Debug.Log(val);
        oldPosition = newPosition;

        if (!firstTrigger && val == 9)
        {
            NewGame.lastScene = SceneManager.GetActiveScene().name;
            NewGame.lastSceneCoord = oldPosition;
            SceneManager.LoadScene("BattleSystem");

        }
        firstTrigger = false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger && (collision.gameObject.GetComponent<PlayerControl>() == null))
        {
            return;
        }

        interactedObject = collision.gameObject;
        
        isTriggered = true;
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        isTriggered = false;
    }
}
