// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

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
using System.Data;
using System.Data.SqlClient;

using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem.UserControls {

	/// <summary>
	/// Interaction logic for EditUser.xaml
	/// </summary>
	public partial class EditUser : UserControl {

		private readonly Database database;
		private readonly int userID;

		#region "constructors"

		public EditUser(Database database, int userID) {

			InitializeComponent();

			this.database = database;
			this.userID = userID;

			Load();

		}

		#endregion

		#region "handlers"

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {

		}

		#endregion

		#region "methods"

		#region "private"

		private void Load() {

			string query =
@"DECLARE @UserID int = @pUserID;

SELECT [Name], Email, [Password]
	FROM dbo.Users
	WHERE ID = @UserID;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pUserID", SqlDbType.Int).Value = this.userID;

			DataTable dataTableUser = new DataTable();

			string result = database.FillDataTable(ref dataTableUser, sqlCommand);

			if (!result.Equals("OK")) return;
			if (dataTableUser.Rows.Count <= 0) return;

			DataRow row = dataTableUser.Rows[0];
			TbName.Text = Tools.StringFromObject(row["Name"]);
			TbEmail.Text = Tools.StringFromObject(row["Email"]);
			PbPassword.Password = Tools.StringFromObject(row["Password"]);

		}

		#endregion

		#endregion

	}
}
