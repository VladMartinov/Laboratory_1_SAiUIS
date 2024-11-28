using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace Laboratory_1.Classes
{
    internal class MGUA
    {
        #region - Variables -
        private List<ExcelDataObject> datasetStudy;
        private List<ExcelTargetObject> datasetTargetStudy;

        private List<ExcelDataObject> datasetPredict;
        private List<ExcelTargetObject> datasetTargetPredict;

        private List<ExcelDataObject> datasetDataPredict;

        private List<Model> modelsDefault;
        private List<Predict> modelsDefaultPredict;
        
        private List<Model> modelsKolmogorovaGabor;
        private List<Predict> modelsKolmogorovaGaborPredict;

        private ushort polynomialDegree;
        #endregion

        public MGUA() {
            this.polynomialDegree = 1;
        }

        #region - Helper Function -
        private List<List<ushort>> GenerateCombinations(ushort[] values, int minLength, int maxLength)
        {
            List<List<ushort>> combinations = new List<List<ushort>>();
            GenerateCombinationsRecursive(values, minLength, maxLength, 0, new List<ushort>(), combinations);
            return combinations;
        }

        private void GenerateCombinationsRecursive(ushort[] values, int minLength, int maxLength, int startIndex, List<ushort> currentCombination, List<List<ushort>> combinations)
        {
            if (currentCombination.Count >= minLength && currentCombination.Count <= maxLength)
            {
                combinations.Add(new List<ushort>(currentCombination));
            }

            if (currentCombination.Count >= maxLength) return;

            for (int i = startIndex; i < values.Length; i++)
            {
                if (!currentCombination.Contains(values[i]))
                {
                    currentCombination.Add(values[i]);
                    GenerateCombinationsRecursive(values, minLength, maxLength, i + 1, currentCombination, combinations);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                }
            }
        }

        public List<List<ushort>> GenerateCombinationsKolmogorovaGabor(ushort[] values, int combinationLength)
        {
            List<List<ushort>> combinations = new List<List<ushort>>();
            GenerateCombinationsKolmogorovaGaborRecursive(values, combinationLength, new List<ushort>(), combinations);
            return combinations;
        }

        private void GenerateCombinationsKolmogorovaGaborRecursive(ushort[] values, int combinationLength, List<ushort> currentCombination, List<List<ushort>> combinations)
        {
            if (currentCombination.Count == combinationLength)
            {
                combinations.Add(new List<ushort>(currentCombination));
                return;
            }

            for (int i = 0; i < values.Length; i++) //Изменение 1: цикл по всему массиву значений
            {
                currentCombination.Add(values[i]);
                GenerateCombinationsKolmogorovaGaborRecursive(values, combinationLength, currentCombination, combinations); //Изменение 2: index = 0 для повторения
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        private double GetFieldDataValue(ExcelDataObject obj, FeatureIndex index)
        {
            switch (index)
            {
                case FeatureIndex.GDPForUnit:
                    return obj.GDPForUnit;
                case FeatureIndex.GDP:
                    return obj.GDP;
                case FeatureIndex.KnowledgeIndex:
                    return obj.KnowledgeIndex;
                case FeatureIndex.QualityLifeIndex:
                    return obj.QualityLifeIndex;
                case FeatureIndex.SustainableDevelopmentIndex:
                    return obj.SustainableDevelopmentIndex;
                case FeatureIndex.DigitalCompetitivenessIndex:
                    return obj.DigitalCompetitivenessIndex;
                case FeatureIndex.EconomicGrowthRate:
                    return obj.EconomicGrowthRate;
                default:
                    return 0;
            }
        }

        private double GetFieldTargetValue(ExcelTargetObject obj, FeatureIndex index)
        {
            switch (index)
            {
                case FeatureIndex.Target:
                    return obj.Tagret;
                default:
                    return 0;
            }
        }

        private Vector<double> GetVectorOfAFeatures(List<ExcelDataObject> data, List<ExcelTargetObject> targetData, List<ushort> combination)
        {
            var xMatrix = Matrix<double>.Build.Dense(data.Count, this.polynomialDegree + 1);
            var yVector = Vector<double>.Build.Dense(data.Count);

            for (int i = 0; i < data.Count; i++)
            {
                // 1-й столбец заполняется 1-ми для свободного члена
                xMatrix[i, 0] = 1;

                for (int j = 1, k = 0; j < (int) FeatureIndex.EconomicGrowthRate + 2; j++)
                {
                    if (!combination.Contains((ushort) (j - 1)))
                    {
                        k++;
                        continue;
                    }

                    xMatrix[i, j - k] = GetFieldDataValue(data[i], (FeatureIndex) j - 1);
                }

                yVector[i] = GetFieldTargetValue(targetData[i], FeatureIndex.Target);
            }

            // Вычислить X^T * X
            var XTX = xMatrix.Transpose() * xMatrix;

            // Найти обратную матрицу X^T * X
            var XTXInverse = XTX.Inverse();

            // Вычислить вектор коэффициентов a
            return XTXInverse * xMatrix.Transpose() * yVector;
        }

        private Vector<double> GetVectorOfAKGFeatures(List<ExcelDataObject> data, List<ExcelTargetObject> targetData, List<ushort> combination)
        {
            int numDataPoints = data.Count;
            int numFeatures = combination.Count;

            // Проверка на валидность комбинации (треугольное число)
            int n = 0;
            while ((n * (n + 1)) / 2 < numFeatures) { n++; }
            if ((n * (n + 1)) / 2 != numFeatures)
            {
                throw new ArgumentException("Invalid combination length. Must be a triangular number.");
            }

            int numVariables = n;

            var xMatrix = Matrix<double>.Build.Dense(numDataPoints, this.polynomialDegree + 1);
            var yVector = Vector<double>.Build.Dense(numDataPoints);

            // Заполняем yVector
            for (int i = 0; i < numDataPoints; i++)
            {
                yVector[i] = GetFieldTargetValue(targetData[i], FeatureIndex.Target);
            }

            // Заполняем xMatrix с использованием рекурсии для генерации комбинаций
            for (int i = 0; i < numDataPoints; i++)
            {
                xMatrix[i, 0] = 1; // Добавляем свободный член в первый столбец
                GenerateAKGMatrixRow(data[i], combination, numVariables, 0, 1, xMatrix, i, 1); // Начинаем со второго столбца
            }

            try
            {
                var XTX = xMatrix.Transpose() * xMatrix;
                var XTXInverse = XTX.Inverse();
                return XTXInverse * xMatrix.Transpose() * yVector;
            }
            catch (Exception ex)
            {
                // Обработка случая, когда матрица XTX сингулярна (нет единственного решения)
                Console.WriteLine($"Error calculating KG coefficients: {ex.Message}");
                return Vector<double>.Build.Dense(numFeatures, 0); // Возвращаем нулевой вектор
            }
        }

        private void GenerateAKGMatrixRow(ExcelDataObject dataPoint, List<ushort> combination, int numVariables, int currentVariable, double currentValue, Matrix<double> matrix, int rowIndex, int columnIndex)
        {
            if (currentVariable >= numVariables) return;

            int combinationIndex = (currentVariable * (currentVariable + 1)) / 2;

            double value = currentValue;
            if (combination[combinationIndex] < numVariables) // Проверка на валидный индекс
            {
                value *= GetFieldDataValue(dataPoint, (FeatureIndex)combination[combinationIndex]);
                matrix[rowIndex, columnIndex] = value;

                for (int nextVariable = currentVariable + 1; nextVariable < numVariables; nextVariable++)
                {
                    GenerateAKGMatrixRow(dataPoint, combination, numVariables, nextVariable, value, matrix, rowIndex, columnIndex + 1);
                }
            }
            else
            {
                //Обработка невалидного индекса - либо игнорируем, либо выбрасываем исключение
                matrix[rowIndex, columnIndex] = 0; //Или  throw new ArgumentOutOfRangeException("Invalid index in combination array.");
                GenerateAKGMatrixRow(dataPoint, combination, numVariables, currentVariable + 1, currentValue, matrix, rowIndex, columnIndex + 1);
            }
        }

        private int GetPolynomDegreeByCombination(List<ushort> combination)
        {
            int degree = 0;

            for (int i = 1; degree < combination.Count; i++)
                degree += i;

            return degree;
        }

        private Model GetModel(Vector<double> data, ModelType type, List<ushort> combination)
        {
            var model = new Model();

            int count;
            List<double> weight = new List<double>();

            switch (type)
            {
                case ModelType.yKDefault:
                case ModelType.yKSquare:
                case ModelType.yKCube:
                    count = (int)FeatureIndex.EconomicGrowthRate + 2;
                    weight.AddRange(Enumerable.Repeat(0d, count));

                    // 1-й столбец заполняется свободным членом
                    weight[0] = data[0];

                    for (int i = 1, k = 0; i < (int)FeatureIndex.EconomicGrowthRate + 2; i++)
                    {
                        if (!combination.Contains((ushort)(i - 1)))
                        {
                            k++;
                            continue;
                        }

                        weight[i] = data[i - k];
                    }

                    model.Weight = weight;

                    break;
                case ModelType.yKolmogorovaGabor:
                    count = 1 + Enumerable.Range(1, (int)FeatureIndex.EconomicGrowthRate).Sum();
                    weight.AddRange(Enumerable.Repeat(0d, count));

                    //Копируем значения из data в weight
                    int dataLength = data.Count;
                    for (int i = 0; i < Math.Min(count, dataLength); i++)
                        weight[i] = data[i];

                    // Проверяем на случай нехватки данных
                    if (count > dataLength)
                        Console.WriteLine($"Warning: Not enough coefficients in data for Kolmogorov-Gabor model.  Expected {count}, got {dataLength}");

                    model.Weight = weight;

                    break;
                default:
                    model.Weight = new List<double>();

                    break;
            }
            
            model.Combination = combination;
            model.Type = type;

            return model;
        }

        private string GetModelText(Model model)
        {
            StringBuilder sb = new StringBuilder("y = ");
            sb.Append(model.Weight[0]);

            for (int i = 0; i < model.Combination.Count; i++)
            {
                sb.Append(" + ");
                sb.Append(model.Weight[model.Combination[i] + 1]);
                sb.Append("*x");
                sb.Append(model.Combination[i]);

                switch (model.Type)
                {
                    case ModelType.yKSquare:
                        sb.Append("^2");
                        break;
                    case ModelType.yKCube:
                        sb.Append("^3");
                        break;
                }
            }

            return sb.ToString();
        }

        private string GetModelKolmogorovaGaborText(Model model)
        {
            StringBuilder sb = new StringBuilder("y = ");
            sb.Append(model.Weight[0]);

            for (int i = 1; i < GetPolynomDegreeByCombination(model.Combination); i++)
            {
                List<ushort> newCombination = new List<ushort>(model.Combination);
                List<ushort> indices = GetCombinationIndices(newCombination, i);

                sb.Append(" + ");
                sb.Append(model.Weight[i]);

                foreach (var index in indices)
                    sb.Append("*x" + index);
            }

            return sb.ToString();
        }

        private double GetModelQuality(Model model, List<ushort> combination, List<ExcelDataObject> training, List<ExcelDataObject> checking)
        {
            double result = 0;

            for (int i = 0; i < training.Count && i < checking.Count; i++)
            {
                double trainingResult = 0,
                       checkingResult = 0;

                if (model.Type != ModelType.yKolmogorovaGabor)
                {
                    for (int j = 0; j < model.Weight.Count; j++)
                    {
                        if (j == 0)
                        {
                            trainingResult += model.Weight[j];
                            checkingResult += model.Weight[j];
                            continue;
                        }

                        switch (model.Type)
                        {
                            case ModelType.yKDefault:
                                trainingResult += model.Weight[j] * GetFieldDataValue(training[i], (FeatureIndex) j - 1);
                                checkingResult += model.Weight[j] * GetFieldDataValue(checking[i], (FeatureIndex) j - 1);
                                break;
                            case ModelType.yKSquare:
                                trainingResult += model.Weight[j] * Math.Pow(GetFieldDataValue(training[i], (FeatureIndex) j - 1), 2);
                                checkingResult += model.Weight[j] * Math.Pow(GetFieldDataValue(checking[i], (FeatureIndex) j - 1), 2);
                                break;
                            case ModelType.yKCube:
                                trainingResult += model.Weight[j] * Math.Pow(GetFieldDataValue(training[i], (FeatureIndex) j - 1), 3);
                                checkingResult += model.Weight[j] * Math.Pow(GetFieldDataValue(checking[i], (FeatureIndex) j - 1), 3);
                                break;
                            default:
                                trainingResult += 0;
                                checkingResult += 0;

                                break;
                        }
                    }
                }
                else
                {
                    trainingResult += model.Weight[0];
                    checkingResult += model.Weight[0];

                    for (int k = 1; k <= this.polynomialDegree; k++)
                    {
                        List<ushort> newCombination = new List<ushort>(combination);
                        List<ushort> indices = GetCombinationIndices(newCombination, k);

                        if (indices.Count > 0)
                        {
                            trainingResult += CalculateCombinationValue(training[i], indices, model.Weight[k]);
                            checkingResult += CalculateCombinationValue(checking[i], indices, model.Weight[k]);
                        }
                    }
                }

                result += Math.Pow(trainingResult - checkingResult, 2);
            }

            return result;
        }

        private List<ushort> GetCombinationIndices(List<ushort> combination, int index)
        {
            int startIndex = Enumerable.Range(0, index).Sum();

            return combination.Splice(startIndex, index);
        }

        private double CalculateCombinationValue(ExcelDataObject obj, List<ushort> indices, double weight)
        {
            double value = 1.0;
            foreach (var index in indices)
            {
                value *= GetFieldDataValue(obj, (FeatureIndex)index);
            }
            return value * weight;
        }
        #endregion

        #region - Methods -
        public void SetData(List<ExcelDataObject> dataset)
        {
            this.datasetStudy = new List<ExcelDataObject>();
            this.datasetPredict = new List<ExcelDataObject>();

            for (int i = 0; i < dataset.Count; i++)
            {
                if (i < dataset.Count / 2) this.datasetStudy.Add(dataset[i]);
                else this.datasetPredict.Add(dataset[i]);
            }
        }

        public void SetTargetData(List<ExcelTargetObject> dataset)
        {
            this.datasetTargetStudy = new List<ExcelTargetObject>();
            this.datasetTargetPredict = new List<ExcelTargetObject>();

            for (int i = 0; i < dataset.Count; i++)
            {
                if (i < dataset.Count / 2) this.datasetTargetStudy.Add(dataset[i]);
                else this.datasetTargetPredict.Add(dataset[i]);
            }
        }

        public void SetPredictData(List<ExcelDataObject> dataset)
        {
            this.datasetDataPredict = new List<ExcelDataObject>();

            for (int i = 0; i < dataset.Count; i++)
                this.datasetDataPredict.Add(dataset[i]);
        }

        public List<Model> GetModelsDefault()
        {
            return this.modelsDefault;
        }

        public List<string> GetModelsDefaultText()
        {
            List<string> result = new List<string>();

            foreach (var model in this.modelsDefault)
                result.Add(GetModelText(model));

            return result;
        }

        public List<Predict> GetModelsDefaultPredict()
        {
            return this.modelsDefaultPredict;
        }

        public List<Model> GetModelsKolmogorovaGabor()
        {
            return this.modelsKolmogorovaGabor;
        }

        public List<string> GetModelsKolmogorovaGaborText()
        {
            List<string> result = new List<string>();

            foreach (var model in this.modelsKolmogorovaGabor)
                result.Add(GetModelKolmogorovaGaborText(model));

            return result;
        }

        public List<Predict> GetModelsKolmogorovaGaborPredict()
        {
            return this.modelsKolmogorovaGaborPredict;
        }

        public void ClearModels()
        {
            this.modelsDefault?.Clear();
            this.modelsKolmogorovaGabor?.Clear();
        }

        public void ClearPredict()
        {
            this.modelsDefaultPredict?.Clear();
            this.modelsKolmogorovaGaborPredict?.Clear();
        }

        public void Start()
        {
            // Очистка моделей перед обучением
            this.ClearModels();

            this.polynomialDegree = 1;

            bool breakCycle = false;

            var models = new List<Model>();

            for (int i = 0; i < (int) FeatureIndex.EconomicGrowthRate && !breakCycle; i++)
            {
                List<List<ushort>> combinations = GenerateCombinations(new ushort[] { 0, 1, 2, 3, 4, 5, 6 }, this.polynomialDegree, this.polynomialDegree);
                var tempModels = new List<Model>();

                // Получение стандартных моделей
                foreach (var combination in combinations)
                {
                    // Получение вектора коэффициентов A
                    Vector<double> aVector = GetVectorOfAFeatures(this.datasetStudy, this.datasetTargetStudy, combination);

                    // Если не удалось расчитать A коэффициенты в текущей комбинации, переходим к следующей
                    if (aVector.Any(x => double.IsNaN(x)))
                        continue;

                    // Получение модели
                    Model modelKDefault = GetModel(aVector, ModelType.yKDefault, combination);
                    Model modelKSquare = GetModel(aVector, ModelType.yKSquare, combination);
                    Model modelKCube = GetModel(aVector, ModelType.yKCube, combination);

                    // Оценка полученной модели
                    modelKDefault.Quality = GetModelQuality(modelKDefault, combination, this.datasetStudy, this.datasetPredict);
                    modelKSquare.Quality = GetModelQuality(modelKSquare, combination, this.datasetStudy, this.datasetPredict);
                    modelKCube.Quality = GetModelQuality(modelKCube, combination, this.datasetStudy, this.datasetPredict);

                    // Добавление новой модели
                    tempModels.Add(modelKDefault);
                    tempModels.Add(modelKSquare);
                    tempModels.Add(modelKCube);
                }

                int lengthCombinationKolmogorovaGabor = Enumerable.Range(1, this.polynomialDegree).Sum();

                // Получение моделей Колмогорова-Габора
                foreach (var combination in combinations)
                {
                    // Получение всевозможных комбинаций доступных для 
                    List<List<ushort>> combinationsKolmogorovaGabor = GenerateCombinationsKolmogorovaGabor(combination.ToArray(), lengthCombinationKolmogorovaGabor);

                    foreach (var combinationKolmogorovaGabor in combinationsKolmogorovaGabor)
                    {
                        // Получение вектора коэффициентов A
                        Vector<double> aVector = GetVectorOfAKGFeatures(this.datasetStudy, this.datasetTargetStudy, combinationKolmogorovaGabor);

                        // Если не удалось расчитать A коэффициенты в текущей комбинации, переходим к следующей
                        if (aVector.Any(x => double.IsNaN(x)))
                            continue;

                        // Получение модели
                        Model modelKolmogorovaGabor = GetModel(aVector, ModelType.yKolmogorovaGabor, combinationKolmogorovaGabor);

                        // Оценка полученной модели
                        modelKolmogorovaGabor.Quality = GetModelQuality(modelKolmogorovaGabor, combinationKolmogorovaGabor, this.datasetStudy, this.datasetPredict);

                        // Добавление новой модели
                        tempModels.Add(modelKolmogorovaGabor);
                    }
                }

                // Если наш полином больше 2-х и результаты обучения хуже, заканчиваем обучение
                if (this.polynomialDegree > 2 && models.Count != 0 && tempModels.Min(m => m.Quality) > models.Min(m => m.Quality))
                {
                    breakCycle = true;
                }
                else
                {
                    this.polynomialDegree++;
                    models = models.Union(tempModels).ToList();
                }
            }

            // Получаем 10 лучших моделей стандартного типа
            this.modelsDefault = models
                .Where(m => m.Type != ModelType.yKolmogorovaGabor)
                .OrderBy(m => m.Quality)
                .Take(10)
                .ToList();

            // Получаем 10 лучших моделей Колмогорова-Габора
            this.modelsKolmogorovaGabor = models
                .Where(m => m.Type == ModelType.yKolmogorovaGabor)
                .OrderBy(m => m.Quality)
                .Take(10)
                .ToList();
        }

        public void Predict()
        {
            // Очистка данных перед предсказаниями
            this.ClearPredict();
            this.modelsDefaultPredict = new List<Predict>();
            this.modelsKolmogorovaGaborPredict = new List<Predict>();

            // Получение лучших моделей
            var bestDefaultModel = this.modelsDefault.FirstOrDefault();
            var bestKolmogorovaGaborModel = this.modelsKolmogorovaGabor.FirstOrDefault();

            for (int i = 0; i < this.datasetDataPredict.Count; i++)
            {
                this.modelsDefaultPredict.Add(new Predict { Name = this.datasetDataPredict[i].Name });
                this.modelsKolmogorovaGaborPredict.Add(new Predict { Name = this.datasetDataPredict[i].Name });

                this.modelsDefaultPredict[i].Value += bestDefaultModel.Weight[0];

                for (int j = 1; j < bestDefaultModel.Weight.Count; j++)
                {
                    switch (bestDefaultModel.Type)
                    {
                        case ModelType.yKDefault:
                            this.modelsDefaultPredict[i].Value += bestDefaultModel.Weight[j] * GetFieldDataValue(this.datasetDataPredict[i], (FeatureIndex)j - 1);
                            break;
                        case ModelType.yKSquare:
                            this.modelsDefaultPredict[i].Value += bestDefaultModel.Weight[j] * Math.Pow(GetFieldDataValue(this.datasetDataPredict[i], (FeatureIndex)j - 1), 2);
                            break;
                        case ModelType.yKCube:
                            this.modelsDefaultPredict[i].Value += bestDefaultModel.Weight[j] * Math.Pow(GetFieldDataValue(this.datasetDataPredict[i], (FeatureIndex)j - 1), 3);
                            break;
                        default:
                            this.modelsDefaultPredict[i].Value += 0;
                            break;
                    }
                }

                this.modelsKolmogorovaGaborPredict[i].Value += bestKolmogorovaGaborModel.Weight[0];

                for (int k = 1; k <= this.GetPolynomDegreeByCombination(bestKolmogorovaGaborModel.Combination) - 1; k++)
                {
                    List<ushort> newCombination = new List<ushort>(bestKolmogorovaGaborModel.Combination);
                    List<ushort> indices = GetCombinationIndices(newCombination, k);

                    if (indices.Count > 0)
                    {
                        this.modelsKolmogorovaGaborPredict[i].Value += CalculateCombinationValue(this.datasetDataPredict[i], indices, bestKolmogorovaGaborModel.Weight[k]);
                    }
                }
            }
        }
        #endregion
    }

    internal static class ListExtensions
    {
        public static List<T> Splice<T>(this List<T> list, int index, int count)
        {
            List<T> range = list.GetRange(index, count);
            list.RemoveRange(index, count);
            return range;
        }
    }
}
