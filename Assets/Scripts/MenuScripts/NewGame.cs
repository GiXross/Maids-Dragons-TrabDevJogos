using UnityEngine;
using UnityEngine.SceneManagement;
public class NewGame : MonoBehaviour
{
    public static string lastScene = "" ;
    public static string loseScene = "Menu";
    public static bool dBattleTrigger = false; //Tive que fazer para resetar posição certo dependendo do evento que ativou
    #nullable enable
    public static Transform? assignedBattleOST;
    public static Vector2? lastSceneCoord; //feio pensar em como mudar

    public void LoadNewGame()
    {
        lastScene = "";
        loseScene= "Menu" ;
        lastSceneCoord = null;
        dBattleTrigger = false;
        assignedBattleOST = null;
        SceneManager.LoadScene("InitialScene");

    }


        public void LoadTutorial()
    {
 
        SceneManager.LoadScene("TutorialScene");

    }
}
