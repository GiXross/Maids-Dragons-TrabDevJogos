using UnityEngine;
using UnityEngine.SceneManagement;
public class NewGame : MonoBehaviour
{
    public static string lastScene;
    public static Vector2? lastSceneCoord; //feio pensar em como mudar

    public void LoadNextScene()
    {
        lastScene = "";
        lastSceneCoord = null;
        SceneManager.LoadScene("InitialScene");

    }
}
