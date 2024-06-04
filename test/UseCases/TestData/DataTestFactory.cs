using Domain.Enums;
using Domain.Models.Users;
using Domain.Models.Vehicles;

namespace UseCases.TestData
{
    internal static class DataTestFactory
    {

        #region TEACHER        
        public static Teacher GetTeacher(Guid id, LicenceType type) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", type);
        public static Teacher GetTeacher(Guid id, string name = "Student", string firstName = "Student", string email = "s.s@gmail.com", string password = "mdp123", LicenceType type = LicenceType.Car) =>
            new(id, name, firstName, email, password, type);
        public static Teacher GetCarTeacher(Guid id) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", LicenceType.Car);
        public static Teacher GetMotorcycleTeacher(Guid id) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", LicenceType.Motorcycle);
        public static Teacher GetTruckTeacher(Guid id) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", LicenceType.Truck);
        public static Teacher GetBusTeacher(Guid id) => new(id, "Teacher", "Teacher", "t.t@gmail.com", "mdp123", LicenceType.Bus);
        #endregion

        #region STUDENT        
        public static Student GetStudent(Guid id, LicenceType type) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", type);
        public static Student GetStudent(Guid id, string name="Student", string firstName="Student", string email="s.s@gmail.com", string password="mdp123", LicenceType type=LicenceType.Car) => 
            new(id, name, firstName, email, password, type);
        public static Student GetCarStudent(Guid id) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", LicenceType.Car);
        public static Student GetMotorcycleStudent(Guid id) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", LicenceType.Motorcycle);
        public static Student GetTruckStudent(Guid id) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", LicenceType.Truck);
        public static Student GetBusStudent(Guid id) => new(id, "Student", "Student", "s.s@gmail.com", "mdp123", LicenceType.Bus);
        #endregion


        #region VEHICLES       
        public static Car GetCar(int id) => new(id, "VV123VV", "Peugeot 208"); 
        public static Motorcycle GetMotorcycle(int id) => new(id, "MM123MM", "Yamaha MT07");        
        public static Truck GetTruck(int id) => new(id, "TT123TT", "Man ...");        
        public static Bus GetBus(int id) => new(id, "BB123BB", "Mercedes ...");
        #endregion

    }
}
