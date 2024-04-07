using System;

namespace LegacyApp
{
    public interface IUserDataAccessAdapter
    {
        void AddUser(User user);
    }
    
    public interface IUserCreditService
    {
        int GetCreditLimit(string lastName, DateTime dateOfBirth);
    }
    public interface IClientRepository
    {
        Client GetById(int clientId);
    }
    public class UserService
    {   
        private IClientRepository _clientRepository;
        private IUserCreditService _userCreditService;
        private IUserDataAccessAdapter _userDataAccessAdapter;

        public UserService()
        {
            _clientRepository = new ClientRepository();
            _userCreditService = new UserCreditService();
            _userDataAccessAdapter = new UserDataAccessAdapter();
        }
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsValidInput(firstName, lastName, email, dateOfBirth))
            {
                return false;
            }

            Client client = new ClientRepository().GetById(clientId);
            User user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            CalculateCreditLimit(user, client);

            if (ShouldDenyWHenCreditLimit(user))
            {
                return false;
            }
            UserDataAccess.AddUser(user);
            return true;
        }

        private bool IsValidInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            return !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
                   email.Contains("@") && email.Contains(".") && 
                   CalculateAge(dateOfBirth) >= 21;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age;
        }

        private void CalculateCreditLimit(User user, Client client)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else
            {
                UserCreditService creditService = new UserCreditService();
                int creditLimit = creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                if (client.Type == "ImportantClient")
                {
                    creditLimit = creditLimit * 2;
                }
                user.CreditLimit = creditLimit;
                user.HasCreditLimit = true;
            }
        }
        

        private bool ShouldDenyWHenCreditLimit(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }
    }
}
