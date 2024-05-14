using Domain.Entities.Database;
using Domain.Enums;

namespace UseCases.TestData
{
    internal static class PlanningDataSet
    {
        public static readonly DateTime Date = new DateTime(2024, 05, 15, 12, 00, 00);

        public static readonly User Student = DataSet.GetCarStudent(new Guid("00000000-0000-0000-0000-000000000001"));

        private static readonly User Teacher1 = DataSet.GetCarStudent(new Guid("00000000-0000-0000-0000-000000000010"));
        private static readonly User Teacher2 = DataSet.GetCarStudent(new Guid("00000000-0000-0000-0000-000000000020"));
        private static readonly User Teacher3 = DataSet.GetCarStudent(new Guid("00000000-0000-0000-0000-000000000030"));

        public static List<User> GetAllTeachers() => new List<User>() { Teacher1, Teacher2, Teacher3 };


        private static readonly Vehicle Car1 = DataSet.GetCar(1);
        private static readonly Vehicle Car2 = DataSet.GetCar(2);
        private static readonly Vehicle Car3 = DataSet.GetCar(3);

        public static List<Vehicle> GetAllVehicles() => new List<Vehicle>() { Car1, Car2, Car3 };




        private static readonly Lesson lessonCarStudent1_1 = new Lesson() { Id = 1, Name = "Course 1", Start = new DateTime(2024, 04, 30, 8, 0, 0), Duration = 30, Teacher = Teacher1, Student = Student, Vehicle = Car1, Type = LicenceType.Car }; // Last month
        private static readonly Lesson lessonCarStudent1_2 = new Lesson() { Id = 2, Name = "Course 2", Start = new DateTime(2024, 04, 1, 8, 0, 0), Duration = 45, Teacher = Teacher2, Student = Student, Vehicle = Car2, Type = LicenceType.Car }; // Last month

        private static readonly Lesson lessonCarStudent1_3 = new Lesson() { Id = 3, Name = "Course 3", Start = new DateTime(2024, 03, 29, 8, 0, 0), Duration = 60, Teacher = Teacher3, Student = Student, Vehicle = Car3, Type = LicenceType.Car }; // 2 months ago

        private static readonly Lesson lessonCarStudent1_4 = new Lesson() { Id = 4, Name = "Course 4", Start = new DateTime(2024, 05, 1, 8, 0, 0), Duration = 30, Teacher = Teacher1, Student = Student, Vehicle = Car1, Type = LicenceType.Car }; // 2 weeks ago
        private static readonly Lesson lessonCarStudent1_5 = new Lesson() { Id = 5, Name = "Course 5", Start = new DateTime(2024, 05, 3, 8, 0, 0), Duration = 120, Teacher = Teacher2, Student = Student, Vehicle = Car2, Type = LicenceType.Car }; // 2 weeks ago

        private static readonly Lesson lessonCarStudent1_6 = new Lesson() { Id = 6, Name = "Course 6", Start = new DateTime(2024, 05, 13, 8, 0, 0), Duration = 30, Teacher = Teacher3, Student = Student, Vehicle = Car3, Type = LicenceType.Car }; // this week
        private static readonly Lesson lessonCarStudent1_7 = new Lesson() { Id = 7, Name = "Course 7", Start = new DateTime(2024, 05, 14, 8, 0, 0), Duration = 45, Teacher = Teacher1, Student = Student, Vehicle = Car1, Type = LicenceType.Car }; // this week

        private static readonly Lesson lessonCarStudent1_8 = new Lesson() { Id = 8, Name = "Course 8", Start = new DateTime(2024, 05, 15, 8, 0, 0), Duration = 60, Teacher = Teacher2, Student = Student, Vehicle = Car2, Type = LicenceType.Car }; // today
        private static readonly Lesson lessonCarStudent1_9 = new Lesson() { Id = 9, Name = "Course 9", Start = new DateTime(2024, 05, 15, 13, 0, 0), Duration = 30, Teacher = Teacher3, Student = Student, Vehicle = Car3, Type = LicenceType.Car }; // today
        private static readonly Lesson lessonCarStudent1_10 = new Lesson() { Id = 10, Name = "Course 10", Start = new DateTime(2024, 05, 15, 15, 0, 0), Duration = 60, Teacher = Teacher1, Student = Student, Vehicle = Car1, Type = LicenceType.Car }; // today

        private static readonly Lesson lessonCarStudent1_11 = new Lesson() { Id = 11, Name = "Course 11", Start = new DateTime(2024, 05, 16, 8, 0, 0), Duration = 30, Teacher = Teacher2, Student = Student, Vehicle = Car2, Type = LicenceType.Car }; // tomorrow
        private static readonly Lesson lessonCarStudent1_12 = new Lesson() { Id = 12, Name = "Course 12", Start = new DateTime(2024, 05, 16, 12, 0, 0), Duration = 30, Teacher = Teacher3, Student = Student, Vehicle = Car3, Type = LicenceType.Car }; // tomorrow

        private static readonly Lesson lessonCarStudent1_13 = new Lesson() { Id = 13, Name = "Course 13", Start = new DateTime(2024, 05, 17, 15, 0, 0), Duration = 75, Teacher = Teacher1, Student = Student, Vehicle = Car1, Type = LicenceType.Car }; // this week

        private static readonly Lesson lessonCarStudent1_14 = new Lesson() { Id = 14, Name = "Course 14", Start = new DateTime(2024, 05, 20, 8, 0, 0), Duration = 30, Teacher = Teacher2, Student = Student, Vehicle = Car2, Type = LicenceType.Car }; // next week
        private static readonly Lesson lessonCarStudent1_15 = new Lesson() { Id = 15, Name = "Course 15", Start = new DateTime(2024, 05, 24, 8, 0, 0), Duration = 120, Teacher = Teacher3, Student = Student, Vehicle = Car3, Type = LicenceType.Car }; // next week

        private static readonly Lesson lessonCarStudent1_16 = new Lesson() { Id = 16, Name = "Course 16", Start = new DateTime(2024, 06, 1, 8, 0, 0), Duration = 45, Teacher = Teacher1, Student = Student, Vehicle = Car1, Type = LicenceType.Car }; // next month
                                                                                                                                                                                                                                                      // 
        private static readonly Lesson lessonCarStudent1_17 = new Lesson() { Id = 17, Name = "Course 17", Start = new DateTime(2024, 07, 1, 8, 0, 0), Duration = 30, Teacher = Teacher2, Student = Student, Vehicle = Car2, Type = LicenceType.Car }; // 2 months later

        private static readonly Lesson emptyLesson1 = new Lesson() { Id = 18, Name = "Course 18", Start = new DateTime(2024, 05, 3, 10, 0, 0), Duration = 30, Teacher = Teacher3, Student = null, Vehicle = Car3, Type = LicenceType.Car }; // 2 weeks ago
        private static readonly Lesson emptyLesson2 = new Lesson() { Id = 19, Name = "Course 19", Start = new DateTime(2024, 05, 13, 10, 0, 0), Duration = 30, Teacher = Teacher1, Student = null, Vehicle = Car1, Type = LicenceType.Car }; // this week

        public static List<Lesson> GetAllLessons() => new List<Lesson>()
        {
            lessonCarStudent1_1,
            lessonCarStudent1_2,
            lessonCarStudent1_3,
            lessonCarStudent1_4,
            lessonCarStudent1_5,
            lessonCarStudent1_6,
            lessonCarStudent1_7,
            lessonCarStudent1_8,
            lessonCarStudent1_9,
            lessonCarStudent1_10,
            lessonCarStudent1_11,
            lessonCarStudent1_12,
            lessonCarStudent1_13,
            lessonCarStudent1_14,
            lessonCarStudent1_15,
            lessonCarStudent1_16,
            lessonCarStudent1_17,
            emptyLesson1,
            emptyLesson2
        };

        public static List<Lesson> AchievedLessons() => new List<Lesson>()
        {
            lessonCarStudent1_8,
            lessonCarStudent1_7,
            lessonCarStudent1_6,
            lessonCarStudent1_5,
            lessonCarStudent1_4,
            lessonCarStudent1_1,
            lessonCarStudent1_2,
            lessonCarStudent1_3,
        };
        public static int AchievedLessonsTotalTime() => AchievedLessons().Sum(l => l.Duration);

        public static List<Lesson> TodayLessons() => new List<Lesson>() { lessonCarStudent1_9, lessonCarStudent1_10 };
        public static List<Lesson> TomorrowLessons() => new List<Lesson>() { lessonCarStudent1_11, lessonCarStudent1_12 };
        public static List<Lesson> ThisWeekLessons() => new List<Lesson>() { lessonCarStudent1_13 };
        public static List<Lesson> ThisMonthLessons() => new List<Lesson>() { lessonCarStudent1_14, lessonCarStudent1_15 };
        public static List<Lesson> NextMonthsLessons() => new List<Lesson>() { lessonCarStudent1_16, lessonCarStudent1_17 };

        public static int NextLessonsTotal() =>
         TodayLessons().Count +
         TomorrowLessons().Count +
         ThisWeekLessons().Count +
         ThisMonthLessons().Count +
         NextMonthsLessons().Count;

        public static int NextLessonsTotalTime() =>
            TodayLessons().Sum(l => l.Duration) +
            TomorrowLessons().Sum(l => l.Duration) +
            ThisWeekLessons().Sum(l => l.Duration) +
            ThisMonthLessons().Sum(l => l.Duration) +
            NextMonthsLessons().Sum(l => l.Duration);


        public static Lesson NextLesson() => lessonCarStudent1_9;
        public static Lesson LastLesson() => lessonCarStudent1_8;

        public static User FavoriteTeacher() => Teacher2;
        public static int FavoriteTeacherTotalTime() => 225;

        public static Vehicle FavoriteVehicle() => Car2;
        public static int FavoriteVehicleTotalTime() => 225;

        public static int LessonTotalTimeThisWeek() => lessonCarStudent1_6.Duration + lessonCarStudent1_7.Duration + lessonCarStudent1_8.Duration;


    }
}
