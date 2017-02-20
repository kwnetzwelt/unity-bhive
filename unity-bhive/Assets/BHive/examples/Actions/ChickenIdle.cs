using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BHive.Examples
{
    [BHiveParameters("tummyThreshold")]
    public class ChickenIdle : BHiveAction
    {
        Vector3 targetPos;
        AIWalker character;
        protected override void OnBecomeActive()
        {
            character = Target.GetComponent<AIWalker>();
            targetPos = new Vector3( UnityEngine.Random.value, 0, UnityEngine.Random.value);
            targetPos *= 5;
            //Ray r = new Ray(targetPos + Vector3.up * 5000, Vector3.down);

        }

        protected override void OnReset()
        {

        }

        protected override IEnumerator<BHiveState> Update()
        {
            float timeElapsed = 0;
            while (timeElapsed < 1)
            {
                timeElapsed += Time.deltaTime;
                var tummy = Controller.GetFloat("tummy");
                tummy -= Time.deltaTime * 20;
                Controller.SetValue("tummy", tummy);
                character.WalkTo(targetPos, false);
                yield return BHiveState.Running;
            }
            yield return BHiveState.Done;
        }
    }
}
