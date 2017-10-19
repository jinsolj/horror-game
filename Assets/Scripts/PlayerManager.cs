using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private float speed;
    private float health = 100;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] LayerMask interactableMask;
    [SerializeField] float interactDistance = 0.1f;
    private Vector2 faceDir;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}

    // Called once for every physics calculation time step
    private void FixedUpdate() {
        // track WASD for movement
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // set movement animation
        anim.SetFloat("horizontalSpeed", x);
        anim.SetFloat("verticalSpeed", y);
        anim.SetBool("isMoving", x != 0 || y != 0);

        // actually move character
        Vector2 move = new Vector2(x, y).normalized;
        rb.velocity = move * speed;
        faceDir = rb.velocity.magnitude > 0 ? rb.velocity.normalized : Vector2.down;
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetButtonDown("Fire1"))
        {
            TryInteracting(Item.NONE);
        }
	}

    protected void TryInteracting(Item item)
    {
        Vector3 start = transform.position;
        Vector3 dir = faceDir;
        float distance = interactDistance;

        Debug.DrawLine(start, start + dir.normalized * distance, Color.red, 5f);
        RaycastHit2D hitInfo = Physics2D.Raycast(start, dir, distance, interactableMask);
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
