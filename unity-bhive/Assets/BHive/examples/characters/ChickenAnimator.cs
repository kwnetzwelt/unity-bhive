using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChickenAnimator : MonoBehaviour {

    Animator mAnimator;
    CharacterController mCharacter;
    BHive.BHiveRuntimeController mBhiveController;
    int speedId;
    int eatId;
    System.Type eatNodeType;
    Vector3 lastPos;

    List<BHive.BHiveAction> listeningNodes = new List<BHive.BHiveAction>();
    void OnEnable()
    {
        eatNodeType = typeof(BHive.Examples.Eat);
        mAnimator = GetComponent<Animator>();
        mCharacter = GetComponent<CharacterController>();
        mBhiveController = GetComponent<BHive.BHiveRuntimeController>();
        speedId = Animator.StringToHash("speed");
        eatId = Animator.StringToHash("eat");
    }

    void Start()
    {
        foreach(var n in mBhiveController.configController.AllNodes)
        {
            if (n.Value is BHive.BHiveAction && n.Value.Name == eatNodeType.Name)
            {
                var action = n.Value as BHive.BHiveAction;
                listeningNodes.Add(action);
                action.BecomeActive += action_BecomeActive;
                action.WillBeReset += action_WillBeReset;
            }
        }
        lastPos = this.transform.position;
    }

    void action_WillBeReset(BHive.BHiveAction obj)
    {
        mAnimator.SetBool(eatId, false);
    }

    void action_BecomeActive(BHive.BHiveAction obj)
    {
        mAnimator.SetBool(eatId, true);
    }

    void OnDisable()
    {
        foreach(var n in listeningNodes)
        {
            if(n != null)
            {
                n.WillBeReset -= action_WillBeReset;
                n.BecomeActive -= action_BecomeActive;
            }
        }
    }


    
    void Update()
    {
        Vector3 newPos = this.transform.position;
        if(lastPos != newPos)
        { 
            mAnimator.SetFloat(speedId, (newPos - lastPos).magnitude);
            lastPos = newPos;

        }else
        {
            mAnimator.SetFloat(speedId, 0);
        }
    }
}
