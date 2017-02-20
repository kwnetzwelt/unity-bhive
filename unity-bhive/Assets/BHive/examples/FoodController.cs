using BHive.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BHive.Examples
{
    public class FoodController : Singleton<FoodController>
    {
        List<FoodSpawner> foodSpawners = new List<FoodSpawner>();

        public void AddSpawner(FoodSpawner spawner)
        {
            foodSpawners.Add(spawner);
        }

        public void RemoveSpawner(FoodSpawner spawner)
        {
            foodSpawners.Remove(spawner);
        }

        public Food GetFoodNear(Vector3 position)
        {
            // find closest food spawner

            float mostAttractive = float.MinValue;
            FoodSpawner bestFit = null;
            foreach(var sp in foodSpawners)
            {
                float attr = -(sp.transform.position - position).sqrMagnitude;
                attr += sp.CompleteFoodValue; // spawners with more food are more attractive (to a degree)
                
                if(attr > mostAttractive)
                {
                    bestFit = sp;
                    mostAttractive = attr;
                }
            }

            if (bestFit == null)
                return null;

            mostAttractive = float.MaxValue;
            Food bestFood = null;
            foreach(var f in bestFit.food)
            {
                float attr = Vector3.Distance(f.position, position);
                if (attr < mostAttractive)
                {
                    mostAttractive = attr;
                    bestFood = f;
                }
            }

            return bestFood;

        }

        public bool HasFood()
        {
            foreach (var sp in foodSpawners)
            {
                if (sp.CompleteFoodValue > 0)
                    return true;
            }
            return false;
        }
    }
}
