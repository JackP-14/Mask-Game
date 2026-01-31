using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    int previousDirection;
    bool walk_decision = false;
    float speed;
    int direction=-1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("walkQuestion", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (walk_decision)
        {
            transform.position += new Vector3(speed * direction, 0f, 0f) * Time.deltaTime;
        }

    }
    void walkQuestion()
    {
        walk_decision = false;
        previousDirection = direction;
        int decider = Random.Range(0, 10);
        if (decider == 5) {
            walk_decision = true;
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