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
 
        public MainWindow()
        {
            InitializeComponent();
            using (var reader = new StreamReader(@"D:\大二視窗應用程式\course_selection_data\2B.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Student>();
                foreach(Student record in records)
                {
                    students.Add(record);
                }
            }
        }
    }
}
