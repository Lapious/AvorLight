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
    class CartAdapter : BaseAdapter
    {
        List<CartProduct> _products;
        Activity _activity;
        IDataService _dataService;

        public CartAdapter(Activity activity, IDataService dataService)
        {
            _products = dataService.GetCartProduct().ToList();
            _activity = activity;
            _dataService = dataService;
        }
        
        public override int Count => _products.Count;

        public override Java.Lang.Object GetItem(int position) => null;

        public override long GetItemId(int position) => _products[position].ID;

        public void DeleteItem(int position)
        {
            var entry = _products[position];

            _dataService.DeleteCartProduct(entry);

            _products.RemoveAt(position);

            NotifyDataSetChanged();
        }

        public void ClearItems()
        {
            _products.Clear();
            NotifyDataSetChanged();
        }

        public float GetSum() => _products.Sum(p => p.Price ?? 0);

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.CartProductItem, parent, false);

            var entry = _products[position];

            view.FindViewById<TextView>(Resource.Id.titleTextView).Text = entry.Title;
            view.FindViewById<TextView>(Resource.Id.subtitleTextView).Text = entry.Subtitle;
            view.FindViewById<TextView>(Resource.Id.priceTextView).Text = entry.Price?.ToString("€ 0.##") ?? "FREE";

            // load image as Drawable
            var ims = _activity.Assets.Open(entry.ImagePath);

            // set image to ImageView
            view.FindViewById<ImageView>(Resource.Id.imageView).SetImageDrawable(Android.Graphics.Drawables.Drawable.CreateFromStream(ims, null));

            return view;
        }
    }
}