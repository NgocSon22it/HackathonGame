using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Base : MonoBehaviour
{
    [SerializeField] GameObject Effect;

    [Header("Component")]
    [SerializeField] string playerName;
    private bool isMouseOver = false;

    [Header("Component")]
    [SerializeField] Rigidbody2D rigidbody2d;
    [SerializeField] Collider2D collider2d;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Animator animator;

    [Header("Player Movement")]
    [SerializeField] Vector2 MoveDirection;
    Vector3 Movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Walk();
    }

    public void Walk()
    {
        Movement = new Vector3(MoveDirection.x, MoveDirection.y, 0f);
        transform.Translate(Movement * 5 * Time.fixedDeltaTime);
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    private void OnMouseDown()
    {
        if (isMouseOver)
        {
            Instantiate(Effect, new(transform.position.x, transform.position.y - 0.45f, transform.position.z), Quaternion.identity);
            Debug.Log("Object Name: " + playerName);
        }
    }
}
