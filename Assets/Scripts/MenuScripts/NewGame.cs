using UnityEngine;
using UnityEngine.SceneManagement;
public class NewGame : MonoBehaviour
{
    public static string lastScene = "" ;
    public static string loseScene = "Menu";
    #nullable enable
    public static Transform? assignedBattleOST;
    public static Vector2? lastSceneCoord; //feio pensar em como mudar

    public void LoadNextScene()
    {
        lastScene = "";
        loseScene= "Menu" ;
        lastSceneCoord = null;
        assignedBattleOST = null;
        SceneManager.LoadScene("InitialScene");

    }
}
