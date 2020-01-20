using UnityEngine;

// process the input from the player to tell the game
public class InputManager : MonoBehaviour
{
    // need access to the word manager to tell it we need a key
    WordManager wordManager;

    // Start...
    void Start()
    {
        // get the component we use to run the game with
        wordManager = GetComponent<WordManager>();
    }

    //Update...
    void Update()
    {
        // if you want to leave
        if (Input.GetKey(KeyCode.Escape))
        {
            // quit if a built application
            Application.Quit();
        }
        else
        {
            // used a foreach because we may type multiple letters in on a single frame
            // for every key we press on the keyboard
            foreach (char character in Input.inputString)
            {
                // turn it from a char to a string
                string key = character.ToString();
                // tell the word manager what keys we pressed
                wordManager.PressedAKey(key);
            }
        }
    }
}
