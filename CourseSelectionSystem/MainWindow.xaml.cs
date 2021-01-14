using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CsvHelper;

namespace CourseSelectionSystem
{
    public partial class MainWindow : Window
    {

        ObservableCollection<Student> students = new ObservableCollection<Student>();
        ObservableCollection<Teacher> teachers = new ObservableCollection<Teacher>();
        ObservableCollection<Course> courses = new ObservableCollection<Course>();
        ObservableCollection<Course> displayedCourses = new ObservableCollection<Course>();
        ObservableCollection<Teacher> displayedTeachers = new ObservableCollection<Teacher>();
        ObservableCollection<CsvSelectionRecord> csvRecords = new ObservableCollection<CsvSelectionRecord>();
        Student selectedStudent;
        Course selectedCourse;
        Teacher selectedTeacher;
        bool requiredChecked = true;
        bool electiveChecked = true;
        bool deptElectiveChecked = true;

        public MainWindow()
        {
            InitializeComponent();
            LoadStudents();
            LoadCourses();
            PopulateTeachers();
            PopulateDisplayedCourses();
            PopulateDisplayedTeachers();
            SetItemsSources();
            cbRequired.Click += TypeSelection_Changed;
            cbElective.Click += TypeSelection_Changed;
            cbDeptElective.Click += TypeSelection_Changed;
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
        }

        private void LoadStudents()
        {
            using (StreamReader reader = new StreamReader(@"D:\大二視窗應用程式\course_selection_data\2B.csv", Encoding.Default))
            {
                using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Student>();
                    foreach (Student record in records)
                    {
                        students.Add(record);
                    }
                }
            }
        }

        private void SetItemsSources()
        {
            cbStudent.ItemsSource = students;
            trvTeacher.ItemsSource = displayedTeachers;
            lvCourse.ItemsSource = displayedCourses;
            lvRecord.ItemsSource = csvRecords;
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
        }

        private void PopulateDisplayedCourses()
        {
            displayedCourses = new ObservableCollection<Course>(courses);
            foreach(var record in csvRecords)
            {
                displayedCourses = new ObservableCollection<Course>(
                    displayedCourses.
                    Where(course => !(course.CourseName == record.CourseName &&
                    course.OpeningClass == record.OpeningClass))
                    );
            }
            displayedCourses = new ObservableCollection<Course>(displayedCourses.OrderBy(c => c.Tutor.TeacherName));
            FilterCourses();
        }

        private void PopulateDisplayedTeachers()
        {
            displayedTeachers = new ObservableCollection<Teacher>();
            foreach (var courseGroup in displayedCourses.GroupBy(course => course.TeacherName))
            {
                Teacher teacher = new Teacher();
                teacher.TeacherName = courseGroup.ToList()[0].TeacherName;
                foreach (var course in courseGroup)
                {
                    teacher.Courses.Add(course);
                    course.Tutor = teacher;
                }
                displayedTeachers.Add(teacher);
            }
            displayedTeachers = new ObservableCollection<Teacher>(displayedTeachers.OrderBy(t => t.TeacherName));
            trvTeacher.ItemsSource = displayedTeachers;
        }

        private void cbStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbStudent.SelectedItem != null)
            {
                LoadSelectionCsvRecords();
                PopulateDisplayedCourses();
                PopulateDisplayedTeachers();
            }
        }

        private void LoadSelectionCsvRecords()
        {
            selectedStudent = cbStudent.SelectedItem as Student;
            if (File.Exists($"D:\\大二視窗應用程式\\course_selection_data\\Records\\{selectedStudent.ID}.csv"))
            {
                using (var reader = new StreamReader($"D:\\大二視窗應用程式\\course_selection_data\\Records\\{selectedStudent.ID}.csv", Encoding.Default))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<CsvSelectionRecord>();
                    csvRecords = new ObservableCollection<CsvSelectionRecord>(records);
                }
                lvRecord.ItemsSource = csvRecords;
            }
            else
            {
                csvRecords = new ObservableCollection<CsvSelectionRecord>();
                lvRecord.ItemsSource = csvRecords;
            }
        }

        private void TypeSelection_Changed(object sender, RoutedEventArgs e)
        {
            FilterCourses();
        }

        private void cbPoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCourses();
        }

        private void FilterCourses()
        {
            requiredChecked = (bool)cbRequired.IsChecked;
            electiveChecked = (bool)cbElective.IsChecked;
            deptElectiveChecked = (bool)cbDeptElective.IsChecked;
            lvCourse.ItemsSource = new ObservableCollection<Course>(displayedCourses.Where(c =>
            {
                bool result = ((requiredChecked && c.Type == "必修") 
                || (electiveChecked && c.Type == "選修") 
                || (deptElectiveChecked && c.Type == "系定選修"));
                return result;
            }).Where(c =>
            {

                if (cbPoint.SelectedItem == null) return true;
                int selectedPoint = cbPoint.SelectedIndex;
                if (selectedPoint == 0) return true;
                if (c.Point == selectedPoint) return true;
                return false;
            }));
        }

        private void trvTeacher_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (trvTeacher.SelectedItem is Course)
            {
                selectedCourse = trvTeacher.SelectedItem as Course;
                selectedTeacher = selectedCourse.Tutor;
                statusLabel.Content = $"{selectedCourse.ToString()} {selectedTeacher.ToString()}";
            }
        }

        private void lvCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCourse.SelectedItem is Course)
            {
                selectedCourse = lvCourse.SelectedItem as Course;
                selectedTeacher = selectedCourse.Tutor;
                statusLabel.Content = $"{selectedCourse.ToString()} {selectedTeacher.ToString()}";
            }
        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCourse != null && selectedTeacher != null && selectedStudent != null)
            {
                CsvSelectionRecord csvRecord = new CsvSelectionRecord()
                {
                    StudentID = selectedStudent.ID,
                    StudentName = selectedStudent.Name,
                    CourseName = selectedCourse.CourseName,
                    TeacherName = selectedTeacher.TeacherName,
                    Type = selectedCourse.Type,
                    Point = selectedCourse.Point,
                    ClassTime = selectedCourse.ClassTime,
                    OpeningClass = selectedCourse.OpeningClass
                };
                csvRecords.Add(csvRecord);
                displayedCourses.Remove(selectedCourse);
                selectedTeacher.Courses.Remove(selectedCourse);
                FilterCourses();
                selectedCourse = null;
                selectedTeacher = null;
            }
        }

        private void withdrawalButton_Click(object sender, RoutedEventArgs e)
        {
            if (lvRecord.SelectedItem != null)
            {
                var selectedRecord = lvRecord.SelectedItem as CsvSelectionRecord;
                csvRecords.Remove(selectedRecord);
                Course course = new Course()
                {
                    CourseName = selectedRecord.CourseName,
                    TeacherName = selectedRecord.TeacherName,
                    Type = selectedRecord.Type,
                    ClassTime = selectedRecord.ClassTime,
                    OpeningClass = selectedRecord.OpeningClass,
                    Point = selectedRecord.Point
                };
                Teacher teacher = teachers.Where(t => t.TeacherName == course.TeacherName).ToArray<Teacher>()[0];
                course.Tutor = teacher;
                displayedCourses.Add(course);
                PopulateDisplayedCourses();
                PopulateDisplayedTeachers();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStudent != null)
            {
                using (var writer = new StreamWriter($"D:\\大二視窗應用程式\\course_selection_data\\Records\\{selectedStudent.ID}.csv", false, Encoding.Default))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(csvRecords);
                }

                using (var writer = new StreamWriter($"D:\\大二視窗應用程式\\course_selection_data\\Results\\{selectedStudent.ID}{selectedStudent.Name}.csv", false, Encoding.Default))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    CsvResult result = new CsvResult()
                    {
                        StudentID = selectedStudent.ID,
                        CourseCount = csvRecords.Count,
                        StudentName = selectedStudent.Name,
                        TotalPoint = csvRecords.Sum(r => r.Point)
                    };
                    List<CsvResult> results = new List<CsvResult>();
                    results.Add(result);
                    csv.WriteRecords(results);
                }
            }
        }      
    }
}
