using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;


public class PlayerCallbackManager : MonoBehaviour
{
    public static List<KeyCode> keyCodes = new List<KeyCode>();

    public static PlayerController _PlayerController 
        = GameObject.Find("Player").GetComponent<PlayerController>();


    public static void AddKeyCodeList(KeyCode keyCode)
    {
        keyCodes.Add(keyCode);
    }

    public static void DeleteKeyCodeList(KeyCode keyCode)
    {
        if (keyCodes.Contains(keyCode))
        {

        }
    }
}
