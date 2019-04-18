using Assets.Code;
using NetworkingCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;

    public void LoginClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        var messageId = NetworkManager.Instance.userService.Login(username, password);
        NetworkManager.Instance.RegisterCallback(messageId, OnLoginComplete);
    }

    private void OnLoginComplete(object result)
    {
        if ((BooleanResult)result)
        {
            SceneManager.LoadScene("CustomCard");
        }
        else
        {
            Debug.Log("Login failed");
        }
    }
}
