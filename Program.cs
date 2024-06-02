using System.Security.Cryptography;
using System.Text;

namespace PasswordGenerator
{
  public static class Program
  { 
    public static void Main(string[] args) 
    {
      IPasswordGenerator _passwordGen = new PasswordGen();
      PasswordRequirements _newRequirements = new PasswordRequirements();
      _newRequirements.MaxLength = 10;
      _newRequirements.MinLength = 5;
      _newRequirements.MinNumericChars = 1;
      _newRequirements.MinSpecialChars = 1;
      _newRequirements.MinUpperAlphaChars = 1;
      _newRequirements.MinLowerAlphaChars = 2;

      Console.WriteLine(_passwordGen.GeneratePassword(_newRequirements));
    }
  }
  public class PasswordRequirements
  {
    public int MaxLength { get; set; }
    public int MinLength { get; set; }
    public int MinUpperAlphaChars { get; set; }
    public int MinLowerAlphaChars { get; set; }
    public int MinNumericChars { get; set; }
    public int MinSpecialChars { get; set;}
  }

  public interface IPasswordGenerator
  {
    string GeneratePassword(PasswordRequirements requirements);
  }

  public class PasswordGen : IPasswordGenerator
  {
    public string GeneratePassword(PasswordRequirements requirements) 
    {
      Random _rndLength = new Random();

      int _passwordLength = _rndLength.Next(requirements.MinLength, requirements.MaxLength + 1);
      char[] _password = new char[_passwordLength];

      _password = FillPassword(_password, requirements);

      return new string(_password);
    }

    private char[] FillPassword(char[] password, PasswordRequirements requirements) 
    {
      Random _rndGen = new Random();
      int _index;
      while(password.Contains('\0'))
      {
        _index = _rndGen.Next(password.Length);
        if(password[_index] == '\0')
        {
          password[_index] = FillChar(password, requirements);
        }
      }
      return password;
    }
    private char FillChar(char[] password, PasswordRequirements requirements)
    {
      string[] _potentialChars = { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "1234567890", " !\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~" };

      Random _rndGen = new Random();
      List<char> _tempPassword = password.ToList();

      int _numOfUpperAlphaChars = _tempPassword.FindAll(_char => char.IsUpper(_char)).Count;
      int _numOfLowerAlphaChars = _tempPassword.FindAll(_char => char.IsLower(_char)).Count;
      int _numOfNumerics = _tempPassword.FindAll(_char => char.IsNumber(_char)).Count;
      int _numOfSpecials = _tempPassword.FindAll(_char => char.IsSymbol(_char)).Count;

      char _returnVal;
      int _overflowIndex;

      if(_numOfUpperAlphaChars < requirements.MinUpperAlphaChars)
      {
        _returnVal = _potentialChars[0][_rndGen.Next(_potentialChars[0].Length)];
      }
      else if(_numOfLowerAlphaChars < requirements.MinLowerAlphaChars)
      {
        _returnVal = _potentialChars[1][_rndGen.Next(_potentialChars[1].Length)];
      }
      else if(_numOfNumerics < requirements.MinNumericChars)
      {
        _returnVal = _potentialChars[2][_rndGen.Next(_potentialChars[2].Length)];
      }
      else if(_numOfSpecials < requirements.MinSpecialChars)
      {
        _returnVal = _potentialChars[3][_rndGen.Next(_potentialChars[3].Length)];
      }
      else
      {
        _overflowIndex = _rndGen.Next(_potentialChars.Length);
        _returnVal = _potentialChars[_overflowIndex][_rndGen.Next(_potentialChars[_overflowIndex].Length)];
      }
      return _returnVal;
    }
  }
}
