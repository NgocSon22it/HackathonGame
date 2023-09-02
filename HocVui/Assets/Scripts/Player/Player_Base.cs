using Assets.Scripts.Common;
using Assets.Scripts.Database.Entity;
using Cinemachine;
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
    public GameObject PlayerAllUIInstance;
    [SerializeField] GameObject PlayerCameraPrefabs;
    public GameObject PlayerCameraInstance;

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

    [Header("Camera Box")]
    public PolygonCollider2D CameraBox;

    [Header("Pile")]
    [SerializeField] SpriteRenderer Pile_Handle;
    [SerializeField] public bool IsPile, IsPileBase;

    [Header("Effect")]
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
            PlayerCameraInstance = Instantiate(PlayerCameraPrefabs);

            PlayerCameraInstance.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
            PlayerCameraInstance.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = CameraBox;
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
        }
    }

    private void OnMouseExit()
    {
        isMouseOver = false;

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
                PlayerAllUIInstance.GetComponent<Player_AllUI>().Message_On(Message.Game_PickUpPile_Already);
            }
            else
            {
                photonView.RPC(nameof(SyncSpriteChange), RpcTarget.All, true);
                IsPile = true;
            }
        }
    }

    public void PuttingPile(int answer)
    {
        if (photonView.IsMine)
        {
            if (!IsPile)
            {
                PlayerAllUIInstance.GetComponent<Player_AllUI>().Message_On(Message.Game_PuttingPile_DoNotHavePile);
            }
            else
            {
                if (!IsPileBase)
                {
                    PlayerAllUIInstance.GetComponent<Player_AllUI>().SelectedAnswer_On(answer);
                    photonView.RPC(nameof(SyncSpriteChange), RpcTarget.All, false);
                    GameManager.Instance.SelectOption(answer);
                    GameManager.Instance.SubmitValue(photonView.Owner.NickName, answer);
                    IsPileBase = true;
                }
                else
                {
                    PlayerAllUIInstance.GetComponent<Player_AllUI>().Message_On(Message.Game_PuttingPile_Already);
                }
            }
        }
    }

    [PunRPC]
    void SyncSpriteChange(bool value)
    {
        if (value)
        {
            Pile_Handle.sprite = Resources.Load<Sprite>("Player/Coc 1");
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
            if (x > 0)
            {
                UI_Transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                UI_Transform.localScale = new Vector3(-1, 1, 1);
            }
        }
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
