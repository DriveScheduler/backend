using Domain.Entities;
using Domain.Enums;

namespace UseCases.TestData
{
    internal static class DataSet
    {

        #region TEACHER        
        public static User GetTeacher(Guid id, LicenceType type) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", type, UserType.Teacher);
        public static User GetTeacher(Guid id, string name = "Student", string firstName = "Student", string email = "s.s@gmail.com", string password = "mdp123", LicenceType type = LicenceType.Car) =>
            new(id, name, firstName, email, password, type, UserType.Teacher);
        public static User GetCarTeacher(Guid id) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", LicenceType.Car, UserType.Teacher);
        public static User GetMotorcycleTeacher(Guid id) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", LicenceType.Motorcycle, UserType.Teacher);
        public static User GetTruckTeacher(Guid id) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", LicenceType.Truck, UserType.Teacher);
        public static User GetBusTeacher(Guid id) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", LicenceType.Bus, UserType.Teacher);
        #endregion

        #region STUDENT        
        public static User GetStudent(Guid id, LicenceType type) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", type, UserType.Student);
        public static User GetStudent(Guid id, string name="Student", string firstName="Student", string email="s.s@gmail.com", string password="mdp123", LicenceType type=LicenceType.Car) => 
            new(id, name, firstName, email, password, type, UserType.Student);
        public static User GetCarStudent(Guid id) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", LicenceType.Car, UserType.Student);
        public static User GetMotorcycleStudent(Guid id) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", LicenceType.Motorcycle, UserType.Student);
        public static User GetTruckStudent(Guid id) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", LicenceType.Truck, UserType.Student);
        public static User GetBusStudent(Guid id) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", LicenceType.Bus, UserType.Student);
        #endregion


        #region VEHICLES       
        public static Vehicle GetCar(int id) => new(id, "VV123VV", "Peugeot 208", LicenceType.Car); 
        public static Vehicle GetMotorcycle(int id) => new(id, "MM123MM", "Yamaha MT07", LicenceType.Motorcycle);        
        public static Vehicle GetTruck(int id) => new(id, "TT123TT", "Man ...", LicenceType.Truck);        
        public static Vehicle GetBus(int id) => new(id, "BB123BB", "Mercedes ...", LicenceType.Bus);
        #endregion

    }
}
