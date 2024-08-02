using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    // Player Stats
    public float health = 500;
    public float maxHealth = 500;
    public float movementSpeed = 3.5f;
    public float attacksPerSecond = 1.5f;
    public float attackRange = 2.0f;
    public float attackDamage = 10.0f;

    // Cache refrences to important components for easy access later.
    private NavMeshAgent agentNavigation;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agentNavigation = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateMovement();
    }

    // Handle all update logic associated with the character's movement.
    private void UpdateMovement()
    {
        if (Input.GetMouseButton(0))
            agentNavigation.SetDestination(Utilities.GetMouseWorldPosition());
    }
}
