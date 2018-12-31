using System.Collections;
using System.Collections.Generic;
using Assets.Code;
using NetworkingCore;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Text usernameInput;
    public Text passwordInput;

    private void Start()
    {
          
    }

    private void Update()
    {
        
    }

    public void LoginClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        NetworkManager.Instance.userService.Login(username, password);

        
    }
}
