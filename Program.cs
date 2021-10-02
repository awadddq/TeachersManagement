using ManagementApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersManagement
{
    class Program
    {
        public List<TeachersModel> _teachersModelList;
        public static Program program = new Program();
        public static string filePath = @"FilePath\TeachersData.txt";
        public static void Main(string[] args)
        {
            int processType = 0;
            do
            {
                Console.WriteLine("Teacher record");
                Console.WriteLine("1 For Insert Teacher Entry");
                Console.WriteLine("2 For Update Teacher Entry");
                Console.WriteLine("3 For Exit");
                Console.Write("Enter Your Choice: ");
                processType = Convert.ToInt32(Console.ReadLine());

                if (processType != 1 && processType != 2 && processType != 3)
                {
                    do
                    {
                        Console.Write("Please enter proper inputs : ");
                        processType = Convert.ToInt32(Console.ReadLine());

                    } while (processType != 3 && processType != 2 && processType != 1);
                }

                if (processType == 3)
                {
                    Environment.Exit(0);
                }
                else if (processType == 1)
                {
                    program.AddTeacher();
                }
                else
                {
                    program.UpdateTeacher();
                }
                Console.ReadLine();
            } while (processType != 3);

        }

        /// <summary>
        /// Making object for teacherModel
        /// </summary>
        public void AddTeacher()
        {
            TeachersModel _teachersModel = new TeachersModel();
            Console.Write("Please provide ID : ");
            _teachersModel.ID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please provide Name : ");
            _teachersModel.Name = Console.ReadLine();
            Console.Write("Please provide Class : ");
            _teachersModel.Class = Console.ReadLine();
            Console.Write("Please provide Section : ");
            _teachersModel.Section = Console.ReadLine();
            if (WriteToFile(_teachersModel) == 1)
            {
                Console.WriteLine("Record inserted", _teachersModel.Name);
            }
            else
            {
                Console.WriteLine("System error accured try again.");
            }
        }

        /// <summary>
        /// Read data from existing file
        /// </summary>
        public void ReadExistingData()
        {
            string line;
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(filePath);
            //Read the first line of text
            line = sr.ReadLine();
            //Continue to read until you reach end of file
            // Making teachers model list for saved data.
            _teachersModelList = new List<TeachersModel>();
            while (line != null && line != "")
            {
                _teachersModelList.Add(new TeachersModel()
                {
                    ID = Convert.ToInt32(line.Split(';')[0].Split(':')[1]),
                    Name = line.Split(';')[1].Split(':')[1],
                    Class = line.Split(';')[2].Split(':')[1],
                    Section = line.Split(';')[3].Split(':')[1],
                }); ;

                //write the line to console window
                //Read the next line
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();
        }

        /// <summary>
        /// updating teacher record.
        /// </summary>
        public void UpdateTeacher()
        {
            try
            {
                int updateTeacherId = 0;
                ReadExistingData();
                try
                {
                    updateTeacherId = GetTeacherId();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please provide proper id.");
                    GetTeacherId();
                }

                var teacherObject = _teachersModelList.Where(x => x.ID == updateTeacherId).FirstOrDefault();

                // Check if id exist or not for update
                if (teacherObject == null)
                {
                    do
                    {
                        Console.WriteLine("Id not exist please provide proper id.");
                        updateTeacherId = GetTeacherId();
                        teacherObject = _teachersModelList.Where(x => x.ID == updateTeacherId).FirstOrDefault();

                    } while (teacherObject == null);
                }
                Console.Write("You have selected record of : {0} ", teacherObject.Name);
                Console.Write(Environment.NewLine);
                Console.WriteLine("Teacher record");
                Console.WriteLine("1 For Update Name");
                Console.WriteLine("2 For Update Next Step");
                if (Console.ReadLine().ToString() == "1")
                {
                    Console.Write("Please provide Name : ");
                    teacherObject.Name = Console.ReadLine();
                }
                Console.WriteLine("1 For Update Class");
                Console.WriteLine("2 For Update Next Step");
                if (Console.ReadLine().ToString() == "1")
                {
                    Console.Write("Please provide Class : ");
                    teacherObject.Class = Console.ReadLine();
                }
                Console.WriteLine("1 For Update Section");
                Console.WriteLine("2 For Update Next Step");
                if (Console.ReadLine().ToString() == "1")
                {
                    Console.Write("Please provide Section : ");
                    teacherObject.Section = Console.ReadLine();
                }
                var oldLines = System.IO.File.ReadAllLines(filePath);
                var newLines = oldLines.Where(oldLine => !oldLine.Contains(teacherObject.ID.ToString()));
                System.IO.File.WriteAllLines(filePath, newLines);
                if (WriteToFile(teacherObject) == 1)
                {
                    Console.WriteLine("Record  updated.");
                }
                else
                {
                    Console.WriteLine("System error accured try again.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        /// <summary>
        /// Adding new data / update to the txt file
        /// </summary>
        /// <param name="_teachersModel"></param>
        /// <returns></returns>
        public int WriteToFile(TeachersModel _teachersModel)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true)) //// true to append data to the file
                {
                    string str = "Id: {0}; Name:{1};Class:{2};Section:{3} ";
                    writer.WriteLine(str,
                        _teachersModel.ID, _teachersModel.Name, _teachersModel.Class, _teachersModel.Section);
                    Console.WriteLine(Environment.NewLine);
                    return 1;
                }
            }
            catch (Exception ex)
            {

                return -1;
            }

        }

        /// <summary>
        /// Getting Id from user.
        /// </summary>
        /// <returns></returns>
        public int GetTeacherId()
        {
            int updateTeacherId = 0;
            Console.Write("Please enter Id you want to update:");
            updateTeacherId = Convert.ToInt32(Console.ReadLine());
            return updateTeacherId;
        }
    }
}
