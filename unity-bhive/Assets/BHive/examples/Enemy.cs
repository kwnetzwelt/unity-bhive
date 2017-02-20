using System;
using System.Collections.Generic;
using UnityEngine;

namespace BHive.Examples
{
    public class Enemy : MonoBehaviour
    {
        public float strength;
        public float randomize; 
        void OnEnable()
        {
            strength += UnityEngine.Random.value * randomize;
            EnemyController.Instance.AddEnemy(this);
        }
        void OnDisable()
        {
            EnemyController.Instance.RemoveEnemy(this);
        }
    }
}
