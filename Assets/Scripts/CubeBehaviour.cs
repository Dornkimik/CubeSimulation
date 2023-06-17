using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    // General
    [SerializeField] private string gender;
    [SerializeField] private int attraction;
    [Range(1f, 5f)] public float visionRange;
    [SerializeField] private float health;
    [SerializeField] private float thirst;
    [SerializeField] private float thirstMultiplier;

    // Movement
    private float moveSpeed;
    private float rotateSpeed;

    private bool canRotate;
    private bool canMoveForwards;
    private int rotateValue;

    private RaycastHit2D[] rayCasts;

    private GameObject waterSource;
    private Vector2 waterDir;
    private int rotateDir; // 0 clockwise | 1 anti-clockwise

    private bool isWandering;

    private bool isObstacleAhead;

    private bool leftRayHit;
    private bool midRayHit;
    private bool rightRayHit;

    private State state;

    private SpriteRenderer sR;

    [SerializeField] private Color maleColor;
    [SerializeField] private Color femaleColor;

    private void Awake()
    {        
        Physics2D.queriesStartInColliders = false;
        state = State.Wandering;
        sR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Movement
        canRotate = true;
        canMoveForwards = false;
        isObstacleAhead = false;
        rotateDir = 0;

        thirstMultiplier = Random.Range(1f, 2f);
        rayCasts = new RaycastHit2D[3];

        waterSource = GameObject.FindWithTag("Water");

        SetGender();
        StartCoroutine(Wandering());
        SetStartStats();
    }

    private void SetStartStats()
    {
        health = 100;
        thirst = GetRndmInt(0, 100);
    }

    void Update()
    {
        Vision();
        Hydration();
        StateHandling();
        Health();
    }

    private void FixedUpdate()
    {
        // Movement
        MoveForwards();
        Rotate();
    }

    private void Health()
    {
        if (thirst <= 0)
        {
            health -= 1 * Time.deltaTime;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void StateHandling()
    {
        switch (state)
        {
            case State.Wandering:
                if (!isWandering)
                {
                    StartCoroutine(Wandering());
                }
                break;
            case State.FindWater:
                FindWater();
                break;
        }
    }
    private enum State
    {
        Wandering,
        FindWater,
    }

    private void FindWater()
    {
        //StopAllCoroutines();

        StopCoroutine("Wandering");

        isWandering = false;

        canRotate = false;
        canMoveForwards = false;

        if (Vector2.Distance(transform.position, waterSource.transform.position) <= 2)
        {
            thirst = 100;
        }
        else if (thirst <= 50)
        {
            transform.up = waterDir;

            if (!isObstacleAhead)
            {
                transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            }
        }
    }

    private void Hydration()
    {
        thirst -= 1 * thirstMultiplier * Time.deltaTime;

        if (thirst < 50)
        {
            state = State.FindWater;
        }
        else if (thirst > 50)
        {
            state = State.Wandering;
        }

        waterDir = new Vector2(
            waterSource.transform.position.x - transform.position.x,
            waterSource.transform.position.y - transform.position.y
        );
    }

    private void Vision()
    {
        Vector3 leftDirection = new Vector3(-0.3f, 0.95f, 0f);
        Vector3 midDirection = new Vector3(0f, 1f, 0f);
        Vector3 rightDirection = new Vector3(0.3f, 0.95f, 0f);

        rayCasts[0] = Physics2D.Raycast(transform.position, transform.TransformDirection(leftDirection), visionRange);
        rayCasts[1] = Physics2D.Raycast(transform.position, transform.up, visionRange);
        rayCasts[2] = Physics2D.Raycast(transform.position, transform.TransformDirection(rightDirection), visionRange);

        CheckRaycast(0, leftDirection, ref leftRayHit);
        CheckRaycast(1, midDirection, ref midRayHit);
        CheckRaycast(2, rightDirection, ref rightRayHit);

        if (leftRayHit || midRayHit || rightRayHit)
        {
            isObstacleAhead = true;
        }
        else
        {
            isObstacleAhead = false;
        }
    }
    private void CheckRaycast(int rayIndex, Vector3 direction, ref bool boolToSet)
    {
        if (rayCasts[rayIndex].collider != null)
        {
            if (rayCasts[rayIndex].collider.CompareTag("Border"))
            {
                boolToSet = true;
                Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(direction) * visionRange, Color.red);
            }
            else
            {
                boolToSet = false;
                Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(direction) * visionRange, Color.green);
            }
        }
        else
        {
            boolToSet = false;
            Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(direction) * visionRange, Color.green);
        }
    }

    private IEnumerator Wandering()
    {
        isWandering = true;

        yield return new WaitForSeconds(GetRndmInt(1, 3));
        RotateDirectionAlgorithm();
        canRotate = true;

        yield return new WaitForSeconds(GetRndmInt(0, 3));
        RotateDirectionAlgorithm();

        canMoveForwards = true;
        if (!isObstacleAhead)
        {
            canRotate = false;
        }

        if (state == State.Wandering)
        {
            StartCoroutine(Wandering());
        }

        yield return null;

        isWandering = false;
    }

    private void RotateDirectionAlgorithm()
    {
        if (!isObstacleAhead)
        {
            rotateDir = GetRndmInt(0, 2);
        }
        else
        {
            if (leftRayHit || !midRayHit || !rightRayHit)
            {
                rotateDir = 0;
            }
            else if (!leftRayHit || !midRayHit || rightRayHit)
            {
                rotateDir = 1;
            }
            else if (leftRayHit || midRayHit || !rightRayHit)
            {
                rotateDir = 0;
            }
            else if (!leftRayHit || midRayHit || rightRayHit)
            {
                rotateDir = 1;
            }
            else
            {
                rotateDir = GetRndmInt(0, 2);
            }
        }
    }

    private void Rotate()
    {
        switch (rotateDir)
        {
            case 0:
                rotateValue = -50;
                break;
            case 1:
                rotateValue = 50;
                break;
            default:
                print("Error in rotate direction");
                break;
        }

        if (canRotate)
        {
            transform.Rotate(new Vector3(0f, 0f, rotateValue * rotateSpeed) * Time.fixedDeltaTime);
        }
    }

    private void MoveForwards()
    {
        if (canMoveForwards && !isObstacleAhead)
        {
            transform.Translate(new Vector3(0f, 1f * moveSpeed, 0f) * Time.fixedDeltaTime);
        }
    }

    private void SetGender()
    {
        int genderIndex = GetRndmInt(0, 2);

        switch (genderIndex)
        {
            // Male
            case 0:
                transform.name = "Male Cube";
                gender = "male";
                sR.color = maleColor;
                moveSpeed = 1f;
                rotateSpeed = 1f;
                break;
            // Female
            case 1:
                transform.name = "Female Cube";
                gender = "female";
                sR.color = femaleColor;
                moveSpeed = 1.25f;
                rotateSpeed = 1.25f;
                break;
            default:
                print("No available gender Index");
                break;
        }
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }

    private int GetRndmInt(int min, int max)
    {
        int randomNum = Random.Range(min, max);
        return randomNum;
    }
}