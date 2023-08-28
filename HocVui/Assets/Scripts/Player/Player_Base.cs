using Assets.Scripts.Database.Entity;
using Pathfinding;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player_Base : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("UI")]
    [SerializeField] RectTransform UI_Transform;
    [SerializeField] TMP_Text PlayerNameTxt;
    [SerializeField] UnityEngine.UI.Slider BlockProgress;

    [Header("Component")]
    [SerializeField] public string playerName;
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
    [SerializeField] public bool IsPile, IsPileBase;

    [Header("Effect")]
    [SerializeField] GameObject MouseOverEffect_Pile;
    [SerializeField] GameObject MouseOverEffect_Player;
    bool LoseControl;
    Coroutine BlockPlayer_Coroutine;
    public float animationDuration = 5f;
    private float startValue;
    private float targetValue;
    private float startTime;

    [Header("Select Question")]
    [SerializeField] public int SelectionIndex;

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != null && targetPlayer.Equals(photonView.Owner))
        {
            SelectionIndex = (int)changedProps["SelectOption"];
        }
    }

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

        playerName = photonView.Owner.NickName;
        PlayerNameTxt.text = playerName;

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
            if (LoseControl)
            {
                aIPath.canMove = false;
                realPosition = Vector3.zero;
            }
            else
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
        if (!photonView.IsMine)
        {
            isMouseOver = true;
            MouseOverEffect_Player_On(transform.position);
        }
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        MouseOverEffect_Player_Off();

    }

    private void OnMouseDown()
    {
        if (isMouseOver)
        {
            photonView.RPC(nameof(BlockPlayer), RpcTarget.All);
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

    public void Click(InputAction.CallbackContext context)
    {
        if (context.performed && photonView.IsMine)
        {


        }
    }


    public bool GetIsPile()
    {
        return IsPile;
    }

    public void CollectPile()
    {
        if (photonView.IsMine)
        {
            if (IsPile)
            {
                PlayerAllUIInstance.GetComponent<Player_AllUI>().PickUp_Already_Show();
            }
            else
            {
                photonView.RPC(nameof(SyncSpriteChange), RpcTarget.All, true);
                IsPile = true;
                MouseOverEffect_Pile_Off();
            }
        }
    }

    public void PuttingPile(int Index)
    {
        if (photonView.IsMine)
        {
            if (IsPile && !IsPileBase)
            {
                photonView.RPC(nameof(SyncSpriteChange), RpcTarget.All, false);
                GameManager.Instance.SelectOption(Index);
                GameManager.Instance.SubmitValue(photonView.Owner.NickName, Index);
                IsPileBase = true;
                MouseOverEffect_Pile_Off();
            }
            else
            {
                Debug.Log("Cam r ai cho rut");
            }
        }
    }

    [PunRPC]
    void SyncSpriteChange(bool value)
    {
        if (value)
        {
            Pile_Handle.sprite = Resources.Load<Sprite>("Player/Pile");
        }
        else
        {
            Pile_Handle.sprite = null;
        }
    }

    [PunRPC]
    void BlockPlayer()
    {
        LoseControl = true;
        if (BlockPlayer_Coroutine != null)
        {
            StopCoroutine(BlockPlayer_Coroutine);
        }

        BlockPlayer_Coroutine = StartCoroutine(BlockPlayer_Start());

    }

    [PunRPC]
    public void ClearEffect()
    {
        BlockProgress.value = 5f;
        LoseControl = false;
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

    public void MouseOverEffect_Player_On(Vector3 Position)
    {
        MouseOverEffect_Player.transform.position = Position;
        MouseOverEffect_Player.SetActive(true);
    }

    public void MouseOverEffect_Player_Off()
    {
        MouseOverEffect_Player.SetActive(false);
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

    IEnumerator BlockPlayer_Start()
    {
        startValue = 5f;
        targetValue = 0f;

        startTime = Time.time;
        BlockProgress.gameObject.SetActive(true);

        while (Time.time - startTime < animationDuration)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / animationDuration;

            BlockProgress.value = Mathf.Lerp(startValue, targetValue, t);

            yield return null;
        }

        BlockProgress.value = targetValue;
        BlockProgress.gameObject.SetActive(false);
        photonView.RPC(nameof(ClearEffect), RpcTarget.All);
    }

}
