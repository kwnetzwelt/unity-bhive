using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace BHive.Examples
{
    public class FoodSpawner : MonoBehaviour
    {
        
        public float spawnFrequency = 5.0f;
        public float spawnRadius = 10;
        public int maxFoodCount = 15;

        public List<Food> food = new List<Food>();
        List<Food> tempFood = new List<Food>();

        public float CompleteFoodValue
        {
            get;
            private set;
        }
             
        float currentTime;

        void OnEnable()
        {
            FoodController.Instance.AddSpawner(this);
        }
        void OnDisable()
        {
            FoodController.Instance.RemoveSpawner(this);
        }

        // Update is called once per frame
        void Update()
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                Spawn();
                currentTime = spawnFrequency;
            }

            // maintainance

            CompleteFoodValue = 0;
            tempFood.Clear();
            foreach (var f in food)
            {
                if (f.foodValue > 0)
                {
                    tempFood.Add(f);
                    CompleteFoodValue += f.foodValue;
                }
            }
            food.Clear();
            food.AddRange(tempFood);


        }

        void Spawn()
        {

            Vector2 r = UnityEngine.Random.insideUnitCircle;
            Vector3 position = new Vector3(r.x, 0, r.y) * spawnRadius + this.transform.position;
            Ray ray = new Ray(position + Vector3.up * spawnRadius, Vector3.down);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, 1000.0f))
            {
                position = ray.GetPoint(hitInfo.distance);
            }

            Food newFood = new Food();
            newFood.foodValue = 50;
            newFood.position = position;
            food.Add(newFood);
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, spawnRadius);
            Gizmos.color = new Color(0,1,0,0.5f);
            Gizmos.DrawCube(this.transform.position
                 + (Vector3.up * CompleteFoodValue/10.0f / 2.0f), new Vector3(1, CompleteFoodValue/10.0f, 1));

            
            foreach(var f in food)
            {
                Gizmos.DrawCube(f.position, Vector3.one);
            }

        }

    }
}