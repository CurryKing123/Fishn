using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button cameraButton;

    //Change to Camera Scene
    public void CameraScene()
    {
        SceneManager.LoadScene("PhoneCamera");
    }

    //
}
