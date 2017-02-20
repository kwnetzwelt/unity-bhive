using BHive.Helpers;
using System;
using System.Collections.Generic;

namespace BHive.Examples
{
    public class EnemyController : Singleton<EnemyController>
    {
        List<Enemy> enemyList = new List<Enemy>();

        public void AddEnemy(Enemy pEnemy)
        {
            enemyList.Add(pEnemy);
        }
        
        public void RemoveEnemy(Enemy pEnemy)
        {
            enemyList.Remove(pEnemy);
        }

        public IEnumerable<Enemy> AllEnemiesStrongerThan(Enemy pEnemy)
        {
            foreach(var e in enemyList)
            {
                if(e.strength > pEnemy.strength)
                    yield return e;
            }

            yield break;
            
        }
        public IEnumerable<Enemy> AllEnemiesWeakerThan(Enemy pEnemy)
        {
            foreach (var e in enemyList)
            {
                if (e.strength < pEnemy.strength)
                    yield return e;
            }

            yield break;

        }
    }
}
