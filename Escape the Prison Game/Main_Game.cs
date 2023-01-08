using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Escape_the_Prison_Game
{
    class Main_Game
    {
        static void Main(string[] args)
        {
            Game();
            Console.ReadLine();
        }

        static void Game()
        {
            //Console.Write("Enter path location to documents: ");
            //saveDataHandling.userfilelocation = Console.ReadLine();
            //saveDataHandling.filepath = $@"{saveDataHandling.userfilelocation}\Escape_Prison"; 
            saveDataHandling.filepath = $@"C:\Escape_Prison";
            //C:\Users\yonat\Documents
            bool loop = false;
            int folderCount = 0;
            Display display = new Display();   
            Console.WriteLine("Commands: \nNEW GAME\nLOAD GAME\nEXIT");  
            while (loop == false)
            //making sure the only way the user can close the program is by exiting
            {
                display.menuDisplay();
                Puzzles.Tracker = new List<string>();
                Console.Write("What would you like to do: ");
                Puzzles.userStringInput = Console.ReadLine().ToUpper();
                try
                {
                    if (System.IO.Directory.Exists(saveDataHandling.filepath)) 
                    { 
                        folderCount = System.IO.Directory.GetDirectories(saveDataHandling.filepath).Length;
                        int numberofFolders = 0;
                        //this checks to see how many folders actually contain a text file, rather than containing nothing
                        for (int i = 1; i <= folderCount; i++)
                        {
                            if (System.IO.File.Exists($@"{saveDataHandling.filepath}\Game_{i}\Progress.xml"))
                            {
                                numberofFolders += 1;
                            }
                        }
                        folderCount = numberofFolders;
                    }
                    if (Puzzles.userStringInput == "NEW GAME" || Puzzles.userStringInput == "NEWGAME" || Puzzles.userStringInput == "N")
                        //making sure that either input is allowed for the user to progress through the game.
                    {
                        display = new Display("NEW GAME");
                        if (saveDataHandling.saveDataSlot < 4)
                        {
                            //when user closes application and load it up again, the program does not recognise that the same file already exists.
                            saveDataHandling.pathString = System.IO.Path.Combine(saveDataHandling.filepath, $"Game_{saveDataHandling.saveDataSlot}");
                            saveDataHandling.pathString2 = $@"{saveDataHandling.filepath}\Game_{saveDataHandling.saveDataSlot}";
                            while (!System.IO.Directory.Exists(saveDataHandling.pathString) || saveDataHandling.saveDataSlot < 3)
                            //this checks through the escapem game folder, seeing which folders include a text file or not.
                            {
                                if (System.IO.File.Exists($@"{saveDataHandling.pathString2}\Progress.xml"))
                                {
                                    saveDataHandling.saveDataSlot += 1;
                                    saveDataHandling.pathString = System.IO.Path.Combine(saveDataHandling.filepath, $"Game_{saveDataHandling.saveDataSlot}");
                                    saveDataHandling.pathString2 = $@"{saveDataHandling.filepath}\Game_{saveDataHandling.saveDataSlot}";
                                }
                                else
                                {
                                    //if a folder does not include a text file, then create the folder to allow the user to save to it.
                                    System.IO.Directory.CreateDirectory(saveDataHandling.pathString);
                                    break;
                                }
                            }
                        }
                        
                        if (folderCount == 3)
                        {
                            //if the 3 folders to contain text files, the user is required to overwrite one file.
                            Console.Write("You have reached the maximum number of saved data.\nDo you want to overwrite a file (YES or NO)? ");
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (Puzzles.userStringInput == "YES")
                            {
                                
                                display = new Display("LOAD GAME", folderCount);
                                Console.Write("Which file location do you want to overwrite: ");
                                saveDataHandling.saveDataSlot = Convert.ToInt32(Console.ReadLine());
                                bool validLocation = false;
                                while (validLocation == false)
                                //due to the user already saying yes to overwrititng a file, it is safe to assume they are commited to do so,
                                //therefor I use the loop here to make sure they select a valid file.
                                {
                                    if (saveDataHandling.saveDataSlot > 3 || saveDataHandling.saveDataSlot < 0) 
                                    { 
                                        Console.WriteLine("You tried to overwrite a non existing file.");
                                        Console.Write("Which file location do you want to overwrite: ");
                                        saveDataHandling.saveDataSlot = Convert.ToInt32(Console.ReadLine());
                                    }
                                    else
                                    {
                                        //System.IO.File.Delete($@"{saveDataHandling.filepath}\Game_{saveDataHandling.saveDataSlot}\Progress.txt");
                                        System.IO.File.Delete($@"{saveDataHandling.filepath}\Game_{saveDataHandling.saveDataSlot}\Progress.xml");
                                        saveDataHandling.pathString = System.IO.Path.Combine(saveDataHandling.filepath, $"Game_{saveDataHandling.saveDataSlot}");
                                        saveDataHandling.pathString2 = $@"{saveDataHandling.filepath}\Game_{saveDataHandling.saveDataSlot}";
                                        validLocation = true;
                                    }
                                }      
                            }
                        }
                        Display.displayFunction(" EASY ");
                        Display.displayFunction("MEDIUM");
                        Display.displayFunction(" HARD ");
                        Console.Write("Choose Difficulty: ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        bool validInput = false;
                        while (validInput == false)
                        {
                            // a small loop to make sure the user can not input something ridicolous mistake.
                            if (Puzzles.userStringInput == "EASY" || Puzzles.userStringInput == "MEDIUM" || Puzzles.userStringInput == "HARD" || Puzzles.userStringInput == "E" || Puzzles.userStringInput == "M" || Puzzles.userStringInput == "H") 
                            { 
                                if (Puzzles.userStringInput == "E") { Puzzles.userStringInput = "EASY"; }
                                if (Puzzles.userStringInput == "M") { Puzzles.userStringInput = "MEDIUM"; }
                                if (Puzzles.userStringInput == "H") { Puzzles.userStringInput = "HARD"; }
                                validInput = true; 
                            }
                            else { Console.Write("Choose Difficulty: "); Puzzles.userStringInput = Console.ReadLine().ToUpper(); }
                        }
                        IntialGame("NEW GAME", Puzzles.userStringInput);
                        //made this method as now I can make the user go to a saved spot from loaded data via if statements rather than goto commands.
                        //this allows the load game functions to be easier.
                    }
                    if (Puzzles.userStringInput == "LOAD GAME" || Puzzles.userStringInput == "LOADGAME" || Puzzles.userStringInput == "L")
                    {
                        if (folderCount > 0)
                        {
                            //making sure that files include text files to begin with.
                            display = new Display("LOAD GAME", folderCount);
                            Console.Write("What file location do you want to access? ");
                            int userchoice = Convert.ToInt32(Console.ReadLine());
                            bool validLocation = false;
                            while (validLocation == false)
                            {
                                //this again makes sure that the user inputs a valid file location.
                                if (userchoice > folderCount || userchoice < 1) 
                                { 
                                    Console.WriteLine("You are trying to access data that does not exist.");
                                    Console.Write("What file location do you want to access? ");
                                    userchoice = Convert.ToInt32(Console.ReadLine());
                                }
                                else
                                {
                                    saveDataHandling.saveDataSlot = userchoice;                                   
                                    saveDataHandling.pathString2 = $@"{saveDataHandling.filepath}\Game_{userchoice}";
                                    validLocation = true;
                                }
                            }
                            IntialGame("LOAD GAME", "");

                            // saved data ... percentage - are you sure you want to access file.
                        }
                        else { Display.displayFunction("No Saved Data"); }
                        
                    }
                    if (Puzzles.userStringInput == "QUIT" || Puzzles.userStringInput == "Q")
                    {
                        System.Environment.Exit(0);
                        //if the user quits, the program closes.
                    }
                }
                catch 
                { 
                    Console.WriteLine("Error");
                }
            }
            
        }
        static void populateTracker(string type, int occurances)
        {
            string[] keys = new string[] { };
            for (int i = 0; i < occurances; i++)
            {
                if (type == "OBTAINED")
                {
                    foreach (var value in Character.Inventory)
                    {
                        foreach(var key in value)
                        {
                            if (key.Value == $"Pin {i + 1}") { Puzzles.Tracker.Add($"{type} PIN"); }
                            if (key.Value == "SEQUENCE_SOLVER" && !Puzzles.Tracker.Contains($"OBTAINED SEQUENCE_SOLVER")) { Puzzles.Tracker.Add($"{type} SEQUENCE_SOLVER"); }
                            if (key.Value == "CALCULATOR" && !Puzzles.Tracker.Contains($"OBTAINED CALCULATOR")) { Puzzles.Tracker.Add($"{type} CALCULATOR"); }
                            if (key.Key == $"Key {i + 1}" && !Puzzles.Tracker.Contains($"OBTAINED : ")) 
                            {
                                if (key.Value == "N-HANDLE") { Puzzles.Tracker.Add($"{type} : N KEY 1"); }
                                if (key.Value == "S-HANDLE") { Puzzles.Tracker.Add($"{type} : S KEY 1"); }
                                if (key.Value == "N-KEY_SHAFT") { Puzzles.Tracker.Add($"{type} : N KEY 2"); }
                                if (key.Value == "S-KEY_SHAFT") { Puzzles.Tracker.Add($"{type} : S KEY 2"); }
                                if (key.Value == "N-COLLAR") { Puzzles.Tracker.Add($"{type} : N KEY 3"); }
                                if (key.Value == "S-COLLAR") { Puzzles.Tracker.Add($"{type} : S KEY 3"); }
                                if (key.Value == "N-TEETH") { Puzzles.Tracker.Add($"{type} : N KEY 3"); }
                                if (key.Value == "S-TEETH") { Puzzles.Tracker.Add($"{type} : S KEY 3"); }
                                //Puzzles.Tracker.Add($"{type} {key.Value}"); 
                            }
                            if (key.Value == "BOOK" && !Puzzles.Tracker.Contains($"OBTAINED BOOK")) { Puzzles.Tracker.Add($"{type} BOOK"); }
                        }
                    }
                }
                if (type == "SUICIDE") { Puzzles.Tracker.Add("SUICIDE"); }
                if (type == "DEAD") { Puzzles.Tracker.Add("DEAD"); }
                if (type == "CAUGHT") { Puzzles.Tracker.Add("CAUGHT"); }
                if (type == "FINISH") { Puzzles.Tracker.Add("FINISH"); }
                else if (type == "OPEN" || type == "LEAVE" || type == "SEARCH")
                {
                    Puzzles.Tracker.Add($"{type} - STAGE {i}");
                }
            }
        }
        static void IntialGame(string modeType, string difficulty)
        {
            bool stage0 = false;
            bool stage1 = false;
            bool stage2 = false;
            bool stage3 = false;
            bool stage4 = false;
            bool stage5 = false;
            bool sequencesolver = false;
            bool book = false;
            bool nextstage = false;
            Puzzles puzzle = new Puzzles();
            Character character = new Character();
            Display display = new Display();
            if (modeType == "NEW GAME")
            {
                character = new Character("NEW GAME", difficulty);
                Puzzles.loot = 1;
                Puzzles.Tracker = new List<string>();
                Puzzles.Tracker_In_Order = new Dictionary<string, int>();
                Console.WriteLine("Commands: \nOPEN\nSEARCH\nLEAVE\nCHECK INVENTORY\nOPTIONS\nYELL\n? - To Help if you forget commands");
                Console.WriteLine("\n You awake with a start. Your ears are ringing, and the air tastes metallic. \n A shuddering breath fills your nostrils with dust, and you begin to cough. You blink the sleep from your eyes. \n The buzz of yellow light illuminates your small prison cell, you know nothing of how you got here.\nYou must find a way to escape before your life comes to an end.");
                character.statsDisplay("NEW GAME");
                puzzle.newGame();
                stage0 = false;
                stage1 = false;
                stage2 = false;
                stage3 = false;
                stage4 = false;
                //this basically sets everything up for a new game,
                //making sure that nothing can accidently be set, changing the flow of the game.
            }    
            if (modeType == "LOAD GAME")
            {                
                character = new Character("LOAD GAME");
                XmlTextReader reader = new XmlTextReader($@"{saveDataHandling.filepath}\Game_{saveDataHandling.saveDataSlot}\Progress.xml");
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Text:
                            if (reader.Value.Contains("HEALTH") || reader.Value.Contains("FITNESS") || reader.Value.Contains("IQ")) { character = new Character(reader.Value); }
                            if (reader.Value == "EASY" || reader.Value == "MEDIUM" || reader.Value == "HARD") { difficulty = reader.Value; }
                            if (reader.Value.Contains("Tool") || reader.Value.Contains("Key"))
                            {
                                string[] array = reader.Value.Split(':');
                                for (int j = 0; j <= 7 ; j++)
                                {
                                    if (array[0] == $"Tool {j}") { Character.Inventory[0].Add(array[0], array[1]); break; }
                                    if (array[0] == $"Key {j}") { Character.Inventory[1].Add(array[0], array[1]); break; }
                                }
                            }
                            if (reader.Value.Contains("STAGE"))
                            {
                                if (reader.Value == "STAGE 0") { stage0 = true; }
                                if (reader.Value == "STAGE 1") { stage1 = true; }
                                if (reader.Value == "STAGE 2") { stage2 = true; }
                                if (reader.Value == "STAGE 3") { stage3 = true; }
                                if (reader.Value == "STAGE 4") { stage4 = true; }
                            }
                            if (reader.Value.Contains("OPEN") || reader.Value.Contains("LEAVE") || reader.Value.Contains("OBTAINED") || reader.Value.Contains("SEARCH")
                                || reader.Value.Contains("SUICIDE") || reader.Value.Contains("CAUGHT") || reader.Value.Contains("DEAD") || reader.Value.Contains("FINISH"))
                            {
                                string[] array = reader.Value.Split(' ');
                                populateTracker(array[1], Convert.ToInt32(array[0]));

                            }
                            if (reader.Value.Contains("PROBLEM")) { puzzle.loadGame(reader.Value); }
                            break;
                    }
                }
                reader.Close();
                puzzle.loadGame();


            }
            bool menuChoice = false;
            while (menuChoice == false)
            {
                if (Puzzles.Tracker.Contains("FINISH"))
                {
                    Console.WriteLine("You have completed the game, start a new game to play the game.");
                }
                //Template for different stages.

                //this basically sets up the game for the following puzzles.
                //gives the user information and a sort of tutorial into the game, as it is hard not to complete the stage and leave.
                while (stage0 == false)
                {
                    if (Puzzles.Tracker.Contains("SUICIDE"))
                    {
                        //in case the user tries to load a game where they have died.
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break;
                    }
                    if (character.checkDead()) { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break; }
                    character.user_Location("CELL");
                    Console.Write("What do you do? ");
                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                    Console.WriteLine();
                    if (Puzzles.userStringInput == "DIE" || Puzzles.userStringInput == "D")
                    {
                        //one of the endings, were you die and can not respoawn.
                        Display.displayFunction("You killed yourself");
                        Puzzles.Tracker.Add("SUICIDE");
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); menuChoice = true; break;
                    }
                    else if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nOPEN\nSEARCH\nLEAVE\nCHECK INVENTORY\n(LOCATION\nOPTIONS\nYELL\n?"); }
                    else if (Puzzles.userStringInput == "OPTIONS" || Puzzles.userStringInput == "OPTIONS" || Puzzles.userStringInput == "OP")
                    {
                        Puzzles.userStringInput = display.optionsDisplay();
                        if (Puzzles.userStringInput == "QUIT" || Puzzles.userStringInput == "Q") { System.Environment.Exit(0); }
                        if (Puzzles.userStringInput == "DIFFICULTY" || Puzzles.userStringInput == "D")
                        {
                            Display.displayFunction(" EASY ");
                            Display.displayFunction("MEDIUM");
                            Display.displayFunction(" HARD ");
                            Console.Write("What do you want to change the game difficulty to: ");
                            difficulty = Console.ReadLine().ToUpper();
                            bool validInput = false;
                            while (validInput == false)
                            {
                                if (difficulty == "EASY" || difficulty == "MEDIUM" || difficulty == "HARD" || difficulty == "E" || difficulty == "M" || difficulty == "H") 
                                {
                                    if (difficulty == "E") { difficulty = "EASY"; }
                                    if (difficulty == "M") { difficulty = "MEDIUM"; }
                                    if (difficulty == "H") { difficulty = "HARD"; }
                                    validInput = true; 
                                }
                                else { Console.Write("Enter difficulty correctly: "); difficulty = Console.ReadLine().ToUpper(); }
                                Console.WriteLine();
                            }
                            Display.displayFunction("DIFFICULTY CHANGED");
                        }
                        if (Puzzles.userStringInput == "RESUME" || Puzzles.userStringInput == "R") { }
                        if (Puzzles.userStringInput == "SAVE" || Puzzles.userStringInput == "S") { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); }
                        if (Puzzles.userStringInput == "CHECK STATS" || Puzzles.userStringInput == "C") { character.statsDisplay(); }
                        if (Puzzles.userStringInput == "MENU" || Puzzles.userStringInput == "M") { stage0 = true; menuChoice = true; }
                        if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nOPEN\nSEARCH\nLEAVE\nnCHECK INVENTORY\nLOCATION\nOPTIONS\nYELL\n?"); }
                    }
                    else if (Puzzles.userStringInput == "LOCATION" || Puzzles.userStringInput == "LO") { Console.WriteLine(character.user_Location()) ; }
                    else if (Puzzles.userStringInput == "CHECK INVENTORY" || Puzzles.userStringInput == "INVENTORY" || Puzzles.userStringInput == "C") { character.statsDisplay("INVENTORY"); }
                    else if (Puzzles.userStringInput == "SEARCH" || Puzzles.userStringInput == "S")
                    {
                        if (Puzzles.Tracker.Contains("SEARCH - STAGE 0"))
                        {
                            Console.WriteLine("You alreadys searched this room");
                        }
                        else
                        {
                            Console.WriteLine($"You look around the room, and find {3 - Puzzles.loot} bent pin(s).");
                            while (Puzzles.loot <= 2)
                            {
                                Console.Write($"Do you want to pick up pin {Puzzles.loot}? (YES/NO) ");
                                Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                if (Puzzles.userStringInput == "YES")
                                {
                                    Character.Inventory[0].Add($"Tool {Puzzles.loot}", $"Pin {Puzzles.loot}");
                                    Console.WriteLine($"Picked up pin {Puzzles.loot}");
                                    Puzzles.Tracker.Add($"OBTAINED PIN");
                                    Puzzles.loot += 1;
                                }
                                if (Puzzles.userStringInput == "NO") { break; }
                            }
                            if (Puzzles.loot == 3) { Puzzles.Tracker.Add("SEARCH - STAGE 0"); }
                        }
                    }
                    else if (Puzzles.userStringInput == "LEAVE" || Puzzles.userStringInput == "L")
                    {
                        
                        if (Puzzles.Tracker.Contains("OPEN - STAGE 0") && !Puzzles.Tracker.Contains("YELL"))
                        {
                            if (Guard.isOfficerPresent())
                            {
                                Console.WriteLine("You were caught by the prison guard trying to open your cell, he beat you and shoved you back in.\nHowever you still have your pins");
                                character.numberoftimescaught += 1;
                                character.totalHealth(20);
                            }
                            else
                            {
                                Console.WriteLine("You have left your cell.");
                                stage0 = true;
                                Puzzles.Tracker.Add("LEAVE - STAGE 0");
                            }
                        }
                        else { Console.WriteLine("You have not yet unlocked the door to the cell"); }

                        if (Puzzles.Tracker.Contains("YELL"))
                        {
                            Console.WriteLine("You were caught by the prison guard trying to leave your cell");
                            character.numberoftimescaught += 1;
                            character.totalHealth(20);
                            Puzzles.Tracker.Remove("YELL");
                        }
                    }
                    else if (Puzzles.userStringInput == "OPEN" || Puzzles.userStringInput == "O")
                    {
                        if (Puzzles.Tracker.Contains("OPEN - STAGE 0")) { Console.WriteLine("You have already opened the cell door"); }
                        else if (Puzzles.Tracker.Contains("SEARCH - STAGE 0") && !Puzzles.Tracker.Contains("YELL"))
                        {
                            if (Guard.isOfficerPresent())
                            {
                                Console.WriteLine("You were caught by the prison guard trying to open your cell, he beat you and shoved you back in.\nHowever you still have your pins");
                                character.numberoftimescaught += 1;
                                character.totalHealth(20);
                            }
                            else if (PathAuthorization.validRoute("PIN")) { Console.WriteLine("You managed to unlock the jail cell door."); Puzzles.Tracker.Add("OPEN - STAGE 0"); }
                            else { Console.WriteLine("You need to find something else."); }
                        }
                        else if (!Puzzles.Tracker.Contains("YELL")) { Console.WriteLine("The door was locked, you need to find something to pick it."); }
                        
                        if (Puzzles.Tracker.Contains("YELL"))
                        {
                            Console.WriteLine("You were caught by the prison guard trying to open your cell");
                            character.numberoftimescaught += 1;
                            character.totalHealth(20);
                            Puzzles.Tracker.Remove("YELL");
                        }                  
                        
                        
                    }
                    else if (Puzzles.userStringInput == "YELL" || Puzzles.userStringInput == "Y")
                    {
                        Console.WriteLine("*You yelled, attracting the guard*");
                        Puzzles.Tracker.Add("YELL");
                    }
                    else if (character.numberoftimescaught == 3)
                    {
                        Console.WriteLine("The guard took away your tools and locked the door");
                        Display.displayFunction("Your Dead");
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break;
                    }
                    else { Console.WriteLine("Do you want to escape jail or no?"); }
                }                
                if (menuChoice == true) { break; }
                if (Puzzles.Tracker.Contains("SUICIDE") || Puzzles.Tracker.Contains("DEAD")) { break; }

                //this stage contains a simple guessing password hash table algorithm.
                //if the user gets the bad password, they get a key
                //a special key allows a unique ending, but it is not easy for the user to understand what to do to get the special key.
                //else they get a normal key which is used for a basic ending.
                Console.WriteLine();
                if (stage1 == false)
                {
                    if (Puzzles.Tracker.Contains("CAUGHT"))
                    {
                        Console.WriteLine("A siren goes on above you, the room turns red, and with it guard suddenly teleport into the room");
                        Console.WriteLine("You were caught by the guards, You are returned to your cell and can not escape.");
                        Console.WriteLine("Game Over");  break;
                    }
                    else
                    {
                        Console.WriteLine("--- You escaped the jail cell, as you exit, you hear a soft click. The floor drops from beneath you. ---");
                        Console.WriteLine("--- You land with a crash, and appear to have obliterated a small side table. You look around your new room. ---");
                        Console.WriteLine("--- It is similar to your first cell, but these unfamiliar walls contain within them a small table with a computer... ---");
                        character.user_Location("COMPUTER ROOM");
                        Console.WriteLine("NEW COMMAND: USE");
                    }                    
                }                
                while (stage1 == false)
                {
                    if (Puzzles.Tracker.Contains("CAUGHT"))
                    {
                        Console.WriteLine("A siren goes on above you, the room turns red, and with it guard suddenly teleport into the room");
                        Console.WriteLine("You were caught by the guards, You are returned to your cell and can not escape.");
                        Console.WriteLine("Game Over"); 
                        menuChoice = true; puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break;
                    }
                    if (character.checkDead()) { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break; }                    
                    Console.Write("What do you do? ");
                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                    Console.WriteLine();
                    if (Puzzles.userStringInput == "DIE")
                    {
                        Display.displayFunction("You killed yourself");
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5);
                        Puzzles.Tracker.Add("SUICIDE");
                        menuChoice = true;
                        break;
                    }
                    else if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nOPEN\nSEARCH\nLEAVE\nCHECK INVENTORY\nLOCATION\nOPTIONS\nUSE\n?"); }
                    else if (Puzzles.userStringInput == "LOCATION" || Puzzles.userStringInput == "LO") { Console.WriteLine(character.user_Location()); }
                    else if (Puzzles.userStringInput == "OPTIONS" || Puzzles.userStringInput == "OP")
                    {
                        Puzzles.userStringInput = display.optionsDisplay();
                        if (Puzzles.userStringInput == "QUIT" || Puzzles.userStringInput == "Q") { System.Environment.Exit(0); }
                        if (Puzzles.userStringInput == "DIFFICULTY" || Puzzles.userStringInput == "D")
                        {
                            Display.displayFunction(" EASY ");
                            Display.displayFunction("MEDIUM");
                            Display.displayFunction(" HARD ");
                            Console.Write("What do you want to change the game difficulty to: ");
                            difficulty = Console.ReadLine().ToUpper();
                            bool validInput = false;
                            while (validInput == false)
                            {
                                if (difficulty == "EASY" || difficulty == "MEDIUM" || difficulty == "HARD" || difficulty == "E" || difficulty == "M" || difficulty == "H") 
                                {
                                    if (difficulty == "E") { difficulty = "EASY"; }
                                    if (difficulty == "M") { difficulty = "MEDIUM"; }
                                    if (difficulty == "H") { difficulty = "HARD"; }
                                    validInput = true; 
                                }
                                else { Console.Write("Enter difficulty correctly: "); difficulty = Console.ReadLine().ToUpper(); }
                                Console.WriteLine();
                            }
                            Display.displayFunction("DIFFICULTY CHANGED");
                        }
                        if (Puzzles.userStringInput == "RESUME") { }
                        if (Puzzles.userStringInput == "SAVE" || Puzzles.userStringInput == "S") { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); }
                        if (Puzzles.userStringInput == "CHECK STATS" || Puzzles.userStringInput == "C") { character.statsDisplay(); }
                        if (Puzzles.userStringInput == "MENU" || Puzzles.userStringInput == "M") { menuChoice = true; break; }
                        if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nOPEN\nSEARCH\nLEAVE\nCHECK INVENTORY\nOPTIONS\nUSE\n?"); }
                    }
                    else if (Puzzles.userStringInput == "CHECK INVENTORY" || Puzzles.userStringInput == "INVENTORY" || Puzzles.userStringInput == "C") { character.statsDisplay("INVENTORY"); }
                    else if (Puzzles.userStringInput == "OPEN" || Puzzles.userStringInput == "LEAVE" || Puzzles.userStringInput == "O" || Puzzles.userStringInput == "L")
                    {
                        if (nextstage)
                        {
                            if (Puzzles.userStringInput == "OPEN" || Puzzles.userStringInput == "O")
                            {
                                Console.WriteLine("You have managed to open the door");
                                Puzzles.Tracker.Add("OPEN - STAGE 1");
                            }
                            if ((Puzzles.userStringInput == "LEAVE" || Puzzles.userStringInput == "L") && Puzzles.Tracker.Contains("OPEN - STAGE 1"))
                            {
                                Console.WriteLine("You have left the room.");
                                Puzzles.Tracker.Add("LEAVE - STAGE 1"); stage1 = true;
                            }
                        }
                        else { Console.WriteLine("You have to use the computer first."); }
                    }
                    else if (Puzzles.userStringInput == "SEARCH" || Puzzles.userStringInput == "S")
                    {
                        if (Character.InventoryReturn("SEQUENCE_SOLVER")) { sequencesolver = true; book = true; }
                        else if (Character.InventoryReturn("BOOK")) { book = true; }
                        Puzzles.Tracker.Add("SEARCH - STAGE 1");
                        if (!sequencesolver && book)
                        {
                            Console.WriteLine("You find a sequence solver. What could it be for?");
                            Console.Write("Do you want to pick it up? (YES or NO) ");
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (Puzzles.userStringInput == "YES")
                            {
                                Console.WriteLine("Picked up Sequence Solver");
                                Character.Inventory[0].Add($"Tool {Puzzles.loot}", "SEQUENCE_SOLVER");
                                Puzzles.loot += 1;
                                Puzzles.Tracker.Add("OBTAINED SEQUENCE_SOLVER");
                                sequencesolver = true;
                                Console.WriteLine();
                            }
                            else if (Puzzles.userStringInput == "NO") { }
                            else { Console.WriteLine("Already picked up item."); }
                        }
                        else if (!book)
                        {
                            Console.WriteLine("You have found a book, reading it could be useful.");
                            Console.Write("Do you want to pick it up? (YES or NO) ");
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (Puzzles.userStringInput == "YES")
                            {
                                Console.WriteLine("You read the book, it said \"Looking carefully, at problem 3, would be useful to thee.\"");
                                Console.WriteLine("What could that mean. Oh, your IQ increased, thought you couldn't read. Well now you do.");
                                Character.Inventory[0].Add($"Tool {Puzzles.loot}", "BOOK");
                                Puzzles.loot += 1;
                                Puzzles.Tracker.Add("OBTAINED BOOK");
                                character.totaliq(2);
                                book = true;
                                Console.WriteLine();
                            }
                            else if (Puzzles.userStringInput == "NO") { }
                            else if (Character.InventoryReturn("BOOK")) { Console.WriteLine("Already picked up item."); }
                        }
                    }
                    else if (Puzzles.userStringInput == "USE")
                    {
                        if (character.problems(1)) { nextstage = true; }
                        else { }
                    }
                    else { Console.WriteLine("You do not have time to mess around, you just escaped jail!"); }
                }
                if (menuChoice == true) { break; }
                if (Puzzles.Tracker.Contains("SUICIDE")||Puzzles.Tracker.Contains("DEAD")) { break; }

                //this contains a sequence solving algorithm containing a queue and a counting sort, which can only be used if the user picks 
                //up a sequence solver. This is only found if the user chooses to search for a second time.
                Console.WriteLine();
                if (stage2 == false)
                {
                    Console.WriteLine("--- You go to unlock the door and a bang you head. You find that behind it was a brick wall ---");
                    Console.WriteLine("--- You turn around to see the room has completely changed. ---\n--- And with it, another door appears with a sequence lock on it. ---");
                    Console.WriteLine("--- Hopefully this door leads somewhere ---");
                    character.user_Location("MYSTERY ROOM");
                    nextstage = false;
                }                                
                while (stage2 == false)
                {
                    if (character.checkDead()) 
                    {
                        Console.WriteLine("You try to hear what the book is saying more clearly, you put your head up to the book.");
                        Console.WriteLine("The book then suddenly sticks out its tongue and latches onto your face, and bites your head off");
                        Puzzles.Tracker.Add("DEAD");
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); menuChoice = true; break; 
                    }
                    if (Puzzles.Tracker.Contains("CAUGHT"))
                    {
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5);
                        Console.WriteLine("You were caught by the guards, You are returned to your cell and can not escape.");
                        Console.WriteLine("Game Over"); stage2 = true; menuChoice = true;
                    }
                    Console.Write("What do you do? ");
                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                    Console.WriteLine();
                    if (Puzzles.userStringInput == "DIE")
                    {
                        Display.displayFunction("You killed yourself");
                        Puzzles.Tracker.Add("SUICIDE");
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); menuChoice = true; break;
                    }
                    else if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nOPEN\nSEARCH\nLEAVE\nCHECK INVENTORY\nLOCATION\nOPTIONS\n?"); }
                    else if (Puzzles.userStringInput == "LOCATION" || Puzzles.userStringInput == "LO") { Console.WriteLine(character.user_Location()); }
                    else if (Puzzles.userStringInput == "OPTIONS" || Puzzles.userStringInput == "OP")
                    {
                        Puzzles.userStringInput = display.optionsDisplay();
                        if (Puzzles.userStringInput == "QUIT" || Puzzles.userStringInput == "Q") { System.Environment.Exit(0); }
                        if (Puzzles.userStringInput == "DIFFICULTY" || Puzzles.userStringInput == "D")
                        {
                            Display.displayFunction(" EASY ");
                            Display.displayFunction("MEDIUM");
                            Display.displayFunction(" HARD ");
                            Console.Write("What do you want to change the game difficulty to: ");
                            difficulty = Console.ReadLine().ToUpper();
                            bool validInput = false;
                            while (validInput == false)
                            {
                                if (difficulty == "EASY" || difficulty == "MEDIUM" || difficulty == "HARD" || difficulty == "E" || difficulty == "M" || difficulty == "H") 
                                {
                                    if (difficulty == "E") { difficulty = "EASY"; }
                                    if (difficulty == "M") { difficulty = "MEDIUM"; }
                                    if (difficulty == "H") { difficulty = "HARD"; }
                                    validInput = true; 
                                }
                                else { Console.Write("Enter difficulty correctly: "); difficulty = Console.ReadLine().ToUpper(); }
                                Console.WriteLine();
                            }
                            Display.displayFunction("DIFFICULTY CHANGED");
                        }
                        if (Puzzles.userStringInput == "RESUME") { }
                        if (Puzzles.userStringInput == "SAVE" || Puzzles.userStringInput == "S") { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); }
                        if (Puzzles.userStringInput == "CHECK STATS" || Puzzles.userStringInput == "C") { character.statsDisplay(); }
                        if (Puzzles.userStringInput == "MENU" || Puzzles.userStringInput == "M") { stage2 = true; menuChoice = true; }
                        if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nOPEN\nSEARCH\nLEAVE\nCHECK INVENTORY\nLOCATION\nOPTIONS\n?"); }
                    }
                    else if (Puzzles.userStringInput == "CHECK INVENTORY" || Puzzles.userStringInput == "INVENTORY" || Puzzles.userStringInput == "C" || Puzzles.userStringInput == "I") { character.statsDisplay("INVENTORY"); }
                    else if (Puzzles.userStringInput == "SEARCH" || Puzzles.userStringInput == "S")
                    {
                        Puzzles.Tracker.Add("SEARCH - STAGE 2");
                        Console.WriteLine("You look around, and see a book, it seems to be murmering to itself?");
                        Console.Write("Do you want to check it out. (YES OR NO): ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            Console.WriteLine("You get close enough to hear what the book is saying, it seems to be a list of numbers.");
                            if (character.statsReturn("IQ") >= 6)
                            {
                                Console.WriteLine("You have an idea that you might have to order the numbers");
                            }
                            if (character.problems(2)) { nextstage = true; Console.WriteLine("You got another key part, and can remove the lock.");  }
                            else
                            {
                                Console.WriteLine("The book took a bit out of you, your health went down.");
                                character.totalHealth(15);
                            }
                        }
                    }
                    else if (Puzzles.userStringInput == "OPEN" || Puzzles.userStringInput == "LEAVE" || Puzzles.userStringInput == "O" || Puzzles.userStringInput == "L")
                    {
                        if (nextstage)
                        {
                            if (Puzzles.userStringInput == "OPEN" || Puzzles.userStringInput == "O")
                            {
                                Console.WriteLine("You have removed the lock from the door");
                                Puzzles.Tracker.Add("OPEN - STAGE 2");
                            }
                            if ((Puzzles.userStringInput == "LEAVE" || Puzzles.userStringInput == "L") && Puzzles.Tracker.Contains("OPEN - STAGE 2"))
                            {
                                Console.WriteLine("You have left the room.");
                                Puzzles.Tracker.Add("LEAVE - STAGE 2"); stage2 = true;
                            }
                            else if (Puzzles.userStringInput == "LEAVE" || Puzzles.userStringInput == "L")
                            {
                                Console.WriteLine("You have to open the door first.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You have to do something else to open the door");
                        }
                    }
                    else { Console.WriteLine("The book looks hungry, hurry before it tries to eat you!"); }
                }
                if (menuChoice == true) { break; }
                if (Puzzles.Tracker.Contains("SUICIDE") || Puzzles.Tracker.Contains("DEAD")) { break; }

                //this stage is just a break from what the user has already been going through. It is simple with only 2 main commands, if they run, they only get a stat increase,
                //if they walk, they can solve a riddle to get a calculator for the next puzzle.
                //this is interesting as it gives 2 irreversable choices
                //or the user could save, make the choice and if it was wrong, then just load the save point before making the decision.
                Console.WriteLine();
                if (stage3 == false)
                {
                    Console.WriteLine("--- You open the door and it finally does not lead to wall ---");
                    Console.WriteLine("--- Instead it turns to a long corridor that appears not to end ---\n--- You have a sense of unease ---");
                    Console.WriteLine("--- You hope there's nothing waiting for you past the door ---");
                    character.user_Location("SCARY CORRIDOOR");
                    Console.WriteLine("NEW COMMANDS: RUN, WALK");
                    nextstage = false;
                }                
                while (stage3 == false)
                {                    
                    if (character.checkDead()) { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break; }
                    if (Puzzles.Tracker.Contains("CAUGHT"))
                    {
                        Console.WriteLine("You were caught by the guards, You are returned to your cell and can not escape.");
                        Console.WriteLine("Game Over"); stage3 = true;
                    }
                    Console.Write("What do you do? ");
                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                    Console.WriteLine();
                    if (Puzzles.userStringInput == "DIE" || Puzzles.userStringInput == "D")
                    {
                        Display.displayFunction("You killed yourself");
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break;
                    }
                    else if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: RUN\nWALK\nCHECK INVENTORY\nOPTIONS\n?"); }
                    else if (Puzzles.userStringInput == "OPTIONS" || Puzzles.userStringInput == "OP")
                    {
                        Puzzles.userStringInput = display.optionsDisplay();
                        if (Puzzles.userStringInput == "QUIT" || Puzzles.userStringInput == "Q") { System.Environment.Exit(0); }
                        if (Puzzles.userStringInput == "DIFFICULTY" || Puzzles.userStringInput == "D")
                        {
                            Display.displayFunction(" EASY ");
                            Display.displayFunction("MEDIUM");
                            Display.displayFunction(" HARD ");
                            Console.Write("What do you want to change the game difficulty to: ");
                            difficulty = Console.ReadLine().ToUpper();
                            bool validInput = false;
                            while (validInput == false)
                            {
                                if (difficulty == "EASY" || difficulty == "MEDIUM" || difficulty == "HARD" || difficulty == "E" || difficulty == "M" || difficulty == "H") 
                                {
                                    if (difficulty == "E") { difficulty = "EASY"; }
                                    if (difficulty == "M") { difficulty = "MEDIUM"; }
                                    if (difficulty == "H") { difficulty = "HARD"; }
                                    validInput = true; 
                                }
                                else { Console.Write("Enter difficulty correctly: "); difficulty = Console.ReadLine().ToUpper(); }
                                Console.WriteLine();
                            }
                            Display.displayFunction("DIFFICULTY CHANGED");
                        }
                        if (Puzzles.userStringInput == "RESUME" || Puzzles.userStringInput == "R") { }
                        if (Puzzles.userStringInput == "SAVE" || Puzzles.userStringInput == "S") { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); }
                        if (Puzzles.userStringInput == "CHECK STATS" || Puzzles.userStringInput == "C") { character.statsDisplay(); }
                        if (Puzzles.userStringInput == "MENU" || Puzzles.userStringInput == "M") { stage2 = true; menuChoice = true; }
                        if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: RUN\nWALK\nCHECK INVENTORY\nOPTIONS\n?"); }
                    }
                    else if (Puzzles.userStringInput == "CHECK INVENTORY" || Puzzles.userStringInput == "INVENTORY" || Puzzles.userStringInput == "I") { character.statsDisplay("INVENTORY"); }
                    else if (Puzzles.userStringInput == "RUN" || Puzzles.userStringInput == "R")
                    {
                        Console.WriteLine("You decide to start running");
                        Console.WriteLine("Your unease seems justified, as the walls appear to close in.");
                        Console.WriteLine("Your fitness is defintely going to increase.");
                        character.totalFitness(10);
                        if (character.statsReturn("FITNESS") > 40)
                        {                            
                            Console.WriteLine("You see a small opening up ahead, diverging from the long corridor.\nDo you want to take the shortcut (YES/NO).");
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (Puzzles.userStringInput == "YES" || Puzzles.userStringInput == "Y")
                            {
                                Console.WriteLine("You try to take a shortcut, as you travel further, debris start to fall onto your head.\nYou look up and find a large boulder coming to squash you.");
                                Console.WriteLine("In life sometimes the shortcut is not the best way to succed, by the way your died!");
                                Puzzles.Tracker.Add("DEAD");
                                menuChoice = true;
                                puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break;
                            }
                            else
                            {
                                Console.WriteLine("You decide to stick with the path, in doing so, as the walls are about to squish you, they stop suddenly.");
                                Console.WriteLine("With your heart racing, you finally manage to get to the end of the corridor and open this gate to another dimension");
                                stage3 = true;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Whilst running, you vision goes blurry, I think your going to pass out.");
                            Console.WriteLine("No wonder, your fitness is so low, even with your stat increase.");
                            Console.WriteLine("You collapse as the walls close in and kill you.");
                            Puzzles.Tracker.Add("DEAD");
                            menuChoice = true;
                            puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break;
                        }
                        //user can take shortcut - dies
                        //or takes normal path, does not die, next stage
                    }
                    else if (Puzzles.userStringInput == "WALK" || Puzzles.userStringInput == "W")
                    {
                        int relationship = 0;
                        bool riddle = false;
                        //character can guess for a certain number of tries
                        int count = 0;
                        Console.WriteLine("As your walking, you hear something trying to get your attention.");
                        Console.WriteLine("You turn to see an old man, who looks homeless, and he points for you to come towards him");
                        Console.WriteLine("He grabs you with a grip that is unescapable. You look at him and he tells you a riddle");
                        Console.WriteLine("God knows if you get it wrong. The old man asks\n");
                        Console.WriteLine();
                        Console.Write("\"Hey you, what is your name?\" ");
                        string c = Console.ReadLine();
                        Console.Write($"\"Well... {c}, are you not going to ask what my name is?\" ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "WHAT IS YOUR NAME" || Puzzles.userStringInput == "WHAT IS YOUR NAME?" || Puzzles.userStringInput == "YES" 
                            || Puzzles.userStringInput == "WHAT IS IT" || Puzzles.userStringInput == "WHAT IS IT?")
                        {
                            Console.WriteLine($"\"I am glad you asked {c}, my name is Adeodatus Liberius, the wisest of them all\"");
                            relationship += 1;
                        }
                        else { Console.WriteLine("\"You're so rude, my name is Sir to you.\""); relationship--; }
                        Console.WriteLine();
                        Console.Write("\"Tell me, do you know why the call me the wisest man to ever live?\" ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput.Contains("SMART") || Puzzles.userStringInput.Contains("WISE") || Puzzles.userStringInput.Contains("SMARTEST") || Puzzles.userStringInput.Contains("WISEST"))
                        {
                            Console.WriteLine($"\"Exactly {c}. No one has ever been able to solve my riddles, can you?\"");
                            relationship += 1;
                        }
                        else { Console.WriteLine("\"Are you serious, is because I have never met anyone smarter than me. No one can answer my riddles, can you?\""); relationship--; }
                        if (difficulty == "EASY")
                        {
                            string riddle1 = "\"What question can you never answer yes too?\" ";
                            string riddle2 = "\"What is always in front of you and can never be seen?\" ";
                            string riddle3 = "\"What can you break, even if you never pick it up or touch it\" ";
                            string[] riddles = new string[] { riddle1, riddle2, riddle3 };
                            Random random = new Random();
                            int riddlechosen = random.Next(1, 4);
                            Console.Write(riddles[riddlechosen - 1]);
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (riddlechosen == 1)
                            {
                                while (count < 3)
                                {

                                    if (Puzzles.userStringInput == "ARE YOU ASLEEP YET?" || Puzzles.userStringInput == "ARE YOU ASLEEP YET")
                                    {
                                        riddle = true;
                                        character.totaliq(1);
                                        Console.WriteLine();
                                        if (relationship >= 0)
                                        {
                                            Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                            Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                            if (Puzzles.userStringInput == "YES")
                                            {
                                                Console.WriteLine("Picked up Calculator");
                                                Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                                Puzzles.loot += 1;
                                                Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                            break;
                                        }
                                    }
                                    else { count++; Console.WriteLine($"Try again, {3 - count } guesses left"); Puzzles.userStringInput = Console.ReadLine().ToUpper(); }
                                }
                            }
                            if (riddlechosen == 2)
                            {
                                while (count < 3)
                                {
                                    if (Puzzles.userStringInput == "THE FUTURE" || Puzzles.userStringInput == "FUTURE")
                                    {
                                        riddle = true;
                                        character.totaliq(1);
                                        Console.WriteLine();
                                        if (relationship >= 0)
                                        {
                                            Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                            Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                            if (Puzzles.userStringInput == "YES")
                                            {
                                                Console.WriteLine("Picked up Calculator");
                                                Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                                Puzzles.loot += 1;
                                                Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                            break;
                                        }
                                    }
                                    else { count++; Console.WriteLine($"Try again, {3 - count } guesses left"); Puzzles.userStringInput = Console.ReadLine().ToUpper(); }
                                }
                            }
                            if (riddlechosen == 3)
                            {
                                while (count < 3)
                                {

                                    if (Puzzles.userStringInput == "A PROMISE" || Puzzles.userStringInput == "PROMISE")
                                    {
                                        riddle = true;
                                        character.totaliq(1);
                                        Console.WriteLine();
                                        if (relationship >= 0)
                                        {
                                            Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                            Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                            if (Puzzles.userStringInput == "YES")
                                            {
                                                Console.WriteLine("Picked up Calculator");
                                                Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                                Puzzles.loot += 1;
                                                Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                            break;
                                        }
                                    }
                                    else { count++; Console.WriteLine($"Try again, {3 - count } guesses left"); Puzzles.userStringInput = Console.ReadLine().ToUpper(); }
                                }
                            }
                            if (!riddle) { Console.WriteLine("Psst, your wasting my time, your not smart at all"); character.totaliq(-1); }
                            Console.WriteLine("You walk to what seems to be the end of the corrdior, you open the door and instantly start to fall into another dimension");
                            stage3 = true;
                            break;
                        }
                        if (difficulty == "MEDIUM")
                        {
                            string riddle1 = "\"Two in a corner, one in a room, zero in a house, but one in a shelter. What am I?\" ";
                            string riddle2 = "\"What runs, but never walks. Murmurs, but never talks.\nHas a bed, but never sleeps. Has a mouth, but never eats?\" ";
                            string riddle3 = "\"If two's company, and three's a croud, what are four and five?\" ";
                            string[] riddles = new string[] { riddle1, riddle2, riddle3 };
                            Random random = new Random();
                            int riddlechosen = random.Next(1, 4);
                            Console.Write(riddles[riddlechosen - 1]);
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (riddlechosen == 1)
                            {
                                while (count < 2)
                                {
                                    if (Puzzles.userStringInput == "THE LETTER R" || Puzzles.userStringInput == "R")
                                    {
                                        riddle = true;
                                        character.totaliq(1);
                                        Console.WriteLine();
                                        if (relationship >= 0)
                                        {
                                            Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                            Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                            if (Puzzles.userStringInput == "YES")
                                            {
                                                Console.WriteLine("Picked up Calculator");
                                                Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                                Puzzles.loot += 1;
                                                Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                            break;
                                        }
                                    }
                                    else { count++; Console.WriteLine($"Try again, {2 - count } guesses left"); Puzzles.userStringInput = Console.ReadLine().ToUpper(); }
                                }
                            }
                            if (riddlechosen == 2)
                            {
                                while (count < 2)
                                {
                                    if (Puzzles.userStringInput == "A RIVER" || Puzzles.userStringInput == "RIVER")
                                    {
                                        riddle = true;
                                        character.totaliq(1);
                                        Console.WriteLine();
                                        if (relationship >= 0)
                                        {
                                            Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                            Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                            if (Puzzles.userStringInput == "YES")
                                            {
                                                Console.WriteLine("Picked up Calculator");
                                                Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                                Puzzles.loot += 1;
                                                Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                            break;
                                        }
                                    }
                                    else { count++; Console.WriteLine($"Try again, {2 - count } guesses left"); Puzzles.userStringInput = Console.ReadLine().ToUpper(); }
                                }
                            }
                            if (riddlechosen == 3)
                            {
                                while (count < 2)
                                {
                                    if (Puzzles.userStringInput == "9" || Puzzles.userStringInput == "NINE")
                                    {
                                        riddle = true;
                                        character.totaliq(1);
                                        Console.WriteLine();
                                        if (relationship >= 0)
                                        {
                                            Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                            Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                            if (Puzzles.userStringInput == "YES")
                                            {
                                                Console.WriteLine("Picked up Calculator");
                                                Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                                Puzzles.loot += 1;
                                                Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                            break;
                                        }
                                    }
                                    else { count++; Console.WriteLine($"Try again, {2 - count } guesses left"); Puzzles.userStringInput = Console.ReadLine().ToUpper(); }
                                }
                            }

                            if (!riddle) { Console.WriteLine("Psst, your wasting my time, your not smart at all"); character.totaliq(-2); }
                            Console.WriteLine("You walk to what seems to be the end of the corrdior, you open the door and instantly start to fall into another dimension");
                            stage3 = true;
                            break;
                        }
                        if (difficulty == "HARD")
                        {
                            string riddle1 = "\"What english word has three consecutive double letters?\" ";
                            string riddle2 = "\"I come from a mind and always get surronded by wood. Everyone uses me. What am I?\" ";
                            string riddle3 = "\"I have keys but no locls and space and no rooms. You can enter, but you can't go outside. What am I?\" ";
                            string[] riddles = new string[] { riddle1, riddle2, riddle3 };
                            Random random = new Random();
                            int riddlechosen = random.Next(1, 4);
                            Console.Write(riddles[riddlechosen - 1]);
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (riddlechosen == 1)
                            {
                                if (Puzzles.userStringInput == "BOOKKEEPER")
                                {
                                    riddle = true;
                                    character.totaliq(1);
                                    Console.WriteLine();
                                    if (relationship >= 0)
                                    {
                                        Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                        Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                        if (Puzzles.userStringInput == "YES")
                                        {
                                            Console.WriteLine("Picked up Calculator");
                                            Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                            Puzzles.loot += 1;
                                            Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            Console.WriteLine("*You can only use the calculator 3 times*");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                    }
                                }
                            }
                            if (riddlechosen == 2)
                            {
                                if (Puzzles.userStringInput == "PENCIL LEAD" || Puzzles.userStringInput == "LEAD")
                                {
                                    riddle = true;
                                    character.totaliq(1);
                                    Console.WriteLine();
                                    if (relationship >= 0)
                                    {
                                        Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                        Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                        if (Puzzles.userStringInput == "YES")
                                        {
                                            Console.WriteLine("Picked up Calculator");
                                            Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                            Puzzles.loot += 1;
                                            Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            Console.WriteLine("*You can only use the calculator 3 times*");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                    }
                                }
                            }
                            if (riddlechosen == 3)
                            {
                                if (Puzzles.userStringInput == "A KEYBOARD" || Puzzles.userStringInput == "KEYBOARD")
                                {
                                    riddle = true;
                                    character.totaliq(1);
                                    Console.WriteLine();
                                    if (relationship >= 0)
                                    {
                                        Console.WriteLine("\"Your smarter than I thought you were, as a present, you get this:\nA brand new calculator, rarest of them all!\"");
                                        Console.Write("You wonder what year it is as calculators are not rare, \nbut regardless do you want the calculator? (YES/NO)");
                                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                        if (Puzzles.userStringInput == "YES")
                                        {
                                            Console.WriteLine("Picked up Calculator");
                                            Character.Inventory[0].Add($"Tool {Puzzles.loot}", "CALCULATOR");
                                            Puzzles.loot += 1;
                                            Puzzles.Tracker.Add("OBTAINED CALCULATOR");
                                            Console.WriteLine("*You can only use the calculator 3 times*");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("\"Lucky guess, I did have a prize but you were so rude to me, leave before I take your head!\"");
                                    }
                                }
                            }
                            if (!riddle) { Console.WriteLine("Psst, your wasting my time, your not smart at all"); character.totaliq(-3); puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); }
                            Console.WriteLine("You walk to what seems to be the end of the corridor, you open the door and instantly start to fall into another dimension");
                            stage3 = true;
                            break;
                        }
                    }
                    else { Console.WriteLine("Hurry before the walls close in!"); }
                }
                if (menuChoice == true) { break; }
                if (Puzzles.Tracker.Contains("SUICIDE") || Puzzles.Tracker.Contains("DEAD")) { break; }

                //matrices puzzles
                //this involves the user being face to face with a giant calculator.
                //if the user got the calculator, here they can only use it 3 times, before the 'calculator' breaks it.
                //if the user does not manage to get it correct, they die.
                Console.WriteLine();
                if (stage4 == false)
                {
                    Console.WriteLine("--- You walk through the portal and are meet with a booming voice. ---");
                    Console.WriteLine("--- \"There is no way your getting through, you have to be smarter than me to do so, HA HA HA!\" ---");
                    Console.WriteLine("--- You look up to see a CALCULATOR towering over you with a menacing grin ---\n--- Lets hope you can beat it ---");
                    character.user_Location("GUARDIAN ROOM");
                    Console.WriteLine("NEW COMMAND: BEGIN");
                    nextstage = false;
                }
                while (stage4 == false)
                {
                    if (character.checkDead()) 
                    {
                        Console.WriteLine("\"You are as stupid as you look, HA HA HA, your soul is mine!\"");
                        Console.WriteLine("You look up, wondering how you even managed to get in your situation, before your squished to death.");
                        Puzzles.Tracker.Add("DEAD");
                        menuChoice = true;
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break; 
                    }
                    if (Puzzles.Tracker.Contains("CAUGHT"))
                    {
                        Console.WriteLine("You were caught by the guards, You are returned to your cell and can not escape.");
                        Console.WriteLine("Game Over"); stage4 = true;
                    }
                    Console.Write("What do you do? ");
                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                    Console.WriteLine();
                    if (Puzzles.userStringInput == "DIE" || Puzzles.userStringInput == "D")
                    {
                        Display.displayFunction("You killed yourself");
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5);
                        Puzzles.Tracker.Add("SUICIDE");
                        break;
                    }
                    if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nBEGIN\nSEARCH\nOPEN\nLEAVE\nCHECK INVENTORY\nOPTIONS\n?"); }
                    if (Puzzles.userStringInput == "OPTIONS" || Puzzles.userStringInput == "OP")
                    {
                        Puzzles.userStringInput = display.optionsDisplay();
                        if (Puzzles.userStringInput == "QUIT" || Puzzles.userStringInput == "Q") { System.Environment.Exit(0); }
                        if (Puzzles.userStringInput == "DIFFICULTY" || Puzzles.userStringInput == "D")
                        {
                            Display.displayFunction(" EASY ");
                            Display.displayFunction("MEDIUM");
                            Display.displayFunction(" HARD ");
                            Console.Write("What do you want to change the game difficulty to.");
                            bool validInput = false;
                            while (validInput == false)
                            {
                                if (difficulty == "EASY" || difficulty == "MEDIUM" || difficulty == "HARD" || difficulty == "E" || difficulty == "M" || difficulty == "H") 
                                {
                                    if (difficulty == "E") { difficulty = "EASY"; }
                                    if (difficulty == "M") { difficulty = "MEDIUM"; }
                                    if (difficulty == "H") { difficulty = "HARD"; }
                                    validInput = true; 
                                }
                                else { Console.Write("Enter difficulty correctly: "); difficulty = Console.ReadLine().ToUpper(); }
                                Console.WriteLine();
                            }
                            difficulty = Console.ReadLine().ToUpper();
                        }
                        if (Puzzles.userStringInput == "RESUME") { }
                        if (Puzzles.userStringInput == "SAVE" || Puzzles.userStringInput == "S") { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); }
                        if (Puzzles.userStringInput == "CHECK STATS" || Puzzles.userStringInput == "C") { character.statsDisplay(); }
                        if (Puzzles.userStringInput == "MENU" || Puzzles.userStringInput == "M") { menuChoice = true; break; }
                        if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nOPEN\nBEGIN\nSEARCH\nLEAVE\nCHECK INVENTORY\nOPTIONS\n?"); }
                    }
                    else if (Puzzles.userStringInput == "LOCATION" || Puzzles.userStringInput == "LO") { character.user_Location(); }
                    else if (Puzzles.userStringInput == "CHECK INVENTORY" || Puzzles.userStringInput == "INVENTORY" || Puzzles.userStringInput == "C") { character.statsDisplay("INVENTORY"); }
                    else if (Puzzles.userStringInput == "OPEN" || Puzzles.userStringInput == "LEAVE" || Puzzles.userStringInput == "O" || Puzzles.userStringInput == "L")
                    {
                        if (nextstage)
                        {
                            Console.WriteLine("As you struggle to open the door meant for a giant, you see a smaller do besides it.");
                            Console.WriteLine("You open it and a change of pressure immediately throws you through the door.");
                            Console.WriteLine("You wonder where this will take you.");
                            stage4 = true;
                        }
                        else { Console.WriteLine("\"I am not letitng you go anywhere without beating me\", \nbooms the Calculator when you try to make a step around it."); }
                    }
                    else if (Puzzles.userStringInput == "SEARCH" || Puzzles.userStringInput == "S")
                    {
                        if (nextstage)
                        {
                            Console.WriteLine("You find a healing potion but there is a crack in the glass and it is pouring out.\nYou have to drink it now if you want it's full effects");
                            Console.WriteLine("Do you want to drink it (YES/NO)");
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (Puzzles.userStringInput == "YES")
                            {
                                Console.WriteLine("You drink the potion, slowly regaining strength and stamina");
                                character.TotalHealth(20);
                                character.totalFitness(5);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\"I am not letitng you go anywhere without beating me\", \nbooms the Calculator when you try to make a step around it.");
                        }
                    }
                    if (Puzzles.userStringInput == "BEGIN" || Puzzles.userStringInput == "B")
                    {
                        if (character.problems(3, difficulty)) { nextstage = true; }
                        else 
                        {
                            Console.WriteLine("The Calculator laughs as it steps on you, breaking some bones. Your health went down.");                            
                        }
                    }
                }
                if (menuChoice == true) { break; }
                if (Puzzles.Tracker.Contains("SUICIDE") || Puzzles.Tracker.Contains("DEAD")) { break; }

                Console.WriteLine();
                if (stage5 == false)
                {
                    Console.WriteLine("--- You feel a rush of wind as you open your eyes to se yourself falling from the sky ---");
                    Console.WriteLine("--- As you descend, you can make out the shape of a figure and a table, you feel a force pushing you upright ---");
                    Console.WriteLine("--- As you land in the empty chair, feeling a little pain. You look at the figure in front of you. ---");
                    Console.WriteLine();
                    Console.WriteLine("--- \"You... You have been given me the biggest problem I have faced in over 10 centuries.\" ---");
                    Console.WriteLine("--- \"No one dares escape our prison, do you even know where you are.\" ---");
                    Console.WriteLine("--- \"No answer? Well this is a prison given grace by the devil himself, hence the unexplainable events you have experienced.\" ---");
                    Console.WriteLine("--- \"Due to your courage, or stupidness, I do not which one to pick, you get to play a simple game, with the Devil himself.\" ---");
                    Console.WriteLine("--- \"A game of poker, you choose when we start.\" ---");
                    character.user_Location("FINAL BOSS ROOM");
                    Console.WriteLine("NEW COMMAND: BEGIN");
                    nextstage = false;
                }
                while(stage5 == false)
                {
                    if (Character.InventoryReturn("N-TEETH")) { nextstage = true; }
                    if (character.checkDead())
                    {
                        Console.WriteLine("\"You are as stupid as you look, HA HA HA, your soul is mine!\"");
                        Console.WriteLine("You look up, wondering how you even managed to get in your situation, before your squished to death.");
                        Puzzles.Tracker.Add("DEAD");
                        menuChoice = true;
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); break;
                    }
                    if (Puzzles.Tracker.Contains("CAUGHT"))
                    {
                        Console.WriteLine("You were caught by the guards, You are returned to your cell and can not escape.");
                        Console.WriteLine("Game Over"); stage4 = true;
                    }
                    Console.Write("What do you do? ");
                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                    Console.WriteLine();
                    if (Puzzles.userStringInput == "DIE" || Puzzles.userStringInput == "D")
                    {
                        Display.displayFunction("You killed yourself");
                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5);
                        Puzzles.Tracker.Add("SUICIDE");
                        break;
                    }
                    if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nBEGIN\nSEARCH\nLEAVE\nCHECK INVENTORY\nOPTIONS\n?"); }
                    if (Puzzles.userStringInput == "OPTIONS" || Puzzles.userStringInput == "OP")
                    {
                        Puzzles.userStringInput = display.optionsDisplay();
                        if (Puzzles.userStringInput == "QUIT" || Puzzles.userStringInput == "Q") { System.Environment.Exit(0); }
                        if (Puzzles.userStringInput == "DIFFICULTY" || Puzzles.userStringInput == "D")
                        {
                            Display.displayFunction(" EASY ");
                            Display.displayFunction("MEDIUM");
                            Display.displayFunction(" HARD ");
                            Console.Write("What do you want to change the game difficulty to.");
                            bool validInput = false;
                            while (validInput == false)
                            {
                                difficulty = Console.ReadLine().ToUpper();
                                if (difficulty == "EASY" || difficulty == "MEDIUM" || difficulty == "HARD" || difficulty == "E" || difficulty == "M" || difficulty == "H")
                                {
                                    if (difficulty == "E") { difficulty = "EASY"; }
                                    if (difficulty == "M") { difficulty = "MEDIUM"; }
                                    if (difficulty == "H") { difficulty = "HARD"; }
                                    validInput = true;
                                }
                                else { Console.Write("Enter difficulty correctly: "); }
                                Console.WriteLine();
                            }
                        }
                        if (Puzzles.userStringInput == "RESUME") { }
                        if (Puzzles.userStringInput == "SAVE" || Puzzles.userStringInput == "S") { puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5); }
                        if (Puzzles.userStringInput == "CHECK STATS" || Puzzles.userStringInput == "C") { character.statsDisplay(); }
                        if (Puzzles.userStringInput == "MENU" || Puzzles.userStringInput == "M") { menuChoice = true; break; }
                        if (Puzzles.userStringInput == "?") { Console.WriteLine("Commands: \nSTART\nSEARCH\nLEAVE\nCHECK INVENTORY\nOPTIONS\n?"); }
                    }
                    else if (Puzzles.userStringInput == "LOCATION" || Puzzles.userStringInput == "LO") { character.user_Location(); }
                    else if (Puzzles.userStringInput == "CHECK INVENTORY" || Puzzles.userStringInput == "INVENTORY" || Puzzles.userStringInput == "C") { character.statsDisplay("INVENTORY"); }
                    else if (Puzzles.userStringInput == "OPEN" || Puzzles.userStringInput == "LEAVE" || Puzzles.userStringInput == "O" || Puzzles.userStringInput == "L")
                    {
                        if (nextstage)
                        {
                            int normalkeys = character.KeyReturn("NORMAL");
                            int specialkeys = character.KeyReturn("SPECIAL");
                            Console.WriteLine();
                            Console.WriteLine("You attempt to open the door and see that the door will not open. You look around and to see a small keyhole.");
                            Console.WriteLine("You check you pockets, remebering that you collected items, maybe they would be useful.");
                            Console.Write("Do you want to check your inventory? (YES/NO) ");
                            Puzzles.userStringInput = Console.ReadLine().ToUpper();
                            if (Puzzles.userStringInput == "YES")
                            {
                                Console.WriteLine();
                                Console.WriteLine("You check your inventory, finding key parts, and you sense that putting it together may be the way to unlock the door");
                                if (normalkeys == 4)
                                {
                                    Console.Write("Do you want to combine the normal keys?(YES/NO) ");
                                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                    if (Puzzles.userStringInput == "YES")
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("You put the keys together, and with it barely staying together, you manage to unlock the door");
                                        Console.WriteLine("A bead of sweat rolls down your face, as you squint, looking at the sun, feeling a light breeze around you.");
                                        Console.WriteLine("You look around to see green everywhere, freedom at last. A world to explore, you managed to escape hell.");
                                        Puzzles.Tracker.Add("FINISH");
                                        Display.displayFunction("NORMAL ENDING");
                                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5);
                                        stage5 = true;
                                        menuChoice = true;
                                        break;
                                    }
                                    else { }
                                }
                                if (specialkeys == 4 && !Puzzles.Tracker.Contains("FINISH"))
                                {
                                    Console.Write("Do you want to combine the special keys?(YES/NO) ");
                                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                    if (Puzzles.userStringInput == "YES")
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("You put the keys together, as though they a magentically attracted to eachother, you open the door");
                                        Console.WriteLine("You gaze around a greyish room, feeling restrained and wondering what is going on.");
                                        Console.WriteLine("As you are confused, a man comes in, explaining what is going on.");
                                        Console.WriteLine("Turns out, this was the last try to see if you could ever be sane again, you had a mental breakdown, and since have never recovered.");
                                        Console.WriteLine("You realise the demons, the challenges you faced, was all a mental game, it was your bodies last effort to save you, and it worked.");
                                        Puzzles.Tracker.Add("FINISH");
                                        Display.displayFunction("SECRET ENDING");
                                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5);
                                        stage5 = true;
                                        menuChoice = true;
                                        break;

                                    }
                                    else { }
                                }
                                if (specialkeys > 2 && normalkeys > 2 && !Puzzles.Tracker.Contains("FINISH"))
                                {
                                    Console.WriteLine();
                                    Console.Write("Do you want to combine the normal and special keys together?(YES/NO) ");
                                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                                    if (Puzzles.userStringInput == "YES")
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("You put the keys together, but you have a bad feeling about it. You hear the devil snickering behind you.");
                                        Console.WriteLine("Before you can change your mind, the key forces itself into the key hole, the door opens and you are sucked in.");
                                        Console.WriteLine("You come out with a different outfit similar to the guards outfits from when you woke up, as well as the same equipement.");
                                        Console.WriteLine("It dawns on you that the key officially made you a worker of the devil, meant to serve him as a guard for the rest of time.");
                                        Puzzles.Tracker.Add("FINISH");
                                        Display.displayFunction("BAD ENDING");
                                        puzzle.savedGame(difficulty, stage0, stage1, stage2, stage3, stage4, stage5);
                                        stage5 = true;
                                        menuChoice = true;
                                        break;

                                    }
                                    else { }
                                }
                            }
                            else { Console.WriteLine("\"How many times, you are wasting your time staying here, there is nothiing valuable to you here, GO!\""); }
                        }
                        else { Console.WriteLine("\"Where do you think your going, we haven't even played yet\""); }
                    }
                    else if (Puzzles.userStringInput == "SEARCH" || Puzzles.userStringInput == "S")
                    {
                        if (nextstage)
                        {
                            Console.WriteLine("\"I told you there was nothing here, you wasted your time and mine, just leave already\"");
                        }
                        else
                        {
                            Console.WriteLine("\"You do realise there is nothing in this room that can help you\".");
                        }
                    }
                    else if (Puzzles.userStringInput == "BEGIN" || Puzzles.userStringInput == "B")
                    {
                        if (character.problems(4, difficulty)) { nextstage = true; }
                        else
                        {
                            Console.WriteLine("You realise you got weaker, and notice that your opponent is slowly seeping your soul away from you.");
                            Console.WriteLine("Your losing health, don't die at the finish line now.");
                            character.totalHealth(20);
                        }
                    }
                }
            }
        }
    }
    class Character
    {
        protected static int health { get; set; }
        protected static int fitness { get; set; }
        protected static int iq { get; set; }
        public static List<Dictionary<string, string>> Inventory = new List<Dictionary<string, string>>();
        public int numberoftimescaught = 0;
        protected string userLocation { get; set; }
        public Character() { }
        public Character(string mode)
        {
            string[] statspop = new string[] { };
            if (mode == "LOAD GAME")
            {
                Inventory = new List<Dictionary<string, string>>();
                Inventory.Add(new Dictionary<string, string>());
                Inventory.Add(new Dictionary<string, string>());
            }
            if (mode.Contains("HEALTH"))
            {
                statspop = mode.Split(' ');
                health = Convert.ToInt32(statspop[1]);
            }
            if (mode.Contains("FITNESS"))
            {
                statspop = mode.Split(' ');
                fitness = Convert.ToInt32(statspop[1]);
            }
            if (mode.Contains("IQ"))
            {
                statspop = mode.Split(' ');
                iq = Convert.ToInt32(statspop[1]);
            }
        }
        public Character(string mode, string difficulty)
        {
            if (mode == "NEW GAME")
            {
                Inventory = new List<Dictionary<string, string>>();
                Inventory.Add(new Dictionary<string, string>());
                Inventory.Add(new Dictionary<string, string>());
                Random statSetter = new Random();
                bool validStat = false;
                if (difficulty == "EASY")
                {
                    while (validStat == false)
                    {
                        health = statSetter.Next(75, 101);
                        if ((health % 5) == 0) { validStat = true; }
                        else { health = statSetter.Next(75, 101); }
                    }
                    validStat = false;
                    while (validStat == false)
                    {
                        fitness = statSetter.Next(60, 101);
                        if ((fitness % 10) == 0) { validStat = true; }
                        else { fitness = statSetter.Next(60, 101); }
                    }
                    iq = statSetter.Next(6, 11);
                }
                if (difficulty == "MEDIUM")
                {
                    while (validStat == false)
                    {
                        health = statSetter.Next(50, 81);
                        if ((health % 5) == 0) { validStat = true; }
                        else { health = statSetter.Next(50, 81); }
                    }
                    validStat = false;
                    while (validStat == false)
                    {
                        fitness = statSetter.Next(40, 71);
                        if ((fitness % 10) == 0) { validStat = true; }
                        else { fitness = statSetter.Next(40, 71); }
                    }
                    iq = statSetter.Next(4, 8);
                }
                if (difficulty == "HARD")
                {
                    while (validStat == false)
                    {
                        health = statSetter.Next(40, 61);
                        if ((health % 5) == 0) { validStat = true; }
                        else { health = statSetter.Next(40, 61); }
                    }
                    validStat = false;
                    while (validStat == false)
                    {
                        fitness = statSetter.Next(20, 51);
                        if ((fitness % 10) == 0) { validStat = true; }
                        else { fitness = statSetter.Next(20, 51); }
                    }
                    iq = statSetter.Next(1, 6);
                }
            }            
        }
        
        public virtual string user_Location()
        {
            return userLocation;
        }
        public virtual string user_Location(string userlocation)
        {
            this.userLocation = userlocation;
            return userLocation;
        }
        public bool problems(int problemNumber)
        //for puzzles that do not need the difficulty of the game to be involved.
        {
            bool problemSolved = false;
            Puzzles p = new Puzzles();
            if (problemNumber == 1)
            {
                if (p.Problem1())
                {
                    problemSolved = true;
                }
            }
            if (problemNumber == 2)
            {
                if (p.Problem2())
                {
                    problemSolved = true;
                }
            }
            return problemSolved;
        }
        public bool problems(int problemNumber, string difficult)
        //for problems that involve the difficulty of the game.
        {
            bool problemSolved = false;
            Puzzles p = new Puzzles();
            if (problemNumber == 3)
            {
                if (p.Problem3(difficult))
                {
                    problemSolved = true;
                }
            }
            if (problemNumber == 4)
            {
                if (p.Problem6(difficult)) { problemSolved = true; }
            }
            return problemSolved;
        }
        public int KeyReturn(string key)
        {
            int numberofnormalkeys = 0;
            int numberofspecialkeys = 0;
            string[] array = new string[] { };
            foreach (var item in Inventory[1])
            {
                array = item.Value.Split('-');
                if (array[0] == "N") { numberofnormalkeys += 1; }
                if (array[0] == "S") { numberofspecialkeys += 1; }
            }
            if (key == "NORMAL")
            {
                return numberofnormalkeys;
            }
            else if (key == "SPECIAL")
            {
                return numberofspecialkeys;
            }
            else { return 0; }
        }
  
        public bool checkDead()
        {
            if (health <= 0) { return true; }
            //if the user is dead, then if the checkDead() is returned as true, the game ends.
            else { return false; }
            //if there are still alive, the game still runs.
        }
        //polymorisphm:
        public void statsDisplay()
        {
            Display.displayFunction($"Health:   {health}");
            Display.displayFunction($"Fitness:  {fitness}");
            Display.displayFunction($"IQ:        {iq}");
            foreach (var index in Inventory)
            {
                //for each variable (string) in the list.
                foreach (var keyValue in index)//and foreach key and value in the dictionary
                { Display.displayFunction($"  {keyValue.Key}  "); }
                //this function will display to the user, the items they have.
            }
        }
        public void statsDisplay(string type)
        {
            if (type == "NEW GAME")
            //when the user starts a new game, they will have no items in their inventory, however they will have stats,
            //which will be displayed at the start of a new game.
            {
                Display.displayFunction($"Health:   {health}");
                Display.displayFunction($"Fitness:  {fitness}");
                Display.displayFunction($"IQ:        {iq}");
            }
            if (type == "INVENTORY")
            //if the user chooses to look at their inventory only, using the check inventory function
            {
                if (Inventory[0].Count != 0)
                {
                    foreach (var index in Inventory)
                    {
                        foreach (var keyValue in index) { Display.displayFunction($"{keyValue.Key} : {keyValue.Value}"); }
                    }
                    Console.Write("Do you want to remove an item? (YES or NO) ");
                    Puzzles.userStringInput = Console.ReadLine().ToUpper();
                    if (Puzzles.userStringInput == "YES")
                    {
                        Console.Write($"Which item? (e.g Tool 2) ");
                        Puzzles.userStringInput = Console.ReadLine();
                        if (Puzzles.userStringInput.Contains("Tool"))
                        {
                            Inventory[0].Remove(Puzzles.userStringInput);
                            Console.WriteLine("Item removed.");
                        }
                        if (Puzzles.userStringInput.Contains("Key"))
                        {
                            Inventory[1].Remove(Puzzles.userStringInput);
                            Console.WriteLine("Item removed.");
                        }                        
                        string[] array = Puzzles.userStringInput.Split(' ');
                        if (array[0] == "Tool")
                        {
                            int secondaryCount = 1;
                            Dictionary<string, string> temp = new Dictionary<string, string>(Inventory[0]);
                            Inventory[0] = new Dictionary<string, string>();
                            foreach (var tool in temp)
                            {
                                Inventory[0].Add($"Tool {secondaryCount}", tool.Value); secondaryCount++;
                            }
                            Puzzles.loot = Inventory[0].Count;
                        }
                        if (array[0] == "Key")
                        {
                            int secondaryCount = 1;
                            Dictionary<string, string> temp = new Dictionary<string, string>(Inventory[1]);
                            Inventory[1] = new Dictionary<string, string>();
                            foreach (var tool in temp)
                            {
                                Inventory[1].Add($"Key {secondaryCount}", tool.Value); secondaryCount++;
                            }
                            Puzzles.key = Inventory[1].Count;
                        }
                    }
                    else { }
                }
            }
        }
        public int totalHealth (int damage)
        {
            health = health - damage;
            if (health <= 0) { Console.WriteLine("You have died"); health = 0; }
            //checks to see if the user has died.
            return health;
            //returns the health, and stores it for later on.
        }
        public int TotalHealth (int increaseInHealth)
        {
            health = health + increaseInHealth;
            //this makes sure that the user can also increase their health.
            if (health > 100) { health = 100; }
            //if the health is over 100, then the cap is at 100 so health is made 100
            return health;
        }
        public int totalFitness (int hoursofwork)
        {
            fitness = fitness + hoursofwork;
            //user gets fitness added on. 
            if (fitness > 100) { fitness = 100; }
            //makes sure the fitness doesn't go above the stats cap.
            return fitness;
        }
        public int totaliq (int iqincrease)
        {
            iq = iq + iqincrease;
            if (iq > 10) { iq = 10; }
            //makes sure the iq can not surpass 10.
            return iq;
        }        
        public int statsReturn(string stat)
        {
            int statHolder = 0;
            if (stat == "HEALTH") { statHolder = health; }
            if (stat == "FITNESS") { statHolder = fitness; }
            if (stat == "IQ") { statHolder = iq; }
            return statHolder;
        }
        public static  bool InventoryReturn(string tool)
        {
            bool hasItem = false;
            foreach (var type in Inventory)
            {
                if (hasItem == true) { break; }
                foreach (var key in type)
                {
                    if (key.Value == tool) { hasItem = true; }
                }
            }
            return hasItem;
        }
    }
    class PathAuthorization  : Character
    //inheritance
    {
        public override string user_Location(string userlocation) { return base.user_Location(userlocation); }
        //overriding
        public static bool validRoute(string tool)
        {
            int counter = 0;
            if (tool == "PIN")
            {
                int pins = 0;
                counter = 2;
                while (counter != 0)
                {
                    counter -= 1;
                    if (Puzzles.Tracker.Contains($"OBTAINED PIN")) { pins++; }
                    
                }
                if (pins == 2) { return true; }
            }
            return false;
        }
    }
    static class Guard
    {
        public static bool isOfficerPresent()
        {
            Random showUpTime = new Random();
            //allows me to use a random function to basically give a 20% chance of the guard showing up
            int number = showUpTime.Next(1, 5);
            if (number == 1) { return true; }
            else { return false; }
        }
    }
    class Puzzles: Character
    {
        public static List<string> Tracker = new List<string>();
        public static Dictionary<string, int> Tracker_In_Order = new Dictionary<string, int>();
        public static string userStringInput { get; set; }
        public static int userIntegerInput { get; set; }
        public static int key = 1;
        public static int loot = 1;
        static bool problem1 = false;
        static bool problem2 = false;
        static bool problem3 = false;
        static bool problem5 = false;
        static bool problem6 = false;
        public void savedGame(string difficulty, bool Stage1, bool Stage2, bool Stage3, bool Stage4, bool Stage5, bool Stage6)
        {
            saveDataHandling s = new saveDataHandling();
            saveDataHandling.saveDataItems = new List<string>();
            List<string> TrackerReplica = new List<string>(Tracker);
            if (TrackerReplica.Count != 0)
            {
                for (int i = 0; i < TrackerReplica.Count; i++)
                {
                    if (TrackerReplica[i].Contains("OPEN"))
                    {
                        TrackerReplica[i] = "OPEN";
                    }
                    if (TrackerReplica[i].Contains("LEAVE"))
                    {
                        TrackerReplica[i] = "LEAVE";
                    }
                    if (TrackerReplica[i].Contains("SEARCH"))
                    {
                        TrackerReplica[i] = "SEARCH";
                    }
                    if (TrackerReplica[i].Contains("OBTAINED"))
                    {
                        TrackerReplica[i] = "OBTAINED";
                    }
                }
                s.sort(TrackerReplica);
            }

            File.Delete($@"{saveDataHandling.pathString2}\Progress.xml");
            using (XmlWriter writer = XmlWriter.Create($@"{saveDataHandling.pathString2}\Progress.xml"))
            {
                writer.WriteStartElement($"Progress{saveDataHandling.saveDataSlot}");
                writer.WriteElementString("Save", $"{saveDataHandling.saveDataSlot}");
                writer.WriteElementString("stats", $"HEALTH {health}");
                writer.WriteElementString("stats", $"FITNESS {fitness}");
                writer.WriteElementString("stats", $"IQ {iq}");
                writer.WriteElementString("difficulty", difficulty);
                if (Stage1) { writer.WriteElementString("stage", "STAGE 0"); }
                if (Stage2) { writer.WriteElementString("stage", "STAGE 1"); }
                if (Stage3) { writer.WriteElementString("stage", "STAGE 2"); }
                if (Stage4) { writer.WriteElementString("stage", "STAGE 3"); }
                if (Stage5) { writer.WriteElementString("stage", "STAGE 4"); }
                if (Stage5) { writer.WriteElementString("stage", "STAGE 5"); }
                if (problem1) { writer.WriteElementString("problem", "PROBLEM 1"); }
                if (problem2) { writer.WriteElementString("problem", "PROBLEM 2"); }
                if (problem3) { writer.WriteElementString("problem", "PROBLEM 3"); }
                if (problem6) { writer.WriteElementString("problem", "PROBLEM 4"); }
                foreach (var value in Inventory)
                {
                    foreach (var key in value) { writer.WriteElementString("inventory", $"{key.Key}:{key.Value}"); }
                }
                foreach (var value in Tracker_In_Order) { writer.WriteElementString("tracker", $"{value.Value} {value.Key}"); }
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
            }
        }
        public void loadGame()
        {
            loot = Inventory[0].Count + 1;
            key = Inventory[1].Count + 1;
        }
        public void loadGame(string mode)
        {
            if (mode == "PROBLEM 1") { problem1 = true; }
            if (mode == "PROBLEM 2") { problem2 = true; }
            if (mode == "PROBLEM 3") { problem3 = true; }
            if (mode == "PROBLEM 4") { problem6 = true; }            
        }
        public void newGame()
        {
            problem1 = false;
            problem2 = false;
            problem3 = false;
            problem5 = false;
        }
        public void checkCompletedTasks(string[] data)
        {
            problem1 = false;
            problem2 = false;
            problem3 = false;
            problem5 = false;
            int j = 1;
            foreach (string element in data)
            {
                while (j < 6)
                {
                    if (element == $"PROBLEM{j}")
                    {
                        if (j == 1) { problem1 = true; break; }
                        if (j == 2) { problem2 = true; break; }
                        if (j == 3) { problem3 = true; break; }
                        if (j == 4) { problem3 = true; break; }
                        if (j == 5) { problem3 = true; break; }
                    }
                    else { j++; }
                }

            }
        }
        void Calculator()
        //using advanced matrices
        {
            bool loop = false;
            while (loop == false)
            {
                Display.displayFunction("   ADDITION   ");
                Display.displayFunction(" SUBSTRACTION ");
                Display.displayFunction("MULTIPLICATION");
                Display.displayFunction("   DIVISION   ");
                Display.displayFunction("   MATRICES   ");
                Display.displayFunction("     EXIT     ");
                Console.Write("What function do you want to perform? ");
                string function = Console.ReadLine().ToUpper();
                if (function == "EXIT") { loop = true; }
                if (function == "ADDITION" || function == "SUBTRACTION" || function == "MULTIPLICATION" || function == "DIVISION")
                {
                    double result = 0;
                    Console.Write("How many variables are involved? ");
                    int numberofnumbers = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter the variables (Pressing Enter after each one): ");
                    int[] numbersEntered = new int[numberofnumbers];
                    for (int i = 0; i < numberofnumbers; i++)
                    {
                        numbersEntered[i] = Convert.ToInt32(Console.ReadLine());
                    }
                    if (function == "ADDITION")
                    {
                        for (int j = 0; j < numbersEntered.Length; j++)
                        {
                            result = result + numbersEntered[j];
                        }
                        for (int j = 0; j < numbersEntered.Length; j++)
                        {
                            if (j == numbersEntered.Length - 1)
                            {
                                Console.Write($"{numbersEntered[j]} = {result}");
                                break;
                            }
                            Console.Write($"{numbersEntered[j]} + ");
                        }
                        Console.WriteLine(Environment.NewLine);
                    }
                    if (function == "SUBTRACTION")
                    {
                        for (int j = 0; j < numbersEntered.Length; j++)
                        {
                            result = result - numbersEntered[j];
                        }
                        for (int j = 0; j < numbersEntered.Length; j++)
                        {
                            if (j == numbersEntered.Length - 1)
                            {
                                Console.Write($"{numbersEntered[j]} = {result}");
                                break;
                            }
                            Console.Write($"{numbersEntered[j]} - ");
                        }
                        Console.WriteLine(Environment.NewLine);
                    }
                    if (function == "MULTIPLICATION")
                    {
                        result = 1;
                        for (int j = 0; j < numbersEntered.Length; j++)
                        {
                            result = result * numbersEntered[j];
                        }
                        for (int j = 0; j < numbersEntered.Length; j++)
                        {
                            if (j == numbersEntered.Length - 1)
                            {
                                Console.Write($"{numbersEntered[j]} = {result}");
                                break;
                            }
                            Console.Write($"{numbersEntered[j]} X ");
                        }
                        Console.WriteLine(Environment.NewLine);
                    }
                    if (function == "DIVISION")
                    {
                        result = numbersEntered[0] / numbersEntered[1];
                        for (int j = 2; j < numbersEntered.Length; j++)
                        {
                            result = result / numbersEntered[j];
                        }
                        for (int j = 0; j < numbersEntered.Length; j++)
                        {
                            if (j == numbersEntered.Length - 1)
                            {
                                Console.Write($"{numbersEntered[j]} = {result}");
                                break;
                            }
                            Console.Write($"{numbersEntered[j]} * ");
                        }
                        Console.WriteLine(Environment.NewLine);
                    }
                }
                if (function == "MATRICES")
                {
                    Console.WriteLine("What is the size of the matrix? 2x2 or 3x3: ");
                    function = Console.ReadLine();
                    if (function == "2x2")
                    {
                        Console.WriteLine("*Note: write the answer in the order left to right and enter and repeat*");
                        Console.WriteLine("Matrix 1: ");
                        int[,] matrix1 = new int[2, 2];
                        for (int i = 0; i < 2; i++)
                        {
                            string userinput = Console.ReadLine();
                            string[] usernumbers = userinput.Split();
                            matrix1[0, i] = Convert.ToInt32(usernumbers[0]);
                            matrix1[1, i] = Convert.ToInt32(usernumbers[1]);
                        }
                        Console.WriteLine("Matrix 2: ");
                        int[,] matrix2 = new int[2, 2];
                        for (int i = 0; i < 2; i++)
                        {
                            string userinput = Console.ReadLine();
                            string[] usernumbers = userinput.Split();
                            matrix2[0, i] = Convert.ToInt32(usernumbers[0]);
                            matrix2[1, i] = Convert.ToInt32(usernumbers[1]);
                        }
                        Console.WriteLine("   ADDITION   ");
                        Console.WriteLine(" SUBSTRACTION ");
                        Console.WriteLine("MULTIPLICATION");
                        function = Console.ReadLine().ToUpper();
                        int[,] newMatrix = new int[2, 2];
                        if (function == "MULTIPLICATION")
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    newMatrix[j, i] = (matrix1[0, i] * matrix2[j, 0]) + (matrix1[1, i] * matrix2[j, 1]);
                                }
                            }
                        }
                        if (function == "ADDITION")
                        {
                            for (int y = 0; y < 2; y++)
                            {
                                for (int x = 0; x < 2; x++) { newMatrix[x, y] = matrix1[x, y] + matrix2[x, y]; }
                            }
                        }
                        if (function == "SUBTRACTION")
                        {
                            for (int y = 0; y < 2; y++)
                            {
                                for (int x = 0; x < 2; x++) { newMatrix[x, y] = matrix1[x, y] - matrix2[x, y]; }
                            }
                        }
                        Console.WriteLine("The answer is: ");
                        for (int y = 0; y < 2; y++)
                        {
                            for (int x = 0; x < 2; x++)
                            {
                                Console.Write($"{newMatrix[x, y]} ");
                            }
                            Console.WriteLine();
                        }

                    }
                    if (function == "3x3")
                    {
                        Console.WriteLine("*Note: write the answer in the order left to right and enter and repeat*");
                        Console.WriteLine("Matrix 1: ");
                        int[,] matrix1 = new int[3, 3];
                        for (int i = 0; i < 3; i++)
                        {
                            string userinput = Console.ReadLine();
                            string[] usernumbers = userinput.Split();
                            matrix1[0, i] = Convert.ToInt32(usernumbers[0]);
                            matrix1[1, i] = Convert.ToInt32(usernumbers[1]);
                            matrix1[2, i] = Convert.ToInt32(usernumbers[2]);
                        }
                        Console.WriteLine("Matrix 2: ");
                        int[,] matrix2 = new int[3, 3];
                        for (int i = 0; i < 3; i++)
                        {
                            string userinput = Console.ReadLine();
                            string[] usernumbers = userinput.Split();
                            matrix2[0, i] = Convert.ToInt32(usernumbers[0]);
                            matrix2[1, i] = Convert.ToInt32(usernumbers[1]);
                            matrix2[2, i] = Convert.ToInt32(usernumbers[2]);
                        }
                        Console.WriteLine("   ADDITION   ");
                        Console.WriteLine(" SUBSTRACTION ");
                        Console.WriteLine("MULTIPLICATION");
                        function = Console.ReadLine().ToUpper();
                        int[,] newMatrix = new int[3, 3];
                        if (function == "MULTIPLICATION")
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    newMatrix[j, i] = (matrix1[0, i] * matrix2[j, 0]) + (matrix1[1, i] * matrix2[j, 1]) + (matrix1[2, i] * matrix2[j, 2]);
                                }
                            }
                        }
                        if (function == "ADDITION")
                        {
                            for (int y = 0; y < 3; y++)
                            {
                                for (int x = 0; x < 3; x++) { newMatrix[x, y] = matrix1[x, y] + matrix2[x, y]; }
                            }
                        }
                        if (function == "SUBTRACTION")
                        {
                            for (int y = 0; y < 3; y++)
                            {
                                for (int x = 0; x < 3; x++) { newMatrix[x, y] = matrix1[x, y] - matrix2[x, y]; }
                            }
                        }
                        Console.WriteLine("The answer is: ");
                        for (int y = 0; y < 3; y++)
                        {
                            for (int x = 0; x < 3; x++)
                            {
                                Console.Write($"{newMatrix[x, y]} ");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        void Sequence_Solver(string[] LON_s)
        //counting sort algorithm
        {
            //Big O Notation of O(n), better than mergesort for certain number of data inputted.
            List<int> LON = new List<int>();
            LON = LON_s.Select(s => Convert.ToInt32(s)).ToList();
            int max = LON.Max();
            int[] count = new int[max + 1];
            //because index starts from 0, and #8 will be stored in the index eight, and not 7, otherwise algorithm will not work.
            var list1 = new List<int>(LON);
            var group = list1.GroupBy(j => j);
            //using linq and var variables allowed me to element a large for loop and allow me to input numbers > 10
            foreach (var grp in group)
            {
                count[grp.Key] = grp.Count();
                //this gets the amount of recurrances of a number within the unsorted array
                //then inputs it into the count array at the same position as the number
                //(number of recurrances of #4 is inputted into index 4)
            }
            int cumulativen0 = 0;
            for (int i = 0; i < count.Length; i++)
            {
                //this keeps the running total of the array, then adding the total in the next position in the list
                //[0, 2, 0, 3, 4] = [0, 2, 2, 5, 9]
                //there is 2 recurrances of the number 1, 3 recurrances of 3 but no recurrances of 2 and so on.
                cumulativen0 += count[i];
                count[i] = cumulativen0;
            }
            int[] output = new int[LON.Count];
            for (int i = 0; i < LON.Count; i++)
            {
                //line 949 gets the integer stored at position i.
                int holder = LON[i];//for example [2, 4, 4, 1, 1, 1, 3, 5], therefore holder = 2
                int position = count[holder];//count = [0, 3, 4, 5, 7, 8]
                //therefore number 2 will go into position 4 in the output array.
                if (output.Contains(holder))
                {
                    position -= 1;
                    //if there is already a number in the same spot calculated, then but the number in the spot in position - 1.
                }
                output[position - 1] = LON[i];//then 2 to the position - 1, as this index follows the proper rules
                                              //starting from 0.
            }
            int count1 = 0;
            while (output.Contains(0))
            {
                //encountered a problem: if there were 3 or more recurrances of a number, 1 spot or more would be 0
                //this elimantes the problem, making sure there are no 0s in the list.
                if (output[count1] == 0) { output[count1] = output[count1 + 1]; count1 += 1; }
                else { count1 += 1; }
                if (count1 == output.Length - 1 && output.Contains(0)) { count1 = 0; }
                //if the loop as reached the end, but missed a 0, because if the list contains [..., 0, 0, 2, ...]
                //then there would be a 0 left -> [..., 0, 2, 2, ...]
            }
            Console.WriteLine("Sequence In Order: ");
            for (int i = 0; i < output.Length; i++)
            {
                Console.Write($"{output[i]} ");
            }

        }
        public bool Problem1()
        //uses hash tables
        {
            int numberofhints = 0;
            if (iq < 3) { numberofhints = 1; }
            else if (iq < 6) { numberofhints = 2; }
            else { numberofhints = 3; }
            string[] hints = { "They lay eggs", "You find them in a farm", "Eaten everyday by humans", "Used to make nuggets" };
            string[] randomPassword = { "alphabet", "tunasandwich", "back2school", "gnome&juliet", "theSecondAvenger" };
            Console.WriteLine("\n--- Guess the password ---");
            Console.WriteLine("They lost the needle, to sow their wings together.\nThey could never fly, staying on the groud forever");
            Random rndm = new Random();
            Hashtable password = new Hashtable(); 
            password.Add(0, "badpassword");
            password.Add(1, "chicken");
            for (int i = 0; i < randomPassword.Length; i++)
            {
                int number = rndm.Next(0, randomPassword.Length);
                password.Add(2 + i, randomPassword[number]);
            }
            int position = 0;
            char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            int numberofGuesses = 1;
            while (numberofGuesses < 5)
            {
                position = 0;
                Console.Write("Enter the password: ");
                string userInput = Console.ReadLine();
                char[] charuserInput = userInput.ToCharArray();
                for (int i = 0; i < charuserInput.Length; i++)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (charuserInput[i] == alphabet[j]) { position = position + j; }
                    }
                }
                int index = position % 3;
                string value = Convert.ToString(password[index]);
                if (userInput == value)
                {
                    Console.WriteLine("Correct password");
                    //this option is too appear, if the user chooses to try solve the puzzle again
                    if (userInput == "badpassword") 
                    {
                        if (InventoryReturn("S-HANDLE")) { Console.WriteLine("Already got normal key"); }
                        else
                        {
                            Inventory[1].Add($"Key {key}", "S-HANDLE"); Console.WriteLine("You obtained the special key handle");
                            Tracker.Add("OBTAINED S KEY 1"); key += 1;
                        }
                        
                    }
                    if (userInput == "chicken") 
                    {
                        if (InventoryReturn("N-HANDLE")) { Console.WriteLine("Already got normal part"); }
                        else 
                        {
                            Inventory[1].Add($"Key {key}", "N-HANDLE"); Console.WriteLine("You obtained the key handle");
                            Tracker.Add("OBTAINED N KEY 1"); key += 1;
                        }                        
                        if (statsReturn("IQ") > 5) { Console.WriteLine("That was a very \"badpassword\""); }
                    }
                    problem1 = true;
                    break;
                }
                else
                {
                    if (numberofhints == 0)
                    {
                        Console.WriteLine("Incorrect password.");
                        Console.WriteLine($"0 guesses left!");
                        numberofGuesses++;
                    }
                    else
                    {
                        
                        numberofhints -= 1;
                        Console.WriteLine("Incorrect password.");
                        Console.WriteLine($"Hint {numberofGuesses}: {hints[rndm.Next(0, hints.Length)]}");
                        numberofGuesses++;
                        Console.WriteLine($"{numberofhints} guesses left!");
                        
                        //add the incorrect guess into the hashtable.
                        if (password[index] == null) { password[index] = userInput; }
                        else
                        {
                            while (password[index] != null) { index += 1; }
                            password[index] = userInput;
                        }
                    }
                }
                if (numberofGuesses == 5 || numberofhints == 0) { Console.WriteLine($"{numberofhints} guesses left!"); Tracker.Add("CAUGHT"); break; }
            }
            return problem1;
        }
        public bool Problem2()
        //uses queues
        {
            Random random = new Random();
            //int loop = random.Next(10, 30);
            int loop = 5;
            List<int> r_orderOfNumber = new List<int>();
            List<int> orderofnumber = new List<int>();
            Queue<int> orderofNumber = new Queue<int>();
            int incorrectCounter = 0;
            int numberOfGuesses = 0;
            if (iq > 6) { numberOfGuesses = 3; }
            else if (iq > 3) { numberOfGuesses = 2; }
            else { numberOfGuesses = 1; }
            for (int i = 0; i < loop; i++)
            {
                int randomNumber = random.Next(1, 101);
                r_orderOfNumber.Add(randomNumber);
            }
            foreach (int number in r_orderOfNumber) { Console.Write($"{number} "); }
            int minimumValue = 0;
            while (r_orderOfNumber.Count != 0)
            {
                minimumValue = r_orderOfNumber.Min();
                orderofNumber.Enqueue(minimumValue);
                r_orderOfNumber.Remove(minimumValue);
            }
            Console.Write(Environment.NewLine);
            if (InventoryReturn("SEQUENCE_SOLVER"))
            {
                Console.Write("You have the sequence solver, do you want to use it? (YES or NO) ");
                Puzzles.userStringInput = Console.ReadLine().ToUpper();
                if (Puzzles.userStringInput == "YES")
                {
                    Display.displayFunction("SEQUENCE SOLVER");
                    Console.Write("Enter Unsolved Sequence: ");
                    userStringInput = Console.ReadLine();
                    string[] UN_Sequence = userStringInput.Split();
                    Sequence_Solver(UN_Sequence);
                    Console.WriteLine();
                    Display.displayFunction("SEQUENCE SOLVED");
                    Console.WriteLine("As you try to put the sequence solver away, it burns, you throw it away quickly as you see it disintegrate.");
                    Tracker.Remove("OBTAINED SEQUENCE_SOLVER");
                    for (int i = 1; i < Inventory[0].Count + 1; i++)
                    {
                        if (Inventory[0][$"Tool {i}"] == "SEQUENCE_SOLVER") { Character.Inventory[0].Remove($"Tool {i}"); }
                    }
                    loot--;
                }
                else { }
            }
            while (numberOfGuesses != 0)
            {
                Console.WriteLine("Enter the numbers: ");
                Queue<int> userinput = new Queue<int>();
                Queue<int> secretOrderOfNumber = new Queue<int>(orderofNumber);
                int secretIncorrectCounter = 0;
                string[] order = Console.ReadLine().Split();
                int integer = 0;
                for (int i = 0; i < order.Length; i++)
                {
                    integer = Convert.ToInt32(order[i]);
                    userinput.Enqueue(integer);
                }
                //if the user inputs the numbers backwards, they get a special key.
                for (int i = 0; i < order.Length; i++)
                {
                    if (Convert.ToInt32(order[order.Length - (1 + i)]) == secretOrderOfNumber.Peek()) { secretOrderOfNumber.Dequeue(); }
                    else { secretIncorrectCounter += 1; }
                }
                for (int i = 0; i < userinput.Count; i++)
                {
                    if (userinput.Peek() == orderofNumber.Peek()) { userinput.Dequeue(); orderofNumber.Dequeue(); }
                    else { incorrectCounter += 1; }
                }
                if (secretIncorrectCounter == 0)
                {

                    Console.WriteLine("Correct sequence, special key part acquired.");
                    if (InventoryReturn("S-KEY_SHAFT")) { Console.WriteLine("Already got normal part"); }
                    else
                    {
                        Inventory[1].Add($"Key {key}", "S-KEY_SHAFT");
                        key += 1;
                        Tracker.Add("OBTAINED : S KEY 2");
                    }                    
                    problem2 = true;
                    break;
                }
                if (incorrectCounter == 0)
                {
                    Console.WriteLine("Correct sequence, normal key part acquired.");
                    if (InventoryReturn("N-KEY_SHAFT")) { Console.WriteLine("Already got normal part"); }
                    else
                    {
                        Inventory[1].Add($"Key {key}", "N-KEY_SHAFT");
                        key += 1;
                        Tracker.Add("OBTAINED : N KEY 2");
                    }
                    problem2 = true;
                    break;

                }
                else
                {
                    Console.WriteLine("Incorrect Sequence, try again");
                    numberOfGuesses--;
                }
            }
            return problem2;
        }
        public bool Problem3(string diff)
        //uses advanced matrices
        {
            int NOTCIU = 0;
            bool question1 = false;
            bool question2 = false;
            bool question3 = false;
            bool question4 = false;
            Console.WriteLine("\"Lets Start with a warm up!\"");
            int numberOfGuesses = 0;
            if (iq > 6) { numberOfGuesses = 3; }
            else if (iq > 3) { numberOfGuesses = 2; }
            else { numberOfGuesses = 1; }
            int numberOfInitialGuesses = numberOfGuesses;
            if (diff == "EASY")
            {
                //question1 
                Console.Write("\"Question 1, What is 5 + 3?\" ");
                while (numberOfGuesses != 0)
                {                    
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userIntegerInput = Convert.ToInt32(Console.ReadLine());
                    if (userIntegerInput == 8) { question1 = true; Console.WriteLine("\"Don't get too exicted, that was the easiest question yet\""); break; }
                    else 
                    { 
                        numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n");
                        if (numberOfGuesses == 0)
                        {
                            Console.WriteLine("\"You really got that question wrong?\""); break;
                        }
                        else { Console.Write("\"What is 5 + 3?\""); }
                    }
                    
                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //question2
                Console.Write("\"Question 2, What is 5 x 9 ?\" ");
                while (numberOfGuesses != 0)
                {                    
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userIntegerInput = Convert.ToInt32(Console.ReadLine());
                    if (userIntegerInput == 45) { question2 = true; Console.WriteLine("\"Okay, okay...\""); break; }
                    else
                    {
                        numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n");
                        if (numberOfGuesses == 0)
                        {
                            Console.WriteLine("\"If you are getting this wrong, the next questions is going to be the end of you, HA HA HA!\""); break;
                        }
                        else { Console.Write("\"What is 5 * 9?\""); }
                    }

                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //question3
                Console.Write("\"Question 3, What is 7 / 2 ?\" ");
                while (numberOfGuesses != 0)
                {                    
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        userStringInput = Console.ReadLine().ToUpper();
                        if (userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userStringInput = Console.ReadLine();
                    if (userStringInput == "3.5") { question3 = true; Console.WriteLine("\"That's it, I have had enough! Here comes the hard one!\""); break; }

                    else
                    {
                        numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n");
                        if (numberOfGuesses == 0)
                        {
                            Console.WriteLine("\"I am getting bored, answer this question and let's be done with it.\""); break;
                        }
                        else { Console.Write("\"What is 7 / 2 ?\""); }
                    }

                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //question4
                Console.WriteLine("\"Question 4 what is:\"");
                while (numberOfGuesses != 0)
                {            
                    int[,] matrix1 = new int[2, 2];
                    int[,] matrix2 = new int[2, 2];
                    Random randominteger = new Random();
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            matrix1[j, i] = randominteger.Next(1, 21);
                            matrix2[j, i] = randominteger.Next(1, 21);
                        }
                    }
                    Console.WriteLine("Matrix 1");
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++) { Console.Write($"{matrix1[x, y]} "); }
                        Console.WriteLine();
                    }
                    Console.WriteLine("Matrix 2");
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++) { Console.Write($"{matrix2[x, y]} "); }
                        Console.WriteLine();
                    }
                    string[] signs = { "-", "+" };
                    string sign = signs[randominteger.Next(0, 2)];
                    Console.WriteLine("*Note: write the answer in the order left to right and enter and repeat*");
                    Console.WriteLine($"Matrix 1 {sign} Matrix 2 = ??");
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        else { }
                    }
                    if (NOTCIU == 3)
                    {
                        Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                        Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                        Tracker.Remove("OBTAINED CALCULATOR");
                        for (int i = 1; i < Inventory[0].Count + 1; i++)
                        {
                            if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                        }
                    }
                    int[,] userMatrix = new int[2, 2];
                    for (int i = 0; i < 2; i++)
                    {
                        string userinput = Console.ReadLine();
                        string[] usernumbers = userinput.Split();
                        userMatrix[0, i] = Convert.ToInt32(usernumbers[0]);
                        userMatrix[1, i] = Convert.ToInt32(usernumbers[1]);
                    }
                    int[,] newMatrix = new int[2, 2];
                    if (sign == "+")
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            for (int x = 0; x < 2; x++) { newMatrix[x, y] = matrix1[x, y] + matrix2[x, y]; }
                        }
                    }
                    if (sign == "-")
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            for (int x = 0; x < 2; x++) { newMatrix[x, y] = matrix1[x, y] - matrix2[x, y]; }
                        }
                    }
                    int validCount = 0;
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            if (userMatrix[x, y] == newMatrix[x, y]) { validCount += 1; }
                        }
                    }
                    if (validCount == 4)
                    {
                        Console.WriteLine("\"No way, you got the correct answer, you must be cheating. There is no way you're as smart as me!\"");
                        question4= true;
                        break;

                    }

                    else
                    {
                        numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n");
                        if (numberOfGuesses == 0)
                        {
                            Console.WriteLine("\"I knew you were getting those question on a fluke, your so stupid\" says that Calculator with a large grin.\""); break;
                        }
                    }
                }
                Console.WriteLine();
            }
            if (diff == "MEDIUM")
            {
                //question 1
                Console.Write("\"Question 1, What is 5 + 3 + 5 + 8?\"");
                while (numberOfGuesses != 0)
                {
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userIntegerInput = Convert.ToInt32(Console.ReadLine());
                    if (userIntegerInput == 21) { question1 = true; Console.WriteLine("\"Don't get too exicted, that was the easiest question yet\""); break; }
                    else if (numberOfGuesses == 0) { Console.WriteLine("\"Oh, that is embrassing, on to the next, Ha ha ha!\""); break; }
                    else { numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n"); Console.Write("\"What is 5 + 3 + 5 + 8?\""); }
                    
                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //Question 2
                Console.Write("\"Question 2, What is (5 x 9) + 2 ?\"");
                while (numberOfGuesses != 0)
                {                    
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userIntegerInput = Convert.ToInt32(Console.ReadLine());
                    if (userIntegerInput == 47) { question2 = true; Console.WriteLine("\"That wasn't even that hard, do not get ahead of yourself. We still have 2 more questions to go.\""); break; }
                    else if (numberOfGuesses == 0) { Console.WriteLine("\"That wasn't even that hard, it's simple math, what's is wrong with the childern of this generation!\""); break; }
                    else { numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n"); Console.Write("\"What is (5 x 9) + 2 ?\""); }
                    
                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //question3
                Console.Write("\"Question 3, What is 12 x 12 ?\"");
                while (numberOfGuesses != 0)
                {                    
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userIntegerInput = Convert.ToInt32(Console.ReadLine());
                    if (userIntegerInput == 144) { question3 = true; Console.WriteLine("\"Okay, I genuinly thought you were going to get that one wrong.\""); break; }
                    else if (numberOfGuesses == 0) { Console.WriteLine("\"Have you never learnt of square number,psst, next question!\""); break; }
                    else { numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n"); Console.Write("\"What is 12 x 12 ?\""); }
                    
                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //quesion4
                Console.WriteLine("\"Question 4, what is:\"");
                while (numberOfGuesses != 0)
                {                    
                    int[,] matrix1 = new int[2, 2];
                    int[,] matrix2 = new int[2, 2];
                    Random randominteger = new Random();
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            matrix1[j, i] = randominteger.Next(1, 21);
                            matrix2[j, i] = randominteger.Next(1, 21);
                        }
                    }
                    Console.WriteLine("Matrix 1");
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++) { Console.Write($"{matrix1[x, y]} "); }
                        Console.WriteLine();
                    }
                    Console.WriteLine("Matrix 2");
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++) { Console.Write($"{matrix2[x, y]} "); }
                        Console.WriteLine();
                    }
                    string[] signs = { "*", "-", "+" };
                    string sign = signs[randominteger.Next(0, 3)];
                    Console.WriteLine("*Note: write the answer in the order left to right and enter and repeat*");
                    Console.WriteLine($"Matrix 1 {sign} Matrix 2 = ??");
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        else { }
                    }
                    if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                    {
                        Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                        Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                        Tracker.Remove("OBTAINED CALCULATOR");
                        for (int i = 1; i < Inventory[0].Count + 1; i++)
                        {
                            if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                        }
                    }
                    int[,] userMatrix = new int[2, 2];
                    for (int i = 0; i < 2; i++)
                    {
                        string userinput = Console.ReadLine();
                        string[] usernumbers = userinput.Split();
                        userMatrix[0, i] = Convert.ToInt32(usernumbers[0]);
                        userMatrix[1, i] = Convert.ToInt32(usernumbers[1]);
                    }
                    int[,] newMatrix = new int[2, 2];
                    if (sign == "*")
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                newMatrix[j, i] = (matrix1[0, i] * matrix2[j, 0]) + (matrix1[1, i] * matrix2[j, 1]);
                            }
                        }
                    }
                    if (sign == "+")
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            for (int x = 0; x < 2; x++) { newMatrix[x, y] = matrix1[x, y] + matrix2[x, y]; }
                        }
                    }
                    if (sign == "-")
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            for (int x = 0; x < 2; x++) { newMatrix[x, y] = matrix1[x, y] - matrix2[x, y]; }
                        }
                    }
                    int validCount = 0;
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            if (userMatrix[x, y] == newMatrix[x, y]) { validCount += 1; }
                        }
                    }
                    if (validCount == 4)
                    {
                        Console.WriteLine("\"I am suprised, that was infact the correct answer, fair play.\"");
                        question4 = true;
                        break;
                    }
                    else if (numberOfGuesses == 0) {  Console.WriteLine("\"You were so close... JOKING! HA HA HA!\""); break; }
                    else { numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} left.\""); }
                }
            }
            if (diff == "HARD")
            {
                //quetion1
                Console.Write("\"Question 1, What is (5*5) * 2?\"");
                while (numberOfGuesses != 0)
                {                    
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userIntegerInput = Convert.ToInt32(Console.ReadLine());
                    if (userIntegerInput == 50) { question1 = true; Console.WriteLine("\"Don't get too exicted, that was the easiest question yet\""); break; }
                    else if (numberOfGuesses == 0) {  Console.WriteLine("\"Oh, that is embrassing, on to the next, Ha ha ha!\""); break; }
                    else { numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n"); Console.Write("\"What is (5*5) * 2?\""); }
                    
                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //question 2
                Console.Write("\"Question 2, What is (5 - 9) * 9 ?\"");
                while (numberOfGuesses != 0)
                {                    
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userIntegerInput = Convert.ToInt32(Console.ReadLine());
                    if (userIntegerInput == -36) { question2 = true; Console.WriteLine("\"Okay, I genuinly thought you were going to get that one wrong.\""); break; }
                    else if (numberOfGuesses == 0) {  Console.WriteLine("\"My dead grandma could've gotten that and she was born in 1642!\""); break; }
                    else { numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n"); Console.Write("\"What is (5 - 9) * 9 ?\""); }
                    
                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //question3
                Console.Write("\"Question 3, What is ((49 / 9) * 6) - 20?\"");
                while (numberOfGuesses != 0)
                {                    
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.WriteLine();
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            NOTCIU++;
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                        {
                            Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                            Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                            Tracker.Remove("OBTAINED CALCULATOR");
                            for (int i = 1; i < Inventory[0].Count + 1; i++)
                            {
                                if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                            }
                        }
                        else { }
                    }
                    userIntegerInput = Convert.ToInt32(Console.ReadLine());
                    if (userIntegerInput == 23) { question3 = true; Console.WriteLine("\"Okay, okay...\""); break; }
                    else if (numberOfGuesses == 0) {  Console.WriteLine("\"If you are getting this wrong, the next questions is going to be the end of you, HA HA HA!\""); break; }
                    else { numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n"); Console.Write("\"What is ((49 / 9) * 6) - 20?\""); }
                    
                }
                numberOfGuesses = numberOfInitialGuesses;
                Console.WriteLine();
                //Question4
                Console.WriteLine("\"Question 4, what is:\"");
                while (numberOfGuesses != 0)
                {
                    
                    Console.WriteLine("--- 3 by 3 Matrix ---");
                    int[,] matrix1 = new int[3, 3];
                    int[,] matrix2 = new int[3, 3];
                    Random randominteger = new Random();
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            matrix1[j, i] = randominteger.Next(1, 21);
                            matrix2[j, i] = randominteger.Next(1, 21);
                        }
                    }
                    Console.WriteLine("Matrix 1");
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 3; x++) { Console.Write($"{matrix1[x, y]} "); }
                        Console.WriteLine();
                    }
                    Console.WriteLine("Matrix 2");
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 3; x++) { Console.Write($"{matrix2[x, y]} "); }
                        Console.WriteLine();
                    }
                    string[] signs = { "*", "-", "+" };
                    string sign = signs[randominteger.Next(0, 3)];
                    Console.WriteLine("*Note: write the answer in the order left to right and enter and repeat*");
                    Console.WriteLine($"Matrix 1 {sign} Matrix 2 = ??");
                    if (InventoryReturn("CALCULATOR"))
                    {
                        Console.Write("You have the CALCULATOR, do you want to use it? (YES or NO) ");
                        Puzzles.userStringInput = Console.ReadLine().ToUpper();
                        if (Puzzles.userStringInput == "YES")
                        {
                            Display.displayFunction("CALCULATOR");
                            Calculator();
                            Console.WriteLine();
                        }
                        else { }
                    }
                    if (NOTCIU == 3 && Tracker.Contains("OBTAINED CALCULATOR"))
                    {
                        Console.WriteLine("The Giant Calculator notices that you keep looking at a certain device to answer the questions");
                        Console.WriteLine("It snatches the calculator out your hands. \"What is this, you are CHEATING!\"\nYou watch in horror as the calculator destorys your ... calculator. ");
                        Tracker.Remove("OBTAINED CALCULATOR");
                        for (int i = 1; i < Inventory[0].Count + 1; i++)
                        {
                            if (Inventory[0][$"Tool {i}"] == "CALCULATOR") { Character.Inventory[0].Remove($"Tool {i}"); }
                        }
                    }
                    int[,] userMatrix = new int[2, 2];
                    for (int i = 0; i < 3; i++)
                    {
                        string userinput = Console.ReadLine();
                        string[] usernumbers = userinput.Split();
                        userMatrix[0, i] = Convert.ToInt32(usernumbers[0]);
                        userMatrix[1, i] = Convert.ToInt32(usernumbers[1]);
                        userMatrix[2, i] = Convert.ToInt32(usernumbers[2]);
                    }
                    int[,] newMatrix = new int[3, 3];
                    if (sign == "*")
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                newMatrix[j, i] = (matrix1[0, i] * matrix2[j, 0]) + (matrix1[1, i] * matrix2[j, 1]) + (matrix1[2, i] * matrix2[j, 2]);
                            }
                        }
                    }
                    if (sign == "+")
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            for (int x = 0; x < 3; x++) { newMatrix[x, y] = matrix1[x, y] + matrix2[x, y]; }
                        }
                    }
                    if (sign == "-")
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            for (int x = 0; x < 3; x++) { newMatrix[x, y] = matrix1[x, y] - matrix2[x, y]; }
                        }
                    }
                    int validCount = 0;
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            if (userMatrix[x, y] == newMatrix[x, y]) { validCount += 1; }
                        }
                    }
                    if (validCount == 9)
                    {
                        Console.WriteLine("\"I am suprised, that was infact the correct answer, fair play.\"");
                        question4 = true;
                        break;
                    }

                    else
                    {
                        numberOfGuesses--; Console.WriteLine($"\"Incorrect, you have {numberOfGuesses} guesses left\"\n");
                        if (numberOfGuesses == 0)
                        {
                            Console.WriteLine("\"You were so close... JOKING! HA HA HA!\""); break;
                        }
                    }
                }
            }
            if (question1 && question2 && question3 && question4) 
            {
                if (!InventoryReturn("N-COLLAR"))
                {
                    Console.WriteLine("\"Credit were credit is due, I did not expect you to get it, as I am a generous man, heres a key I found, and the door is on your left.\"");
                    Console.WriteLine("*Obtained normal key*");
                    problem3 = true; Inventory[1].Add($"Key {key}", "N-COLLAR");
                    key += 1;
                    Tracker.Add("OBTAINED : N KEY 3");
                }
                else { Console.WriteLine("Already obtained key."); }
            }
            else if (!question1 && !question2 && !question3 && !question4)
            {
                Console.WriteLine("\"How did you manage to get every question wrong, HA HA HA HA!\"");
                Console.WriteLine("While the calculator is crying in laughter, you see a shiny key parts glistening beside the calculator.");
                Console.Write("Do you want to risk it and attempt to get the special key. (YES/NO)? ");
                Puzzles.userStringInput = Console.ReadLine().ToUpper();
                if (Puzzles.userStringInput == "YES")
                {
                    if (!InventoryReturn("S-COLLAR"))
                    {
                        if (fitness > 60 || iq > 6)
                        {
                            Console.WriteLine("You managed to grab it in the knick of time, before the Calculator collects themselves and looks back at you again.");
                            Console.WriteLine("\"Because you got everything wrong and made me laugh, I will not hit you this time... well not as hard.\"");
                            Console.WriteLine("*Obtained special key*");
                            Inventory[1].Add($"Key {key}", "S-COLLAR");
                            key += 1;
                            Tracker.Add("OBTAINED : S KEY 3");

                        }
                        else
                        {
                            Console.WriteLine("The calculator notices you trying to get the key out the corner of their high, you were not quick enough.");
                            Console.WriteLine("\"You really thought you could get away with that\"");
                            health -= 10;
                        }
                    }
                    else { Console.WriteLine("You already have the key."); }
                    
                }
            }
            else { Console.WriteLine("\"In return for wasting my time, you get this\""); health -= 20; }
            return problem3;
        }
        public bool Problem5()
        //matrices - advanced matrices?
        {
            Chess chess = new Chess();
            chess.Bishop();
            chess.Queen();
            chess.Rook();
            chess.Pawn();
            chess.Knight();
            chess.ChessBoard();
            bool validPosition = false;
            while (validPosition == false)
            {
                try
                {
                    Console.WriteLine("Where would you like to place your King");
                    chess.kingPosition = Console.ReadLine().Split();
                    if (chess.board[Convert.ToInt32(chess.kingPosition[1]) - 1, Convert.ToInt32(chess.kingPosition[0]) - 1] != ".")
                    {
                        Console.WriteLine("There is a piece here");
                    }
                    else if (Convert.ToInt32(chess.kingPosition[1]) >= 1 && Convert.ToInt32(chess.kingPosition[1]) <= 8 && Convert.ToInt32(chess.kingPosition[0]) >= 1 && Convert.ToInt32(chess.kingPosition[0]) <= 8)
                    {
                        chess.King(chess.kingPosition);
                        chess.Check_Bishop();
                        chess.Check_Knight();
                        chess.Check_Pawn();
                        chess.Check_Queen();
                        chess.Check_Rook();

                        if (chess.Check_Rook() == true)
                        {
                            Console.WriteLine("You are in check by the opponents rook");
                        }
                        if (chess.Check_Queen() == true)
                        {
                            Console.WriteLine("You are in check by the opponents Queen");
                        }
                        if (chess.Check_Pawn() == true)
                        {
                            Console.WriteLine("You are in check by the opponents Pawn");
                        }
                        if (chess.Check_Bishop() == true)
                        {
                            Console.WriteLine("You are in check by the opponents Bishop");
                        }
                        if (chess.Check_Knight() == true)
                        {
                            Console.WriteLine("You are in check by the opponents Knight");
                        }
                        else if (chess.Check_Knight() == true && chess.Check_Bishop() == true && chess.Check_Pawn() == true && chess.Check_Queen() == true && chess.Check_Rook() == true)
                        {
                            Console.WriteLine("You are in checkmate");
                            problem5 = true;
                        }
                        else if (chess.CheckMate())
                        {
                            Console.WriteLine("You are in checkmate");
                            problem5 = true;
                        }
                        validPosition = true;
                    }
                    else
                    {
                        Console.WriteLine("You have to place the King on the board, not outside of it");
                    }
                }
                catch { Console.WriteLine("You have to enter a number for the position"); }
            }
            return problem5;
        }
        public bool Problem6(string difficulty)
        //queues but better example than problem2()
        {
            int numberOfGames = 0;
            if (difficulty == "EASY") { numberOfGames = 1; }
            else if (difficulty == "MEDIUM") { numberOfGames = 3; }
            else { numberOfGames = 1; }
            int userRoundWins = 0;
            int NPCRoundWins = 0;
            Poker cardGame = new Poker("NEW GAME");
            Console.WriteLine("COMMANDS: CALL|RAISE|FOLD|CHEAT|CHECK CARDS|");
            userCards u = new userCards();
            NPCcards n = new NPCcards();
            cardGame.cardsDealtDisplay();
            Console.WriteLine("There is £20 in the pot.");
            Console.WriteLine($"\nYour Cards (You have £{Poker.user_money}):");
            u.actionByUser("CHECK CARDS");
            Poker.user_Action = false;
            Poker.npc_Action = false;
            while (numberOfGames != 0)
            {
                u.userCheck();
                n.npcCheck();
                if (cardGame.winner())
                {
                    if (Poker.NPCWin) { NPCRoundWins++; numberOfGames--; Poker.NPC_money = Poker.pot; }
                    else if (Poker.user_money == 0) 
                    {
                        NPCRoundWins = 1;
                        userRoundWins = 0;
                        Console.WriteLine("\"How did you manage to lose all your money, HA HA HA, you lose entirely!\"");
                        break;
                    }
                    if (Poker.userWin) { userRoundWins++; numberOfGames--; Poker.user_money = Poker.pot; }
                    else if (Poker.NPC_money == 0) 
                    {
                        NPCRoundWins = 0;
                        userRoundWins = 1;
                        Console.WriteLine("\"No, no, NOOO. How did I manage to lose everything, this can't be happening\"");
                        break;
                    }

                    if (numberOfGames != 0) 
                    { 
                        cardGame = new Poker("NEW GAME"); u = new userCards(); n = new NPCcards();
                        cardGame.cardsDealtDisplay();
                        Console.WriteLine($"\nYour Cards (You have £{Poker.user_money}):");
                        u.actionByUser("CHECK CARDS");                        
                    }
                }
                else
                {
                    while (!Poker.user_Action && !Poker.npc_Action)
                    {
                        if (Poker.user_action == "FOLD" || Poker.npc_action == "FOLD")
                        {
                            break;
                        }
                        if (!Poker.user_Action)
                        {
                            Console.Write("What do you want to do: ");
                            Poker.user_action = Console.ReadLine().ToUpper();
                            if (Poker.user_action == "CHEAT")
                            {
                                Random cheat = new Random();
                                int chanceofbeingcaught = cheat.Next(1, 50);
                                if (chanceofbeingcaught % 5 == 0 || chanceofbeingcaught % 3 == 0)
                                {
                                    Console.WriteLine("\"You, a human out of all people, tried to cheat against me, by default you lose this round.\"");
                                    Poker.NPCWin = true;
                                    Poker.userWin = false;
                                    Poker.user_Action = true;
                                    Poker.npc_Action = true;
                                }
                                else
                                {
                                    cardGame.cheat();
                                }
                            }
                            else
                            {
                                u.actionByUser(Poker.user_action);
                            }
                            
                        }
                        if (!Poker.npc_Action)
                        {
                            n.decisionmakingbyNPC();
                        }
                    }
                    cardGame.cardsDealtDisplay();
                }
            }
            if (userRoundWins > NPCRoundWins) 
            { 
                if (difficulty == "HARD")
                {
                    Console.WriteLine("\"You have given me the best few games of poker I have ever had. Therefore, out of respect, here is 2 gifts for you.");
                    if (!InventoryReturn("N-TEETH"))
                    {
                        Console.WriteLine("Obtained Normal and Special Keys.");
                        Inventory[1].Add($"Key {key}", "N-TEETH");
                        key += 1;
                        Tracker.Add("OBTAINED : N KEY 3");
                    }
                    if (!InventoryReturn("S-TEETH"))
                    {
                        Inventory[1].Add($"Key {key}", "S-TEETH");
                        key += 1;
                        Tracker.Add("OBTAINED : S KEY 3");
                    }
                    else
                    {
                        Console.WriteLine("\"You already got these items, are you trying to annoy me, just leave already.\"");
                    }
                }
                else
                {
                    Console.WriteLine("\"Had we played more, you have lost certainly, you a not good, so do not get ahead of yourself.\"");
                    Console.WriteLine("\"Regardless, you won, so it is only fair that I give you something in return for beating me\"");
                    if (!InventoryReturn("N-TEETH"))
                    {
                        Console.WriteLine("Obtained Normal");
                        Inventory[1].Add($"Key {key}", "N-TEETH");
                        key += 1;
                        Tracker.Add("OBTAINED : N KEY 3");
                    }
                    else
                    {
                        Console.WriteLine("\"You already got these items, are you trying to annoy me, just leave already.\"");
                    }
                }
                problem6 = true; 
            }
            if (NPCRoundWins > userRoundWins) { Console.WriteLine("\"No one has beaten me at my own game for millenia, what made you think it would start now.\""); }
            return problem6;
        }
    }
    class Chess
    {
        string[] position = new string[] { "0", "1", "2", "3", "4", "5", "6", "7" };
        string[] queenPosition;
        string[] rookPosition;
        string[] bishopPosition;
        string[] pawnPosition;
        string[] knightPosition;
        public string[] kingPosition;
        public string[,] board = new string[8, 8]
        {
        {".",".",".",".",".",".",".","." },
        {".",".",".",".",".",".",".","." },
        {".",".",".",".",".",".",".","." },
        {".",".",".",".",".",".",".","." },
        {".",".",".",".",".",".",".","." },
        {".",".",".",".",".",".",".","." },
        {".",".",".",".",".",".",".","." },
        {".",".",".",".",".",".",".","." },
        };
        public void ChessBoard()
        {
            Console.WriteLine("_______________________________________________");
            for (int a = 0; a < 8; a++)
            {
                for (int z = 0; z < 8; z++)
                {
                    Console.Write("  {0}  " + "|", board[a, z]);
                }
                Console.Write(Environment.NewLine);
                Console.WriteLine("_____|_____|_____|_____|_____|_____|_____|_____|");
            }
        }
        public void King(string[] king)
        {
            this.kingPosition = king;
            board[Convert.ToInt32(kingPosition[1]) - 1, Convert.ToInt32(kingPosition[0]) - 1] = "K";
        }
        public void Bishop()
        {
            bool valid = false;
            Random BishoprndPosition = new Random();
            int Bishopxposition = BishoprndPosition.Next(0, 8);
            int Bishopyposition = BishoprndPosition.Next(0, 8);
            bishopPosition = new string[] { Convert.ToString(Bishopyposition), Convert.ToString(Bishopxposition) };
            while (valid == false)
            {
                if (board[Bishopxposition, Bishopyposition] != ".")
                {
                    Bishopxposition = BishoprndPosition.Next(0, 8);
                    Bishopyposition = BishoprndPosition.Next(0, 8);
                    bishopPosition = new string[] { Convert.ToString(Bishopyposition), Convert.ToString(Bishopxposition) };
                }
                else
                {
                    board[Bishopxposition, Bishopyposition] = "B";
                    valid = true;
                }
            }
        }
        public void Rook()
        {
            int count = 1;
            while (count <= 2)
            {
                bool valid = false;
                Random RookrndPosition = new Random();
                int Rookxposition = RookrndPosition.Next(0, 8);
                int Rookyposition = RookrndPosition.Next(0, 8);
                rookPosition = new string[] { Convert.ToString(Rookyposition), Convert.ToString(Rookxposition) };
                while (valid == false)
                {
                    if (board[Rookxposition, Rookyposition] != ".")
                    {
                        Rookxposition = RookrndPosition.Next(0, 8);
                        Rookyposition = RookrndPosition.Next(0, 8);
                        rookPosition = new string[] { Convert.ToString(Rookyposition), Convert.ToString(Rookxposition) };
                    }
                    else
                    {
                        board[Rookxposition, Rookyposition] = "R";
                        valid = true;
                    }
                }
                count += 1;
            }
        }
        public void Knight()
        {
            bool valid = false;
            Random KnightrndPosition = new Random();
            int Knightxposition = KnightrndPosition.Next(0, 8);
            int Knightyposition = KnightrndPosition.Next(0, 8);
            knightPosition = new string[] { Convert.ToString(Knightyposition), Convert.ToString(Knightxposition) };                
            while (valid == false)
            {
                if (board[Knightxposition, Knightyposition] != ".")
                {
                    Knightxposition = KnightrndPosition.Next(0, 8);
                    Knightyposition = KnightrndPosition.Next(0, 8);
                    bishopPosition = new string[] { Convert.ToString(Knightyposition), Convert.ToString(Knightxposition) };
                }
                else
                {
                    board[Knightyposition, Knightxposition] = "H";
                    valid = true;
                }
            }
        }
        public void Queen()
        {
            bool valid = false;
            Random QueenrndPosition = new Random();
            int Queenxposition = QueenrndPosition.Next(0, 8);
            int Queenyposition = QueenrndPosition.Next(0, 8);
            queenPosition = new string[] { Convert.ToString(Queenyposition), Convert.ToString(Queenxposition) };                
            while (valid == false)
            {
                if (board[Queenxposition, Queenyposition] != ".")
                {
                    Queenxposition = QueenrndPosition.Next(0, 8);
                    Queenyposition = QueenrndPosition.Next(0, 8);
                    bishopPosition = new string[] { Convert.ToString(Queenyposition), Convert.ToString(Queenyposition) };
                }
                else
                {
                    board[Queenxposition, Queenxposition] = "Q";
                    valid = true;
                }
            }
        }
        public void Pawn()
        {
            Random PawnrndPosition = new Random();
            int Pawnxposition = PawnrndPosition.Next(0, 8);
            int Pawnyposition = PawnrndPosition.Next(0, 8);
            pawnPosition = new string[] { Convert.ToString(Pawnyposition), Convert.ToString(Pawnxposition) };
            board[Pawnxposition, Pawnxposition] = "P";
        }
        public bool CheckMate()
        {
            int checkmateCount = 0;
            if (board[Convert.ToInt32(kingPosition[1]) + 1, Convert.ToInt32(kingPosition[0]) + 1] == ".")
            {
                Check_Bishop();
                Check_Pawn();
                Check_Knight();
                Check_Rook();
                Check_Queen();
                if (Check_Queen() || Check_Rook() || Check_Pawn() || Check_Bishop() || Check_Knight()) { checkmateCount += 1; }
            }
            if (board[Convert.ToInt32(kingPosition[1]), Convert.ToInt32(kingPosition[0]) + 1] == ".")
            {
                Check_Bishop();
                Check_Pawn();
                Check_Knight();
                Check_Rook();
                Check_Queen();
                if (Check_Queen() || Check_Rook() || Check_Pawn() || Check_Bishop() || Check_Knight()) { checkmateCount += 1; }
            }
            if (board[Convert.ToInt32(kingPosition[1]) + 1, Convert.ToInt32(kingPosition[0])] == ".")
            {
                Check_Bishop();
                Check_Pawn();
                Check_Knight();
                Check_Rook();
                Check_Queen();
                if (Check_Queen() || Check_Rook() || Check_Pawn() || Check_Bishop() || Check_Knight()) { checkmateCount += 1; }
            }
            if (board[Convert.ToInt32(kingPosition[1]) - 1, Convert.ToInt32(kingPosition[0]) - 1] == ".")
            {
                Check_Bishop();
                Check_Pawn();
                Check_Knight();
                Check_Rook();
                Check_Queen();
                if (Check_Queen() || Check_Rook() || Check_Pawn() || Check_Bishop() || Check_Knight()) { checkmateCount += 1; }
            }
            if (board[Convert.ToInt32(kingPosition[1]) + 1, Convert.ToInt32(kingPosition[0]) - 1] == ".")
            {
                Check_Bishop();
                Check_Pawn();
                Check_Knight();
                Check_Rook();
                Check_Queen();
                if (Check_Queen() || Check_Rook() || Check_Pawn() || Check_Bishop() || Check_Knight()) { checkmateCount += 1; }
            }
            if (board[Convert.ToInt32(kingPosition[1]) - 1, Convert.ToInt32(kingPosition[0]) + 1] == ".")
            {
                Check_Bishop();
                Check_Pawn();
                Check_Knight();
                Check_Rook();
                Check_Queen();
                if (Check_Queen() || Check_Rook() || Check_Pawn() || Check_Bishop() || Check_Knight()) { checkmateCount += 1; }
            }
            if (board[Convert.ToInt32(kingPosition[1]) - 1, Convert.ToInt32(kingPosition[0])] == ".")
            {
                Check_Bishop();
                Check_Pawn();
                Check_Knight();
                Check_Rook();
                Check_Queen();
                if (Check_Queen() || Check_Rook() || Check_Pawn() || Check_Bishop() || Check_Knight()) { checkmateCount += 1; }
            }
            if (board[Convert.ToInt32(kingPosition[1]), Convert.ToInt32(kingPosition[0]) - 1] == ".")
            {
                Check_Bishop();
                Check_Pawn();
                Check_Knight();
                Check_Rook();
                Check_Queen();
                if (Check_Queen() || Check_Rook() || Check_Pawn() || Check_Bishop() || Check_Knight()) { checkmateCount += 1; }
            }
            if (checkmateCount == 8) { return true; }
            else { return false; }
        }
        public bool Check_Rook() // checks if the king is in check by a rook // WORKING
        {
            for (int x = 0; x < 8; x++)  // from 1-8 on the chess board
            {
                if (board[Convert.ToInt32(kingPosition[1]), x] == "R")  // checks if there is a rooke in the same row as the king
                {
                    for (int x_gap = x + 1; x_gap < Convert.ToInt32(kingPosition[0]); x_gap++)  // if a rook is found, for the positions on the board between the rook and the king
                    {
                        if (board[Convert.ToInt32(kingPosition[1]), x_gap] != ".")  // checks if the space is empty
                        {
                            return false; // if the space is not empty this means there is not a clear path between the king and the rook, so the king is not in check
                        }
                    }
                    return true;  // if all the spaces between them are empty, the rooks path is clear and the king is in check.
                }
            }
            for (int y = 0; y < 8; y++)  // from 1-8 on the chess board
            {
                if (board[y, Convert.ToInt32(kingPosition[0])] == "R")  // checks if there is a rooke in the same collumn as the king, repeats code above
                {
                    for (int y_gap = y + 1; y_gap < Convert.ToInt32(kingPosition[0]); y_gap++)
                    {
                        if (board[y_gap, Convert.ToInt32(kingPosition[0])] != ".")
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        public bool Check_Knight() // checks if the king is in check by a knight // WORKING
        {
            if (Convert.ToInt32(kingPosition[0]) < 6 & Convert.ToInt32(kingPosition[1]) > 1) // checks top right corner possible possitions
            {
                if (board[Convert.ToInt32(kingPosition[1]) - 2, Convert.ToInt32(kingPosition[0]) + 1] == "H") // north right spot
                {
                    return true;  // if theres a kinght there, the king is in check
                }
                if (board[Convert.ToInt32(kingPosition[1]) - 1, Convert.ToInt32(kingPosition[0]) + 2] == "H")  // east upper spot
                {
                    return true;
                }
            }
            if (Convert.ToInt32(kingPosition[0]) < 6 & Convert.ToInt32(kingPosition[1]) < 6)  //checks the bottom right possible positions
            {
                if (board[Convert.ToInt32(kingPosition[1]) + 1, Convert.ToInt32(kingPosition[0]) + 2] == "H") // east lower spot
                {
                    return true;
                }

                if (board[Convert.ToInt32(kingPosition[1]) + 2, Convert.ToInt32(kingPosition[0]) + 1] == "H")  // south right spot
                {
                    return true;
                }
            }
            if (Convert.ToInt32(kingPosition[0]) > 1 & Convert.ToInt32(kingPosition[1]) < 6)  //checks the bottom left possible positions
            {
                if (board[Convert.ToInt32(kingPosition[1]) + 2, Convert.ToInt32(kingPosition[0]) - 1] == "H") // south left spot
                {
                    return true;
                }
                if (board[Convert.ToInt32(kingPosition[1]) + 1, Convert.ToInt32(kingPosition[0]) - 2] == "H")  // west lower spot
                {
                    return true;
                }
            }
            if (Convert.ToInt32(kingPosition[0]) > 1 & Convert.ToInt32(kingPosition[1]) > 1)  //checks the top left possible positions
            {
                if (board[Convert.ToInt32(kingPosition[1]) - 1, Convert.ToInt32(kingPosition[0]) - 2] == "H") // west upper spot
                {
                    return true;
                }
                if (board[Convert.ToInt32(kingPosition[1]) - 2, Convert.ToInt32(kingPosition[0]) - 1] == "H")  // north left spot
                {
                    return true;
                }
            }
            return false; // if a knight is not in any of these places, the king is not in check
        }
        public bool Check_Pawn()  // checks if the king is in check by a pawn // WORKING
        {  // the statement below checks if an opponents pawn are in the squares directly northeast and northwest, relative to the king
            if (Convert.ToInt32(kingPosition[1]) > 0)
            {
                if (Convert.ToInt32(kingPosition[0]) > 0) // checking north west diagonal
                {
                    if (board[Convert.ToInt32(kingPosition[1]) - 1, Convert.ToInt32(kingPosition[0]) - 1] == "P")
                    {
                        return true;  // if an opponent's pawn is in either of these spaces the piece is in check
                    }
                }
                if (Convert.ToInt32(kingPosition[0]) < 7) // checking north east diagonal
                {
                    if (board[Convert.ToInt32(kingPosition[1]) - 1, Convert.ToInt32(kingPosition[0]) + 1] == "P")
                    {
                        return true;  // if an opponent's pawn is in either of these spaces the piece is in check
                    }
                }
            }
            return false;
        }
        public bool Check_Bishop()  // checks if the king is in check by a bishop// WOKRING
        {
            if (Convert.ToInt32(kingPosition[1]) > 0 & Convert.ToInt32(kingPosition[0]) < 7) // checking north east diagonal
            {
                for (int i = 1; Convert.ToInt32(kingPosition[1]) - i >= 0 & Convert.ToInt32(kingPosition[0]) + i <= 7; i++)  // for each diagonal postion
                {
                    if (board[Convert.ToInt32(kingPosition[1]) - i, Convert.ToInt32(kingPosition[0]) + i] == "B")  // if there is a bishop in that space
                    {
                        for (int j = 1; i - j > 0; j++) // for each space between the bishop and the king
                        {
                            if (board[(Convert.ToInt32(kingPosition[1]) - i) + j, (Convert.ToInt32(kingPosition[0]) + i) - j] != ".")  // if the space isnt empty
                            {
                                return false; // this means there is a piece in the way and the king is not in check
                            }
                        }
                        return true;  // if no pieces are in the way this means the bishop has a clear path and the king is in check.
                    }
                }
            }
            if (Convert.ToInt32(kingPosition[0]) > 0 & Convert.ToInt32(kingPosition[1]) > 0) // checking north west diagonal
            {
                for (int i = 1; Convert.ToInt32(kingPosition[0]) - i >= 0 & Convert.ToInt32(kingPosition[1]) - i >= 0; i++)
                {
                    if (board[Convert.ToInt32(kingPosition[1]) - i, Convert.ToInt32(kingPosition[0]) - i] == "B")
                    {
                        for (int j = 1; i - j > 0; j++)
                        {
                            if (board[(Convert.ToInt32(kingPosition[1]) - i) + j, (Convert.ToInt32(kingPosition[0]) - i) + j] != ".")
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            if (Convert.ToInt32(kingPosition[0]) > 0 & Convert.ToInt32(kingPosition[1]) < 7) // checking south west diagonal
            {
                for (int i = 1; i + Convert.ToInt32(kingPosition[1]) <= 7 & Convert.ToInt32(kingPosition[0]) - i >= 0; i++)
                {
                    if (board[Convert.ToInt32(kingPosition[1]) + i, Convert.ToInt32(kingPosition[0]) - i] == "B")
                    {
                        for (int j = 1; i - j > 0; j++)
                        {
                            if (board[(Convert.ToInt32(kingPosition[1]) + i) - j, (Convert.ToInt32(kingPosition[0]) - i) + j] != ".")
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            if (Convert.ToInt32(kingPosition[0]) < 7 & Convert.ToInt32(kingPosition[1]) < 7) // checking south east diagonal
            {
                for (int i = 1; Convert.ToInt32(kingPosition[0]) + i <= 7 & Convert.ToInt32(kingPosition[1]) + i <= 7; i++)
                {
                    if (board[Convert.ToInt32(kingPosition[1]) + i, Convert.ToInt32(kingPosition[0]) + i] == "B")
                    {
                        for (int j = 1; i - j > 0; j++)
                        {
                            if (board[(Convert.ToInt32(kingPosition[1]) + i) - j, (Convert.ToInt32(kingPosition[0]) + i) - j] != ".")
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        public bool Check_Queen() // checks if the king is in check by a bishop
        {  // as the movement pattern for a queen is the same as a rook and a bishop combined, i reused the from both in one function
            for (int x = 0; x < 8; x++)  // Rook
            {
                if (board[Convert.ToInt32(kingPosition[1]), x] == "Q")
                {
                    for (int x_gap = x + 1; x_gap < Convert.ToInt32(kingPosition[0]); x_gap++)
                    {
                        if (board[Convert.ToInt32(kingPosition[1]), x_gap] != ".")
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            for (int y = 0; y < 8; y++)
            {
                if (board[y, Convert.ToInt32(kingPosition[0])] == "Q")
                {
                    for (int y_gap = y + 1; y_gap < Convert.ToInt32(kingPosition[0]); y_gap++)
                    {
                        if (board[y_gap, Convert.ToInt32(kingPosition[0])] != ".")
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }  // Bishop
            if (Convert.ToInt32(kingPosition[1]) > 0 & Convert.ToInt32(kingPosition[0]) < 7)
            {
                for (int i = 1; Convert.ToInt32(kingPosition[1]) - i >= 0 & Convert.ToInt32(kingPosition[0]) + i <= 7; i++)
                {
                    if (board[Convert.ToInt32(kingPosition[1]) - i, Convert.ToInt32(kingPosition[0]) + i] == "Q")
                    {
                        for (int j = 1; i - j > 0; j++)
                        {
                            if (board[(Convert.ToInt32(kingPosition[1]) - i) + j, (Convert.ToInt32(kingPosition[0]) + i) - j] != ".")
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            if (Convert.ToInt32(kingPosition[0]) > 0 & Convert.ToInt32(kingPosition[1]) > 0)
            {
                for (int i = 1; Convert.ToInt32(kingPosition[0]) - i >= 0 & Convert.ToInt32(kingPosition[1]) - i >= 0; i++)
                {
                    if (board[Convert.ToInt32(kingPosition[1]) - i, Convert.ToInt32(kingPosition[0]) - i] == "Q")
                    {
                        for (int j = 1; i - j > 0; j++)
                        {
                            if (board[(Convert.ToInt32(kingPosition[1]) - i) + j, (Convert.ToInt32(kingPosition[0]) - i) + j] != ".")
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            if (Convert.ToInt32(kingPosition[0]) > 0 & Convert.ToInt32(kingPosition[1]) < 7)
            {
                for (int i = 1; i + Convert.ToInt32(kingPosition[1]) <= 7 & Convert.ToInt32(kingPosition[0]) - i >= 0; i++)
                {
                    if (board[Convert.ToInt32(kingPosition[1]) + i, Convert.ToInt32(kingPosition[0]) - i] == "Q")
                    {
                        for (int j = 1; i - j > 0; j++)
                        {
                            if (board[(Convert.ToInt32(kingPosition[1]) + i) - j, (Convert.ToInt32(kingPosition[0]) - i) + j] != ".")
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            if (Convert.ToInt32(kingPosition[0]) < 7 & Convert.ToInt32(kingPosition[1]) < 7)
            {
                for (int i = 1; Convert.ToInt32(kingPosition[0]) + i <= 7 & Convert.ToInt32(kingPosition[1]) + i <= 7; i++)
                {
                    if (board[Convert.ToInt32(kingPosition[1]) + i, Convert.ToInt32(kingPosition[0]) + i] == "Q")
                    {
                        for (int j = 1; i - j > 0; j++)
                        {
                            if (board[(Convert.ToInt32(kingPosition[1]) + i) - j, (Convert.ToInt32(kingPosition[0]) + i) - j] != ".")
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }
    class Poker : Queue<string>
    {
        public Poker() { DealtCards = new List<string>(DealtCards); }
        public Poker(string newgame)
        {
            turn1 = false;
            turn2 = false;
            string Heartsfile = System.IO.File.ReadAllText("C:\\Users\\yonat\\OneDrive - Bedford School\\Computer Science NEA\\Hearts.txt");
            Hearts = Heartsfile.Split('£');
            string Clubsfile1 = System.IO.File.ReadAllText("C:\\Users\\yonat\\OneDrive - Bedford School\\Computer Science NEA\\Clubs.txt");
            Clubs = Clubsfile1.Split('£');
            string Spadesfile2 = System.IO.File.ReadAllText("C:\\Users\\yonat\\OneDrive - Bedford School\\Computer Science NEA\\Spades.txt");
            Spades = Spadesfile2.Split('=');
            string Diamondsfile3 = System.IO.File.ReadAllText("C:\\Users\\yonat\\OneDrive - Bedford School\\Computer Science NEA\\Diamonds.txt");
            Diamonds = Diamondsfile3.Split('=');
            string[] HeartsAce = Hearts[0].Split('\n');
            string[] ClubsAce = Clubs[0].Split('\n');
            string[] SpadesAce = Spades[0].Split('\n');
            string[] DiamondsAce = Diamonds[0].Split('\n');
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(HeartsAce[i].Trim('\r') + ClubsAce[i].Trim('\r') + SpadesAce[i].Trim('\r') + DiamondsAce[i].Trim('\r'));
            }
            int oneCard = 0;
            int whichSet = 0;
            Random set = new Random();
            Random cardsInset = new Random();
            while (base.Count < 52)
            {
                whichSet = (set.Next(1, 5));
                if (whichSet == 1)
                {
                    if (HeartSet.Count != 0)
                    {
                        oneCard = cardsInset.Next(0, HeartSet.Count - 1);
                        base.Enqueue(HeartSet[oneCard]);
                        HeartSet.RemoveAt(oneCard);
                    }
                }
                if (whichSet == 2)
                {
                    if (ClubSet.Count != 0)
                    {
                        oneCard = cardsInset.Next(0, ClubSet.Count - 1);
                        base.Enqueue(ClubSet[oneCard]);
                        ClubSet.RemoveAt(oneCard);
                    }
                }
                if (whichSet == 3)
                {
                    if (SpadeSet.Count != 0)
                    {
                        oneCard = cardsInset.Next(0, SpadeSet.Count - 1);
                        base.Enqueue(SpadeSet[oneCard]);
                        SpadeSet.RemoveAt(oneCard);
                    }
                }
                if (whichSet == 4)
                {
                    if (DiamondSet.Count != 0)
                    {
                        oneCard = cardsInset.Next(0, DiamondSet.Count - 1);
                        base.Enqueue(DiamondSet[oneCard]);
                        DiamondSet.RemoveAt(oneCard);
                    }
                }
            }
            int count = 0;
            string[] cardsDealt = new string[] { };
            string[] cardSpecification = new string[] { };
            while (count != 3)
            {
                int cardNumber = 0;
                string card = base.Dequeue();
                DealtCards.Add(card);
                cardSpecification = card.Split(':');
                if (cardSpecification[0] == "HEART")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Hearts[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "CLUB")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Clubs[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "SPADE")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Spades[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "DIAMOND")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Diamonds[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
            }
            users_cards = base.Dequeue() + "-" + base.Dequeue();
            npc_cards = base.Dequeue() + "-" + base.Dequeue();
            string s = base.Peek();
        }
        static public int pot = 20;
        int instance = 0;
        static public int user_money = 1000;
        static public int NPC_money = 1500;
        static public int usermoneytopot = 0;
        static public int npcmoneytopot = 0;
        static public int user_points { get; set; }
        static public int npc_points { get; set; }
        static public string users_cards = "";
        static public string npc_cards = "";
        private string maxset;
        static public string user_action { get; set; }
        static public string npc_action { get; set; }
        protected bool turn1 { get; set; }
        protected bool turn2 { get; set; }
        static public bool user_Action = false;
        static public bool npc_Action = false;
        static public bool NPCWin = false;
        static public bool userWin = false;
        bool gameCompleted = false;
        private bool royalFlush = false; //A,10,J,Q,K
        private bool ht_straightFlush = false;
        private bool straightFlush = false; //A,then any numbers but have to be the same set but have to be in a sequence
        private bool fourOfAKind = false; //4 of the same number
        private bool fullHouse = false; //3 cards of 1 number and 2 of another
        private bool flush = false; //Same set but not in a sequence
        private bool ht_straight = false; //A, then the following cards.
        private bool straight = false; //Five cards in a sequence but not in the same set
        private bool threeOfAKind = false; //3 cards of the same number, the others dont matter
        private bool twoPair = false; //A pair of 2 different numbers
        private bool onePair = false; //A pair of a number and nothing else.
        private bool noPair = false; //nothing as a link.
        static public string[] Hearts = new string[] { };
        static public string[] Clubs = new string[] { };
        static public string[] Spades = new string[] { };
        static public string[] Diamonds = new string[] { };
        private List<string> HeartSet = new List<string>(13)
        {
            "HEART:1","HEART:2","HEART:3","HEART:4","HEART:5","HEART:6","HEART:7","HEART:8","HEART:9"
            , "HEART:T","HEART:J","HEART:Q","HEART:K"
        };
        private List<string> ClubSet = new List<string>(13)
        {
            "CLUB:1","CLUB:2","CLUB:3","CLUB:4","CLUB:5","CLUB:6","CLUB:7","CLUB:8","CLUB:9"
            ,"CLUB:T","CLUB:J","CLUB:Q","CLUB:K"
        };
        private List<string> SpadeSet = new List<string>(13)
        {
            "SPADE:1","SPADE:2","SPADE:3","SPADE:4","SPADE:5","SPADE:6","SPADE:7","SPADE:8","SPADE:9"
            ,"SPADE:T","SPADE:J","SPADE:Q","SPADE:K"
        };
        private List<string> DiamondSet = new List<string>(13)
        {
            "DIAMOND:1","DIAMOND:2","DIAMOND:3","DIAMOND:4","DIAMOND:5","DIAMOND:6","DIAMOND:7",
            "DIAMOND:8","DIAMOND:9","DIAMOND:T","DIAMOND:J","DIAMOND:Q","DIAMOND:K"
        };
        static public List<string> DealtCards = new List<string>(5);
        private List<string> Copy = new List<string>();
        protected Dictionary<int, string[]> cardStorer = new Dictionary<int, string[]>();
        public void cheat()
        {
            int number = 0;
            string s = Peek();
            string[] ar1 = new string[] { };
            ar1 = s.Split(':');
            string[] display = new string[] { };
            
            if (ar1[0] == "HEART")
            {
                number = Convert.ToInt32(ar1[1]) - 1;
                display = Hearts[number].Split('\n');
            }
            if (ar1[0] == "CLUB")
            {
                number = Convert.ToInt32(ar1[1]) - 1;
                display = Clubs[number].Split('\n');
            }
            if (ar1[0] == "SPADE")
            {
                number = Convert.ToInt32(ar1[1]) - 1;
                display = Spades[number].Split('\n');
            }
            if (ar1[0] == "DIAMOND")
            {
                number = Convert.ToInt32(ar1[1]) - 1;
                display = Diamonds[number].Split('\n');
            }
            for (int i = 0; i < 11; i++)
            {
                Console.WriteLine(display[i]);
            }
        }
        public bool winner()
        {
            if (npc_action != "FOLD" && user_action != "FOLD")
            {
                if (turn1 == true && turn2 == true)
                {
                    if (npc_points > user_points) { NPCWin = true; }
                    else if (npc_points < user_points) { userWin = true; }
                    else if (npc_points == user_points) { Console.WriteLine("Tie"); gameCompleted = true; }
                }
            }
            if (NPCWin == true && userWin == false)
            {
                Console.WriteLine($"NPC is the winner of this round with:");
                string[] cardsToDeck = Poker.npc_cards.Split('-');
                string store;
                int cardNumber = 0;
                string[] cardSpecification = new string[] { };
                string[] npccarddisplay = new string[] { };
                Dictionary<int, string[]> usercardStore = new Dictionary<int, string[]>();
                for (int i = 0; i < cardsToDeck.Length; i++)
                {
                    store = cardsToDeck[i];
                    cardSpecification = store.Split(':');
                    if (cardSpecification[0] == "HEART")
                    {
                        if (cardSpecification[1] == "T") { cardNumber = 10; }
                        else if (cardSpecification[1] == "J") { cardNumber = 11; }
                        else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                        else if (cardSpecification[1] == "K") { cardNumber = 13; }
                        else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                        npccarddisplay = Poker.Hearts[cardNumber - 1].Split('\n');
                        usercardStore.Add(i, npccarddisplay);
                    }
                    if (cardSpecification[0] == "CLUB")
                    {
                        if (cardSpecification[1] == "T") { cardNumber = 10; }
                        else if (cardSpecification[1] == "J") { cardNumber = 11; }
                        else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                        else if (cardSpecification[1] == "K") { cardNumber = 13; }
                        else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                        npccarddisplay = Poker.Clubs[cardNumber - 1].Split('\n');
                        usercardStore.Add(i, npccarddisplay);
                    }
                    if (cardSpecification[0] == "SPADE")
                    {
                        if (cardSpecification[1] == "T") { cardNumber = 10; }
                        else if (cardSpecification[1] == "J") { cardNumber = 11; }
                        else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                        else if (cardSpecification[1] == "K") { cardNumber = 13; }
                        else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                        npccarddisplay = Poker.Spades[cardNumber - 1].Split('\n');
                        usercardStore.Add(i, npccarddisplay);
                    }
                    if (cardSpecification[0] == "DIAMOND")
                    {
                        if (cardSpecification[1] == "T") { cardNumber = 10; }
                        else if (cardSpecification[1] == "J") { cardNumber = 11; }
                        else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                        else if (cardSpecification[1] == "K") { cardNumber = 13; }
                        else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                        npccarddisplay = Poker.Diamonds[cardNumber - 1].Split('\n');
                        usercardStore.Add(i, npccarddisplay);
                    }
                }
                string[] ar1 = usercardStore[0];
                string[] ar2 = usercardStore[1];
                for (int i = 0; i < 11; i++)
                {
                    Console.WriteLine(ar1[i].Trim('\r') + " " + ar2[i].Trim('\r'));
                }

                gameCompleted = true;
            }
            if (userWin == true && NPCWin == false)
            {
                Console.WriteLine($"You are the winner");
                gameCompleted = true;
            }
            return gameCompleted;
        }
        void cardsDealt()
        {
            int count = 3;
            string[] cardsDealt = new string[] { };
            string[] cardSpecification = new string[] { };
            if (turn1 == true && turn2 == true)
            {
                count = 4;
                int cardNumber = 0;
                string card = base.Dequeue();
                DealtCards.Add(card);
                cardSpecification = card.Split(':');
                if (cardSpecification[0] == "HEART")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Hearts[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "CLUB")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Clubs[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "SPADE")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Spades[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "DIAMOND")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Diamonds[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
            }
            if (turn1 == true)
            {
                int cardNumber = 0;
                string card = base.Dequeue();
                DealtCards.Add(card);
                cardSpecification = card.Split(':');
                if (cardSpecification[0] == "HEART")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Hearts[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "CLUB")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Clubs[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "SPADE")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Spades[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
                if (cardSpecification[0] == "DIAMOND")
                {
                    if (cardSpecification[1] == "T") { cardNumber = 10; }
                    else if (cardSpecification[1] == "J") { cardNumber = 11; }
                    else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                    else if (cardSpecification[1] == "K") { cardNumber = 13; }
                    else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                    cardsDealt = Diamonds[cardNumber - 1].Split('\n');
                    cardStorer.Add(count, cardsDealt);
                    count += 1;
                }
            }
            else
            {

            }
        }
        public void cardsDealtDisplay()
        {
            completedTurnCheck();
            if (turn1 == true && turn2 == false) { cardsDealt(); }
            if (turn1 == true && turn2 == true)
            {
                if (instance == 0)
                {
                    cardsDealt(); instance++;
                }
            }
            string[] array1 = cardStorer[0];
            string[] array2 = cardStorer[1];
            string[] array3 = cardStorer[2];
            if (turn1 == true && turn2 == true)
            {
                string[] array4 = cardStorer[3];
                string[] array5 = cardStorer[4];
                for (int i = 0; i < 11; i++)
                {
                    Console.WriteLine(array1[i].Trim('\r') + "  " + array2[i].Trim('\r') + "  " + array3[i].Trim('\r') + " "
                        + array4[i].Trim('\r') + " "
                        + array5[i].Trim('\r'));
                }
            }
            else if (turn1 == true)
            {
                string[] array4 = cardStorer[3];
                for (int i = 0; i < 11; i++)
                {
                    Console.WriteLine(array1[i].Trim('\r') + "  " + array2[i].Trim('\r') + "  " + array3[i].Trim('\r') + " "
                        + array4[i].Trim('\r'));
                }
            }
            else
            {
                for (int i = 0; i < 11; i++)
                {
                    Console.WriteLine(array1[i].Trim('\r') + "  " + array2[i].Trim('\r') + "  " + array3[i].Trim('\r'));
                }
            }
        }
        public void wincheck(string npc_or_usercards, string whichPlayer, List<string> dealt_cards)
        {
            royalFlush = false; //A,10,J,Q,K
            ht_straightFlush = false;
            straightFlush = false; //A,then any numbers but have to be the same set but have to be in a sequence
            fourOfAKind = false; //4 of the same number
            fullHouse = false; //3 cards of 1 number and 2 of another
            flush = false; //Same set but not in a sequence
            ht_straight = false; //A, then the following cards.
            straight = false; //Five cards in a sequence but not in the same set
            threeOfAKind = false; //3 cards of the same number, the others dont matter
            twoPair = false; //A pair of 2 different numbers
            onePair = false; //A pair of a number and nothing else.
            Copy = new List<string>(dealt_cards);
            int numberOfSpades = 0;
            int numberOfHearts = 0;
            int numberOfDiamonds = 0;
            int numberOfClubs = 0;
            int[] numeberOfRepeatedNumbers = new int[14];
            string[] cardSpecifications = new string[] { };
            string[] userornpccards = new string[] { };
            userornpccards = npc_or_usercards.Split('-');
            string[] usercardsSpecification = new string[] { };
            foreach (string element in userornpccards) { Copy.Add(element); }
            foreach (string element in Copy)
            {
                cardSpecifications = element.Split(':');
                if (cardSpecifications[0] == "CLUB") { numberOfClubs += 1; }
                if (cardSpecifications[0] == "HEART") { numberOfHearts += 1; }
                if (cardSpecifications[0] == "DIAMOND") { numberOfDiamonds += 1; }
                if (cardSpecifications[0] == "SPADE") { numberOfSpades += 1; }
            }
            int[] maximumOccurance = { numberOfDiamonds, numberOfHearts, numberOfClubs, numberOfSpades };
            if (Copy.Count(n => n[n.Length - 1] == '1') > 0)
            {

                if (maxSet(maximumOccurance, numberOfDiamonds, numberOfClubs, numberOfHearts, numberOfSpades) == "HEART")
                {
                    removingCards("HEART", out List<string> CopyoFDeck);
                    int royal_flush = 0;
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == '1');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'T');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'J');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'Q');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'K');
                    if (royal_flush == 5)
                    {
                        royalFlush = true;
                    }
                    else
                    {
                        CopyoFDeck.Remove("HEART:1");
                        int[] sequence = new int[4];
                        for (int a = 0; a < CopyoFDeck.Count; a++)
                        {
                            cardSpecifications = CopyoFDeck[a].Split(':');
                            if (cardSpecifications[1] == "T") { sequence[a] = 10; }
                            else if (cardSpecifications[1] == "J") { sequence[a] = 11; }
                            else if (cardSpecifications[1] == "Q") { sequence[a] = 12; }
                            else if (cardSpecifications[1] == "K") { sequence[a] = 13; }
                            else
                            {
                                sequence[a] = Convert.ToInt32(cardSpecifications[1]);
                            }

                        }
                        Array.Sort(sequence);
                        if (sequence.SequenceEqual(Enumerable.Range(sequence.Min(), sequence.Count())))
                        {
                            ht_straightFlush = true;
                        }
                    }
                }
                if (maxSet(maximumOccurance, numberOfDiamonds, numberOfClubs, numberOfHearts, numberOfSpades) == "SPADE")
                {
                    removingCards("SPADE", out List<string> CopyoFDeck);
                    int royal_flush = 0;
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == '1');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'T');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'J');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'Q');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'K');
                    if (royal_flush == 5)
                    {
                        royalFlush = true;
                    }
                    else
                    {
                        CopyoFDeck.Remove("SPADE:1");
                        int[] sequence = new int[4];
                        for (int a = 0; a < CopyoFDeck.Count; a++)
                        {
                            cardSpecifications = CopyoFDeck[a].Split(':');
                            if (cardSpecifications[1] == "T") { sequence[a] = 10; }
                            else if (cardSpecifications[1] == "J") { sequence[a] = 11; }
                            else if (cardSpecifications[1] == "Q") { sequence[a] = 12; }
                            else if (cardSpecifications[1] == "K") { sequence[a] = 13; }
                            else
                            {
                                sequence[a] = Convert.ToInt32(cardSpecifications[1]);
                            }

                        }
                        Array.Sort(sequence);
                        if (sequence.SequenceEqual(Enumerable.Range(sequence.Min(), sequence.Count())))
                        {
                            ht_straightFlush = true;
                        }
                    }
                }
                if (maxSet(maximumOccurance, numberOfDiamonds, numberOfClubs, numberOfHearts, numberOfSpades) == "CLUB")
                {
                    removingCards("CLUB", out List<string> CopyoFDeck);
                    int royal_flush = 0;
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == '1');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'T');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'J');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'Q');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'K');
                    if (royal_flush == 5)
                    {
                        royalFlush = true;
                    }
                    else
                    {
                        CopyoFDeck.Remove("CLUB:1");
                        int[] sequence = new int[4];
                        for (int a = 0; a < CopyoFDeck.Count; a++)
                        {
                            cardSpecifications = CopyoFDeck[a].Split(':');
                            if (cardSpecifications[1] == "T") { sequence[a] = 10; }
                            else if (cardSpecifications[1] == "J") { sequence[a] = 11; }
                            else if (cardSpecifications[1] == "Q") { sequence[a] = 12; }
                            else if (cardSpecifications[1] == "K") { sequence[a] = 13; }
                            else
                            {
                                sequence[a] = Convert.ToInt32(cardSpecifications[1]);
                            }

                        }
                        Array.Sort(sequence);
                        if (sequence.SequenceEqual(Enumerable.Range(sequence.Min(), sequence.Count())))
                        {
                            ht_straightFlush = true;
                        }
                    }
                }
                if (maxSet(maximumOccurance, numberOfDiamonds, numberOfClubs, numberOfHearts, numberOfSpades) == "DIAMOND")
                {
                    removingCards("DIAMOND", out List<string> CopyoFDeck);
                    int royal_flush = 0;
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == '1');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'T');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'J');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'Q');
                    royal_flush += CopyoFDeck.Count(n => n[n.Length - 1] == 'K');
                    if (royal_flush == 5)
                    {
                        royalFlush = true;
                    }
                    else
                    {
                        CopyoFDeck.Remove("DIAMOND:1");
                        int[] sequence = new int[4];
                        for (int a = 0; a < CopyoFDeck.Count; a++)
                        {
                            cardSpecifications = CopyoFDeck[a].Split(':');
                            if (cardSpecifications[1] == "T") { sequence[a] = 10; }
                            else if (cardSpecifications[1] == "J") { sequence[a] = 11; }
                            else if (cardSpecifications[1] == "Q") { sequence[a] = 12; }
                            else if (cardSpecifications[1] == "K") { sequence[a] = 13; }
                            else
                            {
                                sequence[a] = Convert.ToInt32(cardSpecifications[1]);
                            }

                        }
                        Array.Sort(sequence);
                        if (sequence.SequenceEqual(Enumerable.Range(sequence.Min(), sequence.Count())))
                        {
                            ht_straightFlush = true;
                        }
                    }
                }
            }
            if (numberOfClubs == 5 || numberOfSpades == 5 || numberOfDiamonds == 5 || numberOfHearts == 5)
            {
                if (maxSet(maximumOccurance, numberOfDiamonds, numberOfClubs, numberOfHearts, numberOfSpades) == "HEART")
                {
                    removingCards("HEART", out List<string> CopyoFDeck);
                    int[] sequence = new int[5];
                    for (int a = 0; a < CopyoFDeck.Count; a++)
                    {
                        cardSpecifications = CopyoFDeck[a].Split(':');
                        if (cardSpecifications[1] == "T") { sequence[a] = 10; }
                        else if (cardSpecifications[1] == "J") { sequence[a] = 11; }
                        else if (cardSpecifications[1] == "Q") { sequence[a] = 12; }
                        else if (cardSpecifications[1] == "K") { sequence[a] = 13; }
                        else
                        {
                            sequence[a] = Convert.ToInt32(cardSpecifications[1]);
                        }

                    }
                    Array.Sort(sequence);
                    if (sequence.SequenceEqual(Enumerable.Range(sequence.Min(), sequence.Count())))
                    {
                        straightFlush = true;
                    }
                }
                if (maxSet(maximumOccurance, numberOfDiamonds, numberOfClubs, numberOfHearts, numberOfSpades) == "SPADE")
                {
                    removingCards("SPADE", out List<string> CopyoFDeck);
                    int[] sequence = new int[5];
                    for (int a = 0; a < CopyoFDeck.Count; a++)
                    {
                        cardSpecifications = CopyoFDeck[a].Split(':');
                        if (cardSpecifications[1] == "T") { sequence[a] = 10; }
                        else if (cardSpecifications[1] == "J") { sequence[a] = 11; }
                        else if (cardSpecifications[1] == "Q") { sequence[a] = 12; }
                        else if (cardSpecifications[1] == "K") { sequence[a] = 13; }
                        else
                        {
                            sequence[a] = Convert.ToInt32(cardSpecifications[1]);
                        }

                    }
                    Array.Sort(sequence);
                    if (sequence.SequenceEqual(Enumerable.Range(sequence.Min(), sequence.Count())))
                    {
                        straightFlush = true;
                    }
                }
                if (maxSet(maximumOccurance, numberOfDiamonds, numberOfClubs, numberOfHearts, numberOfSpades) == "CLUB")
                {
                    removingCards("CLUB", out List<string> CopyoFDeck);
                    int[] sequence = new int[5];
                    for (int a = 0; a < CopyoFDeck.Count; a++)
                    {
                        cardSpecifications = CopyoFDeck[a].Split(':');
                        if (cardSpecifications[1] == "T") { sequence[a] = 10; }
                        else if (cardSpecifications[1] == "J") { sequence[a] = 11; }
                        else if (cardSpecifications[1] == "Q") { sequence[a] = 12; }
                        else if (cardSpecifications[1] == "K") { sequence[a] = 13; }
                        else
                        {
                            sequence[a] = Convert.ToInt32(cardSpecifications[1]);
                        }

                    }
                    Array.Sort(sequence);
                    if (sequence.SequenceEqual(Enumerable.Range(sequence.Min(), sequence.Count())))
                    {
                        straightFlush = true;
                    }
                }
                if (maxSet(maximumOccurance, numberOfDiamonds, numberOfClubs, numberOfHearts, numberOfSpades) == "DIAMOND")
                {
                    removingCards("DIAMOND", out List<string> CopyoFDeck);
                    int[] sequence = new int[5];
                    for (int a = 0; a < CopyoFDeck.Count; a++)
                    {
                        cardSpecifications = CopyoFDeck[a].Split(':');
                        if (cardSpecifications[1] == "T") { sequence[a] = 10; }
                        else if (cardSpecifications[1] == "J") { sequence[a] = 11; }
                        else if (cardSpecifications[1] == "Q") { sequence[a] = 12; }
                        else if (cardSpecifications[1] == "K") { sequence[a] = 13; }
                        else
                        {
                            sequence[a] = Convert.ToInt32(cardSpecifications[1]);
                        }

                    }
                    Array.Sort(sequence);
                    if (sequence.SequenceEqual(Enumerable.Range(sequence.Min(), sequence.Count())))
                    {
                        straightFlush = true;
                    }
                }
            }
            else if (numberOfClubs == 5 || numberOfSpades == 5 || numberOfDiamonds == 5 || numberOfHearts == 5) { Console.WriteLine("Flush"); }
            else
            {
                for (int i = 0; i < 10; i++) { numeberOfRepeatedNumbers[i] = Copy.Count(n => n[n.Length - 1] == Convert.ToChar(Convert.ToString(i))); }
                numeberOfRepeatedNumbers[10] = Copy.Count(n => n[n.Length - 1] == 'T');
                numeberOfRepeatedNumbers[11] = Copy.Count(n => n[n.Length - 1] == 'J');
                numeberOfRepeatedNumbers[12] = Copy.Count(n => n[n.Length - 1] == 'Q');
                numeberOfRepeatedNumbers[13] = Copy.Count(n => n[n.Length - 1] == 'K');
                List<int> sequence = new List<int>(7);
                sequence.Add(0);
                bool endLoop = false;
                //need to find a solution to where the the smaller number are in order and the larger ones are in the way.
                //this why I am eliminating the smaller numbers first, trying to find the number that would then follow the sequence.
                //however a sequence like 4,5,6,7 with then 11,12 at the end will not return true.
                //need to find a way to see if the largerst number is in sequence with the number before it, if not then remove it.
                if (Copy.Count(n => n[n.Length - 1] == '1') >= 1)
                {
                    numeberOfRepeatedNumbers[1] = 0;
                    for (int i = 0; i < sequence.Count; i++)
                    {
                        for (int j = 0; j < numeberOfRepeatedNumbers.Length; j++)
                        {
                            if (numeberOfRepeatedNumbers[j] > 1)
                            {
                                numeberOfRepeatedNumbers[j] = 1;
                            }
                            if (numeberOfRepeatedNumbers[j] == 1)
                            {
                                sequence.Add(j);
                                numeberOfRepeatedNumbers[j] = 0;
                                break;
                            }
                        }
                    }
                    endLoop = false;
                    int count = 0;
                    while (endLoop == false)
                    {
                        if (sequence.Count <= 3) { endLoop = true; }
                        if (sequence.SequenceEqual(Enumerable.Range(count, 4))) { ht_straight = true; endLoop = true; }
                        else
                        {
                            if (sequence[sequence.Count - 1] != sequence[sequence.Count - 2] + 1) { sequence.RemoveAt(sequence.Count - 1); }
                            if (sequence[0] == count)
                            {
                                sequence.RemoveAt(0);
                            }
                            else { count++; }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < sequence.Count; i++)
                    {
                        for (int j = 0; j < numeberOfRepeatedNumbers.Length; j++)
                        {
                            if (numeberOfRepeatedNumbers[j] > 1)
                            {
                                numeberOfRepeatedNumbers[j] = 1;
                            }
                            if (numeberOfRepeatedNumbers[j] == 1)
                            {
                                sequence.Add(j);
                                numeberOfRepeatedNumbers[j] = 0;
                                break;
                            }
                        }
                    }
                    endLoop = false;
                    int count = 0;
                    while (endLoop == false)
                    {
                        if (sequence.Count <= 4) { endLoop = true; }
                        if (sequence.SequenceEqual(Enumerable.Range(count, sequence.Count))) { straight = true; endLoop = true; }
                        else
                        {
                            if (sequence[sequence.Count - 1] != sequence[sequence.Count - 2] + 1) { sequence.RemoveAt(sequence.Count - 1); }
                            if (sequence[0] == count)
                            {
                                sequence.RemoveAt(0);
                            }
                            else { count++; }
                        }
                    }
                }
                for (int i = 0; i < 10; i++) { numeberOfRepeatedNumbers[i] = Copy.Count(n => n[n.Length - 1] == Convert.ToChar(Convert.ToString(i))); }
                numeberOfRepeatedNumbers[10] = Copy.Count(n => n[n.Length - 1] == 'T');
                numeberOfRepeatedNumbers[11] = Copy.Count(n => n[n.Length - 1] == 'J');
                numeberOfRepeatedNumbers[12] = Copy.Count(n => n[n.Length - 1] == 'Q');
                numeberOfRepeatedNumbers[13] = Copy.Count(n => n[n.Length - 1] == 'K');
                for (int i = 0; i < numeberOfRepeatedNumbers.Length; i++) { if (numeberOfRepeatedNumbers[i] == 1) { numeberOfRepeatedNumbers[i] = 0; } }
                int integer = numeberOfRepeatedNumbers.Aggregate((a, b) => a + b);
                if (straight == true || ht_straightFlush == true) { }
                else if (integer == 5) { fullHouse = true; }
                else if (integer == 4)
                {
                    if (numeberOfRepeatedNumbers.Contains(4)) { fourOfAKind = true; }
                    else { twoPair = true; }
                }
                else if (integer == 3) { threeOfAKind = true; }
                else if (integer == 2) { onePair = true; }
                else { noPair = true; }
            }
            if (whichPlayer == "USER")
            {
                user_points = pointCheck();
            }
            if (whichPlayer == "NPC")
            {
                npc_points = pointCheck();
            }
        }
        protected int pointCheck()
        {
            int points = 0;
            if (royalFlush == true) { points = 100; }
            if (ht_straightFlush == true) { points = 90; }
            if (straightFlush == true) { points = 80; }
            if (fourOfAKind == true) { points = 70; }
            if (fullHouse == true) { points = 60; }
            if (flush == true) { points = 60; }
            if (ht_straight == true) { points = 50; }
            if (straight == true) { points = 40; }
            if (threeOfAKind == true) { points = 30; }
            if (twoPair == true) { points = 20; }
            if (onePair == true) { points = 10; }
            if (noPair == true) { points = 0; }
            return points;
        }
        private void removingCards(string maxSet, out List<string> Copy1)
        {
            Copy1 = new List<string>(Copy);
            int count = 0;
            string[] cardSpecifications = new string[] { };
            while (count < Copy1.Count)
            {
                cardSpecifications = Copy1[count].Split(':');
                if (maxSet != cardSpecifications[0]) { Copy1.RemoveAt(count); count = 0; }
                else { count += 1; }
            }
        }
        private string maxSet(int[] occurance, int nod, int noc, int noh, int nos)
        {
            if (occurance.Max() == 5)
            {
                if (occurance.Max() == nod) { maxset = "DIAMOND"; }
                if (occurance.Max() == nos) { maxset = "SPADE"; }
                if (occurance.Max() == noh) { maxset = "HEART"; }
                if (occurance.Max() == noc) { maxset = "CLUB"; }
            }
            return maxset;
        }
        void completedTurnCheck()
        {
            if (npc_Action == true && user_Action == true && turn1 == true && turn2 == true) { winner(); }
            else if (npc_Action == true && user_Action == true && turn1 == true) { turn2 = true; }
            else if (npc_Action == true && user_Action == true) { turn1 = true; }
            user_Action = false; npc_Action = false;
        }
    }
    class userCards
    {
        Poker p = new Poker();
        public void actionByUser(string userAction)
        {
            string[] cardsToDeck = new string[] { };
            cardsToDeck = Poker.users_cards.Split('-');
            if (userAction == "FOLD")
            {
                for (int i = 0; i < cardsToDeck.Length; i++)
                {
                    p.Enqueue(cardsToDeck[i]);
                }
                Poker.user_action = "FOLD";
                Poker.users_cards = " ";
                cardsToDeck = new string[] { };
                Poker.NPCWin = true;
                Poker.user_Action = true;
                Poker.npc_Action = true;
                Console.WriteLine("NPC Wins");
            }
            if (userAction == "CHECK") { Poker.user_Action = true; }
            if (userAction == "CHECK CARDS")
            {
                string store;
                int cardNumber = 0;
                string[] cardSpecification = new string[] { };
                string[] usercarddisplay = new string[] { };
                Dictionary<int, string[]> usercardStore = new Dictionary<int, string[]>();
                for (int i = 0; i < cardsToDeck.Length; i++)
                {
                    store = cardsToDeck[i];
                    cardSpecification = store.Split(':');
                    if (cardSpecification[0] == "HEART")
                    {
                        if (cardSpecification[1] == "T") { cardNumber = 10; }
                        else if (cardSpecification[1] == "J") { cardNumber = 11; }
                        else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                        else if (cardSpecification[1] == "K") { cardNumber = 13; }
                        else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                        usercarddisplay = Poker.Hearts[cardNumber - 1].Split('\n');
                        usercardStore.Add(i, usercarddisplay);
                    }
                    if (cardSpecification[0] == "CLUB")
                    {
                        if (cardSpecification[1] == "T") { cardNumber = 10; }
                        else if (cardSpecification[1] == "J") { cardNumber = 11; }
                        else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                        else if (cardSpecification[1] == "K") { cardNumber = 13; }
                        else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                        usercarddisplay = Poker.Clubs[cardNumber - 1].Split('\n');
                        usercardStore.Add(i, usercarddisplay);
                    }
                    if (cardSpecification[0] == "SPADE")
                    {
                        if (cardSpecification[1] == "T") { cardNumber = 10; }
                        else if (cardSpecification[1] == "J") { cardNumber = 11; }
                        else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                        else if (cardSpecification[1] == "K") { cardNumber = 13; }
                        else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                        usercarddisplay = Poker.Spades[cardNumber - 1].Split('\n');
                        usercardStore.Add(i, usercarddisplay);
                    }
                    if (cardSpecification[0] == "DIAMOND")
                    {
                        if (cardSpecification[1] == "T") { cardNumber = 10; }
                        else if (cardSpecification[1] == "J") { cardNumber = 11; }
                        else if (cardSpecification[1] == "Q") { cardNumber = 12; }
                        else if (cardSpecification[1] == "K") { cardNumber = 13; }
                        else { cardNumber = Convert.ToInt32(cardSpecification[1]); }
                        usercarddisplay = Poker.Diamonds[cardNumber - 1].Split('\n');
                        usercardStore.Add(i, usercarddisplay);
                    }
                }
                string[] ar1 = usercardStore[0];
                string[] ar2 = usercardStore[1];
                for (int i = 0; i < 11; i++)
                {
                    Console.WriteLine(ar1[i].Trim('\r') + " " + ar2[i].Trim('\r'));
                }
            }
            if (userAction == "CALL")
            {
                if ((Poker.user_money > Poker.npcmoneytopot) && Poker.npcmoneytopot != 0)
                {
                    int callby = Poker.npcmoneytopot;
                    Poker.pot += callby;
                    Poker.user_money -= callby;
                    Poker.user_Action = true;
                    Poker.npc_Action = true;
                }
                else
                {
                    Console.WriteLine("\"It seems you do not have enough money to match the bet, I guess you have to go all in.");
                }
                
            }
            if (userAction == "BLUFF")
            {
                Random chance = new Random();
                Character c = new Character();
                int chancebluffworks = chance.Next(1, 10);
                if (chancebluffworks == 3 || chancebluffworks == 6 || chancebluffworks == 7)
                {
                    if (c.statsReturn("IQ") > 6)
                    {
                        Console.WriteLine("You bluff worked. And the enemy folded.");
                        Poker.userWin = true;
                        Poker.NPCWin = false;
                        Poker.user_Action = true;
                        Poker.npc_Action = true;
                    }
                    //bluff works.
                    //checks for high enough iq.
                }
                else
                {
                    Console.WriteLine("\"You really tried to bluff me, a god. How foolish.\"");
                }
            }
            if (userAction == "RAISE")
            {
                Console.Write($"How much do you want to raise by? (You have £{Poker.user_money}) ");
                int money = Convert.ToInt32(Console.ReadLine());
                if (Poker.user_money == money)
                {
                    Console.WriteLine($"You have gone all in");
                    Poker.pot += money;
                    Poker.user_money -= money;
                    Poker.npc_Action = true;
                }
                else
                {
                    Poker.usermoneytopot = money;
                    Poker.pot += money;
                    Poker.user_money -= money;
                }
                Console.WriteLine($"There is now £{Poker.pot} in the pot");
            }
            if (userAction == "PERCENT")
            {
                Console.WriteLine($"{Poker.user_points}% chance of winning");
            }


        }
        public void userCheck()
        {
            p.wincheck(Poker.users_cards, "USER", Poker.DealtCards);
        }
    }
    class NPCcards
    {
        Poker p = new Poker();
        public void npcCheck()
        {
            p.wincheck(Poker.npc_cards, "NPC", Poker.DealtCards);
        }
        public void decisionmakingbyNPC()
        {
            Random randomChanceOfFolding = new Random();
            if (Poker.user_action == "CALL")
            {
                Poker.npc_action = "CALL";
            }
            if (Poker.user_action == "FOLD")
            {
                Poker.NPCWin = true;
                Poker.user_Action = true;
                Poker.npc_Action = true;
            }
            else if (Poker.npc_points <= 30)
            {
                if (randomChanceOfFolding.Next(1, 100) % 10 == 0)
                {
                    if (Poker.user_action != "FOLD")
                    {
                        Poker.npc_action = "FOLD";
                        Poker.userWin = true;
                        Poker.user_Action = true;
                        Poker.npc_Action = true;
                    }
                }
                else if (Poker.npc_points >= 0)
                {
                    if (randomChanceOfFolding.Next(1, 10) % 2 == 0)
                    {
                        Poker.npc_action = "CALL";
                    }
                    else if (Poker.user_action == "CHECK")
                    {
                        Poker.npc_action = "CHECK";
                    }
                    else
                    {
                        Poker.npc_action = "RAISE";
                    }
                }
                else { Poker.NPCWin = true; Poker.npc_Action = true; }
            }
            else if (randomChanceOfFolding.Next(1, 10) % 2 == 0)
            {
                if (randomChanceOfFolding.Next(1, 10) % 2 == 0)
                {
                    Poker.npc_action = "CALL";
                }
                else
                {
                    Poker.npc_action = "RAISE";
                }
            }
            actionByNPC(Poker.npc_action);
        }
        void actionByNPC(string action)
        {
            if (action == "FOLD")
            {
                string[] cardsToDeck = new string[] { };
                for (int i = 0; i < cardsToDeck.Length; i++)
                {
                    p.Enqueue(cardsToDeck[i]);
                }
                Poker.npc_cards = " ";
                cardsToDeck = new string[] { };
                Poker.npc_Action = true;
                Poker.user_Action = true;
                Poker.userWin = true;
                Poker.NPCWin = true;
            }
            if (action == "CHECK") { Poker.npc_Action = true; }
            if (action == "CALL")
            {
                int callby = Poker.usermoneytopot;
                Poker.pot += callby;
                Poker.NPC_money -= callby;
                Poker.npcmoneytopot = callby;
                Poker.npc_Action = true;
                Poker.user_Action = true;
                Console.WriteLine($"There is now £{Poker.pot} in the pot");
            }
            if (action == "RAISE")
            {
                if (Poker.NPC_money != 0)
                {
                    Random numberOfCashRaised = new Random();
                    int money = numberOfCashRaised.Next(Poker.usermoneytopot, Poker.NPC_money + 1);
                    if (Poker.NPC_money == money)
                    {
                        Console.WriteLine($"NPC has gone all in with £{Poker.NPC_money}! You have to match that or go all in!");
                        if (Poker.user_money > Poker.NPC_money)
                        {
                            Console.WriteLine("You have more money than the npc, are you going to check, fold or raise");
                            Poker.user_action = Console.ReadLine().ToUpper();
                            if (Poker.user_action == "CHECK")
                            {
                                Console.WriteLine($"You have matched the bet, £{Poker.NPC_money} is going into the bank");
                                Poker.pot += money;
                                Poker.user_money -= money;
                                Poker.usermoneytopot = money;
                            }
                            if (Poker.user_action == "FOLD")
                            {
                                Poker.NPCWin = true;
                                Poker.npc_Action = true;
                                Poker.userWin = false;
                                Poker.user_Action = true;
                            }
                            else if (Poker.user_action == "RAISE")
                            {
                                Console.WriteLine($"Enter the amount you want to raise by, you can only got to £{Poker.user_money}");
                                Console.Write("£");
                                Puzzles.userIntegerInput = Convert.ToInt32(Console.ReadLine());
                                bool validRaise = false;
                                while (validRaise == false)
                                {
                                    if (Puzzles.userIntegerInput < money) { Console.WriteLine("You have to raise money larger than the NPC"); }
                                    else
                                    {
                                        int callby = Puzzles.userIntegerInput;
                                        Poker.pot += callby;
                                        Poker.user_money -= callby;
                                        Poker.usermoneytopot = callby;

                                    }
                                }
                            }
                        }
                        if (Poker.user_money < Poker.NPC_money)
                        {
                            Console.WriteLine("You have to go all in as you have less money than your opponent");
                            Poker.pot += Poker.user_money;
                            Poker.usermoneytopot = Poker.user_money;
                            Poker.user_money = 0;
                        }
                        Poker.pot += money;
                        Poker.npcmoneytopot = money;
                        Poker.NPC_money -= money;
                    }
                    else
                    {
                        Poker.pot += money;
                        Poker.npcmoneytopot = money;
                        Poker.NPC_money -= money;
                    }
                    Console.WriteLine($"There is now £{Poker.pot} in the pot");
                }
                else { Poker.npc_Action = true; }

            }
        }
    }
    class saveDataHandling
    {
        public static string userfilelocation { get; set; }
        public static string filepath { get; set; }
        public static string pathString { get; set; }
        public static string pathString2 { get; set; }
        public static int saveDataSlot = 0;
        protected static Dictionary<string, int> savedData = new Dictionary<string, int>(3);
        public static List<string> saveDataItems = new List<string>();
        public static string[] loaddataItems = new string[] { };
        public Dictionary<int, List<string>> saveddate = new Dictionary<int, List<string>>(3);
        public void sort(List<string> Tracker)
        {
            List<string> Storer = new List<string>(Tracker);
            Puzzles.Tracker_In_Order = new Dictionary<string, int>();
            //this will store the tracker list because I do not want to remove items from the tracker list itself.
            int counter = 0;
            string StringStorer;
            int index = 0;
            //the counter displays the number of recurances of the StringStorer
            bool loop = false;
            //this ends the loop when there are no more items in the list.
            StringStorer = Storer[index];
            //this will intially store the first item in the list.
            while (loop == false)
            {
                //if the storer contains the string.
                if (Storer.Contains(StringStorer)) { counter += 1; Storer.Remove(StringStorer); }
                //the counter increases and the string is removed. Therefore not being counted again
                else
                {
                    Puzzles.Tracker_In_Order.Add(StringStorer, counter);
                    //if there is no more strings in Storer that contain the string being checked,
                    //then the string and the counter is added to a dictionary<string, int>
                    counter = 0;
                    //counters returns to zero as a counter will commence for a new string.
                    StringStorer = "";
                    //same instance with the StringStorer.
                    if (Storer.Count == 0) { loop = true; }
                    //if the Storer list has nothing in it again, therefore ends the loop
                    else { StringStorer = Storer[0]; }
                    //otherwise, the string is then assigned a new string and the counting continues.
                }
            }
        }
    }
    class Display : saveDataHandling
    //inheritance
    {        
        public Display() { }
        public Display(string type)
        {
            if (type == "NEW GAME")
            //new game runs this
            {
                saveDataSlot += 1;
                //increases the number by 1, as it initally starts at 0
                savedData.Add($"Save Slot {saveDataSlot}: ", 0);
                saveddate.Add(saveDataSlot, new List<string>());
                //this adds a new list and 0 percent to 2 dictionaries, storing them, 1 for the display and the other to check that certain problems had been done.
            }
            if (type == "LOAD GAME")
            //making sure 
            {
                foreach (var key in savedData) { displayFunction($"{key.Key} {key.Value}%"); }
                //displayed the new game when I call display = new display("LOAD");
            }            
        }
        public Display(string type, int number)
        {
            if (type == "LOAD GAME")
            {
                if (number == 0) { displayFunction("No Saved Data"); }
                else { for (int i = 1; i <= number; i++) { displayFunction($"Save Data {i}"); } }
            }
        }
        string userInput = "";
        public static void displayFunction(string title)
        {
            for (int i = 0; i < title.Length; i++) { Console.Write("---"); }
            Console.Write(Environment.NewLine);
            Console.Write("|");
            for (int i = 0; i < title.Length - 1; i++) { Console.Write(" "); }
            Console.Write(title);
            for (int i = 0; i < title.Length - 1; i++) { Console.Write(" "); }
            Console.Write("|");
            Console.Write(Environment.NewLine);
            for (int i = 0; i < title.Length; i++) { Console.Write("---"); }
            Console.Write(Environment.NewLine);
        }
        public void menuDisplay()
        {
            displayFunction(" New Game ");
            displayFunction("Load  Game");
            displayFunction("   Quit   ");
        }
        public string optionsDisplay()
        {
            displayFunction("   OPTIONS  ");
            displayFunction("Check  Stats");
            displayFunction(" Difficulty ");
            displayFunction("    Menu    ");
            displayFunction("    Save    ");
            displayFunction("   Resume   ");
            displayFunction("    Quit    ");
            Console.Write("\nWhat do you want to do: ");
            userInput = Console.ReadLine().ToUpper();
            return userInput;
            //returns the user input from the menu displayed.
        }
    }
}
