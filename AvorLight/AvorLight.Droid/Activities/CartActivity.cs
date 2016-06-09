using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace AvorLight.Droid.Activities
{
    [Activity()]
    public class CartActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the layout resource
            SetContentView(Resource.Layout.MyCart);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "My Shopping Cart";
            //Toolbar will now take on default Action Bar characteristics
            SetSupportActionBar(toolbar);

            var rootLayout = FindViewById<LinearLayout>(Resource.Id.rootLayout);

            var db = new Data.AvorDB();

            var adapter = new Adapters.CartAdapter(this, db);
            var listView = FindViewById<ListView>(Resource.Id.myListView);
            listView.Adapter = adapter;
            listView.ItemClick += (s, e) =>
             {
                 adapter.DeleteItem(e.Position);
             };

            var checkoutBtn = FindViewById<Button>(Resource.Id.checkoutBtn);
            checkoutBtn.Click += (s, e) =>
            {
                if (adapter.Count == 0)
                    Toast.MakeText(this, "Please select at least 1 product", ToastLength.Short)
                         .Show();
                else
                {



                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Confirm")
                           .SetMessage("Are you sure you want to checkout?\n"+
                                       $"Total : € {adapter.GetSum().ToString("0.##")}")
                           .SetPositiveButton("Yes", delegate
                            {
                                db.Checkout();

                                adapter.ClearItems();

                                Snackbar.Make(rootLayout, "Successfully checkedout", Snackbar.LengthShort)
                                        .Show();
                            })
                           .SetNegativeButton("No", delegate { })
                           .Show();
                }
            };
        }
    }
}