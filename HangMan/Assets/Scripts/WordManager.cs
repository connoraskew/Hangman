using UnityEngine;
using TMPro;

// uses the input from from the player to work through the logic that is hangman
// this is what runs the game
public class WordManager : MonoBehaviour
{
    // active word to guess
    public Word activeWord;
    // just a string we use to store all the keys we have rpessed during each run
    string guessedLetters;
    // int used to count how many letters we have gotten right, includes double lettered words like goose or ferret
    int correctLetters;

    // just an int to use as a control of difficulty, 
    public int maxFailedAttempts;
    // private int to store how many letters we have gotten wrong on each run
    int failedAttempts;

    // floats used to control the timed aspect
    public float time;
    float actualTime;

    // I use game objects here for instantiating each letter in the guessing section
    public GameObject guessedTextParent;
    public GameObject guessedTextPrefab;

    // text to show what letters you have used but arent in the actual word
    public TextMeshProUGUI failedText;
    // cheap way of doing a visual, it prints out a fraction showing how many times you have failed
    // as well as your max attempts
    public TextMeshProUGUI guessedAttempts;
    // the clock UI
    public TextMeshProUGUI clock;

    // button used to reset the game
    public GameObject resetButton;

    // bols to control the state of the game
    bool failed;
    bool succeed;

    // Start
    void Start()
    {
        // setting up the time set in the inspector ready to be reset 
        actualTime = time;
        // used in a reset function for the reset button
        Reset();
    }

    void Update()
    {
        // if the current game is still going
        if (!succeed && !failed)
        {
            // taking away from the time left
            time -= Time.deltaTime;
            // setting the UI to show the correct time left
            clock.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(time).ToString();

            // if the clock runs out
            if (time <= 0)
            {
                // failed at guessing the word
                FailedWord();
            }
        }
    }

    // called from start and the reset button
    public void Reset()
    {
        // turn off the reset button when we reset the game
        resetButton.SetActive(false);

        // bools to control the state of the game
        failed = false;
        succeed = false;

        // setting the time limit
        time = actualTime;

        // get a word for the game
        AddWord();
        // set the guessed text after finding a word
        SetTexts();
    }

    // find a word from the word generator script
    public void AddWord()
    {
        // for each of the letters in the guessed word,
        for (int i = 0; i < guessedTextParent.transform.childCount; i++)
        {
            // destroy them, so we can instatiate the right amount of lettered spaces for the next run
            Destroy(guessedTextParent.transform.GetChild(i).gameObject);
        }
        // set active word so we know what were working with
        activeWord = new Word(WordGenerator.GetRandomWord());

        // for each of the letters in the current active word
        for (int i = 0; i < activeWord.word.Length; i++)
        {
            // create a instance of a prefab used to control each letter in the guessed word
            Instantiate(guessedTextPrefab, guessedTextParent.transform);
        }
        // after getting a fresh word, reset the correctly guessed letters count
        correctLetters = 0;
        // reset the failed attemps count
        failedAttempts = 0;
        // reset the string used to track the typed letters
        guessedLetters = "";
    }

    void SetTexts()
    {
        // for all the children we instantiated, 
        for (int i = 0; i < guessedTextParent.transform.childCount; i++)
        {
            // get the text component
            TextMeshProUGUI text = guessedTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            // assign the text so it looks like a blank slate
            text.text = "_";
        }
        // set the guessed letters text to be blank at the start of a new run
        failedText.text = "";
        // set the chances visualisation so the player knows how many attempts they have at the start
        guessedAttempts.text = failedAttempts.ToString() + "/" + maxFailedAttempts.ToString();
        // set the colours as a visual aid
        SetTextColours(Color.white);
    }

    // called from the input manager to work through the hangman logic process
    public void PressedAKey(string _keyPressed)
    {
        // if we havent won or lost already
        if (!succeed && !failed)
        {
            // if the active word has the letter we just pressed/typed
            if (activeWord.word.Contains(_keyPressed))
            {
                // for reach letter in the active word
                for (int i = 0; i < activeWord.word.Length; i++)
                {
                    // if the letter is the key we pressed
                    if (activeWord.word[i].ToString() == _keyPressed)
                    {
                        // if the guessed string is null
                        if (guessedLetters != null)
                        {
                            // if the player has not already typed this letter
                            if (!guessedLetters.Contains(_keyPressed))
                            {
                                // add it to the guessed list
                                guessedLetters += _keyPressed;
                            }
                        }
                        else
                        {
                            // if the guessed list already has letters in it
                            guessedLetters += _keyPressed;
                        }
                        // get the text component
                        TextMeshProUGUI text = guessedTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                        // set the text to be what we pressed,
                        // if the word is pig and we press G, it wont change the first two letters from "_" to a G because what we pressed isnt that part of the word
                        // but it will change the 3rd letter from a "_" to a G because the 3rd letter matches what we typed
                        text.text = _keyPressed;
                    }
                }
                // make an int to see how many letters we have to go
                int lettersLeftToGet = 0;
                // for all the children in the guessed work (for each letter in the guessed word
                for (int i = 0; i < guessedTextParent.transform.childCount; i++)
                {
                    // get the text component
                    TextMeshProUGUI text = guessedTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                    // if the text is "_"
                    if (text.text == "_")
                    {
                        // then the player still needs to work on figuring out what this letter is
                        lettersLeftToGet++;
                    }
                }
                // the amount of correct letters is the length of the word, minus what the player has still to find out
                correctLetters = activeWord.word.Length - lettersLeftToGet;
                // if the player has found out all the letters in the word
                if (correctLetters >= activeWord.word.Length)
                {
                    // call completed word function
                    CompletedWord();
                }
            }
            // if the active word doesnt contain the letter just pressed
            else
            {
                // if the text we use to show what the player has pressed doesnt have what we just pressed
                if (!failedText.text.Contains(_keyPressed))
                {
                    // do some string math to find out what we want to add to the text
                    string textToAdd = _keyPressed.ToString() + ", ";
                    // add it to the text
                    failedText.text += textToAdd;
                    // add to the failed attempts count
                    failedAttempts++;
                    // set the failed attempts text using more string math
                    guessedAttempts.text = failedAttempts.ToString() + "/" + maxFailedAttempts.ToString();
                    // if the amount of times we have failed equal to or more than the amount of times we are allowed to fail
                    if (failedAttempts >= maxFailedAttempts)
                    {
                        // call the fail word function
                        FailedWord();
                    }
                }
            }
        }
    }

    // if you guess the word correctly
    void CompletedWord()
    {
        // set the succeed bool to be true
        succeed = true;
        // set the text colours for a visual aid
        SetTextColours(Color.green);
        // debug for dev purposes
        print("Well done!");
        // turn on the button which allows us to reset and do this all over again
        TurnOnResetButton();
    }

    // if you fail at guessing the word
    void FailedWord()
    {
        // set the failed bool to be true
        failed = true;
        // set the colours to show we failed
        SetTextColours(Color.red);
        // debug for dev purpose
        print("Better luck next time");
        // turn on the reset button
        TurnOnResetButton();

        // for each of the letters in the guessed word
        for (int i = 0; i < guessedTextParent.transform.childCount; i++)
        {
            // get the text component
            TextMeshProUGUI text = guessedTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            // show the player what the word was
            text.text = activeWord.word[i].ToString();
        }
    }

    // turn on the button to allow a hard reset of the game
    void TurnOnResetButton()
    {
        // turn on a game object
        resetButton.SetActive(true);
    }

    // visuals for showing the state of the game
    void SetTextColours(Color _colourToChangeTo)
    {
        // for all the letters in the guessed object
        for (int i = 0; i < guessedTextParent.transform.childCount; i++)
        {
            // get their text component
            TextMeshProUGUI text = guessedTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            // set the colour
            text.color = _colourToChangeTo;
        }
        // set more colours for visuals, to show the player
        failedText.color = _colourToChangeTo;
        guessedAttempts.color = _colourToChangeTo;
        clock.GetComponent<TextMeshProUGUI>().color = _colourToChangeTo;
    }
}