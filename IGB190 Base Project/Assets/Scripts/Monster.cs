using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IDamageable
{
    // Player Stats
    public float health = 100;
    public float maxHealth = 100;
    public float movementSpeed = 3.5f;
    public float attacksPerSecond = 1.0f;
    public float attackRange = 2.0f;
    public float attackDamage = 10.0f;

    // Store a reference to the player for easy access
    private Player player;

    // Variables to control when the unit can attack and move
    private float canCastAt;
    private float canMoveAt;

    // Constants to prevent magic numbers in the code. Makes it easier to edit later
    private const float MOVEMENT_DELAY_AFTER_CASTING = 1.5f;
    private const float TURNING_SPEED = 10.0f;
    private const float TIME_BEFORE_CORPSE_DESTROYED = 5.0f;

    // Cache references to important components for easy access later
    private NavMeshAgent agentNavigation;
    private Animator animator;
    
    // Variables to control ability casting 
    private enum Ability { Slash, /* Add more abilities here */ }
    private Ability? abilityBeingCast = null;
    private float finishAbilityCastAt;
    [Range(0.0f, 1.0f)] public float slashActivationPoint = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        agentNavigation = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindObjectOfType<Player>();
        canMoveAt = Time.time + 1.0f;
        transform.LookAt(player.transform);
    }

    // Update is called once per frame
    private void Update()
    {
        // If the player is dead, don't do anything
        if (player.isDead) return;
        UpdateMovement();
        UpdateAbilityCasting();
    }

    // Handle all update logic associated with the character's movement
    private void UpdateMovement()
    {
        animator.SetFloat("Speed", agentNavigation.velocity.magnitude);
        if (Time.time > canMoveAt)
            agentNavigation.SetDestination(player.transform.position);
    }

    // Handle all update logic associated with ability casting
    private void UpdateAbilityCasting()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange && Time.time > canCastAt)
            StartCastingSlash();

        // If the current ability has reached the end of its cast, run the appropriate actions for the ability
        if (abilityBeingCast != null && Time.time > finishAbilityCastAt)
            if (abilityBeingCast == Ability.Slash)
                FinishCastingSlash();
        
        // If a cast is in progress, face towards the player
        if (abilityBeingCast != null)
        {
            Quaternion look = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * TURNING_SPEED);
        }
    }

    // Perform all logic for when the enemy *starts* casting the slash ability
    private void StartCastingSlash()
    {
        // Stop the character from moving while they attack
        agentNavigation.SetDestination(transform.position);

        // Set the ability being cast to the slash ability
        abilityBeingCast = Ability.Slash;

        // Play the appropriate ability animation at the correct speed
        animator.CrossFadeInFixedTime("Attack", 0.2f);
        animator.SetFloat("AttackSpeed", attacksPerSecond);

        // Calculate when the ability will finish casting, and when the player can next cast and move
        float castTime = (1.0f / attacksPerSecond);
        canCastAt = Time.time + castTime;
        finishAbilityCastAt = Time.time + slashActivationPoint * castTime;
        canMoveAt = finishAbilityCastAt + MOVEMENT_DELAY_AFTER_CASTING;
    }

    // Perform all logic for when the enemy *finishes* casting the slash ability
    private void FinishCastingSlash()
    {
        // Clear the ability currently being used
        abilityBeingCast = null;

        // Find all the targets that should be hit by the attack and damage them
        Vector3 hitPoint = transform.position + transform.forward * attackRange;
        List<Player> targets = Utilities.GetAllWithinRange<Player>(hitPoint, attackRange);
        foreach (Player target in targets)
            target.TakeDamage(attackDamage);
    }

    // Remove the specified amount of health from this unit, killing it if needed
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Kill();
    }

    //Destroy the player, briefly keeping the corpse visible to play the death animation
    public virtual void Kill()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
            animator.transform.SetParent(null);
            Destroy(animator.gameObject, TIME_BEFORE_CORPSE_DESTROYED);
        }
        player.experience = player.experience + 10;
        Destroy(gameObject);
    }

    // Returns the current health percent of the player (a value between 0.0 and 1.0)
    public float GetCurrentHealthPercent()
    {
        return health / maxHealth;
    }
}
