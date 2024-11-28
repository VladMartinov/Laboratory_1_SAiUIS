using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Laboratory_1.Classes
{
    internal class ExcelReader
    {
        #region - Variables -
        private string path;
        private bool isTarget;

        private List<ExcelDataObject> data;
        private List<ExcelTargetObject> targetData;
        #endregion

        public ExcelReader() { }

        #region - Methods -
        public void SetPath(string path)
        {
            this.path = path;
        }

        public void SetTarget(bool value)
        {
            this.isTarget = value;
        }

        public List<ExcelDataObject> GetData()
        {
            return this.data;
        }

        public List<ExcelTargetObject> GetTargetData()
        {
            return this.targetData;
        }

        public void ReadFile()
        {
            // Очищаем считанные данные
            if (this.isTarget) this.targetData = new List<ExcelTargetObject>();
            else this.data = new List<ExcelDataObject>();

            // Создание объекта Excel и рабочей книжки
            var excel = new Application();
            var workbook = excel.Workbooks.Open(this.path,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

            var worksheet = (Worksheet) workbook.Worksheets[1];
            var dataRange = worksheet.UsedRange;

            // Получение списка значений из таблицы
            object[,] objectArray = (object[,]) dataRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);

            // Проход по значениеям таблицы
            for (int row = 3; row <= worksheet.UsedRange.Rows.Count; ++row)
            {
                if (this.isTarget)
                {
                    var excelObject = new ExcelTargetObject
                    {
                        Name = objectArray[row, 1].ToString(),
                        Tagret = double.Parse(objectArray[row, 2].ToString().Replace(',', '.'))
                    };

                    this.targetData.Add(excelObject);
                }
                else
                {
                    var excelObject = new ExcelDataObject
                    {
                        Name = objectArray[row, 1].ToString(),
                        GDPForUnit = double.Parse(objectArray[row, 2].ToString().Replace(',', '.')),
                        GDP = double.Parse(objectArray[row, 3].ToString().Replace(',', '.')),
                        KnowledgeIndex = float.Parse(objectArray[row, 4].ToString().Replace(',', '.')),
                        QualityLifeIndex = float.Parse(objectArray[row, 5].ToString().Replace(',', '.')),
                        SustainableDevelopmentIndex = float.Parse(objectArray[row, 6].ToString().Replace(',', '.')),
                        DigitalCompetitivenessIndex = float.Parse(objectArray[row, 7].ToString().Replace(',', '.')),
                        EconomicGrowthRate = float.Parse(objectArray[row, 8].ToString().Replace(',', '.'))
                    };

                    this.data.Add(excelObject);
                }
            }

            // Очистка памяти
            workbook.Close(false, Type.Missing, Type.Missing);
            Marshal.ReleaseComObject(workbook);

            // Очистка памяти
            excel.Quit();
            Marshal.FinalReleaseComObject(excel);
        }
        #endregion
    }
}
