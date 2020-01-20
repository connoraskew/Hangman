using UnityEngine;
using UnityEngine.SceneManagement;

// this is used in the device detector scene to load difference scenes based off what device this is running on
public class DeviceDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Check if the device running this is a desktop
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            SceneManager.LoadScene("Hangman");
        } // else if it the device is a hand held, load a difference scene with a keyboard
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            SceneManager.LoadScene("HangmanIOS");

        }
    }
}
