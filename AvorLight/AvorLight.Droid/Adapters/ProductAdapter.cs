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
using Android.Views.Animations;

namespace AvorLight.Droid.Adapters
{
    class ProductAdapter : BaseAdapter
    {
        List<Product> _products;
        Activity _activity;

        bool _isSortedByTitleAscending = true,
             _isSortedByPriceAscending = false;

        public ProductAdapter(Activity activity, IDataService dataService, int categoryId)
        {
            _products = dataService.GetProduct(categoryId).ToList();
            _activity = activity;
        }
        
        public override int Count => _products.Count;

        public override Java.Lang.Object GetItem(int position) => null;

        public override long GetItemId(int position) => _products[position].ID;

        public Product GetItemModel(int position) => _products[position];

        public void ToggleSortByTitle()
        {
            // Toggle Sort
            _isSortedByTitleAscending = !_isSortedByTitleAscending;

            // Sort Accordingly
            if (_isSortedByTitleAscending)
                _products = _products.OrderBy(s => s.Title).ToList();
            else
                _products = _products.OrderByDescending(s => s.Title).ToList();
            
            // Refresh Adapter
            NotifyDataSetChanged();
        }

        public void ToggleSortByPrice()
        {
            // Toggle Sort
            _isSortedByPriceAscending = !_isSortedByPriceAscending;

            // Sort Accordingly
            if (_isSortedByPriceAscending)
                _products = _products.OrderBy(s => s.Price).ToList();
            else
                _products = _products.OrderByDescending(s => s.Price).ToList();

            // Refresh Adapter
            NotifyDataSetChanged();
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.ProductItem, parent, false);

            var entry = _products[position];

            view.FindViewById<TextView>(Resource.Id.titleTextView).Text = entry.Title;
            view.FindViewById<TextView>(Resource.Id.subtitleTextView).Text = entry.Subtitle;
            view.FindViewById<TextView>(Resource.Id.priceTextView).Text = entry.Price?.ToString("€ 0.##") ?? "FREE";

            // load image as Drawable
            var ims = _activity.Assets.Open(entry.ImagePath);

            // set image to ImageView
            view.FindViewById<ImageView>(Resource.Id.imageView).SetImageDrawable(Android.Graphics.Drawables.Drawable.CreateFromStream(ims, null));

            // Apply entrance animation
            var animation = AnimationUtils.LoadAnimation(_activity, Resource.Animation.enter_left);
            view.StartAnimation(animation);

            return view;
        }
    }
}