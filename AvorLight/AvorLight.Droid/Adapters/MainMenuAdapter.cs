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
using Java.Lang;
using AvorLight.Data;
using Android.Graphics;

namespace AvorLight.Droid.Adapters
{
    class MainMenuAdapter : BaseAdapter
    {
        List<ProductCategory> _categories;
        Activity _activity;

        public MainMenuAdapter(Activity activity, IDataService dataService)
        {
            _categories = dataService.GetProductCategory().ToList();
            _activity = activity;
        }

        public override int Count => _categories.Count;

        public override Java.Lang.Object GetItem(int position) => null;

        public override long GetItemId(int position) => _categories[position].ID;

        public ProductCategory GetItemModel(int position) => _categories[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.CategoryItem, parent, false);

            var entry = _categories[position];

            // Set dominant color overlay
            view.FindViewById<View>(Resource.Id.colorOverlayView).SetBackgroundColor(Color.ParseColor(entry.DominantColor));

            // Set entry title
            view.FindViewById<TextView>(Resource.Id.textView).Text = entry.Title;

            // Set background Image
            var imageId = _activity.Resources.GetIdentifier("category_" + entry.ImageName, "drawable", _activity.PackageName);
            view.FindViewById<ImageView>(Resource.Id.imageView).SetImageResource(imageId);
            
            return view;
        }
    }
}