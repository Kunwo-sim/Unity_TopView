using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float h;
    float v;
    bool isHorizonMove;
    public float Speed = 2;
    public GameManager manager;
    public GameObject scanObject;

    Animator Anim;
    Rigidbody2D rigid;
    [SerializeField]
    Vector2 dirVec;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal");
        bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical");
        bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal");
        bool vUp = manager.isAction ? false : Input.GetButtonUp("Vertical");

        if (hDown)
        {
            isHorizonMove = true;
        }
        else if (vDown)
        {
            isHorizonMove = false;
        }
        else if (hUp || vUp)
        {
            isHorizonMove = h != 0;
        }

        if(Anim.GetInteger("hAxisRaw") != h)
        {
            Anim.SetBool("isChange", true);
            Anim.SetInteger("hAxisRaw", (int)h);
        }
        else if (Anim.GetInteger("vAxisRaw") != v)
        {
            Anim.SetBool("isChange", true);
            Anim.SetInteger("vAxisRaw", (int)v);
        }
        else
        {
            Anim.SetBool("isChange", false);
        }

        // 보고 있는 방향
        if (vDown && v == 1)
            dirVec = Vector2.up;
        else if (vDown && v == -1)
            dirVec = Vector2.down;
        else if (hDown && h == 1)
            dirVec = Vector2.right;
        else if (hDown && h == -1)
            dirVec = Vector2.left;
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            manager.Action(scanObject);
        }
    }
    void FixedUpdate()
    {
        // Ray
        Debug.DrawRay(transform.position, dirVec, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 1.0f, LayerMask.GetMask("Object"));
        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
        // Move
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * Speed;
    }
}
