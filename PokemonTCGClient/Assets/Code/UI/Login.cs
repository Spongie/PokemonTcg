using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;

    public void LoginClick()
    {
        //TODO register username with server
        SceneManager.LoadScene("UI_2D");
    }
}
