using Assets.Code;
using NetworkingCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    private NetworkId messageId = NetworkId.Empty;

    private void Start()
    {
          
    }

    private void Update()
    {
        if (!messageId.Equals(NetworkId.Empty))
        {
            var x = NetworkManager.Instance;
            var response = x.TryGetResponse(messageId);

            if (response != null)
            {
                messageId = NetworkId.Empty;
                if ((BooleanResult)response.Data)
                {
                    SceneManager.LoadScene("CustomCard");
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
