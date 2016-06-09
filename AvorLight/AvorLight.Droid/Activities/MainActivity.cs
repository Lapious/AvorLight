
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace AvorLight.Droid.Activities
{
	[Activity]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{

			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Sets up the database for the first time (if needed)
			Data.AvorDB.SetupDatabaseIfNeeded(Assets);
			
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			toolbar.Title = "Avor";
			//Toolbar will now take on default Action Bar characteristics
			SetSupportActionBar(toolbar);

			var db = new Data.AvorDB();


			var adapter = new Adapters.MainMenuAdapter(this, db);
			var listView = FindViewById<ListView>(Resource.Id.myListView);
			listView.Adapter = adapter;
			listView.ItemClick += (s, e) =>
			{
				var entry = adapter.GetItemModel(e.Position);

				var intent = new Intent(this, typeof(ProductActivity));

				intent.PutExtra(ProductActivity.CategoryIDKey, entry.ID);
				intent.PutExtra(ProductActivity.CategoryTitleKey, entry.Title);

				StartActivity(intent);
			};
		}
	}
}


