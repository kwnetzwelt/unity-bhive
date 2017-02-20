using UnityEngine;
using System.Collections;

namespace BHive.Examples
{
    [BHiveParameters("Value", "ComparedValueName")]
    public class GreaterThan : BHiveCondition
    {

        protected override bool Condition()
        {
            float value = GetFloat("Value");
            string comparedName = GetString("ComparedValueName");
            float compared = Controller.GetFloat(comparedName);
            return compared > value;
        }
    }
}