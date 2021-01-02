using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Classes {

	class DbTools {

		public static string StringFromSqlScalar(Object obj) {

			if (obj == DBNull.Value) return string.Empty;
			return (string)obj;

		}

	}

}
