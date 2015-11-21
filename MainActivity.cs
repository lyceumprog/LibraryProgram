using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Mono.Data.Sqlite;

namespace LibraryProgramm
{
	[Activity (Label = "LibraryProgramm", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			new DatabaseCopy (this); // Вызов копирования БД из Assets во внутреннюю память телефона

			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			ChangeLastReadTextView (); // Вызов функции изменения текста рубрики Последняя прочитанная книга

			string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal),"library.sqlite"
			); // Получение пути к файлу БД во внутренней памяти телефона

			SqliteConnection connection = new SqliteConnection ("Data source=" + dbPath); // Подключение к БД
			SqliteCommand command = connection.CreateCommand (); // Создание переменной для составления запросов к БД
			SqliteDataReader reader; // Переменная получения входного потока данных из БД

			List<string> book_name = new List<string> (); // Список названий книг

			// ------------------ ПОЛУЧЕНИЕ НАЗВАНИЙ КНИГ
			connection.Open ();
			command.CommandText = "SELECT name FROM book";
			reader = command.ExecuteReader ();
			while (reader.Read ()) {
				book_name.Add (reader [0].ToString ());
			}
			reader.Close ();
			connection.Close ();

			//------------------- ПОЛУЧЕНИЕ НАЗВАНИЙ КНИГ


			// ------------------СОЗДАНИЕ ИНТЕРФЕЙСА ИЗ ИСХОДНОГО КОДА
			LinearLayout mainLayout = 
				FindViewById<LinearLayout> 
				(Resource.Id.linearLayout2);

			for (int i = 0; i < book_name.Count / 2 + book_name.Count % 2; i++) {
				LinearLayout newLinearLayout = 
					new LinearLayout (this);
				newLinearLayout.Orientation = 
					Orientation.Horizontal;

				for (int j = 0; j < 2; j ++) {
					if (i * 2 + j < book_name.Count) {
						Button newButton = new Button (this);
						newButton.Text = book_name [i * 2 + j];
						newButton.Id = i * 2 + j;

						if ((i * 2 + j+1)%3 == 0)
							newButton.SetBackgroundResource (Resource.Drawable.book1);
						else if ((i*2 + j +1)%2 ==0)
							newButton.SetBackgroundResource (Resource.Drawable.book2);
						else
							newButton.SetBackgroundResource (Resource.Drawable.book3);

						LinearLayout.LayoutParams buttonParameters = 
							new LinearLayout.LayoutParams (100, 100);
						buttonParameters.SetMargins
						(0, 0, 5, 5);

						newButton.LayoutParameters = buttonParameters;

						newButton.Click += (object sender, EventArgs e) => 
						{
							Intent intent = new Intent(this,typeof(BookReadActivity));

							intent.PutExtra("book_name", book_name [(sender as Button).Id]); 
							StartActivity(intent);
						};

						newLinearLayout.AddView (newButton);
					}
				}

				mainLayout.AddView (newLinearLayout);
			}
			//-------------------------- СОЗДАНИЕ ИНТЕРФЕСА ИЗ ИСХОДНОГО КОДА
		}

		void ChangeLastReadTextView()
		{
			string dbLocalPath = System.IO.Path.Combine(System.Environment.GetFolderPath
				(System.Environment.SpecialFolder.Personal),"libraryLocal.sqlite"
			); // Путь к локальному файлу БД во внутренней памяти телефона

			SqliteConnection connectionLocal = new SqliteConnection ("Data source=" + dbLocalPath);
			SqliteCommand commandLocal = connectionLocal.CreateCommand();
			SqliteDataReader readerLocal;

			//-----------------------ПОЛУЧЕНИЕ НАЗВАНИЯ ПОСЛЕДНЕЙ ПРОЧИТАННОЙ КНИГИ
			connectionLocal.Open();
			commandLocal.CommandText = "SELECT book_name,last_page_number FROM last_read";
			readerLocal = commandLocal.ExecuteReader ();

			if (readerLocal.HasRows) {
				while (readerLocal.Read ()) {
					FindViewById<TextView> (Resource.Id.textView1).Text = readerLocal [0].ToString ();
				}
			}
			readerLocal.Close ();

			connectionLocal.Close();
			//----------------------ПОЛУЧЕНИЕ НАЗВАНИЯ ПОСЛЕДНЕЙ ПРОЧИТАННОЙ КНИГИ
		}

		protected override void OnResume () // ФУНКЦИЯ, ВЫЗЫВАЕМАЯ ПРИ ВОЗОБНОВЛЕНИИ РАБОТЫ ОКНА
		{
			base.OnResume ();

			ChangeLastReadTextView ();
		}
	}
}