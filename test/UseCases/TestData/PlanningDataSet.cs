using Domain.Models;
using Domain.Enums;

namespace UseCases.TestData
{
    internal class PlanningDataSet
    {
        public readonly DateTime Date = new DateTime(2024, 05, 15, 12, 00, 00);

        public readonly User Student = DataSet.GetCarStudent(new Guid("00000000-0000-0000-0000-000000000001"));

        private readonly User Teacher1 = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000010"));
        private readonly User Teacher2 = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000020"));
        private readonly User Teacher3 = DataSet.GetCarTeacher(new Guid("00000000-0000-0000-0000-000000000030"));

        public List<User> GetAllTeachers() => new List<User>() { Teacher1, Teacher2, Teacher3 };


        private readonly Vehicle Car1 = DataSet.GetCar(1);
        private readonly Vehicle Car2 = DataSet.GetCar(2);
        private readonly Vehicle Car3 = DataSet.GetCar(3);

        public List<Vehicle> GetAllVehicles() => new List<Vehicle>() { Car1, Car2, Car3 };




        private readonly Lesson lessonCarStudent1_1; // Last month
        private readonly Lesson lessonCarStudent1_2; // Last month

        private readonly Lesson lessonCarStudent1_3; // 2 months ago

        private readonly Lesson lessonCarStudent1_4; // 2 weeks ago
        private readonly Lesson lessonCarStudent1_5; // 2 weeks ago

        private readonly Lesson lessonCarStudent1_6; // this week
        private readonly Lesson lessonCarStudent1_7; // this week

        private readonly Lesson lessonCarStudent1_8; // today
        private readonly Lesson lessonCarStudent1_9; // today
        private readonly Lesson lessonCarStudent1_10; // today

        private readonly Lesson lessonCarStudent1_11; // tomorrow
        private readonly Lesson lessonCarStudent1_12; // tomorrow

        private readonly Lesson lessonCarStudent1_13; // this week

        private readonly Lesson lessonCarStudent1_14; // next week
        private readonly Lesson lessonCarStudent1_15; // next week

        private readonly Lesson lessonCarStudent1_16; // next month

        private readonly Lesson lessonCarStudent1_17; // 2 months later

        private readonly Lesson emptyLesson1; // 2 weeks ago
        private readonly Lesson emptyLesson2; // this week

        public PlanningDataSet()
        {
            lessonCarStudent1_1 = new Lesson(1, "Cours 1", new DateTime(2024, 04, 30, 8, 0, 0), 30, Teacher1, LicenceType.Car, Car1, Student); // Last month
            lessonCarStudent1_2 = new Lesson(2, "Cours 2", new DateTime(2024, 04, 1, 8, 0, 0), 45, Teacher2, LicenceType.Car, Car2, Student); // Last month

            lessonCarStudent1_3 = new Lesson(3, "Cours 3", new DateTime(2024, 03, 29, 8, 0, 0), 60, Teacher3, LicenceType.Car, Car3, Student); // 2 months ago

            lessonCarStudent1_4 = new Lesson(4, "Cours 4", new DateTime(2024, 05, 1, 8, 0, 0), 30, Teacher1, LicenceType.Car, Car1, Student); // 2 weeks ago
            lessonCarStudent1_5 = new Lesson(5, "Cours 5", new DateTime(2024, 05, 3, 8, 0, 0), 120, Teacher2, LicenceType.Car, Car2, Student); // 2 weeks ago

            lessonCarStudent1_6 = new Lesson(6, "Cours 6", new DateTime(2024, 05, 13, 8, 0, 0), 30, Teacher3, LicenceType.Car, Car3, Student); // this week
            lessonCarStudent1_7 = new Lesson(7, "Cours 7", new DateTime(2024, 05, 14, 8, 0, 0), 45, Teacher1, LicenceType.Car, Car1, Student); // this week

            lessonCarStudent1_8 = new Lesson(8, "Cours 8", new DateTime(2024, 05, 15, 8, 0, 0), 60, Teacher2, LicenceType.Car, Car2, Student); // today
            lessonCarStudent1_9 = new Lesson(9, "Cours 9", new DateTime(2024, 05, 15, 13, 0, 0), 30, Teacher3, LicenceType.Car, Car3, Student); // today
            lessonCarStudent1_10 = new Lesson(10, "Cours 10", new DateTime(2024, 05, 15, 15, 0, 0), 60, Teacher1, LicenceType.Car, Car1, Student); // today

            lessonCarStudent1_11 = new Lesson(11, "Cours 11", new DateTime(2024, 05, 16, 8, 0, 0), 30, Teacher2, LicenceType.Car, Car2, Student); // tomorrow
            lessonCarStudent1_12 = new Lesson(12, "Cours 12", new DateTime(2024, 05, 16, 12, 0, 0), 30, Teacher3, LicenceType.Car, Car3, Student); // tomorrow

            lessonCarStudent1_13 = new Lesson(13, "Cours 13", new DateTime(2024, 05, 17, 15, 0, 0), 75, Teacher1, LicenceType.Car, Car1, Student); // this week

            lessonCarStudent1_14 = new Lesson(14, "Cours 14", new DateTime(2024, 05, 20, 8, 0, 0), 30, Teacher2, LicenceType.Car, Car2, Student); // next week
            lessonCarStudent1_15 = new Lesson(15, "Cours 15", new DateTime(2024, 05, 24, 8, 0, 0), 120, Teacher3, LicenceType.Car, Car3, Student); // next week

            lessonCarStudent1_16 = new Lesson(16, "Cours 16", new DateTime(2024, 06, 1, 8, 0, 0), 45, Teacher1, LicenceType.Car, Car1, Student); // next month

            lessonCarStudent1_17 = new Lesson(17, "Cours 17", new DateTime(2024, 07, 1, 8, 0, 0), 30, Teacher2, LicenceType.Car, Car2, Student); // 2 months later

            emptyLesson1 = new Lesson(18, "Cours 18", new DateTime(2024, 05, 3, 10, 0, 0), 30, Teacher3, LicenceType.Car, Car3); // 2 weeks ago
            emptyLesson2 = new Lesson(19, "Cours 19", new DateTime(2024, 05, 13, 10, 0, 0), 30, Teacher1, LicenceType.Car, Car1); // this week
        }

        public List<Lesson> GetAllLessons() => new List<Lesson>()
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

        public List<Lesson> AchievedLessons() => new List<Lesson>()
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
        public int AchievedLessonsTotalTime() => AchievedLessons().Sum(l => l.Duration.Value);

        public List<Lesson> TodayLessons() => new List<Lesson>() { lessonCarStudent1_9, lessonCarStudent1_10 };
        public List<Lesson> TomorrowLessons() => new List<Lesson>() { lessonCarStudent1_11, lessonCarStudent1_12 };
        public List<Lesson> ThisWeekLessons() => new List<Lesson>() { lessonCarStudent1_13 };
        public List<Lesson> ThisMonthLessons() => new List<Lesson>() { lessonCarStudent1_14, lessonCarStudent1_15 };
        public List<Lesson> NextMonthsLessons() => new List<Lesson>() { lessonCarStudent1_16, lessonCarStudent1_17 };

        public int NextLessonsTotal() =>
         TodayLessons().Count +
         TomorrowLessons().Count +
         ThisWeekLessons().Count +
         ThisMonthLessons().Count +
         NextMonthsLessons().Count;

        public int NextLessonsTotalTime() =>
            TodayLessons().Sum(l => l.Duration.Value) +
            TomorrowLessons().Sum(l => l.Duration.Value) +
            ThisWeekLessons().Sum(l => l.Duration.Value) +
            ThisMonthLessons().Sum(l => l.Duration.Value) +
            NextMonthsLessons().Sum(l => l.Duration.Value);


        public Lesson NextLesson() => lessonCarStudent1_9;
        public Lesson LastLesson() => lessonCarStudent1_8;

        public User FavoriteTeacher() => Teacher2;
        public int FavoriteTeacherTotalTime() => 225;

        public Vehicle FavoriteVehicle() => Car2;
        public int FavoriteVehicleTotalTime() => 225;

        public int LessonTotalTimeThisWeek() => lessonCarStudent1_6.Duration.Value + lessonCarStudent1_7.Duration.Value + lessonCarStudent1_8.Duration.Value;


    }
}
