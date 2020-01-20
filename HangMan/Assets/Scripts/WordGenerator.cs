using UnityEngine;

public class WordGenerator : MonoBehaviour
{
    // possible words for the game
    // maybe change this to read from a text file
    // every word for the game can be found here
    private static string[] wordList = { "alpaca","cat","chicken","dog", "camel", "duck", "goose","pig", "pigeon", "rabbit", "skunk", "fox", "donkey", "ferret", "goldfish", "horse",    };

    // function to get a random word in our lsit
    public static string GetRandomWord()
    {
        // get a random number between 0 and our list length
        int randomIndex = Random.Range(0, wordList.Length);
        // assign the word to a string
        string randomWord = wordList[randomIndex];
        // return the random word
        return randomWord;
    }
}












