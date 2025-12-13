using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornRunner : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 6f;
    public float jumpForce = 7f;
    public float dashForce = 10f;
    public float doubleJumpDelay = 0.25f; // tempo max para 2º toque
    public float dashDoubleClickTime = 0.20f;

    public Animator anim;
    private Rigidbody rb;

    private bool isGround = true;
    private bool canDoubleJump = false;

    private float lastJumpPressTime = 0f;
    private float lastSpaceTapTime = 0f;

    void Start()
    {
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //=====================================================
        //  MOVIMENTO AUTOMÁTICO (Speed Runner)
        //=====================================================
        transform.position += Vector3.right * runSpeed * Time.deltaTime;

        //=====================================================
        //  VERIFICA SE ESTÁ NO CHÃO
        //=====================================================
        anim.SetBool("isGround", isGround);

        //=====================================================
        //  CONTROLOS DO TECLADO (Somente espaço)
        //=====================================================
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float now = Time.time;

            // ----------------------------------------------
            // 1) BOUNCE (espaço segurado)
            // ----------------------------------------------
            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("Bounce");
                Bounce();
                return;
            }

            // ----------------------------------------------
            // 2) DASH (double-click rápido)
            // ----------------------------------------------
            if (now - lastSpaceTapTime < dashDoubleClickTime)
            {
                Dash();
                anim.SetTrigger("Dash");
                lastSpaceTapTime = now;
                return;
            }
            lastSpaceTapTime = now;

            // ----------------------------------------------
            // 3) JUMP e DOUBLE JUMP
            // ----------------------------------------------
            if (isGround)
            {
                Jump();
                anim.SetTrigger("Jump");
                canDoubleJump = true;
                lastJumpPressTime = now;
                return;
            }

            // DOUBLE JUMP válido somente se:
            // -> ainda não usado
            // -> toque dentro da janela de tempo
            if (canDoubleJump && now - lastJumpPressTime < doubleJumpDelay)
            {
                DoubleJump();
                anim.SetTrigger("DoubleJ");
                canDoubleJump = false;
                return;
            }
        }
    }


    //=====================================================
    //                  FUNÇÕES DO MOVIMENTO
    //=====================================================
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGround = false;
    }

    void DoubleJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        rb.AddForce(Vector3.up * jumpForce * 0.9f, ForceMode.Impulse);
    }

    void Dash()
    {
        rb.AddForce(Vector3.right * dashForce, ForceMode.Impulse);
    }

    void Bounce()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        rb.AddForce(Vector3.up * jumpForce * 1.2f, ForceMode.Impulse);
    }

    //=====================================================
    //          DETEÇÃO DO CHÃO (colisão simples)
    //=====================================================
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}
