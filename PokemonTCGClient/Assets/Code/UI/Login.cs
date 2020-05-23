using Assets.Code;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;

    public void LoginClick()
    {
        NetworkManager.Instance.Me.Name = usernameInput.text;
        NetworkManager.Instance.playerService.Login(usernameInput.text, NetworkManager.Instance.Me.Id);

        SceneManager.LoadScene("MainMenu");
    }
}
