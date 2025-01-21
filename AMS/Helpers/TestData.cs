using AMS.Models.UserProfileViewModel;

namespace AMS.Helpers
{
    public static class TestData
    {
        public static UserProfileCRUDViewModel GetUserProfileTestData()
        {
            UserProfileCRUDViewModel _UserProfileCRUDViewModel = new()
            {
                FirstName = GenerateString(10),
                LastName = GenerateString(5),
                Email = GenerateString(2) + "@gmail.com",
                PasswordHash = "123",
                ConfirmPassword = "123",
                PhoneNumber = StaticData.RandomDigits(11),
                ProfilePicture = "/images/UserIcon/U1.png",
                Address = "California",
                Country = "USA",
                
                IsApprover = 1,
                EmployeeId = StaticData.RandomDigits(6),
                DateOfBirth = DateTime.Now.AddYears(-25),
                Designation = 1,
                Department = 1,
                SubDepartment = 1,
                JoiningDate = DateTime.Now.AddYears(-1),
                LeavingDate = DateTime.Now,
            };
            return _UserProfileCRUDViewModel;
        }
        public static string GenerateString(int size)
        {
            Random _Random = new Random();
            string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[_Random.Next(Alphabet.Length)];
            }
            return new string(chars);
        }
    }
}
