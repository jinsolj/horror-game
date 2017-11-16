using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayerManager>();
            return _instance;
        }
    }

    private static PlayerManager _instance = null;

    [SerializeField] private float speed;
    private float health = 100;
    public bool allowInput = true;
    public Inventory inventory
    {
        get; private set;
    }

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] LayerMask interactableMask;
    [SerializeField] float interactDistance = 0.1f;
    [SerializeField] float interactRaycastRadius = 0.2f;
    [SerializeField]
    Vector2 interactCastOffset;
    private Vector2 faceDir;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventory = new Inventory();
	}

    // Called once for every physics calculation time step
    private void Update() {
        // track WASD for movement
        float x;
        float y;

        if (allowInput)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryInteracting(Item.NONE);
            }
        }
        else
        {
            x = 0;
            y = 0;
        }

        // set movement animation
        if (rb.velocity.magnitude > 0.01f)
        {
            faceDir = rb.velocity.normalized;
        }

        anim.SetFloat("horizontalSpeed", faceDir.x);
        anim.SetFloat("verticalSpeed", faceDir.y);
        anim.SetBool("isMoving", x != 0 || y != 0);

        // actually move character
        Vector2 move = new Vector2(x, y).normalized;
        rb.velocity = move * speed;
    }

    protected void TryInteracting(Item item)
    {
        Vector3 start = transform.position + new Vector3(interactCastOffset.x, interactCastOffset.y);
        Vector3 dir = faceDir;
        float distance = interactDistance;

        Debug.DrawLine(start, start + dir.normalized * distance, Color.red, 5f);
        Debug.DrawLine(start, start + Vector3.up * interactRaycastRadius, Color.red, 5f);
        RaycastHit2D hitInfo = Physics2D.CircleCast(start, interactRaycastRadius, dir.normalized, distance, interactableMask);

        if(hitInfo)
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if(interactable != null)
            {
                interactable.Interact(this, item);
            }
        }
    }
}
