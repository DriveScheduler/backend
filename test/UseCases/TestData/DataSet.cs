using Domain.Entities.Database;
using Domain.Enums;

namespace UseCases.TestData
{
    internal static class DataSet
    {

        #region TEACHER        
        public static User GetTeacher(Guid id, LicenceType type) => new() { Id = id, FirstName = "Teacher", Name = "Teacher", Email = "t.t@gmail.com", Password = "mdp123", LicenceType = type, Type = UserType.Teacher };
        public static User GetTeacher(Guid id, string name = "Student", string firstName = "Student", string email = "s.s@gmail.com", string password = "mdp123", LicenceType type=LicenceType.Car) => 
            new() { Id = id, FirstName = firstName, Name = name, Email = email, Password = password, LicenceType = type, Type = UserType.Teacher };
        public static User GetCarTeacher(Guid id) => new() { Id = id, FirstName = "Teacher", Name = "Teacher", Email = "t.t@gmail.com", Password = "mdp123", LicenceType = LicenceType.Car, Type = UserType.Teacher };
        public static User GetMotorcycleTeacher(Guid id) => new() { Id = id, FirstName = "Teacher", Name = "Teacher", Email = "t.t@gmail.com", Password = "mdp123", LicenceType = LicenceType.Motorcycle, Type = UserType.Teacher };
        public static User GetTruckTeacher(Guid id) => new() { Id = id, FirstName = "Teacher", Name = "Teacher", Email = "t.t@gmail.com", Password = "mdp123", LicenceType = LicenceType.Truck, Type = UserType.Teacher };
        public static User GetBusTeacher(Guid id) => new() { Id = id, FirstName = "Teacher", Name = "Teacher", Email = "t.t@gmail.com", Password = "mdp123", LicenceType = LicenceType.Bus, Type = UserType.Teacher };
        #endregion

        #region STUDENT        
        public static User GetStudent(Guid id, LicenceType type) => new() { Id = id, FirstName = "Student", Name = "Student", Email = "s.s@gmail.com", Password = "mdp123", LicenceType = type, Type = UserType.Student };
        public static User GetStudent(Guid id, string name="Student", string firstName="Student", string email="s.s@gmail.com", string password="mdp123", LicenceType type=LicenceType.Car) => 
            new() { Id = id, FirstName = firstName, Name = name, Email = email, Password = password, LicenceType = type, Type = UserType.Student };
        public static User GetCarStudent(Guid id) => new() { Id = id, FirstName = "Student", Name = "Student", Email = "s.s@gmail.com", Password = "mdp123", LicenceType = LicenceType.Car, Type = UserType.Student };
        public static User GetMotorcycleStudent(Guid id) => new() { Id = id, FirstName = "Student", Name = "Student", Email = "s.s@gmail.com", Password = "mdp123", LicenceType = LicenceType.Motorcycle, Type = UserType.Student };
        public static User GetTruckStudent(Guid id) => new() { Id = id, FirstName = "Student", Name = "Student", Email = "s.s@gmail.com", Password = "mdp123", LicenceType = LicenceType.Truck, Type = UserType.Student };
        public static User GetBusStudent(Guid id) => new() { Id = id, FirstName = "Student", Name = "Student", Email = "s.s@gmail.com", Password = "mdp123", LicenceType = LicenceType.Bus, Type = UserType.Student };
        #endregion


        #region VEHICLES       
        public static Vehicle GetCar(int id) => new() { Id = id, RegistrationNumber = "VV123VV", Name = "Peugeot 208", Type = LicenceType.Car };        
        public static Vehicle GetMotorcycle(int id) => new() { Id = id, RegistrationNumber = "MM123MM", Name = "Yamaha MT07", Type = LicenceType.Motorcycle };        
        public static Vehicle GetTruck(int id) => new() { Id = id, RegistrationNumber = "TT123TT", Name = "Man ...", Type = LicenceType.Truck };        
        public static Vehicle GetBus(int id) => new() { Id = id, RegistrationNumber = "BB123BB", Name = "Mercedes ...", Type = LicenceType.Bus };
        #endregion

    }
}
