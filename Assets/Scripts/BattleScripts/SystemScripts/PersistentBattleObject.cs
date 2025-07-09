using UnityEngine;
using UnityEngine.SceneManagement;
public class PersistentBattleObject : MonoBehaviour
{

    [SerializeField] private Transform pfCharacterBattleAlly;
    [SerializeField] private Transform pfCharacterBattleAlly2;
    [SerializeField] private Transform pfCharacterBattleEnemy;
    public string ally1Name;
    public string ally2Name;
    public string enemy1Name;
    public bool isBoss = false;
    private int counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        counter = 0;//feito para checar se é a primeira cena carregada
                    //se for a primeira aida não faz nada
                    //na segunda se destroy
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (counter == 1)
        {
            BattleHandler temporaryHandler = FindFirstObjectByType<BattleHandler>();
            if (temporaryHandler != null)
            {
                temporaryHandler.StartBattle(pfCharacterBattleAlly, pfCharacterBattleAlly2, pfCharacterBattleEnemy, ally1Name, ally2Name, enemy1Name, isBoss);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
        else
        {
            counter = 1;
        }

    }




}
