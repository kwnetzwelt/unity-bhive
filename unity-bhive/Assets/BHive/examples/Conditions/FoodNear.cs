using UnityEngine;
using System.Collections;
namespace BHive.Examples
{
    [BHiveParameters("distance")]
    public class FoodNear : BHiveCondition
    {

        protected override bool Condition()
        {
            var food = FoodController.Instance.GetFoodNear(Target.transform.position);
            if (food == null)
                return false;
            var dist = Vector3.Magnitude(food.position - Target.transform.position);
            return (dist < GetFloat("distance"));

        }
    }
}