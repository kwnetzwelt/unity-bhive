using System;
using System.Collections.Generic;
using UnityEngine;


namespace BHive.Examples
{
    [BHiveParameters("foodDistance")]
    public class GoToFood : BHiveAction
    {

        AIWalker character;

        int retard = 0;

        protected override IEnumerator<BHiveState> Update()
        {
            Food targetFood = null;
            while(true)
            {
                if (retard > 3)
                {
                    targetFood = FoodController.Instance.GetFoodNear(Target.transform.position);
                    retard = 0;
                }
                retard++;
                if (targetFood == null)
                    break;
                if (targetFood.foodValue < 0)
                    break;
                if (Distance(targetFood.position) <= GetFloat("foodDistance"))
                    break;

                character.WalkTo(targetFood.position, false);
                    
                    
                yield return BHiveState.Running;
            }
            character.StopWalking();
            yield return BHiveState.Done;
        }

        private float Distance(Vector3 position)
        {
            return Vector3.Distance(Target.transform.position, position);

        }


        protected override void OnBecomeActive()
        {
            character = Target.GetComponent<AIWalker>();
        }
        protected override void OnReset()
        {
        }
    }
}
