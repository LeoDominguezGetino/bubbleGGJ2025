using UnityEngine;
using System.Collections.Generic;
using static DebugController;
using UnityEngine.UIElements;
using System;

public class Pufferfish : MonoBehaviour
{
    [SerializeField] Sprite SpriteDeflate;
    [SerializeField] Sprite SpriteInflate;

    [SerializeField] List<Vector2> positions = new List<Vector2>();
    Vector2 currentPosition;
    Vector2 nextPosition;
    [SerializeField] float speed = 5;
    private bool enableMovement;
    float currentDistance;
    int index;
    Animator animator;
    public enum STATE
    {
        MOVE,
        INFLATION
    }
    STATE currentState;
    float inflatedDuration;
    [SerializeField] float flatedDuration = 2;


    private void Start()
    {
        animator = GetComponent<Animator>();
        if (positions.Count <= 1)
        {
            currentState = STATE.INFLATION;
            enableMovement = false;
            if (positions.Count == 1)
            {
                transform.position = positions[0];
            }
        }
        else
        {
            currentState = STATE.MOVE;
            transform.position = positions[0];
            nextPosition = positions[1];
            index = 1;
            enableMovement = true;
        }
    }

    private void Update()
    {

        if (enableMovement && GameManager.Instance.gameOver == false) 
        {
            moveToAPoint(nextPosition);
            if(inflatedDuration > 0)
            {
                inflatedDuration -= Time.deltaTime;
            }
            if (inflatedDuration < 0) inflatedDuration = 0;
        }

    }

    private void moveToAPoint(Vector2 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        currentDistance = Vector2.Distance(transform.position, destination);
        if (currentDistance == 0)
        {

            switch (currentState)
            {
                

                case STATE.MOVE:
                    currentState = STATE.INFLATION;
                    //this.gameObject.transform.localScale
                    inflatedDuration = flatedDuration;
                    break;

                case STATE.INFLATION:

                    if (inflatedDuration > 0)
                    {

                        this.gameObject.layer = 10;
                        animator.SetBool("inflated", true);
                        //Debug.Log("ESEGUI ANIMAZIONE");
                        //ESEGUI CAMBIO SPRITES PUFFERFISH
                    }
                    else if (inflatedDuration == 0)
                    {
                        this.gameObject.layer = 0;
                        animator.SetBool("inflated", false);
                        //GetComponent<SpriteRenderer>().sprite = SpriteDeflate;
                        index++;
                        if (index == positions.Count)
                        {
                            index = 0;
                        }

                        nextPosition = positions[index];
                        currentState = STATE.MOVE;
                    }
                    break;

            }

            /*
            index++;
            if (index == positions.Count)
            {
                index = 0;
            }

            nextPosition = positions[index];
            */

        }
    }

    
}