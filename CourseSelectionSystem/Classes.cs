using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace CourseSelectionSystem
{
    class Student
    {
        [Index(0)]
        public string ID { get; set; }
        [Index(1)]
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{this.ID} {this.Name}";
        }
    }

    class Teacher { 
        public string Name { get; set; }
        public ObservableCollection<Course> Courses { get; set; }

        public override string ToString()
        {
            return $"教師名稱: {this.Name}";
        }
    }

    class Course
    {
        public string CourseName { get; set; }
        public string Type { get; set; }
        public int Point { get; set; }
        public string OpeningClass { get; set; }
        public string ClassTime { get; set; }
        public Teacher Tutor { get; set; }
        public Course(Teacher tutor)
        {
            this.Tutor = tutor;
        }
        public override string ToString()
        {
            return $"{CourseName} {Type} {Point}學分 開課班級: {OpeningClass}";
        }
    }

    class Record
    {
        public Teacher SelectedTeacher { get; set; }
        public Course SelectedCourse { get; set; }
    }
}
