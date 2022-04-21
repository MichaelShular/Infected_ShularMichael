using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCTimerController : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    private int numberOfNPCs;
    [SerializeField]
    private float numberOfSecBeforeNextWave;

    [Header("Game's Game Objects")]
    [SerializeField]
    private GameObject gameObjectNPC;
    private List<GameObject> NPCList;
    private List<GameObject> infectedList;
    [SerializeField]
    private TextMeshProUGUI TimerUI;
    [SerializeField] 
    private TextMeshProUGUI numberOfInfectedForGameOverUI;


    // Start is called before the first frame update
    void Start()
    {
        NPCList = new List<GameObject>();
        infectedList = new List<GameObject>();

        for (int i = 0; i < numberOfNPCs - 1; i++)
        {
            GameObject tempNPC = Instantiate(gameObjectNPC);
            NPCList.Add(tempNPC);
        }

        GameObject tempInfected = Instantiate(gameObjectNPC);
        tempInfected.GetComponent<NPCController>().isInfected = true;
        infectedList.Add(tempInfected);

        GetComponent<GameStateController>().numberOfUninfected = NPCList.Count;
        numberOfInfectedForGameOverUI.text = NPCList.Count.ToString();


        StartCoroutine(InfectingCoolDown(numberOfSecBeforeNextWave));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator InfectingCoolDown(float timer)
    {
        float animationTime = 0f;
        TimerUI.text = timer.ToString();
        while (animationTime <   timer)
        {
            timer -= Time.deltaTime;
            float lerpValue =  timer / animationTime;
            TimerUI.text = Mathf.Lerp( 0.0f, timer, lerpValue).ToString("F2");
            yield return null;
        }

        foreach (var NPC in infectedList)
        {
            GameObject tempNpc = NPCList[Random.Range(0, NPCList.Count)];
            NPC.GetComponent<NPCController>().AIAnimator.SetInteger("AnimationState", 1);
            NPC.GetComponent<NPCController>().setTarget(tempNpc);
        }
        StartCoroutine(InfectingCoolDown(numberOfSecBeforeNextWave));
    }

    public void moveToInfectedList(GameObject NPC)
    {
        NPCList.Remove(NPC);
        infectedList.Add(NPC);
        numberOfInfectedForGameOverUI.text = NPCList.Count.ToString();
        GetComponent<GameStateController>().numberOfUninfected = NPCList.Count;
    }

    public void RemoveFromList(GameObject NPC)
    {
        NPCList.Remove(NPC);
    }
}
