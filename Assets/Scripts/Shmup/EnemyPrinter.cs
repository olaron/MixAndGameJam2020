﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyPrinter : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float minSpawnInterval = 0.2f;
    public float maxSpawnInterval = 1.5f;
    public float shootingForce = 4f;

    public Rigidbody2D rb;
    public GameObject baseEnemyPrefab;

    private bool goingLeft;
    private Vector2 movement;

    private float nextSpawnTime;


    // Update is called once per frame
    void Update()
    {
        float x = moveSpeed;
        if (goingLeft)
        {
            x = -x;
        }

        movement.x = x;
        movement.y = 0;
    }

    void FixedUpdate()
    {
        rb.velocity = movement;

        if (transform.localPosition.x < 0.5f)
        {
            goingLeft = false;
        }

        if (transform.localPosition.x > 6.5f)
        {
            goingLeft = true;
        }

        if (Time.fixedTime > nextSpawnTime)
        {
            GameObject enemy = Instantiate(baseEnemyPrefab, transform.position, transform.rotation);
            enemy.transform.SetParent(ShmupController.Instance.transform);
            GameObject actionObject = enemy.transform.GetChild(0).gameObject;

            float rand = Random.Range(0, 100);
            if (rand <= 60f)
            {
                MoveAction action = actionObject.AddComponent<MoveAction>();
                action.MoveAmount = 1;

                switch ((int)Random.Range(0, 2.99f))
                {
                    case 0:
                        action.Direction = MoveAction.MoveDirection.Down;
                        break;
                    case 1:
                        action.Direction = MoveAction.MoveDirection.Left;
                        break;
                    default:
                        action.Direction = MoveAction.MoveDirection.Right;
                        break;
                }
            }
            else if (rand <= 95f)
            {
                RotateAction action = actionObject.AddComponent<RotateAction>();
                switch ((int)Random.Range(0, 1.99f))
                {
                    case 0:
                        action.Direction = RotateAction.RotateDirection.Clockwise;
                        break;
                    default:
                        action.Direction = RotateAction.RotateDirection.CounterClockwise;
                        break;
                }
            }
            else
            {
                NewLineAction action = actionObject.AddComponent<NewLineAction>();
            }

            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            rb.velocity = -transform.up * shootingForce;

            nextSpawnTime = Time.fixedTime + Random.Range(minSpawnInterval, maxSpawnInterval);
            //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        }
    }
}
