namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        //  FIXME GLOBAL
        //  * Add 'help' as an option for the user
        //  * Ensure the user is informed of the outcome of each action
        //  * Do not allow the user to manipulate the glossary unless it's loaded with data

        private List<Word>? glossary;
        private List<string>? commands;

        public static void Main(string[] args)
        {
            Program dictionaryApplication = new Program();
            if (dictionaryApplication.isBooted())
            {
                dictionaryApplication.run();
            }
            else
            {
                System.Console.WriteLine("The application could not be started.");
            }


        }

        /// <summary>
        /// Initializes the application with standard values
        /// </summary>
        /// <returns>true if the glossary could be loaded from the file, else false.</returns>
        private bool isBooted()
        {
            commands = new List<string>();
            commands.Add("load");
            commands.Add("list");
            commands.Add("new");
            commands.Add("delete");
            commands.Add("translate");
            commands.Add("quit");

            return loadGlossary("dict/sweeng.lis");
        }

        /// <summary>
        /// Runs until the user chooses to stop the program.
        /// </summary>
        private void run()
        {
            Console.WriteLine("Welcome to the dictionary app!");
            do
            {
                Console.Write("> ");
                string userInput = readStdIn();
                if (userInput.Equals("")) continue;

                string[] arguments = userInput.Split();
                if (!hasValidArgument(arguments))
                {
                    if (arguments.Length != 0)
                    {
                        Console.WriteLine("Unknown command: '{0}'", arguments[0]);
                    }
                    else
                    {
                        System.Console.WriteLine("Missing arguments");
                    }
                    continue;
                }

                //  We can now somewhat safely extract the first argument as well
                //  as pass arguments forward.
                string command = arguments[0];
                executeCommand(command, arguments);
            }
            //  FIXME
            //  * Ensure we can exit the program gracefully
            while (true);
        }

        /// <summary>
        /// Ensure that at least the first argument is a valid command.
        /// </summary>
        /// <param name="arguments">User input</param>
        /// <returns>true if the first argument exists in the list of commands, else false.</returns>
        private bool hasValidArgument(string[] arguments)
        {
            if (hasValue(arguments))
            {
                foreach (string argument in arguments)
                {
                    if (commands.Contains(sanitizeInput(argument)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Reads data from standard input and tries to sanitize and validate it.
        /// </summary>
        /// <returns>The input if valid else an empty string.</returns>
        private string readStdIn()
        {
            string input = sanitizeInput(Console.ReadLine() ?? "");
            if (input.Equals(""))
            {
                System.Console.WriteLine("You need to write something!");
            }
            return input;
        }

        /// <summary>
        /// Removes any whitespaces and ensure lowercase.
        /// </summary>
        /// <param name="input">Any given input.</param>
        /// <returns>The sanitized input.</returns>
        private string sanitizeInput(string input)
        {
            if (hasValue(input))
            {
                return input.Trim().ToLower();
            }
            return "";
        }

        /// <summary>
        /// Executes a validated command from the user.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        private void executeCommand(string command, string[] arguments)
        {
            switch (command)
            {
                case "load":
                    {
                        loadGlossary(arguments);
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
            }
        }

        /// <summary>
        /// Loads the default file into the glossary during boot
        /// </summary>
        /// <param name="defaultFile"></param>
        /// <returns></returns>
        private bool loadGlossary(string defaultFile)
        {
            if (isValidPath(defaultFile))
            {
                populateGlossaryList(
                    readFileToArray(defaultFile)
                );
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads the glossary from a custom path.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>true if the file could be loaded into the glossary list, else false.</returns>
        private bool loadGlossary(string[] arguments)
        {
            string customPath = arguments[1];
            if (isValidPath(customPath))
            {
                populateGlossaryList(
                    readFileToArray(customPath)
                    );
                return true;
            }
            else
            {
                System.Console.WriteLine("Invalid path for {0}", customPath);
                return false;
            }
        }

        private bool listWords(string[] arguments)
        {
            //  FIXME
            //  * Ensure we have something in dictionary to iterate over before we do it
            foreach (Word word in glossary)
            {
                //  FIXME 
                //  * Create an overloaded toString() function in Word.cs
                Console.WriteLine($"{word.Origin,-10}  -  {word.Translation,-10}");
            }
            return true;
        }

        private bool newWord(string[] arguments)
        {
            //  FIXME
            //  Sanitize the arguments
            if (arguments.Length == 3)
            {
                glossary.Add(new Word(arguments[1], arguments[2]));
                return true;
            }
            else if (arguments.Length == 1)
            {

                Word? word = convertUserInputToWordObject();
                if (word != null)
                {
                    glossary.Add(word);
                    return true;
                }

                //  FIXME
                //  * Create a function updateGlossaryDatabase() to update the file
            }
            return false;
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
                    if (word.Origin == arguments[1] && word.Translation == arguments[2])
                    {
                        glossary.RemoveAt(i);
                    }
                }
            }
            else if (arguments.Length == 1)
            {
                Word? word = convertUserInputToWordObject();

                //  FIXME
                //  * Call function removeGlossary()
            }

            //  FIXME    
            //  * Call updateGlossaryDatabase()
            return true;
        }

        private bool translateWord(string[] arguments)
        {
            bool hasMatch = false;
            if (arguments.Length == 2)
            {
                foreach (Word word in glossary)
                {
                    if (word.Origin == arguments[1])
                    {
                        word.printTranslation();
                        hasMatch = true;
                    }
                    if (word.Translation == arguments[1])
                    {
                        word.printOrigin();
                        hasMatch = true;
                    }
                }
            }
            else if (arguments.Length == 1)
            {
                Console.WriteLine("Write word to be translated: ");
                string userInputWord = readStdIn();
                foreach (Word word in glossary)
                {
                    if (word.Origin == userInputWord)
                    {
                        word.printTranslation();
                        hasMatch = true;
                    }
                    if (word.Translation == userInputWord)
                    {
                        word.printOrigin();
                        hasMatch = true;
                    }
                }
            }

            if (!hasMatch)
            {
                System.Console.WriteLine("No match found in dictionary!");
            }

            return hasMatch;
        }

        private void quit()
        {

        }

        /// <summary>
        /// Creates a new Word object for each line found in the glossary data file.
        /// </summary>
        /// <param name="data"></param>
        private void populateGlossaryList(string[] data)
        {
            foreach (string entry in data)
            {
                //  FIXME
                //  We should first sanitize and validate the entry
                glossary.Add(new Word(entry));
            }
        }

        /// <summary>
        /// Attempts to read a file into an string array.
        /// Will catch and print any exceptions. and in that case return an array with an empty string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>string array with lines from the file or a single empty string if failed.</returns>
        private string[] readFileToArray(string path)
        {
            try
            {
                if (isValidPath(path))
                {
                    return File.ReadAllLines(path);
                }
                else
                {
                    System.Console.WriteLine("Path {0} not found.", path);
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception.ToString());
            }
            return new string[] { "" };
        }

        /// <summary>
        /// Checks if the file exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true if the file exists else false</returns>
        private bool isValidPath(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Checks if the variable contains a value.
        /// </summary>
        /// <param name="variable">Any data type.</param>
        /// <returns>true as long as the param has a value seperated from null, empty and new line character.</returns>
        private bool hasValue(object variable)
        {
            if (variable != null)
            {
                if (Type.GetTypeCode(variable.GetType()).Equals(TypeCode.String))
                {
                    return !variable.Equals("") && !variable.Equals(Environment.NewLine);
                }

                if (variable.GetType().IsArray)
                {
                    string[] temp = (string[])variable;
                    return temp.Length != 0;
                }

                //  We return true here so we can use this function as a 
                //  pure null check unless the data type matches.
                return true;
            }

            return false;
        }

        /// <summary>
        /// Prompts the user to add origin and translation.
        /// </summary>
        /// <returns>A new Word object.</returns>
        private Word? convertUserInputToWordObject()
        {
            Console.WriteLine("Write word in Swedish: ");
            string origin = readStdIn();
            if (origin.Equals("")) return null;

            Console.Write("Write word in English: ");
            string translation = readStdIn();
            if (translation.Equals("")) return null;

            return new Word(origin, translation);
        }
    }
}