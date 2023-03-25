using static Response;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        //  FIXME GLOBAL
        //  * Ensure the user is informed of the outcome of each action
        //  * Add one dictionary for each language and place them in a dictonary. Use keys to access the correct language.
        //  * Add NUnit tests

        private List<Word> glossary = new List<Word>();
        private List<string> commands = new List<string>();

        //  We initialize an array to be able to reuse it later.
        //  For now we only want the second index to have the default path
        //  to our glossary file.
        private readonly string defaultGlossaryFile = "dict/sweeng.lis";

        public static void Main(string[] args)
        {
            Program dictionaryApplication = new Program();
            if (dictionaryApplication.isBooted().Equals(SUCCESS))
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
        /// <returns>Success if the glossary could be loaded from the file, else Failed.</returns>
        private Response isBooted()
        {
            commands.Add("help");
            commands.Add("load");
            commands.Add("list");
            commands.Add("new");
            commands.Add("delete");
            commands.Add("translate");
            commands.Add("save");
            commands.Add("exit");
            commands.Add("q");
            commands.Add("quit");

            if (!isValidPath(defaultGlossaryFile))
            {
                System.Console.WriteLine("Failed to load default glossary file {0}.", defaultGlossaryFile);
                return FAILED;
            }

            //  We pass an empty string array since we don't want to 
            //  overload the function and create duplicate code.
            if (loadGlossary(new string[] { }).Equals(FAILED))
            {
                System.Console.WriteLine("Unable to load glossary into memory during boot.");
            }

            return SUCCESS;
        }

        /// <summary>
        /// Runs until the user chooses to stop the program.
        /// </summary>
        private void run()
        {
            Response response = FAILED;
            Console.WriteLine("Welcome to the dictionary app!");
            printHelp();
            do
            {
                string userInput = readStdIn();
                if (userInput.Equals("")) continue;

                string[] arguments = userInput.Split();
                if (!isValidCommand(arguments))
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
                response = executeCommand(command, arguments);

                if (response.Equals(FAILED))
                {
                    System.Console.WriteLine("Failed to execute the command: {0}, returning to main menu.", command);
                    continue;
                }
            }
            while (!response.Equals(QUIT));

            //  Try to exit gracefully
            quit();
        }

        /// <summary>
        /// Executes a validated command from the user.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        /// <returns>a response used to determine if we should continue, retry or quit.</returns>
        private Response executeCommand(string command, string[] arguments)
        {
            switch (command)
            {
                case "help": return printHelp();
                case "load": return loadGlossary(arguments);
                case "list": return listWords(arguments);
                case "new": return newWord(arguments);
                case "delete": return deleteWord(arguments);
                case "translate": return translateWord(arguments);
                case "save": return updateFile(arguments);

                case "exit":
                case "q":
                case "quit":
                    return QUIT;
            }

            return FAILED;
        }

        // FIXME document this function
        private Response printHelp()
        {
            System.Console.WriteLine("help - shows this list of commands.");
            System.Console.WriteLine("load - loads a text file into memory.");
            System.Console.WriteLine("list - list all the words in the dictionary.");
            System.Console.WriteLine("new - creates a new word and adds it to the dictionary.");
            System.Console.WriteLine("delete - removes any give word from the dictionary.");
            System.Console.WriteLine("translate - shows the translation of any given word in the dictionary.");
            System.Console.WriteLine("save - saves the glossary list in memory to file.");
            System.Console.WriteLine("exit / q / quit - stops the application.");

            return SUCCESS;
        }

        /// <summary>
        /// Loads the glossary from a custom path.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>Success if the file could be loaded into the glossary list, else Failed.</returns>
        private Response loadGlossary(string[] arguments)
        {
            if (arguments.Length == 2)
            {
                string path = arguments[1];
                if (isValidPath(path))
                {
                    return populateGlossaryList(
                        readFileToArray(path)
                        );
                }
                else
                {
                    System.Console.WriteLine("Invalid path for {0}", path);
                }
            }

            //  At this point we either want to pre-load the data or
            //  the user has not added a second argument to 'load' and
            //  therefore we don't care about the number of arguments.
            return populateGlossaryList(
                        readFileToArray(defaultGlossaryFile)
                        );
        }

        // FIXME document this
        private Response listWords(string[] arguments)
        {
            foreach (Word word in glossary)
            {
                Console.WriteLine(word.ToString());
            }

            return SUCCESS;
        }

        // FIXME document this
        private Response newWord(string[] arguments)
        {
            if (arguments.Length == 3)
            {
                glossary.Add(new Word(arguments[1], arguments[2]));
                return SUCCESS;
            }
            else if (arguments.Length == 1)
            {
                Word? word = convertUserInputToWordObject();
                if (word != null)
                {
                    glossary.Add(word);
                    return SUCCESS;
                }
            }

            return FAILED;
        }

        // FIXME document this
        private Response deleteWord(string[] arguments)
        {
            for (int i = 0; i < glossary.Count; i++)
            {
                Word? word = null;
                if (arguments.Length == 1)
                    word = convertUserInputToWordObject();

                if (arguments.Length == 3)
                    word = glossary[i];

                if (word == null)
                    return FAILED;

                if (word.Origin == arguments[1] && word.Translation == arguments[2])
                {
                    glossary.RemoveAt(i);
                    return SUCCESS;
                }
            }

            return FAILED;
        }

        // FIXME document this
        private Response translateWord(string[] arguments)
        {
            Response isTranslated = FAILED;
            if (arguments.Length == 2)
            {
                foreach (Word word in glossary)
                {
                    if (word.Origin == arguments[1])
                    {
                        word.printTranslation();
                        isTranslated = SUCCESS;
                    }
                    if (word.Translation == arguments[1])
                    {
                        word.printOrigin();
                        isTranslated = SUCCESS;
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
                        isTranslated = SUCCESS;
                    }
                    if (word.Translation == userInputWord)
                    {
                        word.printOrigin();
                        isTranslated = SUCCESS;
                    }
                }
            }

            if (isTranslated.Equals(FAILED))
            {
                System.Console.WriteLine("No match found in dictionary!");
            }

            return isTranslated;
        }

        private Response updateFile(string[] arguments)
        {
            if (arguments.Length == 1)
            {
                return writeGlossaryToFile(arguments[1]);
            }

            if (arguments.Length == 2)
            {
                string path = arguments[1];
                if (isValidPath(path))
                {
                    return writeGlossaryToFile(path);
                }
                else
                {
                    System.Console.WriteLine("Invalid path.");
                }
            }

            return FAILED;
        }

        //  FIXME document this
        private void quit()
        {
            System.Console.WriteLine("Goodbye!");
        }

        /// <summary>
        /// Creates a new Word object for each line found in the glossary data file.
        /// </summary>
        /// <param name="data"></param>
        private Response populateGlossaryList(string[] data)
        {
            glossary = new List<Word>();
            foreach (string entry in data)
            {
                string[] words = entry.Split('|');
                if (words.Length < 2) return FAILED;

                string origin = sanitizeInput(words[0]);
                string translate = sanitizeInput(words[1]);

                if (origin.Equals("") || translate.Equals("")) return FAILED;

                glossary.Add(new Word(origin, translate));
            }

            return SUCCESS;
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

        private Response writeGlossaryToFile(string path)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    foreach (Word word in glossary)
                    {
                        streamWriter.Write(
                            word.Origin + "|" + word.Translation + Environment.NewLine
                            );
                    }
                    streamWriter.Close();
                }
                System.Console.WriteLine("Glossary file {0} updated!", path);
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception.ToString());
                return FAILED;
            }

            return SUCCESS;
        }

        /// <summary>
        /// Ensure that at least the first argument is a valid command.
        /// </summary>
        /// <param name="arguments">User input</param>
        /// <returns>true if the first argument exists in the list of commands, else false.</returns>
        private bool isValidCommand(string[] arguments)
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
            Console.Write("> ");
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
        /// Checks if the file exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true if the file exists else false</returns>
        private bool isValidPath(string path)
        {
            return
            Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute) ||
            File.Exists(path);
        }

        /// <summary>
        /// Checks if the variable contains a value.
        /// </summary>
        /// <param name="variable">Any data type.</param>
        /// <returns>true as long as the param has a value seperated from null, empty and new line character.</returns>
        private bool hasValue(object? variable)
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

            Console.WriteLine("Write word in English: ");
            string translation = readStdIn();
            if (translation.Equals("")) return null;

            return new Word(origin, translation);
        }
    }
}