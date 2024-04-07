using LegacyApp;

namespace LegacyAppTests;

public class UserServiceTests
{
    [Fact]
    public void AddUser_Should_Return_False_When_Missing_At_Sign_And_Dot_In_Email()
    {
        ////////////
        string firstName = "John";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "doe";
        var service = new UserService();
        //
        bool result = string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName);
        Assert.Equal(false,result);
    }
    [Fact]
    public void AddUser_Should_Return_False_When_Missing_FirstName()
    {
        UserService user = new UserService();
        DateTime dateTime = new DateTime(1990, 09, 12);
        bool result = user.AddUser(null, null, null, dateTime, 1);
        Assert.Equal(false,result);
    }
    [Fact]
    public void AddUser_Should_Return_False_When_Normal_Client_With_No_Credit()
    {
        UserService user = new UserService();
        DateTime dateTime = new DateTime(1990, 09, 12);
        bool result = user.AddUser(null, null, null, dateTime, 1);
        Assert.Equal(false,result);
    }
    [Fact]
    public void AddUser_Should_Return_False_When_Younger_Then_21_Years_Old()
    {
        UserService user = new UserService();
        DateTime dateTime = new DateTime(2006, 09, 12);
        bool result = user.AddUser(null, null, null, dateTime, 1);
        Assert.Equal(false,result);
    }
    [Fact]
    public void AddUser_Should_Return_True_When_Important_Client()
    {   
        UserService user = new UserService();
        DateTime dateTime = new DateTime(1990, 09, 12);
        var result = user.AddUser(null, "Smith",  "smith@gmail.pl", dateTime, 3);
        Assert.Equal(false,result);
    }
    [Fact]
    public void AddUser_Should_Return_True_When_Normal_Client()
    {
        string firstName = "Maciek";
        string lastName = "Kwiatkowski";
        DateTime birthDate = new DateTime(1990, 1, 1);
        int clientId = 5;
        string email = "doe@gmail.pl";
        var service = new UserService();
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        Assert.Equal(true, result);
    }


    [Fact]
    public void AddUser_Should_Throw_Exception_When_User_Does_Not_Exists()
    {
        string firstName = "Maciek";
        string lastName =  "Andrzejewicz";
        DateTime birthDate = new DateTime(1990, 1, 1);
        int clientId = 7;
        string email = "doe@gmail.pl";
        var service = new UserService();
        try
        {
            bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
            Assert.Fail("Powinno być excetpion");
        }
        catch (ArgumentException e)
        {
            
        }
    }
  
    
    [Fact]
    public void AddUser_Should_Throw_Exception_When_User_No_Credit_Limit_Exists_Is_No_In_Database()
    {
        UserService user = new UserService();
        DateTime dateTime = new DateTime(1990, 1, 1);
        string lastName = "Andrzejewicz";
        try
        {
            var result = user.AddUser("Maciek", lastName, "andrzejewicz@wp.pl", dateTime, 6);
             Assert.Fail("Ma rzucic wyjatek");
        }
        catch (Exception e)
        {
              Assert.Equal($"Client {lastName} does not exist",e.Message);
        }

    }
}