using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


namespace Salary
{
    [Activity(Label = "Дневник")]
    public class Activity_view : Activity
    {
        ListView lv;
        TextView tvMax;
        Spinner sp;
        int num = 0;
        List<string> lstMont = new List<string>()
        {
            "По умолчанию",
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"
        };

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                foreach (var item in MainActivity.dictJson)
                {
                    if (item.Key == num)
                    {
                        item.Value[numPos].sum = data.GetDoubleExtra("summa", 0);
                        MetodShow();
                        Toast.MakeText(this, "Изменения соханены", ToastLength.Long).Show();
                    }
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_view);
            // Create your application here
            lv = FindViewById<ListView>(Resource.Id.listView1);
            tvMax = FindViewById<TextView>(Resource.Id.textViewMax);
            sp = FindViewById<Spinner>(Resource.Id.spinnerMonth);
            sp.ItemSelected += Sp_ItemSelected;
            sp.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lstMont);

            lv.ItemLongClick += Lv_ItemLongClick; ;
            MetodShow();
        }

        private void Lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            EditList(e.Position);

        }
        internal static double sum;
        internal static int numPos;
        internal static int numpos;
        internal static DateTime dt;
        internal static bool edit;
        private void EditList(int position)
        {
            if (position == -1)
                return;
            // описать разрешение изменения только в этот день

            foreach (var item in MainActivity.dictJson)
            {
                if (num == 0)
                    num = DateTime.Now.Month;
                if (item.Key == num)
                {
                    if (DateTime.Now.Day != item.Value[position].dt.Day)
                        return;
                    sum = item.Value[position].sum;
                    dt = item.Value[position].dt;
                    numPos = position;
                    numpos = num;
                }
            }

            Intent actEdit = new Intent(this, typeof(ActivityEdit));

            StartActivityForResult(actEdit, 0);


        }


        private void Sp_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            num = e.Position;
            MetodShow();
        }

        private void MetodShow()
        {
            int month;
            month = num;
            if (month == 0)
                month = DateTime.Now.Month;
            foreach (var item in MainActivity.dictJson)
            {
                if (item.Key == month)
                {
                    var lst = new List<string>();
                    double summa = 0;
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        if (item.Value[i].sum == 0 && item.Value.Count > 1)
                            item.Value.RemoveAt(i);
                        lst.Add(item.Value[i].dt.ToLongDateString() + " - " + item.Value[i].sum + " р.");
                        summa += item.Value[i].sum;
                    }
                    tvMax.Text = "Зарплата за " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Key) + ": " + summa;
                    if (summa != 0)
                        lv.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, lst);
                    else
                        lv.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, new List<string>() { });

                    lv.ChoiceMode = ChoiceMode.Single;
                }
            }
        }
    }
}