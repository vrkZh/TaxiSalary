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
            if (edt.Text.Contains("."))
                edt.Text = edt.Text.Replace(".", ",");
            try
            {
                //Activity_view.sum = double.Parse(edt.Text);
                //Activity_view.edit = true;

                ////foreach (var item in MainActivity.dictJson)
                ////{
                ////    if (item.Key == Activity_view.numpos)
                ////    {
                ////        item.Value[Activity_view.numPos].sum = double.Parse(edt.Text); 

                ////    }
                ////}
                Intent actView = new Intent(this, typeof(Activity_view));
                actView.PutExtra("summa", double.Parse(edt.Text));
                SetResult(Result.Ok, actView);
                Finish();
            }
            catch
            {
                new Android.App.AlertDialog.Builder(this).
                 SetTitle("Внимание").
                 SetMessage("Ошибка, данные указаны не верно!").
                 SetNegativeButton("Ок", delegate { }
                 ).Show();               
            }
        }
    }
}