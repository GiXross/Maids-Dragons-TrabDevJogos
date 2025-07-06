using UnityEngine;
using System;
using Random = System.Random;
public class CharacterBattle : MonoBehaviour
{

  public CharacterSheet sheet;
  public int currentHP;
  public int actionGauge;
  public bool isVulnerable;

  private GameObject highlightGameObject;

  private GameObject targetGameObject;

  public Random random;
  private State state;
  private Vector3 slideTargetPosition;

  private Action onSlideComplete;
  private Action onSkillComplete;
  private Action onAnimationComplete;
  

  private Animator animator;
  private Animator healAnimator;
  public Transform healTransform;
  private GameObject healObject;

  public event EventHandler OnHPChanged;


  //Auxiliares
  public double damage;
  public int auxRandVal;
  public double auxAttackVal;


  //private CharacterSheet enemy1;

  //[SerializeField] private CharacterSheet test;

  //private Character_Base characterBase;

  // Update is called once per frame

  private float NTime;
  private AnimatorStateInfo animInfo;
  private enum State
  {
    Idle,
    Sliding,
    Busy,
    Animating,
    AnimatingHeal,
  }



  private void Awake()
  {
    random = new Random();
    state = State.Idle;
    animator = GetComponent<Animator>();
    if(healTransform != null){
      //Debug.Log("FUCK");
      Transform transformH = Instantiate(healTransform, GetPosition(), Quaternion.identity);
      healObject = transformH.gameObject;
      healObject.SetActive(false);
      healAnimator = healObject.GetComponent<Animator>();            
    }
    actionGauge = 0;
  }


  private void Update()
  {
    switch (state)
    {
      case State.Idle:
        break;
      case State.Busy:
        break;
      case State.Sliding:
        float slideSpeed = 1f;
        transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

        float reachedDistance = 1f;
        if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
        {
          // Arrived at Slide Target Position
          //transform.position = slideTargetPosition;
          onSlideComplete();
        }
        break;
      case State.Animating:
        NTime =  animator.GetCurrentAnimatorStateInfo(0).normalizedTime; //Pega o tempo normalizado da animacao
                                         //Que resumidamente é um loop dos frames da animacao
        
        if (NTime > 1.0f)
        {
          animator.SetInteger("SkillNumber", 0);
          onAnimationComplete();
        }

        break;
      case State.AnimatingHeal:
          NTime =  healAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime; //Pega o tempo normalizado da animacao
                                         //Que resumidamente é um loop dos frames da animacao
        if (NTime > 1.5f)
        {
          healObject.SetActive(false);
          onAnimationComplete();
        }
        break;
    }
  }






  private void Start()
  {

    /*
    sheet = (CharacterSheet)Activator.CreateInstance(Type.GetType("FaustLeft"));
    sheet.InitStats();
    sheet.isAlly = true;
    */
    /*
    enemy1 = (CharacterSheet)Activator.CreateInstance(Type.GetType("FaustRight"));
    enemy1.InitStats();
    enemy1.isAlly = false;
    */
    //Debug.Log("Ally " + ally1.str);
    //Debug.Log("Enemy " + enemy1.str);
    //  characterBase.PlayAnimMove(new Vector3(1,0));
  }
  public Vector3 GetPosition()
  {
    return transform.position;
  }

  public void Setup(string name, bool isAlly)
  {
    sheet = (CharacterSheet)Activator.CreateInstance(Type.GetType(name));
    sheet.InitStats();
    sheet.isAlly = isAlly;
    currentHP = sheet.hp;
    highlightGameObject = transform.Find("Highlight").gameObject;
    targetGameObject = transform.Find("Target").gameObject;
    HideHighlight();
    HideTarget();
    this.isVulnerable = false;
  }


  public void AttackDamage(CharacterBattle target)
  {
    if (sheet.str >= sheet.agi)
    { //golpe baseado em str
      if (target.isVulnerable) // checa se está em fraqueza
      {
        auxRandVal = sheet.str; // ideia seria fazer ficar 2*str
      }
      else
      {
        auxRandVal = random.Next(sheet.str + 1); //Inclui minimo valor e exclui o maior
      }

      auxAttackVal = sheet.str / 5;
      damage = (20 + (10 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
      target.currentHP -= (int)damage;
      Debug.Log("Total Damage: " + damage);
      Debug.Log("Random Val: " + auxRandVal);
    }
    else
    {  //golpe baseado em agi

      if (target.isVulnerable)
      {
        auxRandVal = sheet.agi;
      }
      else
      {
        auxRandVal = random.Next(sheet.agi + 1); //Inclui minimo valor e exclui o maior
      }

      auxAttackVal = sheet.agi / 5;
      damage = (20 + (5 * (Math.Truncate(auxAttackVal))) + (auxRandVal));
      target.currentHP -= (int)damage;
      Debug.Log("Total Damage: " + damage);
      Debug.Log("Random Val: " + auxRandVal);
    }

  }

  /*
    public void Attack(CharacterBattle target)
    {
      //Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 10f;
      //Vector3 startingPosition = GetPosition();


      // Slide to Target
      //SlideToPosition(slideTargetPosition);

      // Arrived at Target, attack him

      Vector3 attackDir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
      Debug.Log(attackDir);
      this.Damage(target);
      // Attack completed, slide back
      SlideToPosition(startingPosition, () => {
      // Slide back completed, back to idle
      //state = State.Idle;
      //characterBase.PlayAnimIdle(attackDir);
      onAttackComplete();
      });
    }
  */
  public void Attack(CharacterBattle target, Action onAttackComplete)
  {
    //Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 10f;
    //Vector3 startingPosition = GetPosition();
    this.HideHighlight();
    Vector3 slideTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 10f;

    Vector3 startingPosition = GetPosition();

    Vector3 attackDir = (target.GetPosition() - GetPosition()).normalized;

    //Debug.Log(attackDir);
    SlideToPosition(target.GetPosition(), () =>
    {//Primeira func anon
      state = State.Busy;

      PlayAnimation(1, () =>
      {//Segunda func anon
        this.AttackDamage(target);
        if (OnHPChanged != null) OnHPChanged(this, EventArgs.Empty); // Trocar depois
        SlideToPosition(startingPosition - attackDir,  //Depois penso em uma forma de não depender do Attack Dir
          () =>
          {//Terceira func anon
            state = State.Idle;
            onAttackComplete();
          }
            );
      });
    }
    );
  }

  public void Skill(CharacterBattle target, int skillNumber, Action onSkillComplete){ 
      this.onSkillComplete = onSkillComplete;

      if(this.sheet.GetSkillDamage(target, skillNumber) >= 0){
      SkillAttack(target, skillNumber); 
      }else{
        SkillHeal(target, skillNumber);
      }

  }


  public void SkillAttack(CharacterBattle target, int skillNumber)
  { //por default a animação 0 é iddle, 1 é attack. 
    //animator.runtimeAnimatorController.animationClips.Lenght; retorna número de animações não vazias
    this.HideHighlight();
    Vector3 slideTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 10f;

    Vector3 startingPosition = GetPosition();

    Vector3 attackDir = (target.GetPosition() - GetPosition()).normalized;

    //Debug.Log(attackDir);
    //Tem que mudar
    SlideToPosition(target.GetPosition(), () =>
    {//Primeira func anon
      state = State.Busy;

      PlayAnimation(skillNumber, () =>
      {//Segunda func anon
        target.currentHP -= this.sheet.GetSkillDamage(target, skillNumber);
        if (OnHPChanged != null) OnHPChanged(this, EventArgs.Empty); // Trocar depois
        SlideToPosition(startingPosition - attackDir,//Depois penso em uma forma de não depender do Attack Dir
          () =>
          {//Terceira func anon
            state = State.Idle;
            onSkillComplete();
          }
            );
      });
    }
    );
  }

public void SkillHeal(CharacterBattle target, int skillNumber)
 { //por default a animação 0 é iddle, 1 é attack. 
    //animator.runtimeAnimatorController.animationClips.Lenght; retorna número de animações não vazias
    this.HideHighlight();
    Vector3 targetPosition = target.GetPosition();

    Vector3 startingPosition = GetPosition();

    Vector3 attackDir = (target.GetPosition() - GetPosition()).normalized;

    //Debug.Log(attackDir);
    //FIXME: Não sei por que sem dar o Slide SEM ANDAR NADA, o normalized time não zera
    //Imagino que seja porque sem as trocas de contexto que o Slide to Position tras
    //O Unity não tem tempo de atualizar a informação do normalized time a tempo
    SlideToPosition(GetPosition(), () =>
    {//Primeira func anon
      state = State.Busy;

      PlayAnimation(skillNumber, () =>
      {//Segunda func anon
        target.currentHP -= this.sheet.GetSkillDamage(target, skillNumber);
        if (OnHPChanged != null) OnHPChanged(this, EventArgs.Empty); // Trocar depois
        
        PlayHealAnimation(targetPosition,() =>
      {
        SlideToPosition(startingPosition - attackDir,//Depois penso em uma forma de não depender do Attack Dir
          () =>
          {//Terceira func anon
            state = State.Idle;
            onSkillComplete();
          }
            );});


      });
    }
    );
  }
  


  private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
  {
    this.slideTargetPosition = slideTargetPosition;
    this.onSlideComplete = onSlideComplete;
    state = State.Sliding;
  }

  public void IsDeadAnimation()
  {
    animator.SetBool("isDead", this.isDead());
  }

  private void PlayAnimation(int animationIndex, Action onAnimationComplete)
  {
    this.onAnimationComplete = onAnimationComplete;
    //Debug.Log("Indice" + animationIndex);
    animator.SetInteger("SkillNumber", animationIndex);
    animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
    //animator.Play("SkeletonAttackAnimation");

    state = State.Animating;
  }


  private void PlayHealAnimation(Vector3 HealTargetPosition, Action onAnimationComplete)
  {
    this.onAnimationComplete = onAnimationComplete;
    healObject.transform.position = HealTargetPosition;
    healObject.SetActive(true);
    //Debug.Log("Indice" + animationIndex);
    //animator.SetInteger("SkillNumber", animationIndex);
    //animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
    healAnimator.Play("HealAnimation");
    
    //animator.Play("SkeletonAttackAnimation");

    state = State.AnimatingHeal;
  }



  public bool isDead()
  {
    if (this.currentHP <= 0)
    {
      return true;
    }
    else
    {
      return false;
    }

  }

  public void HideHighlight()
  {
    highlightGameObject.SetActive(false);
  }

  public void ShowHighlight()
  {
    highlightGameObject.SetActive(true);
  }
  
   public void HideTarget()
  {
    targetGameObject.SetActive(false);
  }
    
    public void ShowTarget()
  {
    targetGameObject.SetActive(true);
  }
  /*
      private void SlideToPosition(Vector3 slideTargetPosition)
    {

      if (!(state == State.Busy))
      {
        this.slideTargetPosition = slideTargetPosition;
        state = State.Sliding;
        if (slideTargetPosition.x > 0)
        {
          Debug.Log("Sliding");
        }
        else
        {
          Debug.Log("Slidng2");
        }
      }
    }
  */



}
