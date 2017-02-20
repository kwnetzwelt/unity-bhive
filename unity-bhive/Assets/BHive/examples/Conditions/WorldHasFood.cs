using UnityEngine;
using System.Collections;

namespace BHive.Examples
{
    public class WorldHasFood : BHiveCondition
    {
        protected override bool Condition()
        {
            return FoodController.Instance.HasFood();
        }
    }
}