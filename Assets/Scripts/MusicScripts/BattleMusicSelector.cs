using UnityEngine;


public class BattleMusicSelector : MonoBehaviour
{
    public Transform defaultBattleOST;
    private Transform musicTransform;
    private Transform assignedOST;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        assignedOST = NewGame.assignedBattleOST;
        if (assignedOST == null)
        {
            Vector3 position = new Vector3(0, 0);
            musicTransform = Instantiate(defaultBattleOST, position, Quaternion.identity);
            audioSource = musicTransform.GetComponent<AudioSource>();
        }
        else
        {
            Vector3 position = new Vector3(0, 0);
            musicTransform = Instantiate(assignedOST, position, Quaternion.identity);
            audioSource = musicTransform.GetComponent<AudioSource>();
        }

        //audioSource.gameObject.SetActive(true);
        //audioSource.gameObject.SetActive(true); 
        audioSource.Play();
    }

}
