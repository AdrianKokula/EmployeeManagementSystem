﻿// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System;
using System.Collections.Generic;
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

		private void BtnRemoveRecords_Click(object sender, RoutedEventArgs e) {
			DeleteSelectedRecords();
		}

		private void DgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			switch (DgUsers.SelectedItems.Count) {
				case 0:
					BtnRemoveRecords.Visibility = Visibility.Collapsed;
					EditUser.Visibility = Visibility.Collapsed;
					break;

				case 1:
					BtnRemoveRecords.Visibility = Visibility.Visible;
					ShowSelectedRecord();
					break;

				default:
					BtnRemoveRecords.Visibility = Visibility.Visible;
					EditUser.Visibility = Visibility.Collapsed;
					break;
			}

		}

		private void DgUsers_PreviewKeyDown(object sender, KeyEventArgs e) {

			switch (e.Key) {

				case Key.Escape:
					DgUsers.UnselectAll();
					EditUser.Visibility = Visibility.Collapsed;
					break;

				case Key.Delete:
					DeleteSelectedRecords();
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

		private void DeleteSelectedRecords() {

			if (DgUsers.SelectedItems.Count <= 0) {
				return;
			}

			List<DataRowView> list = new List<DataRowView>(DgUsers.SelectedItems.Count);

			foreach (DataRowView selectedRow in DgUsers.SelectedItems) {
				list.Add(selectedRow);
			}

			list.ForEach(row => {
				int userID = (int)row["ID"];
				DeleteUser(userID);
			});

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

		private void ShowSelectedRecord() {

			if (DgUsers.SelectedItems.Count <= 0) {
				return;
			}

			// first selected row
			DataRowView selectedRow = (DataRowView)DgUsers.SelectedItems[0];
			int userID = (int)selectedRow["ID"];

			EditUser.UserID = userID;
			EditUser.Visibility = Visibility.Visible;

		}

		#endregion

		#endregion

	}
}
