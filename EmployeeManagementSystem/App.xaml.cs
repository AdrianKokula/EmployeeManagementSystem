using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		public static Database Database { get; set; }

		public static string LoggedUser { get; set; }

	}
}
