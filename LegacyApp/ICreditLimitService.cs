using System;

namespace LegacyApp;

public interface ICreditLimitService
{
    int getCreditLimit(string lastname, DateTime birthdate);
}