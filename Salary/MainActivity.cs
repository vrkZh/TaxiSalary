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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnSave, btnView;
        ImageView ivTaxi;
        EditText edtSumma;

        internal static string fName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/YearFile.xml";

        internal static XmlSerializer xs = new XmlSerializer(typeof(string));
        internal static Dictionary<int, List<double>> montZarplata = new Dictionary<int, List<double>>();


        string dateIsFile;
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
                        montZarplata.Clear();
                        File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/dictionaryZP.json");
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
            if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/dictionaryZP.json"))
            {
                string json = File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/dictionaryZP.json");
                montZarplata = JsonConvert.DeserializeObject<Dictionary<int, List<double>>>(json);
            }
            else
            {
                montZarplata.Add(1, new List<double> { 0 });
                montZarplata.Add(2, new List<double> { 0 });
                montZarplata.Add(3, new List<double> { 0 });
                montZarplata.Add(4, new List<double> { 0 });
                montZarplata.Add(5, new List<double> { 0 });
                montZarplata.Add(6, new List<double> { 0 });
                montZarplata.Add(7, new List<double> { 0 });
                montZarplata.Add(8, new List<double> { 0 });
                montZarplata.Add(9, new List<double> { 0 });
                montZarplata.Add(10, new List<double> { 0 });
                montZarplata.Add(11, new List<double> { 0 });
                montZarplata.Add(12, new List<double> { 0 });
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
            // Если СУЩЕСТВУЕТ УЖЕ ДАННЫЕ (1-12)
            try
            {
                string json = File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/dictionaryZP.json");
                montZarplata = JsonConvert.DeserializeObject<Dictionary<int, List<double>>>(json);
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

            

            montZarplata[DateTime.Now.Month].Add(double.Parse(edtSumma.Text));
            //try
            //{
            //FileStream file = new FileStream(fName2, FileMode.OpenOrCreate);
            //file.Close();

            string json = JsonConvert.SerializeObject(montZarplata, Formatting.Indented);

            File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/dictionaryZP.json", json);
            new Android.App.AlertDialog.Builder(this).
             SetTitle("Внимание").
             SetMessage("Данные успешно сохранены.").
             SetNegativeButton("Хорошо", delegate { }
             ).Show();



            //dataReader();
            //}
            //catch
            //{
            //    new Android.App.AlertDialog.Builder(this).
            //     SetTitle("Внимание").
            //     SetMessage("Ошибка.").
            //     SetNegativeButton("Ок", delegate { }
            //     ).Show();
            //    dataReader();
            //}
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}