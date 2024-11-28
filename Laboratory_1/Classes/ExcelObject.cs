namespace Laboratory_1.Classes
{
    public enum FeatureIndex
    {
        GDPForUnit,
        GDP,
        KnowledgeIndex,
        QualityLifeIndex,
        SustainableDevelopmentIndex,
        DigitalCompetitivenessIndex,
        EconomicGrowthRate,
        Target
    }

    internal class ExcelDataObject
    {
        // Наименование страны
        public string Name { get; set; }
        // ВВП на душу населения
        public double GDPForUnit { get; set; }
        // ВВП
        public double GDP { get; set; }
        // Индекс знаний
        public double KnowledgeIndex { get; set; }
        // Индекс качества жизни
        public double QualityLifeIndex { get; set; }
        // Индекс устойчивого развития
        public double SustainableDevelopmentIndex { get; set; }
        // Индекс цифровой конкурентоспособности
        public double DigitalCompetitivenessIndex { get; set; }
        // Темп экономического роста
        public double EconomicGrowthRate { get; set; }
    }

    internal class ExcelTargetObject
    {
        // Наименование страны
        public string Name { get; set; }
        // Целевая переменная
        public double Tagret { get; set; }
    }
}
