using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BHive.Examples
{
    [BHiveParameters("enemyTag","duration")]
    public class RunAway : BHiveAction
    {
        GameObject enemy
        {
            get;
            set;

        }
        AIWalker character;

        protected override IEnumerator<BHiveState> Update()
        {
            float elapsedTime = 0;
            while (elapsedTime < GetFloat("duration"))
            {
                if (enemy == null)
                    break;

                Vector3 targetDirection = -(enemy.gameObject.transform.position - Target.transform.position);
                character.WalkTo(Target.transform.position + targetDirection.normalized, true);
                
                elapsedTime += Time.deltaTime;

                yield return BHiveState.Running;
            }
            character.StopWalking();

            yield return BHiveState.Done;
        }
        protected override void OnReset()
        {
            
        }

        private float EnemyDistance()
        {
            float distance = Vector3.Distance(enemy.transform.position, Target.transform.position);
            return distance;
        }
        protected override void OnBecomeActive()
        {
            character = Target.GetComponent<AIWalker>();
            enemy = GetClosestEnemy(false);
        }

        protected GameObject GetClosestEnemy(bool aggressive)
        {
            Enemy me = Target.GetComponent<Enemy>();

            var enemies = aggressive ? EnemyController.Instance.AllEnemiesWeakerThan(me) : EnemyController.Instance.AllEnemiesStrongerThan(me);

            GameObject closest = null;
            float distance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                float nDist = Vector3.SqrMagnitude(Target.transform.position - enemy.transform.position);
                if (nDist < distance)
                {
                    distance = nDist;
                    closest = enemy.gameObject;
                }
            }

            return closest;
        }
    }
}
