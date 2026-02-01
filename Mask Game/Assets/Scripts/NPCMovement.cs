using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    int previousDirection;
    bool walk_decision = false, bounce = false, outOfBoundsY = false;
    float speed, initialY, Ymax, Ymin;
    int direction=-1, directionY = -1;
    
    [Header("How many seconds pass between movement checks")]
    public float checktime;
    [Header("Percentage chance of NPCs walking")]
    public int WalkChancePercent;
    [Header("Y axis animation settings")]
    public float Ymaxvariation;
    public float Yspeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("walkQuestion", 2f, checktime);
        initialY = transform.position.y;
        Ymax = initialY + Ymaxvariation;
        Ymin = initialY - Ymaxvariation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < Ymin || transform.position.y > Ymax)
        {
            outOfBoundsY = true;
        }
        if (outOfBoundsY && transform.position.y < initialY)
        {
            transform.position += new Vector3(0f, 0.01f, 0f);
            directionY *= -1;
            outOfBoundsY = false;
        }
        else if (outOfBoundsY && transform.position.y > initialY)
        {
            transform.position += new Vector3(0f, -0.01f, 0f);
            directionY *= -1;
            outOfBoundsY = false;
        }
        if (walk_decision)
        {
            transform.position += new Vector3(speed * direction, 0f, 0f) * Time.deltaTime;
        }
        if (bounce)
        {
            transform.position += new Vector3(0f, Yspeed * directionY, 0f) * Time.deltaTime;
        }
    }
    void walkQuestion()
    {
        
        walk_decision = false;
        bounce = false;
        previousDirection = direction;
        int decider = Random.Range(0, 100);
        if (decider <= WalkChancePercent) {
            walk_decision = true;
            bounce = true;
            speed = Random.Range(0f, 1f);
            direction = Random.Range(-1, 2);
            while (direction == 0)
            {
                direction = Random.Range(-1, 2);
            }
            if (previousDirection != direction)
            {
                swap();
            }
        }
    }
    void swap ()
    {
        transform.Rotate(0, 180, 0);
    }
}