using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private readonly Database database;

		#region "constructors"

		public MainWindow() {

			InitializeComponent();

			if(String.IsNullOrWhiteSpace(Properties.Settings.Default.ConnectionString)) {

				return;
			}

			database = new Database(Properties.Settings.Default.ConnectionString);
			if(!database.TestConnection()) {

				return;
			}


		}

		#endregion

		#region "private methods"



		#endregion



	}
}
