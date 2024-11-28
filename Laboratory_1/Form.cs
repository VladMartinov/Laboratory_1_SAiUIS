using Laboratory_1.Classes;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laboratory_1
{
    public partial class Form : System.Windows.Forms.Form
    {
        #region - Variabled -
        private readonly ExcelReader ExcelReader;
        private readonly MGUA MGUA;

        private bool isDataSet;
        private bool isTargetDataSet;
        #endregion

        public Form()
        {
            InitializeComponent();

            ExcelReader = new ExcelReader();
            MGUA = new MGUA();

            buttonStart.Enabled = false;
            buttonPredict.Enabled = false;

            isDataSet = false;
            isTargetDataSet = false;
        }

        private void ButtonSelectData_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            // Задание нового пути к файлу для объекта ExcleReader
            ExcelReader.SetPath(openFileDialog.FileName);
            ExcelReader.SetTarget(false);
            ExcelReader.ReadFile();

            // Установка данных в класс МГУА
            MGUA.SetData(ExcelReader.GetData());

            // Установка флага загруженных данных
            isDataSet = true;
            
            // Включение кнопки чтения файла если все данные загружены
            if (isTargetDataSet) buttonStart.Enabled = true;
        }

        private void ButtonSelectTarget_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            // Задание нового пути к файлу для объекта ExcleReader и чтение файла
            ExcelReader.SetPath(openFileDialog.FileName);
            ExcelReader.SetTarget(true);
            ExcelReader.ReadFile();

            // Установка данных в класс МГУА
            MGUA.SetTargetData(ExcelReader.GetTargetData());

            // Установка флага загруженных данных
            isTargetDataSet = true;

            // Включение кнопки чтения файла если все данные загружены
            if (isDataSet) buttonStart.Enabled = true;
        }

        private void ButtonStart_Click(object sender, System.EventArgs e)
        {
            // Начало обучения моделей
            MGUA.Start();

            // Заполнение экранных элементов значениями
            FillListBoxs();

            // Включение кнопки предсказания значений
            buttonPredict.Enabled = true;
        }

        private void ButtonPredict_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            // Задание нового пути к файлу для объекта ExcleReader и чтение файла
            ExcelReader.SetPath(openFileDialog.FileName);
            ExcelReader.SetTarget(false);
            ExcelReader.ReadFile();

            // Установка данных в класс МГУА
            MGUA.SetPredictData(ExcelReader.GetData());

            // Предсказания значений на основе загруженных данных
            MGUA.Predict();

            // Отображение резульатов предсказания
            FillPredictListBoxs();
        }

        private void ButtonClear_Click(object sender, System.EventArgs e)
        {
            // Очистка данных
            MGUA.SetData(new List<ExcelDataObject>());
            MGUA.SetTargetData(new List<ExcelTargetObject>());
            MGUA.SetPredictData(new List<ExcelDataObject>());

            MGUA.ClearModels();
            MGUA.ClearPredict();

            // Блокировка кнопок
            buttonStart.Enabled = false;
            buttonPredict.Enabled = false;

            // Очистка информации о загрузке данных
            isDataSet = false;
            isTargetDataSet = false;

            // Очистка экранных элементов
            listBoxDefault.Items.Clear();
            listBoxKolmogorovGabor.Items.Clear();
        }

        private void FillListBoxs()
        {
            // Очистка экранных элементов
            listBoxDefault.Items.Clear();
            listBoxKolmogorovGabor.Items.Clear();

            // Получение обученных моделей
            List<Model> listDefault = MGUA.GetModelsDefault();
            List<string> listDefaultText = MGUA.GetModelsDefaultText();
            
            List<Model> listKolmogorovGabor = MGUA.GetModelsKolmogorovaGabor();
            List<string> listKolmogorovGaborText = MGUA.GetModelsKolmogorovaGaborText();
            
            // Задаем ширину поля для "Качество"
            int qualityFieldWidth = 15;

            // Заполнение экранных элементов
            for (int i = 0; i < listDefault.Count; i++)
            {
                string qualityString = "Качество: " + listDefault[i].Quality.ToString("F4");
                listBoxDefault.Items.Add(qualityString.PadRight(qualityFieldWidth) + "\t" + listDefaultText[i]);
            }

            for (int i = 0; i < listKolmogorovGabor.Count; i++)
            {
                string qualityString = "Качество: " + listKolmogorovGabor[i].Quality.ToString("F4");
                listBoxKolmogorovGabor.Items.Add(qualityString.PadRight(qualityFieldWidth) + "\t" + listKolmogorovGaborText[i]);
            }
        }

        private void FillPredictListBoxs()
        {
            // Очистка экранных элементов
            listBoxDefaultPredict.Items.Clear();
            listBoxKolmogorovGaborPredict.Items.Clear();

            // Получение предсказаний на основе данных
            List<Predict> listDefault = MGUA.GetModelsDefaultPredict();
            List<Predict> listKolmogorovGabor = MGUA.GetModelsKolmogorovaGaborPredict();

            // Задаем ширину поля для "Страна"
            int qualityFieldWidth = 25;

            // Заполнение экранных элементов
            for (int i = 0; i < listDefault.Count; i++)
            {
                string nameString = "Страна: " + listDefault[i].Name;
                listBoxDefaultPredict.Items.Add(nameString.PadRight(qualityFieldWidth) + "\t" + "Предсказание: " + listDefault[i].Value.ToString("F4"));
            }

            for (int i = 0; i < listKolmogorovGabor.Count; i++)
            {
                string nameString = "Страна: " + listKolmogorovGabor[i].Name;
                listBoxKolmogorovGaborPredict.Items.Add(nameString.PadRight(qualityFieldWidth) + "\t" + "Предсказание: " + listKolmogorovGabor[i].Value.ToString("F4"));
            }
        }
    }
}
