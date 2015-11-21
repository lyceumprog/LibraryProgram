
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;

namespace LibraryProgramm
{
	[Activity (Label = "BookReadActivity")]			
	public class BookReadActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.BookReadLayout);

			string book_name = Intent.GetStringExtra ("book_name"); // Получение названия книги с прошлого окна

			string dbLocalPath = System.IO.Path.Combine(System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal),"libraryLocal.sqlite"
			);

			SqliteConnection connectionLocal = new SqliteConnection ("Data source=" + dbLocalPath);
			SqliteCommand commandLocal = connectionLocal.CreateCommand();
			SqliteDataReader readerLocal;

			connectionLocal.Open();
			commandLocal.CommandText = "INSERT INTO " +
				"last_read(open_date,book_name,last_page_number) " +
				"VALUES ('5469','" + book_name + "',638)";
			commandLocal.ExecuteNonQuery (); // ДОБАВЛЕНИЕ В БД ИНФОРМАЦИИ О ПОСЛЕДНЕЙ ОТКРЫТОЙ КНИГЕ

			connectionLocal.Close();
		}
	}
}

