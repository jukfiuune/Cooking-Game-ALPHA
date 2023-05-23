using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TrumpetLegsScript : MonoBehaviour
{
    GameObject Player;
    public GameObject Nest;
    public AIPath AI;

    float honkTimerMax = 0.5f;
    float honkTimerCurrent;

    float wanderTimerMax = 8f;
    float wanderTimerMin = 4f;
    float wanderTimer;
    bool canWander = true;

    float sleepTimerMax = 180f;
    float sleepTimerMin = 60f;
    float sleepTimer;
    float sleepChance = 5f;

    public float fleeMaxDist;
    public float wanderMaxDist;
    public float wanderToChaseDist;
    public float returnToNestDist;
    
    public float GiveUpDist = 20f;

    GameObject soundPlayer;
    AudioClip honkSound;
    AudioClip chkSound;

    float x;
    float y;

    public enum State
    {
        Chase,
        Honk,
        Flee,
        Wander,
        Sleep
    }

    public State currentState;
    // Start is called before the first frame update
    void Start()
    {
        honkSound = Resources.Load<AudioClip>("Sounds/Honk");
        chkSound = Resources.Load<AudioClip>("Sounds/Chk");
        soundPlayer = Resources.Load<GameObject>("Prefabs/SoundEffect");
        Player = GameObject.Find("Body");
        AI = GetComponent<AIPath>();
        ChangeState(State.Chase);
    }

    // Update is called once per frame
    void Update()
    {

        // * ------------- * Chase * ------------- * //
        if (currentState == State.Chase)
        {
            AI.destination = Player.transform.position;
            if (AI.reachedDestination)
            {
                ChangeState(State.Honk);
            }
            if (Vector3.Distance(Player.transform.position, transform.position) > GiveUpDist)
            {
                ChangeState(State.Wander);
            }
        }
        // * ------------- * Honk * ------------- * //
        else if (currentState == State.Honk)
        {
            honkTimerCurrent -= Time.deltaTime;
            if (honkTimerCurrent <= 0)
            {
                ChangeState(State.Flee);
            }
        }
        // * ------------- * Flee * ------------- * //
        else if (currentState == State.Flee)
        {
            if (AI.reachedDestination)
            {
                ChangeState(State.Chase);
            }
        }
        // * ------------- * Wander * ------------- * //
        else if (currentState == State.Wander)
        {
           
            if (AI.reachedDestination)
            {
                if (canWander)
                {
                    if (Random.Range(0, 99) < sleepChance)
                    {
                        //Sleep
                        ChangeState(State.Sleep);
                    }
                    else
                    {
                        //Wander code
                        wanderTimer = Random.Range(wanderTimerMin, wanderTimerMax);
                        canWander = false;

                        if (Vector3.Distance(Nest.transform.position, transform.position) < returnToNestDist)
                        {
                            RandomizeXY(wanderMaxDist);
                            AI.destination = new Vector3(x, y, 0) + transform.position;
                        }
                        else
                        {
                            AI.destination = Nest.transform.position;
                        }
                    }
                }
                else
                {
                    wanderTimer -= Time.deltaTime;
                    if (wanderTimer <= 0f)
                    {
                        canWander = true;
                    }
                }
            }    
            if (Vector3.Distance(Player.transform.position, transform.position) < wanderToChaseDist)
            {
                ChangeState(State.Chase);
            }
        }
        // * ------------- * Sleep * ------------- * //
        else if (currentState == State.Sleep)
        {
            sleepTimer -= Time.deltaTime;
            if (sleepTimer <= 0f)
            {
                ChangeState(State.Wander);
            }
        }
    }
    void ChangeState(State _state)
    {
        // Return if the state won't change
        if (_state == currentState)
        {
            return;
        }

        currentState = _state;
        if (_state == State.Chase)
        {
            AI.destination = Player.transform.position;
        }
        else if (_state == State.Honk)
        {
            SoundPlayer sm = Instantiate(soundPlayer, transform).GetComponent<SoundPlayer>();
//            Debug.Log(sm.transform.position);
            sm.aClip = honkSound;
            sm.clipTime = 1f;
            sm.useDist = true;
            honkTimerCurrent = honkTimerMax;
        }
        else if (_state == State.Flee)
        {
            RandomizeXY(fleeMaxDist);
            AI.destination = new Vector3(x, y, 0) + transform.position;
        }
        else if (_state == State.Wander)
        {
            RandomizeXY(wanderMaxDist);
            AI.destination = new Vector3(x, y, 0);
        }
        else if (_state == State.Sleep)
        {
            AI.destination = Nest.transform.position;
            sleepTimer = Random.Range(sleepTimerMin, sleepTimerMax);
        }
    }

    void RandomizeXY(float maxValue)
    {
        x = Random.Range(0, maxValue);
        if (x > 2f / 3f * maxValue)
        {
            y = Random.Range(0, maxValue);
        }
        else
        {
            y = Random.Range(2f/3f * maxValue, maxValue);
        }

        if (Random.value * 2 > 1) x = -x;
        if (Random.value * 2 > 1) y = -y;
    }
}
