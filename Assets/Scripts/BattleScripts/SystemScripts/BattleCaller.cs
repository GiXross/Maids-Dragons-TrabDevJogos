using UnityEngine;
using Random = System.Random;
using UnityEngine.SceneManagement;
using System;

public class BattleCaller : MonoBehaviour
{
    public Random random;
#nullable enable
    public Transform? audioCaller = null;
    private Vector2 newPosition;
    private Vector2 oldPosition;
#nullable disable
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
            TreatInteraction();
        }


    }

    public void BattleCall()
    {
        if (audioCaller != null)
        {
            NewGame.assignedBattleOST = audioCaller;
        }

        NewGame.lastScene = SceneManager.GetActiveScene().name;
        NewGame.lastSceneCoord = oldPosition;
        SceneManager.LoadScene("BattleSystem");
    }


    public void TreatInteraction()
    {
        newPosition = interactedObject.transform.position;

        if ((Math.Abs(newPosition.x - oldPosition.x) < 0.2) && ((Math.Abs(newPosition.y - oldPosition.y) < 0.2))) //Meio feio pensar em como fazer melhor
        {
            return;
        }


        int val = random.Next(0, 20);
        //Debug.Log(val);
        oldPosition = newPosition;

        if (!firstTrigger && val == 9)
        {
            BattleCall();

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
