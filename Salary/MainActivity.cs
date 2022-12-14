using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Salary
{
    [Activity(Label = "Моя Зарплата", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnSave, btnView;
        ImageView ivTaxi;
        EditText edtSumma;

        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "dictionaryNw.json");
        string fName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/YearFile.xml";
        internal static XmlSerializer xs = new XmlSerializer(typeof(string));

        internal static Dictionary<int, List<Data>> dictJson = new Dictionary<int, List<Data>>();

        string dateIsFile;
        bool flag;

        public class Data
        {
            public double sum { get; set; }
            public DateTime dt { get; set; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            btnSave = FindViewById<Button>(Resource.Id.buttonSave);
            btnView = FindViewById<Button>(Resource.Id.buttonSumma);
            ivTaxi = FindViewById<ImageView>(Resource.Id.imageViewTaxi);
            edtSumma = FindViewById<EditText>(Resource.Id.editTextCount);
            ivTaxi.SetImageResource(Resource.Drawable.taxi);
            
            btnSave.Click += BtnSave_Click;
            btnView.Click += BtnView_Click;
            
            // если существует файл c датой
            if (File.Exists(fName))
            {
                // прочитать файл
                using (FileStream fs = new FileStream(fName, FileMode.OpenOrCreate))
                {
                    dateIsFile = xs.Deserialize(fs) as string;
                    // сверить дату нынешнию и прошедшую и если не равна
                    if (dateIsFile != DateTime.Now.Year.ToString())
                    {
                        // записать новый год и очистить данные по прошлому году
                        dictJson.Clear();
                        File.Delete(path);
                        fs.Close();
                        SaveYear();

                    }
                }
            }
            // если не существует - записать текущую дату и сохранить
            else
            {
                SaveYear();
            }
            // поулчить и обновить данные
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                dictJson = JsonConvert.DeserializeObject<Dictionary<int, List<Data>>>(json);
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    dictJson.Add(i, new List<Data>() { new Data() { sum = 0, dt = new DateTime(DateTime.Now.Year,i,1)} });
                }
            }
        }

        private void SaveYear()
        {
            dateIsFile = DateTime.Now.Year.ToString();
            using (FileStream fs = new FileStream(fName, FileMode.OpenOrCreate))
            {
                xs.Serialize(fs, dateIsFile);
            }
        }


        private void dataReader()
        {
            try
            {
                string json = File.ReadAllText(path);
                dictJson = JsonConvert.DeserializeObject<Dictionary<int, List<Data>>>(json);
            }
            catch
            {
                new Android.App.AlertDialog.Builder(this).
                 SetTitle("Внимание").
                 SetMessage("Не предвиденная ошибка!").
                 SetNegativeButton("Ок", delegate { }).
                 Show();
                return;
            }
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            edtSumma.Text = "";
            Intent actView = new Intent(this, typeof(Activity_view));
            StartActivity(actView);
        }

        private void BtnSave_Click(object sender, System.EventArgs e)
        {

            new Android.App.AlertDialog.Builder(this).
             SetTitle("Внимание").
             SetMessage("Сумма указана верно?").
             SetNegativeButton("Да", delegate { SaveMetod(); })
             .SetPositiveButton("Нет", delegate { }).
             Show();
        }

        private void SaveMetod()
        {
            if (edtSumma.Text.Contains("."))
                edtSumma.Text = edtSumma.Text.Replace(".", ",");
            try
            {
                foreach (var item in dictJson)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        if (item.Value[i].dt.Day == DateTime.Now.Day && item.Key != 0)
                        {
                            item.Value[i].sum += double.Parse(edtSumma.Text);
                            flag = true;
                        }
                    }
                }
                if (!flag)
                    dictJson[DateTime.Now.Month].Add(new Data() { sum = double.Parse(edtSumma.Text), dt = DateTime.Now });
                string json = JsonConvert.SerializeObject(dictJson, Formatting.Indented);
                edtSumma.Text = "";
                File.WriteAllText(path, json);
                new Android.App.AlertDialog.Builder(this).
                 SetTitle("Внимание").
                 SetMessage("Данные успешно сохранены.").
                 SetNegativeButton("Хорошо", delegate { }
                 ).Show();

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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}