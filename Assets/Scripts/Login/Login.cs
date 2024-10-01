using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class Login : MonoBehaviour
{
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:13756/account/login";
    [SerializeField] private string registerEndpoint = "http://127.0.0.1:13756/account/register";
    private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,})";
    
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    
    public void OnLoginClick()
    {
        alertText.text = "Signing in...";
        ActivateButtons(false);

        StartCoroutine(TryLogin());
    }

    public void OnRegisterClick()
    {
        StartCoroutine(TryRegister());
    }

    private IEnumerator TryLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Invalid username";
            ActivateButtons(true);
            yield break;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX))
        {
            alertText.text = "Invalid credentials";
            ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        //WebRequest
        UnityWebRequest request = UnityWebRequest.Post(loginEndpoint, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while(!handler.isDone)
        {
            alertText.text = "Signing in...";
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success)
        {
            string dH = request.downloadHandler.text;
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(dH);

            if (response.code == 0)
            {
                ActivateButtons(false);
                alertText.text = $"Welcome " + ((response.data.adminFlag == 1) ? " Admin" : "");
            }
            else
            {
                switch (response.code)
                {
                    case 1:
                        alertText.text = "Invalid credentials";
                        ActivateButtons(true);
                        break;
                    default:
                        alertText.text = "Corruption detected";
                        ActivateButtons(false);
                        break;

                }
            }
        }
        else
        {
            alertText.text = "Error connecting to server...";
        }

        ActivateButtons(true);

        yield return null;
    }

    private IEnumerator TryRegister()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Invalid username";
            ActivateButtons(true);
            yield break;
        }

        if (!Regex.IsMatch(password, PASSWORD_REGEX))
        {
            alertText.text = "Password requires to be at least 8 characters long, inluding 1 lowercased letter, 1 uppercased letter, and 1 number";
            ActivateButtons(true);
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        //WebRequest
        UnityWebRequest request = UnityWebRequest.Post(registerEndpoint, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while(!handler.isDone)
        {
            alertText.text = "Signing in...";
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        if(request.result == UnityWebRequest.Result.Success)
        {
            string dH = request.downloadHandler.text;
            Debug.Log(dH);
            RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(dH);

            if (response.code == 0)
            {
                alertText.text = "Account has been created";
            }
            else
            {
                switch (response.code)
                {
                    case 1: 
                        alertText.text = "Invalid credentials";
                        break;
                    case 2:
                        alertText.text = "Username is already taken";
                        break;
                    case 3:
                        alertText.text = "Password requires to be at least 8 characters long, inluding 1 lowercased letter, 1 uppercased letter, and 1 number";
                        break;
                    default:
                        alertText.text = "Corruption detected";
                        break;
                }
            }
        }
        else
        {
            alertText.text = "Error connecting to server...";
        }
        ActivateButtons(true);

        yield return null;
    }

    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
        registerButton.interactable = toggle;
    }
}
