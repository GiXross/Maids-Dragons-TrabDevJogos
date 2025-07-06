using UnityEngine;
using Random = System.Random;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
public class BattleHandler : MonoBehaviour
{
    private Random random;
    public int gaugeFactor = 500;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform pfCharacterBattleAlly;
    [SerializeField] private Transform pfCharacterBattleAlly2;
    [SerializeField] private Transform pfCharacterBattleEnemy;
    //TODO: Primeiro implementar sistema de action gauge e depois fazer cada player
    //TODO: Fazer do playerCharacterBattle o base que fica trocando entre o 1 e o 2
    //player party
    private CharacterBattle playerCharacterBattle;
    private CharacterBattle playerCharacterBattle2;
    //enemy party
    private CharacterBattle enemyCharacterBattle;
    private CharacterBattle enemyCharacterBattle2;
    private CharacterBattle enemyCharacterBattle3;
    //active character
    private CharacterBattle activeCharacterBattle;
    //list of battle participants
    private CharacterBattle[] listOfEnemies;
    private CharacterBattle[] listOfAllies;
    private CharacterBattle[] listOfFighters;
    //number of allies and enemies
    private int numAllies;
    private int numEnemies;
    //bools for end of battle
    private bool areAlliesDead;
    private bool areEnemiesDead;

    public BattleDialogueManager battleDManager;

    public Transform hudActionGauge1;
    private float hudXScale1;
    public Transform hudActionGauge2;
    private float hudXScale2;
    public string ally1Name;
    public string ally2Name;
    public string enemy1Name;
    public bool isBoss = false;


    private bool isWon;
    private bool isWaitOver;
    private int targetIndex;
    private int lenListFight;
    private State state;

    private enum State
    {
        WaitingForPlayer,
        EnemyTurn,
        Busy,
        ChargingGauges,
    }

    void Start()
    {
        battleDManager.ClearSkills();
        StartBattleLogic();
        targetIndex = numAllies;
        lenListFight = listOfFighters.Length;
        battleDManager.StartBattle(this);

        //SetActiveCharacterBattle(playerCharacterBattle); resquicios da implementacao antiga
        //Debug.Log("Enemy " + enemyCharacterBattle.currentHP);


        //playerCharacterBattle.Attack(enemyCharacterBattle);
        //state = State.WaitingForPlayer; resquicios da implementacao antiga
        state = State.ChargingGauges;
        isWaitOver = true;
        areAlliesDead = false;
        areEnemiesDead = false;
        //characterBattle.Attack(characterBattle);

        hudXScale1 = hudActionGauge1.localScale.x;
        if (numAllies > 1)
        {

            hudXScale2 = hudActionGauge2.localScale.x;
        }
        else
        {
            hudActionGauge2.gameObject.SetActive(false);
            hudActionGauge2.parent.gameObject.SetActive(false);
        }

        foreach (CharacterBattle cb in listOfFighters)
        {
            cb.OnHPChanged += CharacterBattle_OnHPChanged;
            if (cb.sheet.isAlly)
            {
                cb.OnManaChanged += CharacterBattle_OnHPChanged;
            }
            /*
            playerCharacterBattle2.OnHPChanged += CharacterBattle_OnHPChanged;
            enemyCharacterBattle.OnHPChanged += CharacterBattle_OnHPChanged;
            enemyCharacterBattle2.OnHPChanged += CharacterBattle_OnHPChanged;
            enemyCharacterBattle3.OnHPChanged += CharacterBattle_OnHPChanged;*/
        }

    }


    void Update()
    {
        //Debug.Log(playerCharacterBattle.actionGauge);
        //Debug.Log(enemyCharacterBattle.actionGauge);
        if (state == State.WaitingForPlayer) //turno player
        {
            //Debug.Log(targetIndex);

            if (isWaitOver)
            {
                StartCoroutine(TargetLogic());
            }

            if (Input.GetAxisRaw("Jump") == 1)//Condicao para ataque normal para player
            {
                listOfFighters[targetIndex].HideTarget();
                state = State.Busy;
                activeCharacterBattle.Attack(listOfFighters[targetIndex], () =>
                {
                    //Debug.Log("Enemy Curr HP" + enemyCharacterBattle.currentHP);
                    ChooseNextActiveCharacter();
                    battleDManager.ClearSkills();
                    targetIndex = numAllies;
                    if (listOfFighters[targetIndex].isDead())
                    {
                        IndexLogicPlus1();
                    }
                    //Debug.Log("Player Curr HP" + playerCharacterBattle.currentHP);
                    //state = State.WaitingForPlayer;

                }
                );
            }

            if (Input.GetAxisRaw("Skill 1") == 1)
            {
                SkillLogic(1);
            }
            if (Input.GetAxisRaw("Skill 2") == 1)
            {
                SkillLogic(2);
            }
            if (Input.GetAxisRaw("Skill 3") == 1)
            {
                SkillLogic(3);
            }

        }
        else
        {
            if (state == State.EnemyTurn) //turno inimigo
            {
                state = State.Busy;
                EnemyAction();

            }
            else
            { //carga de barra
                if (state == State.ChargingGauges)
                {
                    ActionGaugeLogic();
                }
            }
        }
    }



    private void SkillLogic(int val)
    {
        if (activeCharacterBattle.sheet.GetNumSkills() >= val)
        { // indice da skill é val-1
            int castCost = activeCharacterBattle.sheet.GetManaCost(val - 1);
            if (activeCharacterBattle.currentMana - castCost > 0)
            { //Se a mana que sobra depois da skill for menor do que 0, não é castável
                listOfFighters[targetIndex].HideTarget();
                state = State.Busy;

                activeCharacterBattle.Skill(listOfFighters[targetIndex], val + 1, () =>
                {
                    activeCharacterBattle.currentMana -= castCost;
                    activeCharacterBattle.ManaChanged();
                    //Debug.Log("Enemy Curr HP" + enemyCharacterBattle.currentHP);
                    ChooseNextActiveCharacter();
                    battleDManager.ClearSkills();
                    targetIndex = numAllies;
                    if (listOfFighters[targetIndex].isDead())
                    {
                        IndexLogicPlus1();
                    }
                    //Debug.Log("Player Curr HP" + playerCharacterBattle.currentHP);
                    //state = State.WaitingForPlayer;
                }
                );
            }
        }
    }

    private void StartBattleLogic()
    {
        int numberOfFighters = 0;
        numAllies = 1;
        //int numEnemies = 0;
        random = new Random();
        // TODO: Logica ainda não implementada, apenas atribui a lista
        //TODO: Mudar isso daqui Depois para não ficar estatico e poder incorporar numeros de inimgos diferentes
        // De preferencia por alguma função matemática

        //listOfAllies[numAllies];
        //numAllies += 1;
        if (pfCharacterBattleAlly2 != null)
        {// se tiver o segundo cria lista com 2
            numAllies += 1;
            listOfAllies = new CharacterBattle[numAllies];
            listOfAllies[1] = SpawnCharacter(true, pfCharacterBattleAlly2, 3, ally2Name);
        }//TODO: Para projetos futuros, fazer uma lista de prefabs de aliados talvez ajudasse 
        else
        { // se não tiver cria com 1
            listOfAllies = new CharacterBattle[numAllies];
        }
        listOfAllies[0] = SpawnCharacter(true, pfCharacterBattleAlly, 0, ally1Name);

        numEnemies = 1;
        if (!isBoss)
        {
            numEnemies = random.Next(1, 4);
        }

        listOfEnemies = new CharacterBattle[numEnemies];
        /*
        listOfEnemies[0] = enemyCharacterBattle;
        listOfEnemies[1] = enemyCharacterBattle2;
        listOfEnemies[2] = enemyCharacterBattle3;
        */

        for (int i = 0; i < numEnemies; i++)
        {
            //TODO: Daria para expandir aqui para mais inimigos com mais prefabs de inimigos e fazendo uma lista igual
            listOfEnemies[i] = SpawnCharacter(false, pfCharacterBattleEnemy, i * 2, enemy1Name);
            /*        enemyCharacterBattle = SpawnCharacter(false, 0);
                    enemyCharacterBattle2 = SpawnCharacter(false, 2);
                    enemyCharacterBattle3 = SpawnCharacter(false, 4);
            */
        }
        numberOfFighters = numAllies + numEnemies;
        listOfFighters = new CharacterBattle[numberOfFighters];
        //TODO: usar depois para criar de forma dinâmica
        //Array.Rezsize(ref listOfFighters, numberOfFighters);
        //array.Length;

        //adicionar aliados
        for (int i = 0; i < numAllies; i++)
        {
            listOfFighters[i] = listOfAllies[i];
        }
        // Debug.Log(numEnemies);
        //adicionar inimigos
        for (int i = numAllies; i < (numAllies + numEnemies); i++)
        {

            listOfFighters[i] = listOfEnemies[i - numAllies];
        }

    }


    private void IndexLogicMinus1()
    {
        if (targetIndex > 0)
        {
            targetIndex -= 1;
        }
        else
        {
            targetIndex = (lenListFight - 1);
        }
        if (listOfFighters[targetIndex].isDead())
        {
            IndexLogicMinus1(); //TODO: Pode ser perigoso se eu colocar um golpe que possa matar aliado também
        }

    }

    private void IndexLogicPlus1()
    {

        if (targetIndex < (lenListFight - 1))
        {
            targetIndex += 1;
        }
        else
        {
            targetIndex = 0;
        }

        if (listOfFighters[targetIndex].isDead())
        {
            IndexLogicPlus1();  //TODO: Pode ser perigoso se eu colocar um golpe que possa matar aliado também
        }


    }

    private void SkillSetter(CharacterBattle cb)
    {
        battleDManager.SetSkills(cb);
    }

    private IEnumerator TargetLogic()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Vertical") == 1)
        {
            listOfFighters[targetIndex].HideTarget();

            IndexLogicPlus1();

            listOfFighters[targetIndex].ShowTarget();
            //Debug.Log(Input.GetAxisRaw("Horizontal"));
            isWaitOver = false;
            yield return new WaitForSeconds(0.5f);
            isWaitOver = true;
            yield break;
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == -1)
            {
                listOfFighters[targetIndex].HideTarget();

                IndexLogicMinus1();

                listOfFighters[targetIndex].ShowTarget();
                isWaitOver = false;
                yield return new WaitForSeconds(0.5f);
                isWaitOver = true;
                yield break;
                //Debug.Log(Input.GetAxisRaw("Horizontal"));
            }
        }
    }

    private void EnemyAction()
    {
        //SetActiveCharacterBattle(enemyCharacterBattle);
        //state = State.Busy;

        //somente para testes definirei estaticamente o valor randomico gerado, mas a base vai ser Next(1,n),
        //com n sendo o número total de skills
        int auxVal = random.Next(1, activeCharacterBattle.sheet.GetNumSkills() + 2); //reaproveitando o random já criado                  
        if (auxVal == 1)//TODO: Ideia encapsular tudo em uma funcao só, para não precisar do if
        {
            int auxVal2 = random.Next(0, numAllies);
            if (listOfAllies[auxVal2].isDead())
            {
                if (auxVal2 == 1)//se aliado 2 está morto vai no 1
                {
                    auxVal2 = 0;
                }
                else
                { // se aliado 1 está morto vai no 2
                    auxVal2 = numAllies - 1;
                }
            }
            activeCharacterBattle.Attack(listOfFighters[auxVal2], () =>
            {
                ChooseNextActiveCharacter();
            }); // Ataque do inimigo
        }
        else
        {
            int auxVal2 = random.Next(0, numAllies);
            if (listOfFighters[auxVal2].isDead())
            {
                if (auxVal2 == 1)//se aliado 2 está morto vai no 1
                {
                    auxVal2 = 0;
                }
                else
                {// se aliado 1 está morto vai no 2
                    auxVal2 = numAllies - 1;
                }
            }


            activeCharacterBattle.Skill(listOfFighters[auxVal2], auxVal, () =>
           {
               ChooseNextActiveCharacter();
           }); // Ataque do inimigo
        }

    }


    private void UpdateGauge()
    {
        hudActionGauge1.localScale = new Vector3(hudXScale1 * (((float)listOfAllies[0].actionGauge) / gaugeFactor), hudActionGauge1.localScale.y, 0);


        if (numAllies > 1)
        {
            hudActionGauge2.localScale = new Vector3(hudXScale2 * (((float)listOfAllies[1].actionGauge) / gaugeFactor), hudActionGauge2.localScale.y, 0);
        }
    }

    private void ActionGaugeLogic()
    {
        foreach (CharacterBattle cb in listOfFighters)
        {

            if (state == State.ChargingGauges)
            {
                if (!cb.isDead())
                {
                    cb.actionGauge += (int)(5 + Math.Truncate((double)cb.sheet.agi / 5)); //por enquanto acho que fica assim
                }
                if (cb.actionGauge >= gaugeFactor)
                {
                    cb.actionGauge = gaugeFactor;
                    if (cb.sheet.isAlly)
                    {
                        state = State.WaitingForPlayer;
                        SkillSetter(cb);
                        listOfFighters[targetIndex].ShowTarget();
                    }
                    else
                    {
                        state = State.EnemyTurn;
                    }
                    SetActiveCharacterBattle(cb);
                }
            }
        }
        UpdateGauge();

    }
    private void ChooseNextActiveCharacter()
    {
        if (TestBattleOver())
        {
            if (this.isWon)
            {
                SceneManager.LoadScene(NewGame.lastScene);
                //FindFirstObjectByType<PlayerControl>().setCoordinates(NewGame.lastSceneCoord);
            }
            else
            {
                SceneManager.LoadScene("Menu");
            }
            return;
        }
        activeCharacterBattle.actionGauge = 0;
        state = State.ChargingGauges;
        /*
                        if (activeCharacterBattle == playerCharacterBattle)// se é o turno do player quando está para escolher o próximo
                        {



                        }
                            else
                            {
                                SetActiveCharacterBattle(playerCharacterBattle);
                                state = State.WaitingForPlayer;
                            }

                */

    }

    private bool TestBattleOver()
    {
        foreach (CharacterBattle pb in listOfAllies)
        {
            if (pb.isDead())
            {
                areAlliesDead = true;
            }
            else
            {
                areAlliesDead = false;
                break;
            }

        }

        if (areAlliesDead)
        {
            battleDManager.EndOfBattle(this, "Você morreu!\nVitória do Inimigo!");
            this.isWon = false;
            return true;
        }

        foreach (CharacterBattle eb in listOfEnemies)
        {
            if (eb.isDead())
            {
                areEnemiesDead = true;
            }
            else
            {
                areEnemiesDead = false;
                break;
            }
        }
        if (areEnemiesDead)
        {
            battleDManager.EndOfBattle(this, "Você venceu!");
            this.isWon = true;
            return true;
        }

        return false;
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle)
    {
        /*
        if (activeCharacterBattle != null) {
            activeCharacterBattle.HideHighlight();
        }*/

        activeCharacterBattle = characterBattle;
        if (activeCharacterBattle.sheet.isAlly)
        {
            activeCharacterBattle.ShowHighlight();
        }
    }



    private CharacterBattle SpawnCharacter(bool isPlayerTeam, Transform pf, int posicao, string text)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = new Vector3(-6, posicao);
            Transform characterTransform = Instantiate(pf, position, Quaternion.identity);
            CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
            // characterBattle.Setup(ally1Name);
            //CharacterBattle characterBattle = new CharacterBattle();
            /*            
            characterBattle.sheet = (CharacterSheet)Activator.CreateInstance(Type.GetType(ally1Name));
            characterBattle.sheet.initStats();
            characterBattle.sheet.isAlly = true;
            */
            characterBattle.Setup(text, isPlayerTeam);


            return characterBattle;
        }
        else
        {
            position = new Vector3(6, posicao);
            Transform characterTransform = Instantiate(pf, position, Quaternion.identity);
            CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
            characterBattle.Setup(text, isPlayerTeam);

            //Debug.Log("Enemy " + characterBattle.sheet.str);
            return characterBattle;
        }

        //Instantiate(pfCharacterBattle, position, Quaternion.identity);
    }


    private void CharacterBattle_OnHPChanged(object sender, EventArgs e)
    {
        foreach (CharacterBattle cb in listOfFighters)
        { //Para não deixar cura passar do máximo e nem morte ficar menor que 0
            if (cb.currentHP < 0)
            {
                cb.currentHP = 0;
            }
            if (cb.currentHP > cb.sheet.hp)
            {
                cb.currentHP = cb.sheet.hp;
            }

            if (cb.currentMana < 0)
            {
                cb.currentMana = 0;
            }
            if (cb.currentMana > cb.sheet.mana)
            {
                cb.currentMana = cb.sheet.mana;
            }


        }

        battleDManager.UpdateHealth(this);
        foreach (CharacterBattle cb in listOfFighters)
        {
            if (cb.isDead())
            {
                cb.IsDeadAnimation();
            }
        }

    }

    public string GetAllyActualHP(int allyNumber)
    {
        return (listOfAllies[allyNumber].sheet.name + " HP: " + listOfAllies[allyNumber].currentHP + "/" + listOfAllies[allyNumber].sheet.hp +
        "\n Mana: " + listOfAllies[allyNumber].currentMana + "/" + listOfAllies[allyNumber].sheet.mana
        );
        //return (ally1Name + " AG: " + playerCharacterBattle.actionGauge + "/" + 100);
    }

    /*
        public string GetAlly2ActualHP()
            {
                return (ally2Name + " HP: " + playerCharacterBattle2.currentHP + "/" + playerCharacterBattle2.sheet.hp);
                //return (enemy1Name + " AG: " + enemyCharacterBattle.actionGauge + "/" + 100);
            }
    */
    public string GetEnemiesActualHP()
    {
        return (listOfEnemies[0].name + " HP: " + listOfEnemies[0].currentHP + "/" + listOfEnemies[0].sheet.hp);
        //return (enemy1Name + " AG: " + enemyCharacterBattle.actionGauge + "/" + 100);
    }



    public int GetNumAllies()
    {
        return numAllies;
    }

    public int GetNumEnemies()
    {
        return numEnemies;
    }
}
