using Assets.Scripts.Database.Entity;
using Pathfinding;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player_Base : MonoBehaviourPunCallbacks, IPunObservable
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
    Vector3 TargetPoint;
    float LocalScaleX, LocalScaleY;


    [Header("Object Pool")]
    [SerializeField] GameObject ObjectPool;

    [Header("Pile")]
    [SerializeField] SpriteRenderer Pile_Handle;
    [SerializeField] bool IsPile;

    [Header("Effect")]
    [SerializeField] GameObject MouseOverEffect_Pile;


    // Lag Reduce
    Vector3 realPosition;
    Quaternion realRotation;
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        ObjectPool.transform.SetParent(null);

        LocalScaleX = transform.localScale.x;
        LocalScaleY = transform.localScale.y;

        if (photonView.IsMine)
        {
            PlayerAllUIInstance = Instantiate(PlayerAllUIPrefabs);
        }
        else
        {

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            animator.SetFloat("Horizontal", realPosition.x);
            animator.SetFloat("Vertical", realPosition.y);
            animator.SetFloat("Speed", realPosition.sqrMagnitude);
        }
    }

    public void FixedUpdate()
    {

        if (photonView.IsMine)
        {

            if (aIPath != null && aIPath.reachedEndOfPath)
            {
                // Player has reached the destination, so set canMove to false
                aIPath.canMove = false;
                realPosition = Vector3.zero;

            }
            else
            {
                // Player is still moving, so set canMove to true
                aIPath.canMove = true;
                realPosition = aIPath.desiredVelocity;
            }
        }
        else
        {
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
        }


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
        if (context.performed && photonView.IsMine)
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(UI_Transform.localScale);

            stream.SendNext(aIPath.canMove);

        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();
            UI_Transform.localScale = (Vector3)stream.ReceiveNext();

            aIPath.canMove = (bool)stream.ReceiveNext();


            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
        }
    }

}
