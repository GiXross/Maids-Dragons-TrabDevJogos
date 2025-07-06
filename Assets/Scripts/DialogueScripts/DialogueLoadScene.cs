using UnityEngine;
using UnityEngine.SceneManagement;
public class DialogueLoadScene : MonoBehaviour
{
    public string sceneName;
    public void DialogueFinish(){
        SceneManager.LoadScene(sceneName);
    }
}
