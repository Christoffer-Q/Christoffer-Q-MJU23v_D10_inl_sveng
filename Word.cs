public class Word
{
    public string Origin { get; }
    public string Translation { get; }

    public Word(string origin, string translation)
    {
        this.Origin = origin;
        this.Translation = translation;
    }

    //  FIXME
    //  * Move the logic to the read file function instead
    //  * Remove this constructor
    public Word(string line)
    {
        string[] words = line.Split('|');
        this.Origin = words[0]; this.Translation = words[1];
    }

    public void printTranslation()
    {
        Console.WriteLine($"English for {this.Origin} is {this.Translation}");
    }

    public void printOrigin()
    {
        Console.WriteLine($"Swedish for {this.Translation} is {this.Origin}");
    }

    public override string ToString()
    {
        return $"{Origin,-10}  -  {Translation,-10}";
    }
}