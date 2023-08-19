using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player_Base : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] RectTransform UI_Transform;

    [Header("Component")]
    [SerializeField] string playerName;
    private bool isMouseOver = false;

    [Header("Component")]
    [SerializeField] Rigidbody2D rigidbody2d;
    [SerializeField] Collider2D collider2d;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Animator animator;
    [SerializeField] Player_Pool player_Pool;

    [Header("Player Instance")]
    [SerializeField] GameObject PlayerAllUIPrefabs;
    GameObject PlayerAllUIInstance;

    [Header("Player Movement")]
    [SerializeField] Transform MovingPoint;
    [SerializeField] AIPath aIPath;
    [SerializeField] AIDestinationSetter destinationSetter;
    GameObject Effect_ClickToMove;
    bool FacingRight = true;
    Vector3 TargetPoint, MovePosition;
    float LocalScaleX, LocalScaleY;

    [Header("Object Pool")]
    [SerializeField] GameObject ObjectPool;

    [Header("Pile")]
    [SerializeField] SpriteRenderer Pile_Handle;
    [SerializeField] bool IsPile;

    [Header("Effect")]
    [SerializeField] GameObject MouseOverEffect_Pile;


    // Start is called before the first frame update
    void Start()
    {
        ObjectPool.transform.SetParent(null);
        LocalScaleX = transform.localScale.x;
        LocalScaleY = transform.localScale.y;
        PlayerAllUIInstance = Instantiate(PlayerAllUIPrefabs);
    }

    // Update is called once per frame
    void Update()
    {
        MovePosition = aIPath.desiredVelocity;

        animator.SetFloat("Horizontal", MovePosition.x);
        animator.SetFloat("Vertical", MovePosition.y);
        animator.SetFloat("Speed", MovePosition.sqrMagnitude);
    }

    void FixedUpdate()
    {

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
            Debug.Log("Object Name: " + playerName);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TargetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TargetPoint.z = transform.position.z;

            MovingPoint.position = TargetPoint;
            TurnDirection_Check(TargetPoint);

            Effect_ClickToMove = player_Pool.GetEffect_ClickToMove();
            if (Effect_ClickToMove != null)
            {
                Effect_ClickToMove.transform.position = TargetPoint;
                Effect_ClickToMove.SetActive(true);
            }
        }
    }


    public bool GetIsPile()
    {
        return IsPile;
    }

    public void CollectPile(GameObject pile)
    {
        if (IsPile)
        {
            PlayerAllUIInstance.GetComponent<Player_AllUI>().PickUp_Already_Show();
        }
        else
        {
            Pile_Handle.sprite = Resources.Load<Sprite>("Player/Pile");
            IsPile = true;
            Destroy(pile);
            MouseOverEffect_Pile_Off();
        }
    }

    #region Turn Direction
    public void TurnDirection_Check(Vector3 direction)
    {
        if (direction.x > transform.position.x && !FacingRight)
        {
            TurnDirection();
        }
        else if (direction.x < transform.position.x && FacingRight)
        {
            TurnDirection();
        }
    }

    public void TurnDirection()
    {
        FacingRight = !FacingRight;
        LocalScaleX *= -1f;

        SetUpFlip(LocalScaleX, LocalScaleY, 1f);
    }

    public void SetUpFlip(float x, float y, float z)
    {
        transform.localScale = new Vector3(x, y, z);
        if (UI_Transform.gameObject != null)
        {
            UI_Transform.localScale = new Vector3(x, y, z);
        }
    }

    #endregion

    #region Effect
    public void MouseOverEffect_Pile_On(Vector3 Position)
    {
        MouseOverEffect_Pile.transform.position = Position;
        MouseOverEffect_Pile.SetActive(true);
    }

    public void MouseOverEffect_Pile_Off()
    {
        MouseOverEffect_Pile.SetActive(false);
    }

    #endregion

}
