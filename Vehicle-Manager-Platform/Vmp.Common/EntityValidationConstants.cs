namespace Vmp.Common
{
    public static class EntityValidationConstants
    {
        public static class Owner
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int InfoMinLength = 3;
            public const int InfoMaxLength = 255;
        }

        public static class Vehicle
        {
            public const int NumberMinLength = 4;
            public const int NumberMaxLength = 12;

            public const int VinMinLength = 11;
            public const int VinMaxLength = 17;

            public const int MakeMinLength = 1;
            public const int MakeMaxLength = 50;

            public const int ModelMinLength = 1;
            public const int ModelMaxLength = 50;

            public const int MileageMinValue = 0;
            public const int MileageMaxValue = 999999;

            public const decimal FuelQuantityMinValue = 0.00M;

            public const decimal FuelCapacityMinValue = 1.00M;

            public const decimal FuelCostRateMinValue = 0.00M;

            public const int ImgUrlMaxLength = 2048;
        }

        public static class TaskModel
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 1;
            public const int DescriptionMaxLength = 1000;
        }

        public class DateCheck
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public class MileageCheck
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int ExpectedMileageMinValue = 1;
            public const int ExpectedMileageMaxValue = 999999;
        }

        public class Waybill
        {
            public const int StartMileageMinValue = 0;
            public const int StartMileageMaxValue = 999999;

            public const int ExpectedMileageMinValue = 1;
            public const int ExpectedMileageMaxValue = 999999;

            public const int InfoMaxLength = 255;

            public const int RouteTraveledMinLength = 1;
            public const int RouteTraveledMaxLength = 255;
        }

        public class CostCenter
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }
    }
}