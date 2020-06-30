using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerInputManager : MonoBehaviour
{
    public delegate void InputCallback(List<KeyCode> keyCode);
    public InputCallback _InputCallback;

    List<KeyCode> _InputKey = new List<KeyCode>();

    [Header("- Input Key Setting")]
    public KeyCode FUNCTION     =   KeyCode.F;
    public KeyCode JUMP         =   KeyCode.Space;
    public KeyCode SNEAK        =   KeyCode.V;

    public KeyCode USEITEM      =   KeyCode.Q;
    public KeyCode SKILL1       =   KeyCode.E;
    public KeyCode SKILL2      =   KeyCode.R;

    public KeyCode DASH         =   KeyCode.LeftShift;
    public KeyCode ACTION       =   KeyCode.LeftControl;

    public KeyCode LBC          =   KeyCode.Mouse0;
    public KeyCode RBC          =   KeyCode.Mouse1;

    public KeyCode PAUSE        =   KeyCode.Escape;
    public KeyCode UP           =   KeyCode.UpArrow;
    public KeyCode DOWN         =   KeyCode.DownArrow;

    PlayerController _PlayerController;

    private void Awake()
    {
        _PlayerController = GetComponent<PlayerController>();
        _PlayerController._PlayerInputCallBack += NotifyInputKey;
    }

    void Update()
    {
        //if (Input.GetButton("Horizontal"))
        //{
        //    float h = Input.GetAxis("Horizontal");
        //    KeyCode input = h > 0 ? KeyCode.D : KeyCode.A;
        //    _InputKey.Add(input);
        //}

        //if (Input.GetButton("Vertical"))
        //{
        //    float v = Input.GetAxis("Vertical");
        //    KeyCode input = v > 0 ? KeyCode.W : KeyCode.S;
        //    _InputKey.Add(input);
        //}

        //if (Input.GetKey(FUNCTION)) _InputKey.Add(FUNCTION);

        if (Input.GetKeyDown(JUMP)) _InputKey.Add(JUMP);

        if (Input.GetKeyDown(SNEAK)) _InputKey.Add(SNEAK);

        if (Input.GetKeyDown(USEITEM)) _InputKey.Add(USEITEM);

        if (Input.GetKeyDown(SKILL1)) _InputKey.Add(SKILL1);

        if (Input.GetKeyDown(SKILL2)) _InputKey.Add(SKILL2);

        if (Input.GetKeyDown(DASH)) _InputKey.Add(DASH);

        if (Input.GetKeyDown(ACTION)) _InputKey.Add(ACTION);

        //if (Input.GetKey(LBC)) _InputKey.Add(LBC);

        //if (Input.GetKey(RBC)) _InputKey.Add(RBC);

        if (Input.GetKeyDown(PAUSE)) _InputKey.Add(PAUSE);

        if (Input.GetKey(UP)) _InputKey.Add(UP);

        if (Input.GetKey(DOWN)) _InputKey.Add(DOWN);

        try
        {
            if (_InputKey.Count != 0)
                _PlayerController._PlayerInputCallBack(_InputKey);
        }
        catch
        {

        }
    }

    public void NotifyInputKey(List<KeyCode> keyCodes)
    {
        foreach (KeyCode keyCode in keyCodes)
        {
            PlayerController.AddKeyCodeList(keyCode);
            foreach(KeyCode key in PlayerController._PlayerInputKeyCodeList)
                Debug.Log(key.ToString());
        }

        _InputKey.Clear();
    }
}
