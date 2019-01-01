using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code;
using NetworkingCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    private Guid messageId;

    private void Start()
    {
          
    }

    private void Update()
    {
        if (messageId != Guid.Empty)
        {
            var x = NetworkManager.Instance;
            var response = x.TryGetResponse(messageId);

            if (response != null)
            {
                messageId = Guid.Empty;
                if (Serializer.Deserialize<BooleanResult>(response.Data).Result)
                {
                    SceneManager.LoadScene("3dTest");
                }
            }
        }
    }

    public void LoginClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        messageId = NetworkManager.Instance.userService.Login(username, password);
    }
}
