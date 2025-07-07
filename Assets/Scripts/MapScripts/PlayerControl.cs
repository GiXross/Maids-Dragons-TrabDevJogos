using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D controller;
    private Animator animator;
    private Vector2 moveInput;
    private InputAction moveAction;
    public float movespeed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool triggerFlag;
    public bool isTriggeredFlag;
    public bool isPlayer = true;

    private GameObject interactedObject;
    private Collider2D triggerCollider;


    void Awake()
    {
        if (NewGame.lastSceneCoord != null)
        {
            this.transform.position = (Vector2)NewGame.lastSceneCoord; //feio pensar em como mudar
            NewGame.lastSceneCoord = null;
        }

    }


    void Start()
    {
        //settando as flags
        this.triggerFlag = false;
        this.triggerCollider = transform.Find("TriggerCollider").GetComponent<Collider2D>();
        //this.isTriggeredFlag = false;
        controller = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");

        moveAction.Enable();
        moveAction.performed += OnMove; //definindo quando o handler age
        moveAction.canceled += OnMove;

    }

    // Update is called once per frame
    void Update()
    {
        //if (!isTriggeredFlag)
        //{
        Vector2 move = moveAction.ReadValue<Vector2>();//new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                                                       //controller.Move(move * movespeed * Time.deltaTime);//move nas quatro direções
        controller.linearVelocity = (move * movespeed); //Não precisa do delta time porque linearVelocity já leva em conta.
                                                        //  //* Time.deltaTime);//move nas quatro direções
                                                        // this.Move(moveAction);
                                                        //}

        if (triggerFlag)
        {
            this.Interact();

        }

    }

    void OnDisable()//evita memory leak dos event handlers
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        moveAction.Disable();
    }


    /*
        public void Move(Vector2 move)
         {

            // Debug.Log(move.x);

            if ((move.x != 0) || (move.y != 0))
            {
                animator.SetBool("isWalking", true);
                animator.SetFloat("LastInputX", move.x);
                animator.SetFloat("LastInputY", move.y);

            }
            else
            {
                animator.SetBool("isWalking", false);

            }

             //moveInput = context.ReadValue<Vector2>();
             animator.SetFloat("InputX", move.x);
             animator.SetFloat("InputY", move.y);

          }
          */


    public void OnMove(InputAction.CallbackContext context)
    {

        animator.SetBool("isWalking", true);


        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        RotateCollisor(moveInput);
    }


    public void RotateCollisor(Vector2 moveInput)
    {
        if (moveInput.x == 1) // Fiz feio, melhorar depois, mas basicamente faz com que o colisor rode
        {
            this.triggerCollider.transform.rotation = Quaternion.Euler(0, 0, -90);

        }
        else
        {
            if (moveInput.x == -1)
            {
                this.triggerCollider.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                if (moveInput.y == 1)
                {
                    this.triggerCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    if (moveInput.y == -1)
                    {
                        this.triggerCollider.transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                }

            }
        }

    }

    public void Interact()
    {
        if (Input.GetAxisRaw("Interact") != 0)
        {
            //this.isTriggeredFlag = true;
            //this.moveAction.Disable();
            interactedObject.SendMessage("Trigger", this, SendMessageOptions.DontRequireReceiver);
        }
    }


    public void setCoordinates(Vector2 vector2)
    {
       this.transform.position = vector2;

    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        Debug.Log("Pika");
        if (Input.GetAxisRaw("Interact") != 0)
        {
            Debug.Log("Chu");
            GameObject interactedObject = collision.gameObject;
            interactedObject.SendMessage("Trigger", SendMessageOptions.DontRequireReceiver);
        }*/
        //usar depois para eventos de contato e efeito sonoro de bater
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        this.triggerFlag = true;
        Debug.Log("I'm here!");
        this.interactedObject = collision.gameObject;
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        this.triggerFlag = false;
    }




    /*
    public void DialogueFinish()
    {
        this.isTriggeredFlag = false;
        moveAction.Enable();
        moveAction.performed += OnMove; //definindo quando o handler age
        moveAction.canceled += OnMove;
    }*/

}
