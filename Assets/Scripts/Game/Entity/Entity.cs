using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Dogabeey;

public class Entity : MonoBehaviour
{
    public enum EntityState
    {
        Idle,
        Run,
        Dead
    }

    public Transform pickupParent;
    public Transform dropParent;
    public float moveSpeedMultiplier = 1;

    internal Rigidbody2D rb;
    internal Collider2D cd;
    internal EntityState state;

    public float MoveSpeed
    {
        get
        {
            float finalMoveSpeed = moveSpeedMultiplier;

            return finalMoveSpeed;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
