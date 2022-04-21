using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public enum NPCState
{
    Waiting,
    Moving,
    MoveToInfectingTarget
}
public class NPCController : MonoBehaviour, IPointerClickHandler
{
    [Header("GameController")]
    [SerializeField] private GameStateController gameStateController;

    [Header("AI properties") ]
    [SerializeField] private BoxCollider movementBoundsArea;
    [SerializeField] private NPCState currentState;

    public bool isInfected;

    [SerializeField] private float minTimeBeforeMove;
    [SerializeField] private float maxTimeBeforeMove;
    private NavMeshAgent agent;
    private Vector3 nextLocation;
    private GameObject targetToInfect;

    // Start is called before the first frame update
    void Start()
    {
        gameStateController = GameObject.Find("GameController").GetComponent<GameStateController>();
        movementBoundsArea = GameObject.Find("Floor").GetComponent<BoxCollider>();

        agent = GetComponent<NavMeshAgent>();
        randomizeNextLocation();
        currentState = NPCState.Moving;

        //randomizing start position
        transform.position = new Vector3(Random.Range(movementBoundsArea.bounds.min.x, movementBoundsArea.bounds.max.x), 1, Random.Range(movementBoundsArea.bounds.min.z, movementBoundsArea.bounds.max.z));
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

                break;
            case NPCState.MoveToInfectingTarget:

                gameObject.GetComponent<Renderer>().material.color = Color.red; 

                checkDistenceBetween(transform.position, targetToInfect.transform.position);
                agent.SetDestination(targetToInfect.transform.position);


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
    }
    private void randomizeNextLocation()
    {
        nextLocation = new Vector3(Random.Range(movementBoundsArea.bounds.min.x, movementBoundsArea.bounds.max.x), 1, Random.Range(movementBoundsArea.bounds.min.z, movementBoundsArea.bounds.max.z));
    }

    private void checkDistenceBetween(Vector3 a, Vector3 b)
    {
        if(Vector3.Distance(a, b) < 1)
        {
            currentState = NPCState.Waiting;
            StartCoroutine(waitBeforeMoving());
        }
        
    }

    public void setTarget(GameObject target)
    {
        currentState = NPCState.MoveToInfectingTarget;
        targetToInfect = target;
    }


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (isInfected)
        {
            
            gameStateController.changeNumberOfInfected(-1);
            
        }
        Destroy(this.gameObject);
    }
 

}
