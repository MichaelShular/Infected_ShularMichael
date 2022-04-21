using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public enum NPCState
{
    Waiting,
    Moving,
    Infecting
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


    // Start is called before the first frame update
    void Start()
    {
        gameStateController = GameObject.Find("GameController").GetComponent<GameStateController>();
        movementBoundsArea = GameObject.Find("Floor").GetComponent<BoxCollider>();

        agent = GetComponent<NavMeshAgent>();
        randomizeNextLocation();
        currentState = NPCState.Moving;

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

                checkDistenceBetween();
                agent.SetDestination(nextLocation);

                break;
            case NPCState.Infecting:
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

    private void checkDistenceBetween()
    {
        if(Vector3.Distance(transform.position, nextLocation) < 1)
        {
            currentState = NPCState.Waiting;
            StartCoroutine(waitBeforeMoving());
        }
        
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
