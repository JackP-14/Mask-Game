using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    SpawnCultistControl spawncultistcontrol;
    [SerializeField] GameObject Cultist;
    int floor;
    float posX0;
    bool walk_decision = false;

    void Awake()
    {
        spawncultistcontrol = Cultist.GetComponent<SpawnCultistControl>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posX0 = transform.position.x;
        float posY0 = transform.position.y;
        floor = spawncultistcontrol.SpawnLvlY;
        InvokeRepeating("walkQuestion", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (walk_decision)
        {
            int direction = Random.Range(-1, 2);
            while (direction == 0)
            {
                direction = Random.Range(-1, 2);
            }
            transform.position += new Vector3(0.1f*direction, 0f, 0f) * Time.deltaTime;
        }
    }
    void walkQuestion ()
    {
        walk_decision = false;
        int decider = Random.Range(0, 2);
        if (decider == 0) {
            walk_decision = true;
        }
    }

    void goforawalk()
    {
        
        if (floor == 0)
        {

        }
        else if (floor == 1)
        {

        }
        else
        {

        }
    }
}
