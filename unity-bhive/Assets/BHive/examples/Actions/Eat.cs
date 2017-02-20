using System;
using System.Collections.Generic;
using UnityEngine;
namespace BHive.Examples
{
    /// <summary>
    /// will eat until tummy > tummyLimit
    /// </summary>
	[BHive.BHiveInfo("Eat","Some Info on Eat")]
    [BHive.BHiveParameters( "fedUpThreshold")]
	public class Eat : BHiveAction
	{
		#region implemented abstract members of BHiveNode

		
		protected override IEnumerator<BHiveState> Update()
		{
            float tummy = 0;
            Food targetFood = FoodController.Instance.GetFoodNear(Target.transform.position);
            tummy = Controller.GetFloat("tummy");

            while (tummy < GetFloat("fedUpThreshold"))
            {

                if (targetFood == null || targetFood.foodValue <= 0)
                    break;
                targetFood.foodValue -= Time.deltaTime * 30;
                tummy = Controller.GetFloat("tummy");
                tummy += Time.deltaTime * 30;
                Controller.SetValue("tummy", tummy);
                yield return BHive.BHiveState.Running;
            }
            yield return BHive.BHiveState.Done;
		}
		

        protected override void OnBecomeActive()
        {
        }
        protected override void OnReset()
        {
        }
		#endregion



    }
}

