using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Content;

namespace AvorLight.Droid.Activities
{
    [Activity]
    public class ProductActivity : AppCompatActivity
    {
        public const string CategoryIDKey = "CategoryID",
                            CategoryTitleKey = "CategoryTitle";

        Adapters.ProductAdapter _adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the layout resource
            SetContentView(Resource.Layout.Category);


            var categoryId = Intent.GetIntExtra(CategoryIDKey, 0);
            var categoryTitle = Intent.GetStringExtra(CategoryTitleKey);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.Title = categoryTitle;
            //Toolbar will now take on default Action Bar characteristics
            SetSupportActionBar(toolbar);

            var rootLayout = FindViewById<LinearLayout>(Resource.Id.rootLayout);

            var db = new Data.AvorDB();

            _adapter = new Adapters.ProductAdapter(this, db, categoryId);
            var listView = FindViewById<ListView>(Resource.Id.myListView);
            listView.Adapter = _adapter;
            listView.ItemClick += (s, e) =>
             {
                 var entry = _adapter.GetItemModel(e.Position);

                 db.AddCartProduct(entry);

                 // TODO: Go to my shopping item page...
                 Snackbar.Make(rootLayout, $"Added {entry.Title}", Snackbar.LengthShort)
                         .SetAction("UNDO", (view) => { db.DeleteCartProduct(entry); })
                         .Show(); /* Don't forget to show it */

             };
        }

        /// <Docs>The options menu in which you place your items.</Docs>
        /// <returns>To be added.</returns>
        /// <summary>
        /// This is the menu for the Toolbar/Action Bar to use
        /// </summary>
        /// <param name="menu">Menu.</param>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.category_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId)
            {
                case Resource.Id.menuShoppingCart:
                    var intent = new Intent(this, typeof(CartActivity));
                    StartActivity(intent);
                    break;

                case Resource.Id.menuSortByTitle:
                    _adapter.ToggleSortByTitle();
                    break;

                case Resource.Id.menuSortByPrice:
                    _adapter.ToggleSortByPrice();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}