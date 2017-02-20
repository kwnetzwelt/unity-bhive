using UnityEngine;
using System.Collections;

namespace BHive.Examples
{
    [BHiveParameters("enemyTag", "distanceThreshold", "aggressive")]
    public class EnemyNear : BHiveCondition
    {

        protected override bool Condition()
        {
            GameObject enemy = GetClosestEnemy(GetInt("aggressive") == 1);
            if(enemy != null)
            {
                float dist = Vector3.Distance(Target.transform.position, enemy.transform.position);
                if (dist < GetFloat("distanceThreshold"))
                    return true;
            }
            return false;
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