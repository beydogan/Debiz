using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
namespace dersKayit
{
    class Program
    {
        static void Main(string[] args)
        {
         
            Console.WindowWidth = 100;
            Console.Title = "DEBIZ";
            Console.WriteLine("=== DEBIZ ===");
            Console.WriteLine("\n---- Load Files ----\n");
            // ------     Load -------
                //Load Courses File

                StreamReader courses = File.OpenText("C:\\courses.txt");
                Console.WriteLine("Loading Course File");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write("||");
                    System.Threading.Thread.Sleep(10);

                }
                Console.WriteLine("\n>Courses File Loaded.\n");

                //Load Student File
                StreamReader studentFile = File.OpenText("C:\\student.txt");
                Console.WriteLine("Loading Student File");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write("||");
                    System.Threading.Thread.Sleep(10);

                }
                Console.WriteLine("\n>Student File Loaded.");
            Console.WriteLine("\n--------------------");
            Console.Write("Press Any Key To Continue");
            Console.ReadKey();
            // ------ End of Load ------ 

            // ==================================== BACKGROUND OPERATIONS ===============================
            
            // Chars
            char spaceChar = ' ';
            char sep = ':';


            // Courses File and Array Operations
            
            string[][] coursesArray = new string[15][];
            
            for (int i = 0; i < 15; i++)
            {
                string coursesLine = courses.ReadLine();
                coursesArray[i] = coursesLine.Split(sep);
            }

            // Prerequisites

            bool[,] prereqArray = new bool[16,16];
            bool[,] prereqArray2 = new bool[16, 16];
            string[] tempReqArray;
            int pos2;
            for (int j = 0; j < 15; j++)
            {
                if (coursesArray[j][3] != " ") // önşart var mı diye kontrol ediyor
                {
                    tempReqArray = coursesArray[j][3].Split(spaceChar); // önşartları boşluğa göre ayırıyor
                    for (int i = 0; i < tempReqArray.Length; i++)
                    {

                        pos2 = Convert.ToInt32(tempReqArray[i].Substring(1)); // ayırdığı şartı C1 ise 1 yaparak pos2 olarak atıyor
                        prereqArray[j + 1, pos2] = true; // önşartı olan yeri true yapıyor

                    }
                }
            }

            //Copy Old Array
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    prereqArray2[i, j] = prereqArray[i, j];
                }
            }
            // End of Copy

            // Roy-Warshall Algorithm
            int n = 15;
            for(int k= 1; k<=n;k++)
                for(int i= 1; i<=n;i++)
                    for (int j = 1; j <= n; j++)
                    {
                        prereqArray2[i, j] = prereqArray2[i, j] | (prereqArray2[i, k] & prereqArray2[k, j]);
                    }
            // End of Roy-Warshall Algorithm


            //Weekly Program
            string[,] weeklyProgram = new string[5,8];
            string[] timesArr;
            char[] timesCharArr = new char[2];
            string times;
            int scheduleDay =0, scheduleTime=0;
            
            for (int k = 0; k < 15; k++)
            {
                times = coursesArray[k][4];
                timesArr = times.Split(spaceChar);

                for (int l = 0; l < timesArr.Length; l++)
                {
                    timesCharArr = timesArr[l].ToCharArray();

                    // Day switch
                    switch (Convert.ToString(timesCharArr[0]))
                    {
                        case "M":
                            scheduleDay = 0;
                            break;
                        case "T":
                            scheduleDay = 1;
                            break;
                        case "W":
                            scheduleDay = 2;
                            break;
                        case "H":
                            scheduleDay = 3;
                            break;
                        case "F":
                            scheduleDay = 4;
                            break;
                    }

                    // Time Switch
                    switch (Convert.ToString(timesCharArr[1]))
                    {
                        case "1":
                            scheduleTime = 0;
                            break;
                        case "2":
                            scheduleTime = 1;
                            break;
                        case "3":
                            scheduleTime = 2;
                            break;
                        case "4":
                            scheduleTime = 3;
                            break;
                        case "5":
                            scheduleTime = 4;
                            break;
                        case "6":
                            scheduleTime = 5;
                            break;
                        case "7":
                            scheduleTime = 6;
                            break;
                        case "8":
                            scheduleTime = 7;
                            break;
                    }

                    if (weeklyProgram[scheduleDay, scheduleTime] == null)
                    {
                        weeklyProgram[scheduleDay, scheduleTime] = coursesArray[k][0];
                    }
                    else
                    {
                        weeklyProgram[scheduleDay, scheduleTime] = weeklyProgram[scheduleDay, scheduleTime] + "-" + coursesArray[k][0];
                    }
                }
            }
            // End Of Weekly Program


            // Transcript
            string transcriptLine;

            int transcriptCounter = 0 ;
            do
            {
                transcriptLine = studentFile.ReadLine();
                transcriptCounter++;
            } while (!studentFile.EndOfStream);
            studentFile.Close();
            //Open Student File
            StreamReader studentFile2 = File.OpenText("C:\\student.txt");
            string[][] transcriptArr = new string[transcriptCounter][];

            for (int i = 0; i < transcriptCounter; i++)
            {
                transcriptLine = studentFile2.ReadLine();
                transcriptArr[i] = transcriptLine.Split(sep);
            }
            string[] grades = { "FF", "FD", "DD", "DC", "CC", "CB", "BB", "BA", "AA" };
            int transcriptOrder;
            string[,] studentCourseStatus = new string[15,2];
            // studentCourseStatus Initialize
            for (int i = 0; i < 15; i++)
            {

                        studentCourseStatus[i, 0] = "C" + (i + 1);

            }

            // transcript
            List<int> allCourses = new List<int>(); // geçilemeyen ve alınmayan bütün dersler
            for (int i = 1; i < 16; i++)
            {
                allCourses.Add(i);
            }
            for (int i = 0; i < transcriptArr.GetLength(0); i++)
            {
                transcriptOrder = Array.IndexOf(grades, transcriptArr[i][1]);

                for (int j = 0; j < 15; j++)
                {
                    if (coursesArray[j][0] == transcriptArr[i][0])
                    {
                        if (transcriptOrder > 1)
                        {
                            studentCourseStatus[j, 1] = "1";
                            allCourses.Remove(j+1);
                        }
                        else
                        {
                            studentCourseStatus[j, 1] = "0";
                            
                        }
                    }
                }
            }
            int[] allCoursesArr = allCourses.ToArray();
            studentFile2.Close();
            StreamReader studentFile3 = File.OpenText("C:\\student.txt");
            string[][] grade = new string[15][];
            int counter = 0, sayac = 0;
            do
            {
                grade[counter] = studentFile3.ReadLine().Split(sep);
                counter++;
            } while (!studentFile3.EndOfStream);
            sayac = counter;

            // ==================================== END OF  BACKGROUND OPERATIONS ===============================

            int choice;
            string input;
            do
            {
                choice = 0;
                short c;
                ConsoleKeyInfo key;
                string[] menuItems = { "Display", "Query", "Register", "Exit" };
                do
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\n----- Menu -----");
                    Console.ResetColor();
                    for (c = 0; c < menuItems.Length; c++)
                    {
                        if (choice == c)
                        {

                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(">>");
                            Console.WriteLine(menuItems[c].PadRight(14));
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine(menuItems[c].PadRight(16));
                            Console.ResetColor();
                        }
                    }
                    Console.Write("Select your choice with the arrow keys.");
                    key = Console.ReadKey(true);
                    if (key.Key.ToString() == "DownArrow")
                    {
                        choice++;
                        if (choice > menuItems.Length - 1) choice = 0;
                    }
                    else if (key.Key.ToString() == "UpArrow")
                    {
                        choice--;
                        if (choice < 0) choice = Convert.ToInt16(menuItems.Length - 1);
                    }
                } while (key.KeyChar != 13);

                int subChoice = 0;
                choice++;
                if (choice == 1)
                {
                    do
                    {
                        string[] subMenuItems = { "Course List", "Weekly Program", "Your Transcript", "Go To Menu" };
                        do
                        {

                            Console.Clear();
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("\n----- Sub Menu -----");
                            Console.ResetColor();
                            for (c = 0; c < subMenuItems.Length; c++)
                            {
                                if (subChoice == c)
                                {

                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write(">>");
                                    Console.WriteLine(subMenuItems[c].PadRight(18));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.Gray;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.WriteLine(subMenuItems[c].PadRight(20));
                                    Console.ResetColor();
                                }
                            }
                            Console.Write("Select your choice with the arrow keys.");
                            key = Console.ReadKey(true);
                            if (key.Key.ToString() == "DownArrow")
                            {
                                subChoice++;
                                if (subChoice > subMenuItems.Length - 1) subChoice = 0;
                            }
                            else if (key.Key.ToString() == "UpArrow")
                            {
                                subChoice--;
                                if (subChoice < 0) subChoice = Convert.ToInt16(subMenuItems.Length - 1);
                            }
                        } while (key.KeyChar != 13);
                        subChoice++;
                        Console.Clear();
                        // Writing Courses List
                        if (subChoice == 1)
                        {
                            Console.WriteLine("a. Courses List");
                            for (int j = 0; j < 15; j++)
                            {
                                if (j % 2 == 0)
                                {

                                    Console.BackgroundColor = ConsoleColor.Gray;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.CursorLeft = 10;
                                    Console.Write(coursesArray[j][0].PadRight(4));
                                    Console.CursorLeft = 14;
                                    Console.Write(coursesArray[j][1].PadRight(37));
                                    Console.CursorLeft = 51;
                                    Console.Write(coursesArray[j][2] + "\n");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.CursorLeft = 10;
                                    Console.Write(coursesArray[j][0].PadRight(4));
                                    Console.CursorLeft = 14;
                                    Console.Write(coursesArray[j][1].PadRight(37));
                                    Console.CursorLeft = 51;
                                    Console.Write(coursesArray[j][2] + "\n");
                                }

                            }
                        }
                        // Writing Weekly Program
                        else if (subChoice == 2)
                        {
                            Console.WriteLine("╔══════╦══════════════╦══════════════╦══════════════╦══════════════╦══════════════╗");
                            Console.WriteLine("║      ║    Monday    ║   Tuesday    ║  Wednesday   ║   Thursday   ║   Friday     ║");
                            Console.WriteLine("╠══════╬══════════════╬══════════════╬══════════════╬══════════════╬══════════════╣");
                            Console.WriteLine("║  1   ║              ║              ║              ║              ║              ║");
                            Console.WriteLine("║  2   ║              ║              ║              ║              ║              ║");
                            Console.WriteLine("║  3   ║              ║              ║              ║              ║              ║");
                            Console.WriteLine("║  4   ║              ║              ║              ║              ║              ║");
                            Console.WriteLine("║  5   ║              ║              ║              ║              ║              ║");
                            Console.WriteLine("║  6   ║              ║              ║              ║              ║              ║");
                            Console.WriteLine("║  7   ║              ║              ║              ║              ║              ║");
                            Console.WriteLine("║  8   ║              ║              ║              ║              ║              ║");
                            Console.WriteLine("╚══════╩══════════════╩══════════════╩══════════════╩══════════════╩══════════════╝");

                            int x = 3, y;

                            for (int i = 0; i < 8; i++)
                            {
                                y = 10;
                                for (int j = 0; j < 5; j++)
                                {
                                    Console.SetCursorPosition(y, x);
                                    Console.Write(weeklyProgram[j, i]);

                                    y = y + 15;
                                }
                                x = x + 1;
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                        // Writing Transcript
                        else if (subChoice == 3)
                        {
                            int x = 1, division = 0, y;
                            double Point = 0, GPA = 0;
                            Console.WriteLine("╔═══════════════════════════════════════════════════╦═══════════╗");

                            for (int j = 0; j < 15; j++)
                            {
                                for (int i = 0; i < 15; i++)
                                {
                                    if (counter > 0)
                                    {
                                        if (coursesArray[i][0] == grade[j][0])
                                        {
                                            y = 0;
                                            Console.SetCursorPosition(y, x);

                                            Console.WriteLine("║                                                   ║           ║");
                                            y = 1;
                                            Console.SetCursorPosition(y, x);
                                            for (int k = 0; k < 3; k++)
                                            {
                                                if (k == 1)
                                                {
                                                    y = 5;
                                                    Console.SetCursorPosition(y, x);
                                                }
                                                if (k == 2)
                                                {
                                                    y = 45;
                                                    Console.SetCursorPosition(y, x);
                                                }
                                                Console.Write(coursesArray[i][k]);

                                                if (k == 2)
                                                {
                                                    switch (grade[j][1])
                                                    {
                                                        case "AA":
                                                            Point = 4.00 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write(" Succesful ");

                                                            break;
                                                        case "BA":
                                                            Point = 3.50 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write(" Succesful ");
                                                            break;
                                                        case "BB":
                                                            Point = 3.00 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write(" Succesful ");
                                                            break;
                                                        case "CB":
                                                            Point = 2.50 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write(" Succesful ");
                                                            break;
                                                        case "CC":
                                                            Point = 2.00 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write(" Succesful ");
                                                            break;
                                                        case "DC":
                                                            Point = 1.50 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write(" Succesful ");
                                                            break;
                                                        case "DD":
                                                            Point = 1.00 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write(" Succesful ");
                                                            break;
                                                        case "FD":
                                                            Point = 0.50 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.Red;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write("Unsuccesful");
                                                            break;
                                                        case "FF":
                                                            Point = 0.00 * (Convert.ToInt32(coursesArray[i][k]));
                                                            Console.SetCursorPosition(53, x);
                                                            Console.BackgroundColor = ConsoleColor.Red;
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            Console.Write("Unsuccesful");
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    Console.ResetColor();
                                                    division = division + (Convert.ToInt32(coursesArray[i][k]));
                                                    GPA = GPA + Point;
                                                    Point = 0;
                                                }
                                                if (k == 2)
                                                {
                                                    y = 49;
                                                    Console.SetCursorPosition(y, x);
                                                    Console.Write(grade[j][1]);
                                                }
                                            }
                                            counter = counter - 1;
                                            x++;

                                        }
                                    }
                                }
                            }
                            Console.WriteLine();
                            Console.WriteLine("╚═══════════════════════════════════════════════════╩═══════════╝");
                            Console.WriteLine();

                            Console.Write("Your Grade Point Average (GPA): ");
                            Console.WriteLine(GPA / division);
                            counter = sayac;

       
                        }
                        else if (subChoice == 4)
                        {
                            
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Error Choice");
                        }
                        Console.Write("Press Any Key To Continue");
                        Console.ReadKey();
                    } while (subChoice != 3);
                    Console.Clear();
                } // end of choice 1

                // Query
                else if (choice == 2)
                {
                    
                    do
                    {

                        string[] subMenuItems = { "Listing Prerequisites", "Graduate Year", "Recommendation", "Go To Menu" };
                        do
                        {

                            Console.Clear();
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("\n-------- Sub Menu --------");
                            Console.ResetColor();
                            for (c = 0; c < subMenuItems.Length; c++)
                            {
                                if (subChoice == c)
                                {

                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write(">>");
                                    Console.WriteLine(subMenuItems[c].PadRight(24));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.Gray;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.WriteLine(subMenuItems[c].PadRight(26));
                                    Console.ResetColor();
                                }
                            }
                            Console.WriteLine("Select your choice with the arrow keys.");
                            key = Console.ReadKey(true);
                            if (key.Key.ToString() == "DownArrow")
                            {
                                subChoice++;
                                if (subChoice > subMenuItems.Length - 1) subChoice = 0;
                            }
                            else if (key.Key.ToString() == "UpArrow")
                            {
                                subChoice--;
                                if (subChoice < 0) subChoice = Convert.ToInt16(subMenuItems.Length - 1);
                            }
                        } while (key.KeyChar != 13);
                        subChoice++;
                        
                        // Listing Prerequisites
                        
                        if (subChoice == 1)
                        {
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Courses List");
                                for (int j = 0; j < 15; j++)
                                {
                                    if (j % 2 == 0)
                                    {
                                        Console.BackgroundColor = ConsoleColor.Gray;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.CursorLeft = 10;
                                        Console.Write(coursesArray[j][0].Substring(1) + " for  ");
                                        Console.CursorLeft = 17;
                                        Console.WriteLine(coursesArray[j][1].PadRight(36));
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.BackgroundColor = ConsoleColor.DarkGray;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.CursorLeft = 10;
                                        Console.Write(coursesArray[j][0].Substring(1) + " for  ");
                                        Console.CursorLeft = 17;
                                        Console.WriteLine(coursesArray[j][1].PadRight(36));
                                        Console.ResetColor();
                                    }
                                }
                                Console.Write("Your Choice: ");
                                input = Console.ReadLine();
                                if (!int.TryParse(input, out choice)) // Try to parse the string as an integer
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("Error! Please Select Between 1 and 15");
                                    Console.ResetColor();
                                }
                                else if (choice > 15 || choice < 1)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("Error! Please Select Between 1 and 15");
                                    Console.ResetColor();
                                }
                                else
                                {

                                    // Listing Prerequisities
                                    int[] preReqResultsArray; // To Keep Results
                                    List<int> preReqResultsList = new List<int>();
                                    for (int i = 1; i < 16; i++)
                                    {
                                        if (prereqArray2[choice, i] == true)
                                        {
                                            preReqResultsList.Add(i);
                                        }
                                    }
                                    
                                    choice--;
                                    if (preReqResultsList.Count == 0)
                                    {
                                        Console.BackgroundColor = ConsoleColor.DarkRed;
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("There is no prerequisity of " + coursesArray[choice][0] + " "+ coursesArray[choice][1]);
                                        Console.ResetColor();
                                    }
                                    else
                                    {

                                        preReqResultsArray = preReqResultsList.ToArray();
                                        Console.BackgroundColor = ConsoleColor.DarkRed;
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("Prerequisities of " + coursesArray[choice][1] + " : ");
                                        Console.ResetColor();
                                        for (int i = 0; i < preReqResultsArray.Length; i++)
                                        {
                                            Console.WriteLine((i + 1) + ". " + coursesArray[preReqResultsArray[i] - 1][0] + " " + coursesArray[preReqResultsArray[i] - 1][1]);
                                        }
                                    }
                                }
                            } while (!int.TryParse(input, out choice) || choice > 15 || choice < 1);
                            Console.Write("Press Any Key To Continue");
                            Console.ReadKey();
                        }
                        // Graduate Year
                        else if (subChoice == 2)
                        {
                            Console.Write("This option is not avaible. You are being redirected in");
                            for (int i = 0; i < 10; i++)
                            {
                                Console.CursorLeft = 56;
                                Console.Write(10-i);
                                Console.Write(" seconds".PadRight(9));
                                
                                System.Threading.Thread.Sleep(300);

                            }
                            Console.CursorLeft = 60;
                            Console.WriteLine(" seconds");
                        }
                            
                            // Recommendations
                        else if (subChoice == 3)
                        {
                            for (int i = 0; i < allCoursesArr.Length; i++)
                            {                               
                                    bool flag = false;
                                    for (int j = 0; j < 15; j++)
                                    {
                                        if (prereqArray2[allCoursesArr[i], j] == true)
                                        {
                                            if (studentCourseStatus[j - 1, 1] != "1")
                                            {
                                                for (int k = 1; k < 16; k++)
                                                {
                                                    if (prereqArray2[allCoursesArr[i], k] == true)
                                                    {
                                                        flag = true;
                                                        allCourses.Remove(allCoursesArr[i]);
                                                    }
                                                }
                                            }
                                          

                                        }
                                    }
                                    if (!flag) // Önşartı olmayan veya önşartları geçilmiş dersleri yazdırmak için
                                    {
                                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("You are able to take C" + allCoursesArr[i] + ".");
                                        Console.ResetColor();
                                    }                                  
                            }
                            // ----- same as in register part. ---
                            string recPositionOf1; // positions: 'T1 T2'
                            string recPositionOf2;
                            string[] recSplittedPosOf1; // splitted positions : 'T1', 'T2';
                            string[] recSplittedPosOf2;
                            allCoursesArr = allCourses.ToArray();
                            Array.Sort(allCoursesArr);                            
                            int forj = 0; // for içinde j yazınca değerini 15 giriyodu nedenini bulamadım ayrı bi değişken verdim
                            for (int i = 0; i < allCoursesArr.Length; i++)
                            {
                                for (forj = i; forj < allCoursesArr.Length; forj++)
                                {
                                    if (i != forj)
                                    {
                                        recPositionOf1 = coursesArray[allCoursesArr[i] - 1][4];
                                        recPositionOf2 = coursesArray[allCoursesArr[forj] - 1][4];
                                        recSplittedPosOf1 = recPositionOf1.Split(spaceChar);
                                        recSplittedPosOf2 = recPositionOf2.Split(spaceChar);
                                        for (int k = 0; k < recSplittedPosOf1.Length; k++)
                                        {
                                            for (int l = 0; l < recSplittedPosOf2.Length; l++)
                                            {
                                                if (recSplittedPosOf1[k].Equals(recSplittedPosOf2[l]))
                                                {
                                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    Console.Write("Warning! Conflict of C" + allCoursesArr[i] + " and C" + allCoursesArr[forj]);
                                                    Console.WriteLine(" at " + recSplittedPosOf1[k]);
                                                    Console.ResetColor();
                                                    
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            Console.Write("Press Any Key To Continue");
                            Console.ReadKey();

                        }
                        else if (subChoice == 4) // back
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Error!");
                        }
                        
                    } while (subChoice != 4);
                } // End of choice 2


                // Register
                else if (choice == 3)
                {
                    int[] selectedCourses = new int[15];

                    int arrayCounter= 0;
                    Console.WriteLine("a. Courses List");
                    for (int j = 0; j < 15; j++)
                    {
                        if (j % 2 == 0)
                        {

                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.CursorLeft = 10;
                            Console.Write(coursesArray[j][0].PadRight(4));
                            Console.CursorLeft = 14;
                            Console.Write(coursesArray[j][1].PadRight(37));
                            Console.CursorLeft = 51;
                            Console.Write(coursesArray[j][2].PadRight(2));
                            if (studentCourseStatus[j,1] == "1")
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write("Succesful");
                                Console.ResetColor();
                            }
                            Console.WriteLine();
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.CursorLeft = 10;
                            Console.Write(coursesArray[j][0].PadRight(4));
                            Console.CursorLeft = 14;
                            Console.Write(coursesArray[j][1].PadRight(37));
                            Console.CursorLeft = 51;
                            Console.Write(coursesArray[j][2].PadRight(2));
                            if (studentCourseStatus[j, 1] == "1")
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write("Succesful");
                                Console.ResetColor();
                            }
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine("1- Checking Prerequisites");
                    Console.WriteLine("Info: Type course code of course that you want to take and press Enter. Type 'End' to stop.");
                    string selected;
                    bool flag2 = true;
                    int errorCounter1 = 0, errorCounter2 = 0; // hata mesajını göstermek için
                    do
                    {
                        arrayCounter = 0;
                        errorCounter2 = 0;
                        errorCounter1 = 0;
                        if (!flag2)
                        {
                            Console.WriteLine("\n\n");
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("".PadRight(35));
                            Console.WriteLine(" Error! Please Enter Courses Again".PadLeft(2).PadRight(35));
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("".PadRight(35));
                            Console.ResetColor();
                            Console.WriteLine("\n\n");

                        }
                        selectedCourses = new int[15]; // to reset selected courses
                        for (int i = 0; i < 15; i++)
                        {
                            selectedCourses[i] = 16;
                        }

                        do
                        {
                            Console.Write("Course #" + (arrayCounter + 1) + ": ");
                            selected = Console.ReadLine().ToUpper();
                        
                           
                            if (selected != "END")
                            {
                                if (Convert.ToInt32(selected.Substring(1)) > 15 | Convert.ToInt32(selected.Substring(1)) < 1)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("Please Enter Correct Course Code.");
                                    Console.ResetColor();
                                    
                                }
                                else
                                {
                                    selectedCourses[arrayCounter] = Convert.ToInt32(selected.Substring(1));
                                    arrayCounter++;
                                }
                            }
                         
                        } while (selected != "END");
                        int previousListedCourse = 0; // En son listelenen dersi gösteriyor, birden fazla gösterilmemesi için.
                        for (int i = 0; i < 15; i++)
                        {
                            if (selectedCourses[i] != 16)
                            {

                                bool flag = false;
                                for (int j = 0; j < 15; j++)
                                {
                                    if (prereqArray2[selectedCourses[i], j] == true)
                                    {
                                        if (studentCourseStatus[j - 1, 1] != "1" & previousListedCourse != selectedCourses[i])
                                        {
                                            previousListedCourse = selectedCourses[i];
                                            Console.BackgroundColor = ConsoleColor.DarkRed;
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine("You are not able to take C" + selectedCourses[i] + ". There is a list of prereqisities of this lesson below.");
                                            Console.ResetColor();
                                            for (int k = 1; k < 16; k++)
                                            {
                                                if (prereqArray2[selectedCourses[i], k] == true)
                                                {
                                                    Console.WriteLine("".PadRight(83));
                                                    Console.CursorLeft = 7;
                                                    if (studentCourseStatus[k - 1, 1] == "1")
                                                    {
                                                        Console.Write("C" + k + " ");
                                                        Console.Write(coursesArray[k - 1][1].PadRight(37));
                                                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                                                        Console.ForegroundColor = ConsoleColor.White;
                                                        Console.WriteLine("Succesful".PadLeft(3));
                                                        Console.ResetColor();
                                                    }
                                                    else if (studentCourseStatus[k - 1, 1] == "0")
                                                    {
                                                        Console.Write("C" + k + " ");
                                                        Console.Write(coursesArray[k - 1][1].PadRight(37));
                                                        Console.BackgroundColor = ConsoleColor.Red;
                                                        Console.ForegroundColor = ConsoleColor.White;
                                                        Console.WriteLine("Unsuccesful".PadLeft(3));
                                                        Console.ResetColor();
                                                    }
                                                    else
                                                    {
                                                        Console.Write("C" + k + " ");
                                                        Console.Write(coursesArray[k - 1][1].PadRight(37));
                                                        Console.BackgroundColor = ConsoleColor.DarkGray;
                                                        Console.ForegroundColor = ConsoleColor.White;
                                                        Console.WriteLine("Never Taken".PadLeft(3));
                                                        Console.ResetColor();
                                                    }
                                                    flag2 = false;
                                                    flag = true;
                                                    
                                                }
                                            }
                                        }
                                        
                                    }
                                }
                                if (!flag) // Önşartı olmayan veya önşartları geçilmiş dersleri yazdırmak için
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("You are able to take C" + selectedCourses[i] + ".");
                                    Console.ResetColor();
                                    errorCounter1++;
                                }
                                errorCounter2++;
                                Console.WriteLine("".PadRight(83, '-'));
                            }
                        }
                        if (errorCounter2 == errorCounter1)
                        {
                            flag2 = true;
                        }
                    } while (flag2 == false);
                    
                    Console.Write("Press Any Key To Check Conflicts");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("2- Checking Conflicts");
                    string positionOf1; // positions: 'T1 T2'
                    string positionOf2;
                    string[] splittedPosOf1; // splitted positions : 'T1', 'T2';
                    string[] splittedPosOf2;
                    int[,] conflictArray = new int[16, 16];
                    Array.Sort(selectedCourses);
                    int conflictCounter= 0;
                    int forj = 0; // for içinde j yazınca değerini 15 giriyodu nedenini bulamadım ayrı bi değişken verdim
                    for (int i = 0; i < 15; i++)
                    {
                        for (forj = i; forj < 15; forj++)
                        {
                            if (i != forj & selectedCourses[i] != 16 & selectedCourses[forj] != 16)
                            {
                                positionOf1 = coursesArray[selectedCourses[i] - 1][4];
                                positionOf2 = coursesArray[selectedCourses[forj] - 1][4];
                                splittedPosOf1 = positionOf1.Split(spaceChar);
                                splittedPosOf2 = positionOf2.Split(spaceChar);
                                for (int k = 0; k < splittedPosOf1.Length; k++)
                                {
                                    for (int l = 0; l < splittedPosOf2.Length; l++)
                                    {
                                        if (splittedPosOf1[k].Equals(splittedPosOf2[l]))
                                        {
                                            Console.BackgroundColor = ConsoleColor.DarkRed;
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.Write("Warning! Conflict of C" + selectedCourses[i] + " and C" + selectedCourses[forj]);
                                            Console.WriteLine(" at " + splittedPosOf1[k]);
                                            Console.ResetColor();
                                            conflictArray[selectedCourses[i], selectedCourses[forj]]++;
                                            conflictCounter++;
                                        }
                                    }
                                }

                            }
                        }
                    }
                    int courseCredit;
                    int conflictTime;
                    for (int i = 1; i < 15; i++)
                    {
                        for (int j = 1; j < 15; j++)
                        {
                            if (conflictArray[i,j] != 0)
                            {
                                courseCredit = Convert.ToInt32(coursesArray[i][2]);
                                conflictTime = conflictArray[i, j];
                                if (Convert.ToDouble(conflictTime) / Convert.ToDouble(courseCredit) < 0.3)
                                {
                                    Console.WriteLine("Info: C{0} and C{1} conflict but you can take them.",i,j);
                                }
                                else
                                    Console.WriteLine("Info: C{0} and C{1} conflict but you cannot take them.", i, j);
                            }
                        }
                    }


                    Console.WriteLine("Total conflicts :" + conflictCounter);
                    Console.Write("Press Any Key To Continue");
                    Console.ReadKey();
                }// End of choice 3
                else if (choice == 4) // exit
                {
                   break;
                } // End of choice 4
            } while (choice != 5);
           

        }
    }
}
