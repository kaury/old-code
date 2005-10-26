//
// Mono.Data.PostgreSqlClient.PgSqlDataAdapter.cs
//
// Author:
//   Rodrigo Moya (rodrigo@ximian.com)
//   Daniel Morgan (danmorg@sc.rr.com)
//   Tim Coleman (tim@timcoleman.com)
//
// (C) Ximian, Inc 2002
// Copyright (C) 2002 Tim Coleman
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

namespace Mono.Data.PostgreSqlClient
{
	/// <summary>
	/// Represents a set of command-related properties that are used 
	/// to fill the DataSet and update a data source, all this 
	/// from a SQL database.
	/// </summary>
	public sealed class PgSqlDataAdapter : DbDataAdapter, IDbDataAdapter 
	{
		#region Fields
	
		PgSqlCommand deleteCommand;
		PgSqlCommand insertCommand;
		PgSqlCommand selectCommand;
		PgSqlCommand updateCommand;

		static readonly object EventRowUpdated = new object(); 
		static readonly object EventRowUpdating = new object(); 

		#endregion

		#region Constructors
		
		public PgSqlDataAdapter () 	
			: this (new PgSqlCommand ())
		{
		}

		public PgSqlDataAdapter (PgSqlCommand selectCommand) 
		{
			DeleteCommand = new PgSqlCommand ();
			InsertCommand = new PgSqlCommand ();
			SelectCommand = selectCommand;
			UpdateCommand = new PgSqlCommand ();
		}

		public PgSqlDataAdapter (string selectCommandText, PgSqlConnection selectConnection) 
			: this (new PgSqlCommand (selectCommandText, selectConnection))
		{ 
		}

		public PgSqlDataAdapter (string selectCommandText, string selectConnectionString)
			: this (selectCommandText, new PgSqlConnection (selectConnectionString))
		{
		}

		#endregion

		#region Properties

		public PgSqlCommand DeleteCommand {
			get {
				return deleteCommand;
			}
			set {
				deleteCommand = value;
			}
		}

		public PgSqlCommand InsertCommand {
			get {
				return insertCommand;
			}
			set {
				insertCommand = value;
			}
		}

		public PgSqlCommand SelectCommand {
			get {
				return selectCommand;
			}
			set {
				selectCommand = value;
			}
		}

		public PgSqlCommand UpdateCommand {
			get {
				return updateCommand;
			}
			set {
				updateCommand = value;
			}
		}

		IDbCommand IDbDataAdapter.DeleteCommand {
			get { return DeleteCommand; }
			set { 
				if (!(value is PgSqlCommand)) 
					throw new ArgumentException ();
				DeleteCommand = (PgSqlCommand)value;
			}
		}

		IDbCommand IDbDataAdapter.InsertCommand {
			get { return InsertCommand; }
			set { 
				if (!(value is PgSqlCommand)) 
					throw new ArgumentException ();
				InsertCommand = (PgSqlCommand)value;
			}
		}

		IDbCommand IDbDataAdapter.SelectCommand {
			get { return SelectCommand; }
			set { 
				if (!(value is PgSqlCommand)) 
					throw new ArgumentException ();
				SelectCommand = (PgSqlCommand)value;
			}
		}

		IDbCommand IDbDataAdapter.UpdateCommand {
			get { return UpdateCommand; }
			set { 
				if (!(value is PgSqlCommand)) 
					throw new ArgumentException ();
				UpdateCommand = (PgSqlCommand)value;
			}
		}


		ITableMappingCollection IDataAdapter.TableMappings {
			get { return TableMappings; }
		}

		#endregion // Properties

		#region Methods

		protected override RowUpdatedEventArgs CreateRowUpdatedEvent (DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) 
		{
			return new PgSqlRowUpdatedEventArgs (dataRow, command, statementType, tableMapping);
		}


		protected override RowUpdatingEventArgs CreateRowUpdatingEvent (DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) 
		{
			return new PgSqlRowUpdatingEventArgs (dataRow, command, statementType, tableMapping);
		}

		protected override void OnRowUpdated (RowUpdatedEventArgs value) 
		{
         		PgSqlRowUpdatedEventHandler handler = (PgSqlRowUpdatedEventHandler) Events[EventRowUpdated];
			if ((handler != null) && (value is PgSqlRowUpdatedEventArgs))
            			handler(this, (PgSqlRowUpdatedEventArgs) value);
		}

		protected override void OnRowUpdating (RowUpdatingEventArgs value) 
		{
         		PgSqlRowUpdatingEventHandler handler = (PgSqlRowUpdatingEventHandler) Events[EventRowUpdating];
			if ((handler != null) && (value is PgSqlRowUpdatingEventArgs))
            			handler(this, (PgSqlRowUpdatingEventArgs) value);
		}

		#endregion // Methods

		#region Events and Delegates

		public event PgSqlRowUpdatedEventHandler RowUpdated {
			add { Events.AddHandler (EventRowUpdated, value); }
			remove { Events.RemoveHandler (EventRowUpdated, value); }
		}

		public event PgSqlRowUpdatingEventHandler RowUpdating {
			add { Events.AddHandler (EventRowUpdating, value); }
			remove { Events.RemoveHandler (EventRowUpdating, value); }
		}

		#endregion // Events and Delegates

	}
}