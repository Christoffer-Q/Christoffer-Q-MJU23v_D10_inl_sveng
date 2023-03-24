namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        //  FIXME GLOBAL
        //  * Add 'help' as an option for the user
        //  * Ensure the user is informed of the outcome of each action
        //  * Do not allow the user to manipulate the glossary unless it's loaded with data

        private List<Word> glossary;

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
            Program dictionaryApplication = new Program();
            dictionaryApplication.run();
        }

        private void run()
        {
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
                executeCommand(command, arguments, defaultFile);
            }
            //  FIXME
            //  * Ensure we can exit the program gracefully
            while (true);
        }

        private void executeCommand(string command, string[] arguments, string defaultFile)
        {
            switch (command)
            {
                case "load":
                    {
                        loadGlossary(arguments, defaultFile);
                        break;
                    }
                case "list":
                    {
                        listWords(arguments);
                        break;
                    }
                case "new":
                    {
                        newWord(arguments);
                        break;
                    }
                case "delete":
                    {
                        deleteWord(arguments);
                        break;
                    }
                case "translate":
                    {
                        translateWord(arguments);
                        break;
                    }

                case "quit":
                    {
                        Console.WriteLine("Goodbye!");
                        break;
                    }
                default:
                    {
                        Console.WriteLine($"Unknown command: '{command}'");
                        break;
                    }
            }
        }

        private bool loadGlossary(string[] arguments, string defaultFile)
        {
            if (arguments.Length == 2)
            {
                //  FIXME
                //  * Move file reading into its own function called populateGlossaryList()
                //  * Make sure that arguments[1] is a valid path
                using (StreamReader streamReader = new StreamReader(arguments[1]))
                {
                    glossary = new List<Word>();
                    string line = streamReader.ReadLine();
                    while (line != null)
                    {
                        Word word = new Word(line);
                        glossary.Add(word);
                        line = streamReader.ReadLine();
                    }
                }
            }
            else if (arguments.Length == 1)
            {
                //  FIXME
                //  * Call function populateGlossaryList()
                //  * Make sure to pass the defaultFile
                using (StreamReader streamReader = new StreamReader(defaultFile))
                {
                    glossary = new List<Word>();
                    string line = streamReader.ReadLine();
                    while (line != null)
                    {
                        Word word = new Word(line);
                        glossary.Add(word);
                        line = streamReader.ReadLine();
                    }
                }
            }
            return true;
        }

        private bool listWords(string[] arguments)
        {
            //  FIXME
            //  * Ensure we have something in dictionary to iterate over before we do it
            foreach (Word word in glossary)
            {
                //  FIXME 
                //  * Create an overloaded toString() function in Word.cs
                Console.WriteLine($"{word.origin,-10}  -  {word.translation,-10}");
            }
            return true;
        }

        private bool newWord(string[] arguments)
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
                string origin = Console.ReadLine();
                Console.Write("Write word in English: ");
                //  FIXME
                //  * Move this to function readStdIn()
                string translation = Console.ReadLine();
                glossary.Add(new Word(origin, translation));

                //  FIXME
                //  * Create a function updateGlossaryDatabase() to update the file
            }
            return true;
        }

        private bool deleteWord(string[] arguments)
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
            return true;
        }

        private bool translateWord(string[] arguments)
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
                string userInputWord = Console.ReadLine();
                //  FIXME
                //  * Ensure we have something in dictionary to iterate over before we do it
                foreach (Word word in glossary)
                {
                    if (word.origin == userInputWord)
                        // FIXME 
                        //  * Move to function in Word.cs
                        Console.WriteLine($"English for {word.origin} is {word.translation}");
                    if (word.translation == userInputWord)
                        // FIXME 
                        //  * Move to function in Word.cs
                        Console.WriteLine($"Swedish for {word.translation} is {word.origin}");
                }
            }
            return true;
        }

        private void quit()
        {

        }
    }
}