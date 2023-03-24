namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        //  FIXME GLOBAL
        //  * Add 'help' as an option for the user
        //  * Ensure the user is informed of the outcome of each action
        //  * Do not allow the user to manipulate the glossary unless it's loaded with data

        //  FIXME
        //  * Remove static and narrow the access modifier to private
        static List<Word> glossary;

        //  FIXME
        //  * Move class to file
        class Word
        {
            //  FIXME 
            //  * Narrow access modifier to private
            //  * Add getters and setters
            public string origin, translation;

            public Word(string origin, string translation)
            {
                this.origin = origin;
                this.translation = translation;
            }

            //  FIXME
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
            //  FIXME
            //  * Move most of the logic into a run() function
            //  FIXME
            //  * Ensure we can read this path
            string defaultFile = "dict/sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            do
            {
                Console.Write("> ");

                //  FIXME
                //  * Sanitize the input and make it lower case, create new function for it
                //  * Add a new variable to track the length of the arguments array
                string[] arguments = Console.ReadLine().Split();
                string command = arguments[0];
                //  FIXME
                //  * Replace the if-statements with a switch
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                }
                else if (command == "load")
                {
                    if (arguments.Length == 2)
                    {
                        //  FIXME
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
                        //  FIXME
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
                        //  FIXME 
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
                        //  FIXME
                        //  * Move this to function readStdIn()
                        string s = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        //  FIXME
                        //  * Move this to function readStdIn()
                        string e = Console.ReadLine();
                        glossary.Add(new Word(s, e));

                        //  FIXME
                        //  * Create a function updateGlossaryDatabase() to update the file
                    }
                }
                else if (command == "delete")
                {
                    if (arguments.Length == 3)
                    {
                        //  FIXME
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
                        //  FIXME
                        //  * Call function readStdIn()

                        //  FIXME
                        //  * Call function removeGlossary()
                    }

                    //  FIXME    
                    //  * Call updateGlossaryDatabase()
                }
                else if (command == "translate")
                {
                    if (arguments.Length == 2)
                    {
                        //  FIXME
                        //  * Ensure we have something in dictionary to iterate over before we do it
                        foreach (Word word in glossary)
                        {
                            if (word.origin == arguments[1])
                                // FIXME 
                                //  * Move to function in Word.cs
                                Console.WriteLine($"English for {word.origin} is {word.translation}");
                            if (word.translation == arguments[1])
                                // FIXME 
                                //  * Move to function in Word.cs
                                Console.WriteLine($"Swedish for {word.translation} is {word.origin}");
                        }
                    }
                    else if (arguments.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        //  FIXME
                        //  * Move this to function readStdIn()
                        string s = Console.ReadLine();
                        //  FIXME
                        //  * Ensure we have something in dictionary to iterate over before we do it
                        foreach (Word word in glossary)
                        {
                            if (word.origin == s)
                                // FIXME 
                                //  * Move to function in Word.cs
                                Console.WriteLine($"English for {word.origin} is {word.translation}");
                            if (word.translation == s)
                                // FIXME 
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