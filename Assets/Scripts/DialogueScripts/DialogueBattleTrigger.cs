using UnityEngine;
using UnityEngine.SceneManagement;
public class DialogueBattleTrigger : MonoBehaviour
{
    #nullable enable
    public Transform? audioCaller = null;
    #nullable disable
    public string nextScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void BattleCall()
    {
        if (audioCaller != null)
        {
            NewGame.assignedBattleOST = audioCaller;
        }
        NewGame.lastScene = nextScene;
        NewGame.dBattleTrigger = true;
        NewGame.lastSceneCoord = null;
        SceneManager.LoadScene("BattleSystem");
    }



    void DialogueFinish()
    {
        BattleCall();
    }
  
}
