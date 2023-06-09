using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    [SerializeField]
    private LogicType logic;
    private enum LogicType
    {
        Walk,
        Talk,
        Sport,
        Funny
    }

    [SerializeField]
    private int speedMultiplayer;
    [SerializeField]
    private Vector3 positionWalk;
    [SerializeField]
    private Vector3 normalized;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject villager;

    private Animator animator;
    private Vector2 stableVector = new Vector2(0, 1);
    // Start is called before the first frame update
    void Start()
    {
        animator = villager.GetComponent<Animator>();
        positionWalk = transform.transform.position;
        CheckPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(logic)
        {
            case LogicType.Walk:
                {
                    WalkingLogic();
                    break;
                }
            case LogicType.Talk:
                {
                    Talk();
                    break;
                }
            case LogicType.Sport:
                {
                    Sport();
                    break;
                }
            case LogicType.Funny:
                {
                    Funny();
                    break;
                }
        }

    }


    void Talk()
    {
        animator.SetBool("Talk", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Sport", false);
        animator.SetBool("Funny", false);
    }

    void Sport()
    {
        animator.SetBool("Talk", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Sport", true);
        animator.SetBool("Funny", false);
    }

    void Funny()
    {
        animator.SetBool("Talk", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Sport", false);
        animator.SetBool("Funny", true);
    }

    private void WalkingLogic()
    {
        transform.Translate(normalized* Time.deltaTime*speedMultiplayer);
        CheckPosition();
    }

    private void CheckPosition()
    {
        if(Mathf.Abs(positionWalk.x - transform.position.x)<3
            && Mathf.Abs(positionWalk.z - transform.position.z)<3)
        {
            Debug.Log("Select next point");
            target = target.GetComponent<RoadPoint>().GetNextPoint();
            SetNextPoint();
        }
    }

    private void SetNextPoint()
    {
        positionWalk = target.transform.position;
        normalized = Vector3.Normalize(positionWalk - transform.position);
        normalized.y = 0;
        Rotating();
    }

    private void Rotating()
    {
        float angle = Vector3.Angle(normalized, stableVector);
        float ab = normalized.x * stableVector.x + normalized.z * stableVector.y;
        float a = Mathf.Sqrt(normalized.x*normalized.x+normalized.z*normalized.z);
        float b = Mathf.Sqrt(stableVector.x * stableVector.x + stableVector.y * stableVector.y);
        float c = Mathf.Acos(ab / (a * b));
        villager.transform.eulerAngles = new Vector3(0, angle, 0);

        //TODO поворот для каждой точки
    }

    private void TalkingLogic() 
    {

    }

    private void SportLogic()
    {

    }

    private void FunnyLogic()
    {

    }
}
