using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CsvHelper;

namespace CourseSelectionSystem
{
    public partial class MainWindow : Window
    {

        ObservableCollection<Student> students = new ObservableCollection<Student>();
        ObservableCollection<Teacher> teachers = new ObservableCollection<Teacher>();
        ObservableCollection<Course> courses = new ObservableCollection<Course>();
 
        public MainWindow()
        {
            InitializeComponent();
            LoadStudents();
            LoadCourses();
            PopulateTeachers();
        }

        private void PopulateTeachers()
        {
            foreach (var courseGroup in courses.GroupBy(course => course.TeacherName))
            {
                Teacher teacher = new Teacher();
                teacher.TeacherName = courseGroup.ToList()[0].TeacherName;
                foreach (var course in courseGroup)
                {
                    teacher.Courses.Add(course);
                    course.Tutor = teacher;
                }
                teachers.Add(teacher);
            }
            trvTeacher.ItemsSource = teachers;
        }

        private void LoadCourses()
        {
            using (StreamReader reader = new StreamReader("D:\\大二視窗應用程式\\course_selection_data\\course.csv", Encoding.Default))
            {
                using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HeaderValidated = null;
                    var records = csv.GetRecords<CourseRecord>();

                    foreach (CourseRecord record in records)
                    {
                        courses.Add(new Course(record));
                    }
                }

            }
            lvCourse.ItemsSource = courses;
        }

        private void LoadStudents()
        {
            using (StreamReader reader = new StreamReader(@"D:\大二視窗應用程式\course_selection_data\2B.csv", Encoding.Default)) {
                using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Student>();
                    foreach (Student record in records)
                    {
                        students.Add(record);
                    }
                }
            }
            
            cbStudent.ItemsSource = students;
        }
    }
}
