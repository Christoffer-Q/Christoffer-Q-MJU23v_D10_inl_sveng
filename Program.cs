namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        //  TODO GLOBAL
        //  * Add 'help' as an option for the user
        //  * Ensure the user is informed of the outcome of each action
        //  * Do not allow the user to manipulate the glossary unless it's loaded with data

        //  FIXME
        //  * Remove static and narrow the access modifier to private
        static List<Word> glossary;

        //  TODO
        //  * Move class to file
        class Word
        {
            //  TODO 
            //  * Narrow access modifier to private
            //  * Add getters and setters
            public string origin, translation;

            public Word(string origin, string translation)
            {
                this.origin = origin;
                this.translation = translation;
            }

            //  TODO
            //  * Move the logic to the read file function instead
            //  * Remove this constructor
            public Word(string line)
            {
                string[] words = line.Split('|');
                this.origin = words[0]; this.translation = words[1];
            }
        }
        public static void Main(string[] args)
        {
            //  TODO
            //  * Move most of the logic into a run() function
            //  FIXME
            //  * Ensure we can read this path
            string defaultFile = "dict/sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            do
            {
                Console.Write("> ");

                //  TODO
                //  * Sanitize the input and make it lower case, create new function for it
                //  * Add a new variable to track the length of the arguments array
                string[] arguments = Console.ReadLine().Split();
                string command = arguments[0];
                //  TODO
                //  * Replace the if-statements with a switch
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                }
                else if (command == "load")
                {
                    if (arguments.Length == 2)
                    {
                        //  TODO
                        //  * Move file reading into its own function called populateGlossaryList()
                        //  * Make sure that arguments[1] is a valid path
                        using (StreamReader sr = new StreamReader(arguments[1]))
                        {
                            glossary = new List<Word>();
                            string line = sr.ReadLine();
                            while (line != null)
                            {
                                Word word = new Word(line);
                                glossary.Add(word);
                                line = sr.ReadLine();
                            }
                        }
                    }
                    else if (arguments.Length == 1)
                    {
                        //  TODO
                        //  * Call function populateGlossaryList()
                        //  * Make sure to pass the defaultFile
                        using (StreamReader sr = new StreamReader(defaultFile))
                        {
                            glossary = new List<Word>();
                            string line = sr.ReadLine();
                            while (line != null)
                            {
                                Word word = new Word(line);
                                glossary.Add(word);
                                line = sr.ReadLine();
                            }
                        }
                    }
                }
                else if (command == "list")
                {
                    //  FIXME
                    //  * Ensure we have something in dictionary to iterate over before we do it
                    foreach (Word word in glossary)
                    {
                        //  TODO 
                        //  * Create an overloaded toString() function in Word.cs
                        Console.WriteLine($"{word.origin,-10}  -  {word.translation,-10}");
                    }
                }
                else if (command == "new")
                {
                    if (arguments.Length == 3)
                    {
                        glossary.Add(new Word(arguments[1], arguments[2]));
                    }
                    else if (arguments.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        //  TODO
                        //  * Move this to function readStdIn()
                        string s = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        //  TODO
                        //  * Move this to function readStdIn()
                        string e = Console.ReadLine();
                        glossary.Add(new Word(s, e));

                        //  TODO
                        //  * Create a function updateGlossaryDatabase() to update the file
                    }
                }
                else if (command == "delete")
                {
                    if (arguments.Length == 3)
                    {
                        //  TODO
                        //  * Move this to function removeGlossary()
                        for (int i = 0; i < glossary.Count; i++)
                        {
                            Word word = glossary[i];
                            if (word.origin == arguments[1] && word.translation == arguments[2])
                            {
                                glossary.RemoveAt(i);
                            }
                        }
                    }
                    else if (arguments.Length == 1)
                    {
                        //  TODO
                        //  * Call function readStdIn()

                        //  TODO
                        //  * Call function removeGlossary()
                    }

                    //  TODO    
                    //  * Call updateGlossaryDatabase()
                }
                else if (command == "translate")
                {
                    if (arguments.Length == 2)
                    {
                        //  TODO
                        //  * Ensure we have something in dictionary to iterate over before we do it
                        foreach (Word word in glossary)
                        {
                            if (word.origin == arguments[1])
                                // TODO 
                                //  * Move to function in Word.cs
                                Console.WriteLine($"English for {word.origin} is {word.translation}");
                            if (word.translation == arguments[1])
                                // TODO 
                                //  * Move to function in Word.cs
                                Console.WriteLine($"Swedish for {word.translation} is {word.origin}");
                        }
                    }
                    else if (arguments.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        //  TODO
                        //  * Move this to function readStdIn()
                        string s = Console.ReadLine();
                        //  TODO
                        //  * Ensure we have something in dictionary to iterate over before we do it
                        foreach (Word word in glossary)
                        {
                            if (word.origin == s)
                                // TODO 
                                //  * Move to function in Word.cs
                                Console.WriteLine($"English for {word.origin} is {word.translation}");
                            if (word.translation == s)
                                // TODO 
                                //  * Move to function in Word.cs
                                Console.WriteLine($"Swedish for {word.translation} is {word.origin}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            //  FIXME
            //  * Ensure we can exit the program gracefully
            while (true);
        }
    }
}