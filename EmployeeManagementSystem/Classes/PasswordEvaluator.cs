using System.Linq;

namespace EmployeeManagementSystem.Classes {

	public sealed class PasswordEvaluator {

		#region Private fields

		private readonly char[] SpecialSymbols = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')' };

		#endregion

		#region Public properties

		public string Password { get; set; }

		public int RequiredLenght { get; set; }

		public int PasswordLength => Password.Length;

		public bool PasswordLengthOk => PasswordLength >= RequiredLenght;

		public bool ContainsNumber => Password.Any(char.IsDigit);

		public bool ContainsLowerCase => Password.Any(char.IsLower);

		public bool ContainsUpperCase => Password.Any(char.IsUpper);

		public bool ContainsSymbol => Password.IndexOfAny(SpecialSymbols) != -1;

		public bool IsValid => PasswordLengthOk && ContainsNumber && ContainsUpperCase && ContainsLowerCase && ContainsSymbol;

		#endregion

		#region Constructors

		public PasswordEvaluator(string password, int requiredLenght = 8) {
			Password = password;
			RequiredLenght = requiredLenght;
		}

		#endregion

		#region Desctructors

		~PasswordEvaluator() {
			Password = string.Empty;
		}

		#endregion

	}

}
