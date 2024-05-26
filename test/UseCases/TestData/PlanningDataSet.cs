using Domain.Entities;
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




        private static readonly Lesson lessonCarStudent1_1 = new Lesson(1, "Cours 1", new DateTime(2024, 04, 30, 8, 0, 0), 30, Teacher1, LicenceType.Car, Car1, Student); // Last month
        private static readonly Lesson lessonCarStudent1_2 = new Lesson(2, "Cours 2", new DateTime(2024, 04, 1, 8, 0, 0), 45, Teacher2, LicenceType.Car, Car2, Student); // Last month

        private static readonly Lesson lessonCarStudent1_3 = new Lesson(3, "Cours 3", new DateTime(2024, 03, 29, 8, 0, 0), 60, Teacher3, LicenceType.Car, Car3, Student); // 2 months ago

        private static readonly Lesson lessonCarStudent1_4 = new Lesson(4, "Cours 4", new DateTime(2024, 05, 1, 8, 0, 0), 30, Teacher1, LicenceType.Car, Car1, Student); // 2 weeks ago
        private static readonly Lesson lessonCarStudent1_5 = new Lesson(5, "Cours 5", new DateTime(2024, 05, 3, 8, 0, 0), 120, Teacher2, LicenceType.Car, Car2, Student); // 2 weeks ago

        private static readonly Lesson lessonCarStudent1_6 = new Lesson(6, "Cours 6", new DateTime(2024, 05, 13, 8, 0, 0), 30, Teacher3, LicenceType.Car, Car3, Student); // this week
        private static readonly Lesson lessonCarStudent1_7 = new Lesson(7, "Cours 7", new DateTime(2024, 05, 14, 8, 0, 0), 45, Teacher1, LicenceType.Car, Car1, Student); // this week

        private static readonly Lesson lessonCarStudent1_8 = new Lesson(8, "Cours 8", new DateTime(2024, 05, 15, 8, 0, 0), 60, Teacher2, LicenceType.Car, Car2, Student); // today
        private static readonly Lesson lessonCarStudent1_9 = new Lesson(9, "Cours 9", new DateTime(2024, 05, 15, 13, 0, 0), 30, Teacher3, LicenceType.Car, Car3, Student); // today
        private static readonly Lesson lessonCarStudent1_10 = new Lesson(10, "Cours 10", new DateTime(2024, 05, 15, 15, 0, 0), 60, Teacher1, LicenceType.Car, Car1, Student); // today

        private static readonly Lesson lessonCarStudent1_11 = new Lesson(11, "Cours 11", new DateTime(2024, 05, 16, 8, 0, 0), 30, Teacher2, LicenceType.Car, Car2, Student); // tomorrow
        private static readonly Lesson lessonCarStudent1_12 = new Lesson(12, "Cours 12", new DateTime(2024, 05, 16, 12, 0, 0), 30, Teacher3, LicenceType.Car, Car3, Student); // tomorrow

        private static readonly Lesson lessonCarStudent1_13 = new Lesson(13, "Cours 13", new DateTime(2024, 05, 17, 15, 0, 0), 75, Teacher1, LicenceType.Car, Car1, Student); // this week

        private static readonly Lesson lessonCarStudent1_14 = new Lesson(14, "Cours 14", new DateTime(2024, 05, 20, 8, 0, 0), 30, Teacher2, LicenceType.Car, Car2, Student); // next week
        private static readonly Lesson lessonCarStudent1_15 = new Lesson(15, "Cours 15", new DateTime(2024, 05, 24, 8, 0, 0), 120, Teacher3, LicenceType.Car, Car3, Student); // next week

        private static readonly Lesson lessonCarStudent1_16 = new Lesson(16, "Cours 16", new DateTime(2024, 06, 1, 8, 0, 0), 45, Teacher1, LicenceType.Car, Car1, Student); // next month

        private static readonly Lesson lessonCarStudent1_17 = new Lesson(17, "Cours 17", new DateTime(2024, 07, 1, 8, 0, 0), 30, Teacher2, LicenceType.Car, Car2, Student); // 2 months later

        private static readonly Lesson emptyLesson1 = new Lesson(18, "Cours 18", new DateTime(2024, 05, 3, 10, 0, 0), 30, Teacher3, LicenceType.Car, Car3); // 2 weeks ago
        private static readonly Lesson emptyLesson2 = new Lesson(19, "Cours 19", new DateTime(2024, 05, 13, 10, 0, 0), 30, Teacher1, LicenceType.Car, Car1); // this week

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
        public static int AchievedLessonsTotalTime() => AchievedLessons().Sum(l => l.Duration.Value);

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
            TodayLessons().Sum(l => l.Duration.Value) +
            TomorrowLessons().Sum(l => l.Duration.Value) +
            ThisWeekLessons().Sum(l => l.Duration.Value) +
            ThisMonthLessons().Sum(l => l.Duration.Value) +
            NextMonthsLessons().Sum(l => l.Duration.Value);


        public static Lesson NextLesson() => lessonCarStudent1_9;
        public static Lesson LastLesson() => lessonCarStudent1_8;

        public static User FavoriteTeacher() => Teacher2;
        public static int FavoriteTeacherTotalTime() => 225;

        public static Vehicle FavoriteVehicle() => Car2;
        public static int FavoriteVehicleTotalTime() => 225;

        public static int LessonTotalTimeThisWeek() => lessonCarStudent1_6.Duration.Value + lessonCarStudent1_7.Duration.Value + lessonCarStudent1_8.Duration.Value;


    }
}
