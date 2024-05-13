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
    public GameObject projectilePrefab;

    int currentHealth;
    public int health   { get {return currentHealth; } } 
    // Para mexer na vida dela sem modificar a outra variavel e possivel quebrar algo sem querer

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2D;
    float horizontal;
    float vertical;
    Animator animator;
    Vector2 lookDirection = new Vector2 (1,0); //Saber a direção que a animação do sprite vai seguir
    void Start()
    {
        // Designar o que é preciso e ajustando a vida
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
        animator = GetComponent<Animator>();
    }
    // ============================================ //
    void Update()
    {
        // Pegando os inputs do Teclado
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal,vertical);

        if (!Mathf.Approximately(move.x,0.0F) || !Mathf.Approximately(move.y,0.0F))
        {
            // Testar se o boneco está parado, visto que os floats sao levemente nao certos, usamos aproximação
            lookDirection.Set(move.x,move.y);
            lookDirection.Normalize();
        }
        // Mais questao de animação do sprite
        animator.SetFloat("Look X",lookDirection.x);
        animator.SetFloat("Look Y",lookDirection.y);
        animator.SetFloat("Speed", move.magnitude );

        if (isInvincible)
        {// Caso Ruby tome dano, ela vai ficar invencivel, se isso for verdade é feito um timer pra definir quanto tempo isso acontece
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <0)
            {
                isInvincible = false;
            }
        }

        if(Input.GetButtonDown("Fire1"))
        {
            Launch();
        }

    }
    // ============================================ //
    void FixedUpdate()
    {// Essa função serve para qualquer Update envolvendo físicas
        Vector2 position = rigidbody2D.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2D.MovePosition(position);
    }
    // ============================================ //
    public void ChangeHealth(int amount)
    {
        // Aqui é feito mudanças em relação a ganho ou perca de vida

        if (amount < 0 )
        {   // Se o parâmetro é negativo , então tomou dano
            animator.SetTrigger("Hit");

            if (isInvincible)
            { return; } 
            // Se ela está invencivel então encerra, se não ativa o estado de invencivel
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0 ,MaxHealth); // Garantindo os "limites" que a vida do boneco tem.
        Debug.Log(currentHealth + " / " + MaxHealth);
    }

    // ============================================ //
    void Launch()
    {
        GameObject projectileObject = Instantiate( projectilePrefab, rigidbody2D.position + Vector2.up * 0.5F, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

    }

    // ============================================ //
}
