using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public enum NPCState
{
    Waiting,
    Moving,
    MoveToInfectingTarget,
    DeadState,
    Attacking,
    GettingAttacked
}
public class NPCController : MonoBehaviour, IPointerClickHandler
{
    [Header("GameController")]
    [SerializeField] private GameStateController gameStateController;

    [Header("AI properties")]
    [SerializeField] private BoxCollider movementBoundsArea;
    [SerializeField] private NPCState currentState;

    public bool isInfected;

    [SerializeField] private float minTimeBeforeMove;
    [SerializeField] private float maxTimeBeforeMove;
    private NavMeshAgent agent;
    private Vector3 nextLocation;
    private GameObject targetToInfect;
    private IEnumerator waitForMovingTask;
    private IEnumerator gettingAttackTask;
    private IEnumerator AttackingTask;
    private AudioSource hurtEffect;

    [Header("AI Animation")]
    public Animator AIAnimator;


    // Start is called before the first frame update
    void Start()
    {
        gameStateController = GameObject.Find("GameController").GetComponent<GameStateController>();
        movementBoundsArea = GameObject.Find("Floor").GetComponent<BoxCollider>();

        AIAnimator = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();
        randomizeNextLocation();
        currentState = NPCState.Moving;

        //randomizing start position
        transform.position = new Vector3(Random.Range(movementBoundsArea.bounds.min.x, movementBoundsArea.bounds.max.x), 1, Random.Range(movementBoundsArea.bounds.min.z, movementBoundsArea.bounds.max.z));
        AIAnimator.SetInteger("AnimationState", 1);

        hurtEffect = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        AIStateController();
    }

    public void AIStateController()
    {


        switch (currentState)
        {
            case NPCState.Waiting:



                break;
            case NPCState.Moving:


                checkDistenceBetween(transform.position, nextLocation);
                agent.SetDestination(nextLocation);

                if (isInfected)
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.green;

                }

                break;
            case NPCState.MoveToInfectingTarget:

                gameObject.GetComponent<Renderer>().material.color = Color.red;

                checkDistenceBetween(transform.position, targetToInfect.transform.position);
                agent.SetDestination(targetToInfect.transform.position);


                break;

            case NPCState.DeadState:

                break;
            case NPCState.Attacking:

                break;
            case NPCState.GettingAttacked:

                break;
            default:
                break;
        }
    }

    IEnumerator waitBeforeMoving()
    {
        yield return new WaitForSeconds(Random.Range(minTimeBeforeMove, maxTimeBeforeMove));
        randomizeNextLocation();
        currentState = NPCState.Moving;
        AIAnimator.SetInteger("AnimationState", 1);
    }
    private void randomizeNextLocation()
    {
        nextLocation = new Vector3(Random.Range(movementBoundsArea.bounds.min.x, movementBoundsArea.bounds.max.x), 1, Random.Range(movementBoundsArea.bounds.min.z, movementBoundsArea.bounds.max.z));
    }

    private void checkDistenceBetween(Vector3 a, Vector3 b)
    {
        if (isInfected && currentState == NPCState.MoveToInfectingTarget)
        {
            if (Vector3.Distance(a, b) < 2)
            {
                //Play Animation
                currentState = NPCState.Attacking;
                AIAnimator.SetInteger("AnimationState", 2);
                targetToInfect.GetComponent<NPCController>().startGettingAttacked();

                StartCoroutine(waitForAttackToPlay(2.6f));

            }
            return;
        }

        if (Vector3.Distance(a, b) < 1)
        {
            currentState = NPCState.Waiting;
            waitForMovingTask = waitBeforeMoving();
            StartCoroutine(waitForMovingTask);
            AIAnimator.SetInteger("AnimationState", 0);

        }

    }

    public void setTarget(GameObject target)
    {
        if (currentState == NPCState.MoveToInfectingTarget || currentState == NPCState.Attacking || currentState == NPCState.GettingAttacked) return;
        if (waitForMovingTask != null)
        {
            StopCoroutine(waitForMovingTask);
        }


        currentState = NPCState.MoveToInfectingTarget;
        targetToInfect = target;
    }


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (isInfected)
        {

            gameStateController.changeNumberOfInfected(-1);

        }
        else
        {

        }
        agent.SetDestination(transform.position);
        currentState = NPCState.DeadState;
        GameObject.Find("GameController").GetComponent<NPCTimerController>().RemoveFromList(this.gameObject);

        AIAnimator.SetInteger("AnimationState", 3);


        //Destroy(this.gameObject);
    }

    public void settingToInfected()
    {
        if (isInfected) return;
        
        isInfected = true;
        GameObject.Find("GameController").GetComponent<NPCTimerController>().moveToInfectedList(this.gameObject);
        gameObject.GetComponent<Renderer>().material.color = Color.blue;

        
            gameStateController.changeNumberOfInfected(1);
        

    }

    public void startGettingAttacked()
    {
        if (waitForMovingTask != null)
        {
            StopCoroutine(waitForMovingTask);
        }
        hurtEffect.Play();
        currentState = NPCState.GettingAttacked;
        AIAnimator.SetInteger("AnimationState", 4);
        agent.SetDestination(transform.position);

        StartCoroutine(waitForGettingAttackedToPlay(2.6f));
    }

    IEnumerator waitForAttackToPlay(float timeToPlay)
    {
        yield return new WaitForSeconds(timeToPlay);

        targetToInfect.GetComponent<NPCController>().settingToInfected();
        currentState = NPCState.Moving;
        AIAnimator.SetInteger("AnimationState", 1);
    }

    IEnumerator waitForGettingAttackedToPlay(float timeToPlay)
    {


        yield return new WaitForSeconds(timeToPlay);


        currentState = NPCState.Moving;
        AIAnimator.SetInteger("AnimationState", 1);
    }

}
