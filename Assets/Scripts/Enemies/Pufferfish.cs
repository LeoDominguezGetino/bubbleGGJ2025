using UnityEngine;
using System.Collections.Generic;
using static DebugController;
using UnityEngine.UIElements;
using System;

public class Pufferfish : MonoBehaviour
{

    [SerializeField] List<Vector2> positions = new List<Vector2>();
    Vector2 currentPosition;
    Vector2 nextPosition;
    [SerializeField] float speed = 5;
    private bool enableMovement;
    float currentDistance;
    int index;



    private void Start()
    {
        if (positions.Count <= 1)
        {
            enableMovement = false;
            if (positions.Count == 1)
            {
                transform.position = positions[0];
            }
        }
        else
        {

            transform.position = positions[0];
            nextPosition = positions[1];
            index = 1;
            enableMovement = true;
        }
    }

    private void Update()
    {

        if (enableMovement)
        {
            moveToAPoint(nextPosition);
        }

    }

    private void moveToAPoint(Vector2 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        currentDistance = Vector2.Distance(transform.position, destination);
        if (currentDistance == 0)
        {
            index++;
            if (index == positions.Count)
            {
                index = 0;
            }

            nextPosition = positions[index];

        }
    }
}