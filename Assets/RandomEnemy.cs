using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedPackage
{
    public class RandomEnemy : MonoBehaviour
    {
        private CoordinatesController coordinatesController;
        // Use this for initialization
        void Start()
        {
            coordinatesController = GetComponent<CoordinatesController>();
            List<Vector2> points = coordinatesController.points;
            foreach (Vector2 point in points)
            {
                //GameObject newInstance = Instantiate(Resources.Load(""),);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
