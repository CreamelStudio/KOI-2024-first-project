using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerMove : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    public Scrollbar SB;

    public PostProcessVolume PostVolume;
    private Bloom bloom;

    public int Score;

    public bool Trigger;

    public float MoveSpeed;
    public float SetLJumpPower;
    public float JumpPower;
    private float moveInput;
    public bool isJump;
    public bool isStartJump;

    private Rigidbody2D rb;
    public SpriteRenderer sr;

    public Animator Anim;

    public bool canMove;
    public bool Starting;

    public Sprite j01;
    public Sprite j02;

    public Sprite ms;

    public float bloomintensity = 30f;

    public Transform BathSpawn;
    public Transform CookingSpawn;
    public Transform MainSpawn;
    public Transform OutSpawn;

    public Text ScoreText;

    public bool isBloom;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        
        canMove = false;
        Starting = true;
    }

    private void Start()
    {
        ms = sr.sprite;

        SB.gameObject.SetActive(false);
        Invoke("MoveOK", 1f);
        DOTween.To(() => bloomintensity, x => bloomintensity = x, 3.45f, 1f).SetEase(Ease.InQuad);
        
        Debug.Log("Start");
        isBloom = true;

    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput * MoveSpeed, rb.velocity.y);
            if (moveInput != 0)
            {
                Anim.SetBool("isMove", true);
            }
            else
            {
                Anim.SetBool("isMove", false);
            }
            if (moveInput == -1)
            {
                sr.flipX = true;
            }
            else if (moveInput == 1)
            {
                sr.flipX = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            rb.gravityScale = -1;
        }
        if (Input.GetKeyUp(KeyCode.O)){
            rb.gravityScale = 10;
        }

        if (Input.GetKey(KeyCode.K))
        {
            if (Input.GetKeyDown(KeyCode.C)){
                Score++;
                ScoreText.text = Score + " 개";
            }
        }
        if(Score == 9)
        {
            sr.color = new Color(1, 1, 1, 0.6f);
            rb.gravityScale = -0.8f ;
            StartBloom();
            Invoke("Endings", 8f);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Trigger = true;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            Trigger = false;
        }

        SB.value = (JumpPower - 15f) / 15f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJump)
            {
                JumpPower = SetLJumpPower;
                SB.gameObject.SetActive(true);
                canMove = false;
                DOTween.To(() => JumpPower, x => JumpPower = x, 40f, 1f).SetEase(Ease.InSine);

                Anim.enabled = false;
                sr.sprite = j01;
                Invoke("JumpStart", 4f);
                Debug.Log("KeyDown!");
                
                isJump = false;
                
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke();
            DOTween.KillAll();
            JumpStart();
            
        }

        PostVolume.profile.TryGetSettings(out bloom);
        {
            bloom.intensity.value = bloomintensity;
        }


        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isJump)
            {
                isStartJump = true;
                isJump = true;
            }
            
            Debug.Log("Ground Stay");
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isStartJump = true;
            isJump = true;
            Anim.enabled = true;
            JumpPower = 0;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CookingDoor"))
        {
            if (Trigger)
            {
                DOTween.To(() => bloomintensity, x => bloomintensity = x, 40f, 0.5f).SetEase(Ease.InQuad);
                Invoke("resetBloom", 0.5f);
                Invoke("CookingDoor", 0.5f);
            }
        }
        if (collision.gameObject.CompareTag("BathDoor"))
        {
            if (Trigger)
            {
                DOTween.To(() => bloomintensity, x => bloomintensity = x, 40f, 0.5f).SetEase(Ease.InQuad);
                Invoke("resetBloom", 0.5f);
                Invoke("BathDoor", 0.5f);
            }
                
        }
        if (collision.gameObject.CompareTag("Outdoor"))
        {
            if (Trigger)
            {
                if(Score == 3)
                {
                    DOTween.To(() => bloomintensity, x => bloomintensity = x, 40f, 0.5f).SetEase(Ease.InQuad);
                    Invoke("resetBloom", 0.5f);
                    Invoke("OutDoor", 0.5f);
                }
                
            }
                
        }
        if (collision.gameObject.CompareTag("GoMainDoor"))
        {
            if (Trigger)
            {
                DOTween.To(() => bloomintensity, x => bloomintensity = x, 40f, 0.5f).SetEase(Ease.InQuad);
                Invoke("resetBloom", 0.5f);
                Invoke("GoMainDoor", 0.5f);
            }
                
        }



        if (collision.gameObject.CompareTag("Chew"))
        {
            collision.gameObject.SetActive(false);
            Score++;
            ScoreText.text = Score + " 개";
        }
    }

    public void JumpStart()
    {
        if (!Starting)
        {
            canMove = true;
        }
        if (isStartJump)
        {
            isStartJump = false;
            sr.sprite = j02;
            rb.velocity = Vector2.up * JumpPower;
            SB.gameObject.SetActive(false);
            Debug.Log("isJump!");
            Debug.Log("KeyUp!");

        }
        

    }

    public void MoveOK()
    {
        canMove = true;
        Starting = false;
    }

    public void resetBloom()
    {
        DOTween.To(() => bloomintensity, x => bloomintensity = x, 3.45f, 0.5f).SetEase(Ease.InQuad);
    }

    public void CookingDoor()
    {
        transform.position = CookingSpawn.position;
    }
    public void BathDoor()
    {
        transform.position = BathSpawn.position;
    }
    public void GoMainDoor()
    {
        transform.position = MainSpawn.position;
        virtualCamera.m_Lens.OrthographicSize = 5;
    }
    public void OutDoor()
    {
        transform.position = OutSpawn.position;
        virtualCamera.m_Lens.OrthographicSize = 8;
    }

    public void Endings()
    {
        Debug.Log("EndingTo!");
        SceneManager.LoadScene("EndingCutScene");
    }
    
    public void StartBloom()
    {
        if(isBloom){
            DOTween.To(() => bloomintensity, x => bloomintensity = x, 40f, 7f).SetEase(Ease.InQuad);
            isBloom = false;
        }
        
    }
}