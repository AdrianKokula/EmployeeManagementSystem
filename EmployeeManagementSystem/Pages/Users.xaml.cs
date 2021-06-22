// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;

using EmployeeManagementSystem.Classes;
using EmployeeManagementSystem.Windows;
using EmployeeManagementSystem.UserControls;

namespace EmployeeManagementSystem.Pages {

	/// <summary>
	/// Interaction logic for Users.xaml
	/// </summary>
	public partial class Users : Page {

		#region	Constructors

		public Users() {

			InitializeComponent();

			LoadData();

			EditUser.InitUserControl(DeleteUser);

		}

		#endregion

		#region Handlers

		private void BtnAddUser_Click(object sender, RoutedEventArgs e) {

			AddUser userWindow = new AddUser() {
				Owner = Window.GetWindow(this)
			};

			bool? dialogResult = userWindow.ShowDialog();
			if (!dialogResult.Value) {
				return;
			}

			LoadData();

		}

		private void DgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			if (e.AddedItems.Count == 0) {
				return;
			}

			// first selected row
			DataRowView dataRow = (DataRowView)e.AddedItems[0];
			int userID = (int)dataRow["ID"];

			EditUser.UserID = userID;
			EditUser.Visibility = Visibility.Visible;

		}

		private void DgUsers_KeyDown(object sender, KeyEventArgs e) {

			switch (e.Key) {

				case Key.Escape:
					DgUsers.UnselectAll();
					EditUser.Visibility = Visibility.Collapsed;
					break;

			}

		}

		#endregion

		#region Methods

		#region Private methods

		private void LoadData() {

			string query = @"SELECT ID, Name, Email FROM dbo.Users;";
			SqlCommand sqlCommand = new SqlCommand(query);

			DataTable dataTableEmployees = new DataTable();
			string queryResult = App.Database.FillDataTable(ref dataTableEmployees, sqlCommand);

			if (!queryResult.Equals("OK", StringComparison.Ordinal)) {
				return;
			}

			DgUsers.ItemsSource = dataTableEmployees.DefaultView;

		}

		private void DeleteUser(int userID) {

			if (userID <= 0) {
				return;
			}

			string query =
@"DECLARE @ID int = @pID;

DECLARE @Result nvarchar(MAX);
EXEC dbo.DeleteUser @ID, @Result OUTPUT;

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pID", SqlDbType.Int).Value = userID;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK", StringComparison.Ordinal)) {
				_ = MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			EditUser.Visibility = Visibility.Collapsed;
			LoadData();

		}

		#endregion

		#endregion

	}
}
