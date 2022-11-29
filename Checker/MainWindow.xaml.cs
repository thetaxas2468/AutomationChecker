using Checker.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;

namespace Checker
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnclose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();

        }
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        string[] CourseFolders;
        string[] CourseFiles;
        string[] StudentsHomeworks;
        string[] HomeworkFiles;
        Course_info course_Info = new Course_info();
        List<Rules> rules = new List<Rules>();
        string[] Homeworks;
        string[] tac;
        string csvpath;
        string fid;
        int fcnt;
        int fgrade;
        List<Student> studentsHomeworks=new List<Student>();
        List<string> StudentsIds=new List<string>();
        string output;
        StringBuilder stdinhomework=new StringBuilder();
        string coursegradespath;
        string getAllurl = "https://localhost:7077/api/StudentControllercs/GetAll";
        string geturl ="https://localhost:7077/api/StudentControllercs/Get";
        string posturl = "https://localhost:7077/api/StudentControllercs/CreateEdit";
        string deleteurl = "https://localhost:7077/api/StudentControllercs/Delete";
        private void btnfolder_Click(object sender, RoutedEventArgs e)
        {
            if (txturl.Text != "")
            {
                fbd.ShowDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CourseFolders = Directory.GetDirectories(fbd.SelectedPath);

                    foreach (var item in CourseFolders)
                    {
                        CourseFiles = Directory.GetFiles(item);//coursegades.csv and courseinfo files
                        Homeworks = Directory.GetDirectories(item);//all homeworks
                        foreach (var Cfiles in CourseFiles)
                        {
                            if (Path.GetFileName(Cfiles) == "course_info.json")
                            {
                                course_Info = Newtonsoft.Json.JsonConvert.DeserializeObject<Course_info>(File.ReadAllText(Cfiles));
                                StudentsIds = course_Info.Course_students;

                            }
                            else if(Path.GetFileName(Cfiles) == "course_grades.csv")
                            {
                                coursegradespath = Cfiles;
                                System.IO.File.WriteAllText(coursegradespath, string.Empty);
                                File.WriteAllText(coursegradespath, "Id,Avg,Course_Name,Course_Year" + Environment.NewLine);
                            }

                        

                        }
                        foreach (var a in Homeworks)
                        {
                            var b = Directory.GetFiles(a);
                            foreach (var c in b)
                            {
                                if (Path.GetFileName(c) == "grades.csv")
                                {
                                    System.IO.File.WriteAllText(c, string.Empty);
                                    File.WriteAllText(c, "Id,Grade,HomeworkId,HomeworkName"+Environment.NewLine);
                                }
                            }
                        }
                        foreach (var student_checking in StudentsIds)
                        {
                            string fid = student_checking;
                            int fcnt = 0;
                            int fgrade = 0;
                            foreach (var homework in Homeworks)
                            {
                                StudentsHomeworks = Directory.GetDirectories(homework);//Need To Do Each Student(Add Student class and calc his grade by prev info from Homeworkinfo)
                                HomeworkFiles = Directory.GetFiles(homework);//HomeWork Files Like Rules OutPut Grades.csv(need to ignore check again)
                                string[] splitted = homework.Split('\\');
                                string realhomework = splitted[splitted.Length - 1];
                                string homeworkId = realhomework.Split('_')[0];
                                string homeworkName = realhomework.Split('_')[1];
                                //Console.WriteLine(homeworkId);//Home work id 
                                //Console.WriteLine(homeworkName);//Homework name
                                string path_to_this_directory = homework;//path to the current homework to put the grades in it
                                foreach (var hmFile in HomeworkFiles)
                                {
                                    if (Path.GetFileName(hmFile) == "rules.json")
                                    {
                                        rules = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Rules>>(File.ReadAllText(hmFile));
                                    }
                                    else if (Path.GetFileName(hmFile) == "output.txt")
                                    {
                                        tac = File.ReadAllLines(hmFile);
                                        string test = "";
                                        foreach (string x in tac)
                                        {
                                            test += x;
                                            test += "\n";
                                        }
                                        output = test;
                                    }
                                    else if (Path.GetFileName(hmFile) == "grades.csv")
                                    {
                                        csvpath = hmFile;

                                    }

                                }
                                Student tempo = new Student();
                                tempo.Id = Int32.Parse(student_checking);
                                tempo.CourseInfo = course_Info;
                                tempo.HomeworkName = homeworkName;
                                tempo.HomeworkId = homeworkId;
                                tempo.Grade = 100;
                                foreach(var rule in rules)
                                {
                                    if (rule.Rulenumber == 1)
                                    {
                                        bool correct = false;
                                        foreach (var studentFile in StudentsHomeworks)
                                        {
                                            //get the studentfile studentname and id after that add to him the homework id and name after that add to him course info
                                            //after all that start calculate his homework grade and save it to grades.csv ( maybe to course_grades too)
                                            string[] splitted1 = studentFile.Split('\\');
                                            string realhomework1 = splitted1[splitted1.Length - 1];
                                            string studentId1 = realhomework1.Split('_')[0];
                                            string studentName1 = realhomework1.Split('_')[1];
                                            if (studentId1 == student_checking)
                                            {
                                                correct = true;
                                            }


                                        }
                                        if (correct == false)
                                        {
                                            tempo.Grade = tempo.Grade - rule.Points;
                                        }
                                    }
                                    if (rule.Rulenumber == 2)
                                    {
                                        bool correct2 = false;
                                        foreach (var studentFile in StudentsHomeworks)
                                        {
                                            //get the studentfile studentname and id after that add to him the homework id and name after that add to him course info
                                            //after all that start calculate his homework grade and save it to grades.csv ( maybe to course_grades too)
                                            string[] splitted2 = studentFile.Split('\\');
                                            string realhomework2 = splitted2[splitted2.Length - 1];
                                            string studentId2 = realhomework2.Split('_')[0];
                                            string studentName2 = realhomework2.Split('_')[1];
                                            string[] studentFiletemp=Directory.GetFiles(studentFile);
                                            if (studentId2 == student_checking)
                                            {
                                                foreach(var xws in studentFiletemp)
                                                {
                                                    string[] a = xws.Split('\\');
                                                    string b = a[a.Length - 1];
                                                    string c = b.Split('_')[0];
                                                    //Console.WriteLine("-------------");
                                                    //Console.WriteLine(c);
                                                    //Console.WriteLine(rule.File);
                                                    //Console.WriteLine("-------------");
                                                    if (c == rule.File)
                                                    {
                                                        correct2 = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (correct2 == false)
                                        {
                                            tempo.Grade = tempo.Grade - rule.Points;
                                        }
                                    }
                                    if (rule.Rulenumber == 3)
                                    {
                                        bool correct3 = false;
                                        foreach (var studentFile in StudentsHomeworks)
                                        {
                                            //get the studentfile studentname and id after that add to him the homework id and name after that add to him course info
                                            //after all that start calculate his homework grade and save it to grades.csv ( maybe to course_grades too)
                                            string[] splitted2 = studentFile.Split('\\');
                                            string realhomework2 = splitted2[splitted2.Length - 1];
                                            string studentId2 = realhomework2.Split('_')[0];
                                            string studentName2 = realhomework2.Split('_')[1];
                                            string[] studentFiletemp = Directory.GetFiles(studentFile);
                                            if (studentId2 == student_checking)
                                            {
                                                foreach (var xws in studentFiletemp)
                                                {
                                                    string[] a = xws.Split('\\');
                                                    string b = a[a.Length - 1];
                                                    string c = b.Split('_')[0];
                                                    if (c == rule.File)
                                                    {
                                                        //check if there is regular expression in the text
                                                        string text = File.ReadAllText(xws);
                                                        string expression_temp = "@" + rule.Expression;
                                                        Regex regex = new Regex(expression_temp);
                                                        Match match = regex.Match(text);
                                                        if (match.Success)
                                                        {
                                                            correct3 = true;
       
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (correct3 == false)
                                        {
                                            tempo.Grade = tempo.Grade - rule.Points;
                                        }
                                        
                                    }
                                    

                                }
                                string texto = tempo.Id + "," + tempo.Grade.ToString() + "," + tempo.HomeworkId +"," +tempo.HomeworkName;
                                fgrade = fgrade + tempo.Grade;
                                fcnt=fcnt+1;
                                using (StreamWriter sw = File.AppendText(csvpath))
                                {
                                    sw.WriteLine(texto);
                                }
                                Console.WriteLine(tempo.ToString());
                                
                            }
                            //Console.WriteLine(fgrade);
                            //Console.WriteLine(fcnt);
                            //Console.WriteLine(fid);
                            string lol = fid + "," + fgrade / fcnt + "," + course_Info.Course_name + "," + course_Info.Course_year;


                            using (StreamWriter sw = File.AppendText(coursegradespath))
                            {
                                sw.WriteLine(lol);
                            }

                            post_req(fid, (fgrade / fcnt).ToString() , course_Info.Course_name,course_Info.Course_year);
                            

                    }
                    //after we check all homeworks for students in a course we navigate to all grades.csv realted to course info and give us average

                    System.Windows.Forms.MessageBox.Show("Auto Check Process has been Completed");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Enter URL", "Missing URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        }
        private void post_req(string a,string b,string c,string d)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create("https://localhost:7077/api/StudentControllercs/CreateEdit");
            httpRequest.Method = "POST";

            httpRequest.Accept = "*/*";
            httpRequest.ContentType = "application/json";

            var data = new
            {
                studentId = a,
                avgGrade = b,
                courseName = c,
                courseYear = d
            };
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(data));
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            Console.WriteLine(httpResponse.StatusCode);
            Console.WriteLine("--------------");

        }
        public static string URL;
    }
}
