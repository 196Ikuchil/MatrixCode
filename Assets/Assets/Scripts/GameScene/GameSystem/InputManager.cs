using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnitySampleAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour {
	Vector2 slideStartPosition;
	Vector2 prevPosition;
    Vector2 joyStickPrePosition;
	Vector2 delta = Vector2.zero;
    Vector2 PlayerDelta = Vector2.zero;


    public PlayerCtrl pClrt;
    public CommandPanel cPanel;
    public FollowCamera fCamera;
    public GameMasterScript gameMaster;
	bool moved = false;
	
    void Start()
    {
    }
	void Update()
	{


        //GUIの操作をここで受け取る
        if (CrossPlatformInputManager.GetButtonDown("AttackButton"))
        {
            pClrt.AttackButton();
        }

        if (CrossPlatformInputManager.GetButtonDown("CommandButton"))
        {
            Debug.Log("PushButton");
            cPanel.SetCurrentSelectToNext();
        }
        if (CrossPlatformInputManager.GetButtonDown("KaihiButton"))
        {
            pClrt.dodgeButton();
        }
        if (CrossPlatformInputManager.GetButtonDown("LookButton"))
        {
            fCamera.LookForward();
        }
        if (CrossPlatformInputManager.GetButtonDown("StopButton"))
        {
            gameMaster.GamePause();
        }

        //ジョイスティック操作
        PlayerDelta.Set(CrossPlatformInputManager.GetAxisRaw("Horizontal"),CrossPlatformInputManager.GetAxisRaw("Vertical"));
        /*
            // スライド開始地点.
            if (Input.GetButtonDown("Fire1"))
			slideStartPosition = GetCursorPosition();
		
		// 画面の１割以上移動させたらスライド開始と判断する.
		if (Input.GetButton("Fire1")) {
			if (Vector2.Distance(slideStartPosition,GetCursorPosition()) >= (Screen.width * 0.1f))
				moved = true;
		}
		
		// スライド操作が終了したか.
		if (!Input.GetButtonUp("Fire1") && !Input.GetButton("Fire1"))
			moved = false; // スライドは終わった.

        // 移動量を求める.
        if (moved )
        {
            //Vector3 delta3 = GetCursorPosition() - slideStartPosition;
            delta = GetCursorPosition() - slideStartPosition;   
        }
        else if (moved)
            delta = GetCursorPosition() - prevPosition;
        else
            delta = Vector2.zero;    

        // カーソル位置を更新. 
            prevPosition = GetCursorPosition();
	*/}
	
	// クリックされたか.
	public bool Clicked()
	{
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        if (!moved && Input.GetButtonUp("Fire1"))
			return true;
		else
			return false;

        Debug.Log("clickd");
	}	
	
	// スライド時のカーソルの移動量.
	public Vector2 GetDeltaPosition()
	{
		return delta;
	}
    public Vector2 GetPlayerDeltaPosition()
    {
        return PlayerDelta;
    }
	
	// スライド中か.
	public bool Moved()
	{
		return moved;
	}
	
	public Vector2 GetCursorPosition()
	{
		return Input.mousePosition;
	}
}
