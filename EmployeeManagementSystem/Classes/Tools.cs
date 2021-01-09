using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Classes {

	class Tools {

		public static string StringFromObject(object obj) {

			if (obj == DBNull.Value) return string.Empty;
			return (string)obj;

		}

		public static int IntFromObject(object obj) {

			if (obj == DBNull.Value) return 0;
			if (!int.TryParse((string)obj, out int result)) return 0;

			return result;

		}

	}

}
