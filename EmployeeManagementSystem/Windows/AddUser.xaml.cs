﻿// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

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
using System.Windows.Shapes;
using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem.Windows {

	/// <summary>
	/// Interaction logic for AddUser.xaml
	/// </summary>
	public partial class AddUser : Window {

		private readonly Database database;

		private int userID;

		#region "constructors"

		public AddUser(Database database) : this(database, 0) {}

		public AddUser(Database database, int userID) {

			InitializeComponent();
			this.database = database;
			this.userID = userID;

		}

		#endregion

		#region "handlers"

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {

			CreateUser();

		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		#endregion

		#region "methods"

		#region "private"

		private bool ValidateFields() {

			if (string.IsNullOrWhiteSpace(TbName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbEmail.Text)) return false;
			if (string.IsNullOrWhiteSpace(PbPassword.Password)) return false;

			return true;
		}

		private void CreateUser() {

			if (!ValidateFields()) return;



		}

		#endregion

		#endregion


	}
}
