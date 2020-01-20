// this is a base class for the words we could find
// Im still not sure I need to do this because of how I handle the string manipulation in the other managers
[System.Serializable]
public class Word 
{
    // each word has a string which is here
    public string word;
    // this is used to keep track of how many letters we have typed but maybe dont need it because of string math and things used in the main script
    public Word(string _word)
    {
        word = _word;
    }
}
