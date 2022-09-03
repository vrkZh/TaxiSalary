using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salary
{
    [Activity(Label = "Редактирование записи")]
    public class ActivityEdit : Activity
    {
        Button btn;
        EditText edt;
        TextView tv;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_Edit);
            // Create your application here
            btn = FindViewById<Button>(Resource.Id.buttonEdit);
            edt= FindViewById<EditText>(Resource.Id.editTextEdit);
            tv = FindViewById<TextView>(Resource.Id.textView1);

            tv.Text = $"Зарплата за {Activity_view.dt.ToLongDateString()}: ";
            edt.Text = Activity_view.sum.ToString();

            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            // описать проверку 
            Activity_view.sum = double.Parse(edt.Text);
            Activity_view.edit = true;
            Finish();
        }
    }
}