using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    class Course
    {
        [Index(0)]
        public string TeacherName { get; set; }
        [Index(1)]
        public string CourseName { get; set; }
        [Index(2)]
        public int Point { get; set; }
        [Index(3)]
        public string Type { get; set; }
        [Index(4)]
        public string OpeningClass { get; set; }
        [Index(5)]
        public string ClassTime { get; set; }
        [Ignore()]
        public Teacher Tutor { get; set; }

        public Course()
        {

        }

        public Course(CourseRecord record)
        {
            this.TeacherName = record.TeacherName;
            this.CourseName = record.CourseName;
            this.Point = record.Point;
            this.Type = record.Type;
            this.OpeningClass = record.OpeningClass;
            this.ClassTime = record.ClassTime;
        }

        public Course(Teacher tutor)
        {
            this.Tutor = tutor;
        }
        public override string ToString()
        {
            return $"{CourseName} {Type} {Point}學分 開課班級: {OpeningClass}";
        }
    }

    class CourseRecord
    {
        [Index(0)]
        public string TeacherName { get; set; }
        [Index(1)]
        public string CourseName { get; set; }
        [Index(2)]
        public int Point { get; set; }
        [Index(3)]
        public string Type { get; set; }
        [Index(4)]
        public string OpeningClass { get; set; }
        [Index(5)]
        public string ClassTime { get; set; }
    }

    class Teacher { 
        public string TeacherName { get; set; }
        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();

        public override string ToString()
        {
            return $"教師名稱: {this.TeacherName}";
        }
    }

    class CsvSelectionRecord
    {
        [Index(0)]
        [Name("學號")]
        public string StudentID { get; set; }
        [Index(1)]
        [Name("學生姓名")]
        public string StudentName { get; set; }
        [Index(2)]
        [Name("課程名稱")]
        public string CourseName { get; set; }
        [Index(3)]
        [Name("授課教師")]
        public string TeacherName { get; set; }
        [Index(4)]
        [Name("學分數")]
        public int Point { get; set; }
        [Index(5)]
        [Name("必選修")]
        public string Type { get; set; }
        [Index(6)]
        [Name("開課班級")]
        public string OpeningClass { get; set; }
        [Index(7)]
        [Name("開課時間")]
        public string ClassTime { get; set; }

    }

    class CsvResult
    {
        [Name("學號")]
        [Index(0)]
        public string StudentID { get; set; }
        [Name("姓名")]
        [Index(1)]
        public string StudentName { get; set; }
        [Name("選課總科數")]
        [Index(2)]
        public int CourseCount { get; set; }
        [Name("學分數")]
        [Index(3)]
        public int TotalPoint { get; set; }

    }

    class Record
    {
        public Teacher SelectedTeacher { get; set; }
        public Course SelectedCourse { get; set; }
        public Student SelectedStudent { get; set; }
    }
}
