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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_view);
            // Create your application here
            lv = FindViewById<ListView>(Resource.Id.listView1);
            tvMax = FindViewById<TextView>(Resource.Id.textViewMax);
            sp = FindViewById<Spinner>(Resource.Id.spinnerMonth);
            sp.ItemSelected += Sp_ItemSelected;
            ///
            /// Нужно подгрузить в Cmb месяца и по умолчанию текущий месяц
            ///
            sp.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lstMont);
            MetodShow(true);
                      
           
            
        }

        private void Sp_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            num = e.Position;
            MetodShow();
        }

        private void MetodShow(bool flag=false)
        {
           
                int month;
            //if (flag)
            //    month = DateTime.Now.Month;
            //else
            //    // выбранный елемент cmb
            //    month = 1;
            month = num;
            if (month == 0)
                month = DateTime.Now.Month;
            foreach (var item in MainActivity.montZarplata)
            {
                if (item.Key == month)
                {
                    tvMax.Text = "Зарплата за " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Key) + ": " + item.Value.Sum();
                    // В чем ошибка?
                     lv.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, item.Value);
                    lv.ChoiceMode = ChoiceMode.Single;
                }
            }
        }
    }
}