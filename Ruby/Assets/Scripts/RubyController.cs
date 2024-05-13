using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int MaxHealth = 5;
    public float timeInvincible = 2.0f;

    int currentHealth;
    public int health   { get {return currentHealth; } }

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2D;
    float horizontal;
    float vertical;
    Animator animator;
    Vector2 lookDirection = new Vector2 (1,0);
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal,vertical);

        if (!Mathf.Approximately(move.x,0.0F) || !Mathf.Approximately(move.y,0.0F))
        {
            lookDirection.Set(move.x,move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X",lookDirection.x);
        animator.SetFloat("Look Y",lookDirection.y);
        animator.SetFloat("Speed", move.magnitude );

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <0)
            {
                isInvincible = false;
            }
        }

    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2D.MovePosition(position);
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0 )
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
            { return; }

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0 ,MaxHealth);
        Debug.Log(currentHealth + " / " + MaxHealth);
    }
}
